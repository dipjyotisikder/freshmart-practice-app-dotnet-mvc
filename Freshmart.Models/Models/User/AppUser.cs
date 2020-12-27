using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshMart.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class AppUser : IdentityUser
    {
        public long DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public District District { get; set; }
    }
}
