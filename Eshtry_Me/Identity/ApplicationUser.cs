using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Eshtry_Me.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }
       
        public string PhoneNumber { get; set; }
    }
}
