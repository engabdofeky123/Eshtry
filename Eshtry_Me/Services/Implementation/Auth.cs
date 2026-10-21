using Eshtry_Me.Data;
using Eshtry_Me.DTOs;
using Eshtry_Me.HelperClasses;
using Eshtry_Me.Identity;
using Eshtry_Me.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eshtry_Me.Services.Implementation
{
    public class Auth : IAuth
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;


        public Auth(UserManager<ApplicationUser> userManager , IOptions<JwtOptions> options, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _jwtOptions = options.Value;
            _signInManager = signInManager;
            _context = context;
        }
        
        
        public async Task<AuthModel> LoginAsync(LoginDto loginDto)
        {
            ApplicationUser? user;

            if (loginDto.EmailOrPhone.Contains('@'))
            {
                // Login using Email
                user = await _userManager.FindByEmailAsync(loginDto.EmailOrPhone);
            }
            else
            {
                // Login using Phone
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == loginDto.EmailOrPhone);
            }

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return new AuthModel
                {
                    Message = "Email/Phone or Password is not valid"
                };
            }

            var jwtToken = await GenerateToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            return new AuthModel
            {
                ExpiresON = jwtToken.ValidTo,
                EmailOrPhone = loginDto.EmailOrPhone,
                IsAuthenticated = true,
                Roles = rolesList.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Message = "Welcome again ^_^"
            };

        }

        public async Task<AuthModel> RegisterAsync(RegisterDto dto) 
        {
            var FoundMail = await _userManager.FindByEmailAsync(dto.Email);
            var FoundByUserName = await _userManager.FindByNameAsync(dto.FirstName + ' ' + dto.LastName);

            if (FoundByUserName != null || FoundMail != null)
                return new AuthModel { Message = "Email or UserName are already exist" };

            var newAppUser = new ApplicationUser
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address
            };

            var result = await _userManager.CreateAsync(newAppUser, dto.Password);

            if (!result.Succeeded)
            {
                var ErrorList = "";
                foreach (var error in result.Errors)
                    ErrorList += error.Description + ' ';
                return new AuthModel { Message = $"There are some errors : {ErrorList}" };
            }
            await _userManager.AddToRoleAsync(newAppUser, "user");

            // Generate Token

            var token = await GenerateToken(newAppUser);
            return new AuthModel
            {
                EmailOrPhone = newAppUser.Email,
                ExpiresON = token.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "user" },
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = newAppUser.UserName
            };

        }

        public async Task SignOut()
        {
           await _signInManager.SignOutAsync();
        }

        // Generate Tokens

        private async Task<SecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles
                .Select(role => new Claim(ClaimTypes.Role, role))
                .ToList();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("userID" , user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.Key)
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtOptions.ExoiresOn30Days),
                signingCredentials: creds
            );

            return token;
        }

    }
}
