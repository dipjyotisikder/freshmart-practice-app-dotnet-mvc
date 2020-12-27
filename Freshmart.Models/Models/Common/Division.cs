using FreshMart.Core.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshMart.Models
{
    [Table("Divisions")]
    public class Division : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}