using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Models;

namespace FreshMart.ViewModels
{
    public class OrderViewModel : LayoutCategoryViewModel
    {

        public Customer Customer { get; set; }


        public Cart Cart { get; set; }

        public CartItem CartItem { get; set; }

        public List<CartItem> CartItems { get; set; }

        public Product Product { get; set; }

        public ProductOrder ProductOrder { get; set; }

        public Order Order { get; set; }

        public List<District> District { get; set; }


    }
}
