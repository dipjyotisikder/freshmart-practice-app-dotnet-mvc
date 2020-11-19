using FreshMart.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Areas.Admin.Models;

namespace FreshMart.ViewModels
{
    public class ProductViewModel : LayoutCategoryViewModel
    {
        public Product Product { get; set; }
        public IList<District> District { get; set; }
        public IList<Product> Products { get; set; }

        public SellerRequest SellerRequest { get; set; }

        public SearchViewModel SearchVm { get; set; }


    }
}
