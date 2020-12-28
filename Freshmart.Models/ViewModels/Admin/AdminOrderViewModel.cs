using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Models;

namespace FreshMart.ViewModels
{
    public class AdminOrderViewModel
    {
        public Product Product { get; set; }
        public ProductOrder ProductOrder { get; set; }
        public Order Order { get; set; }


        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductOrder> ProductOrders { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public MatchViewModel MatchVm { get; set; }
        public IEnumerable<AgentOrder> AgentOrders { get; set; }
    }
}
