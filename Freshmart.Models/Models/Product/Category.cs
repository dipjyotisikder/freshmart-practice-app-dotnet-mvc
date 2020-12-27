using FreshMart.Core.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshMart.Models
{
    [Table("Categories")]
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Domain { get; set; }

        public long? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual Category Parent { get; set; }

        public bool IsParent { get; set; }
    }
}
