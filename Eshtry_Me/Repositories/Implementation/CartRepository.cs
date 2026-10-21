using Eshtry_Me.Data;
using Eshtry_Me.DTOs;
using Eshtry_Me.Repositories.Interfaces;
using EshtryWepApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Eshtry_Me.Repositories.Implementation
{
    public class CartRepository : ICartRepository
    {
        private ApplicationDbContext _context;
        private readonly IAddressRepository _addressRepository;

        public CartRepository(ApplicationDbContext context, IAddressRepository addressRepository)
        {
            _context = context;
            _addressRepository = addressRepository;
        }


        public async Task<CompleteOrderResult> CompleteOrder( CompleteOrderDto dto, string userId)
        {
            // 2. Validate shipping address belongs to this user
            var address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == dto.AddressId &&a.UserId == userId);

            if (address == null)
            {
                return new CompleteOrderResult
                {
                    IsSuccess = false,
                    Message = "Shipping address not found."
                };
            }

            // 3. Load cart and products
            var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return new CompleteOrderResult
                {
                    IsSuccess = false,
                    Message = "Cart is empty or does not exist."
                };
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var now = DateTime.UtcNow;
                decimal totalAmount = 0m;

                // 4. Prepare order and shipping address snapshot
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = now,
                    ShippingAddress =
                        $"{address.RecipientName}, " +
                        $"{address.PhoneNumber}, " +
                        $"{address.Governorate}, " +
                        $"{address.City}, " +
                        $"{address.StreetAddress}" +
                        (string.IsNullOrWhiteSpace(address.AdditionalDetails)
                            ? string.Empty
                            : $", {address.AdditionalDetails}"),
                    OrderStatus = OrderStatus.Pending,
                    PaymentStatus = PaymentStatus.Pending
                };

                // 5. Validate products, calculate prices and reserve stock
                foreach (var cartItem in cart.CartItems)
                {
                    var product = cartItem.Product;

                    if (product == null || cartItem.Quantity <= 0)
                    {
                        return new CompleteOrderResult
                        {
                            IsSuccess = false,
                            Message = "Invalid cart item."
                        };
                    }

                    var activeOffer =
                        product.DiscountPercent > 0 &&
                        product.OfferEndDate.HasValue &&
                        product.OfferEndDate.Value > now;

                    var unitPrice = activeOffer
                        ? Math.Round(
                            product.Price *
                            (1m - product.DiscountPercent / 100m),
                            2)
                        : product.Price;

                    // Atomic stock check and decrement (EF Core 7+)
                    var updatedRows = await _context.Products
                        .Where(p =>
                            p.Id == product.Id &&
                            p.StockQuantity >= cartItem.Quantity)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(
                                p => p.StockQuantity,
                                p => p.StockQuantity - cartItem.Quantity));

                    if (updatedRows != 1)
                    {
                        return new CompleteOrderResult
                        {
                            IsSuccess = false,
                            Message =
                                $"Insufficient stock for {product.Name}."
                        };
                    }

                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductImageUrl = product.ImageUrl,
                        Quantity = cartItem.Quantity,
                        UnitPrice = unitPrice
                    });

                    totalAmount += unitPrice * cartItem.Quantity;
                }

                // 6. Set the final total
                order.TotalAmount = totalAmount;

                _context.Orders.Add(order);

                // Generate Order.Id
                await _context.SaveChangesAsync();

                // 7. Create a pending Cash on Delivery payment record
                var payment = new Payment
                {
                    OrderId = order.Id,
                    PaymentMethod = "CashOnDelivery",
                    Amount = totalAmount,
                    PaymentDate = now,
                    PaymentStatus = PaymentStatus.Pending
                };

                _context.Payments.Add(payment);

                // 8. Clear cart only after successful validation
                _context.CartItems.RemoveRange(cart.CartItems);

                cart.UpdatedAt = now;

                await _context.SaveChangesAsync();

                // 9. Commit everything together
                await transaction.CommitAsync();

                return new CompleteOrderResult
                {
                    IsSuccess = true,
                    Message = "Order completed successfully."
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<CartDto> GetCartByUserId(string userId) 
        {
            var result = await _context.Carts
                 .Where(c => c.UserId == userId)
                 .Select(c => new CartDto
                 {
                     Id = c.Id,
                     UserId = c.UserId,
                     CreatedAt = c.CreatedAt,
                     UpdatedAt = c.UpdatedAt,
                     CartItems = c.CartItems.Select(ci => new CartItemDto
                     {
                         Id = ci.Id,
                         ProductId = ci.ProductId,
                         Quantity = ci.Quantity,
                         UnitPrice = ci.UnitPrice
                     }).ToList()
                 }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<AddToCartResult> AddToCartAsync(AddToCartModel model, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new AddToCartResult
                {
                    IsSuccess = false,
                    Message = "User is not authenticated"
                };
            }

            if (model.Quantity <= 0)
            {
                return new AddToCartResult
                {
                    IsSuccess = false,
                    Message = "Quantity must be greater than zero"
                };
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == model.ProductId);

            if (product == null)
            {
                return new AddToCartResult
                {
                    IsSuccess = false,
                    Message = "Product not found."
                };
            }

            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Carts.Add(cart);
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == product.Id);

            var newQuantity = (existingItem?.Quantity ?? 0) + model.Quantity;

            if (newQuantity > product.StockQuantity)
            {
                return new AddToCartResult
                {
                    IsSuccess = false,
                    Message = "Requested quantity exceeds available stock"
                };
            }

            var now = DateTime.UtcNow;

            var hasActiveOffer = product.DiscountPercent > 0 && product.OfferEndDate.HasValue && product.OfferEndDate.Value > now;

            var unitPrice = hasActiveOffer ? Math.Round(product.Price * (1m - product.DiscountPercent / 100m), 2) : product.Price;

            if (existingItem != null)
            {
                existingItem.Quantity = newQuantity;
                existingItem.UnitPrice = unitPrice;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = model.Quantity,
                    UnitPrice = unitPrice
                });
            }

            cart.UpdatedAt = now;

            await _context.SaveChangesAsync();

            return new AddToCartResult
            {
                IsSuccess = true,
                Message = "Product added to cart successfully"
            };
        }

    }
}