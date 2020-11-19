using FreshMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Models.ViewModels
{
    public class Cart : LayoutCategoryViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalAmount { get; set; }

    }

    public class CartItem : LayoutCategoryViewModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }

    }
}
