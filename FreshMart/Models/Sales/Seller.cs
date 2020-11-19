using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace FreshMart.Models
{
    [Table("Sellers")]
    public class Seller
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public District District { get; set; }

        public string CompanyName { get; set; }

        public bool? Approval { get; set; }


        public ICollection<Product> Products { get; set; }
    }
}