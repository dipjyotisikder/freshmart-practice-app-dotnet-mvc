using FreshMart.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace FreshMart.Models
{
    [Table("Orders")]
    public class Order : BaseEntity
    {
        public long CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [DisplayName("Your Account Name")]
        public string AccountNo { get; set; }

        [DisplayName("Your Transaction ID")]
        public string TransactionId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string ShippingAddress { get; set; }

        public string PostalCode { get; set; }

        public string Zip { get; set; }

        public string StreetNo { get; set; }

        public float TotalPrice { get; set; }

        public bool IsOrderCompleted { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public virtual ICollection<AgentOrder> AgentOrders { get; set; }

    }
}