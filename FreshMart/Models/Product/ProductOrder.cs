using System.ComponentModel.DataAnnotations.Schema;
namespace FreshMart.Models
{
    [Table("ProductOrders")]
    public class ProductOrder
    {

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }


        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int NumberOfProduct { get; set; }
    }
}