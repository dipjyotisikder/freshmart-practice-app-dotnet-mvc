using FreshMart.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace FreshMart.Models
{
    [Table("Sellers")]
    public class Seller : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        public string CompanyName { get; set; }
    }
}