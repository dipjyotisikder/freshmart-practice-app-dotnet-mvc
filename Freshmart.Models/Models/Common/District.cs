using FreshMart.Core.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshMart.Models
{
    [Table("Districts")]
    public class District : BaseEntity
    {
        [Required]
        public string Name { get; set; }


        public long? DivisionId { get; set; }
        [ForeignKey(nameof(DivisionId))]
        public virtual Division Division { get; set; }
    }
}