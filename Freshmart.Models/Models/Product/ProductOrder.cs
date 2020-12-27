using FreshMart.Core.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
namespace FreshMart.Models
{
    [Table("ProductOrders")]
    public class ProductOrder : BaseEntity
    {
        public long ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }


        public long OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public int NumberOfProduct { get; set; }
    }
}