using EshtryWepApi.Models;
using Microsoft.AspNetCore.Identity;

namespace Eshtry_Me.DTOs
{
    public class SellersViewModel
    {
        public List<IdentityUser> Sellers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SelectedCategory { get; set; }
    }

}
