using FreshMart.Data;
using FreshMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.ViewModels
{
    public class LayoutCategoryViewModel
    {

        public IList<Category> Category { get; set; }
        public IList<string> DistinctCat { get; set; }
        public IList<Product> BaseProduct { get; set; }
        public int CartCount { get; set; }
        public float TotalPrice { get; set; }
        public string Message { get; set; }
        public IList<Seller> Sellers { get; set; }

        public IList<Customer> Customers { get; set; }

    }
}
