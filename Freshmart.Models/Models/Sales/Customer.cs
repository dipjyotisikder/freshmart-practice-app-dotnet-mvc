using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public District District { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
