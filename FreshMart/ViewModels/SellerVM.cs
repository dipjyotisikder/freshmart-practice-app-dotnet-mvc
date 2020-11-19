﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Areas.Admin.Models;
using FreshMart.Models;

namespace FreshMart.ViewModels
{
    public class SellerVM
    {
        public Seller Seller { get; set; }
        public List<Seller> Sellers { get; set; }

        public District District { get; set; }
        public List<District> Districts { get; set; }
        public Product Product { get; set; }

        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        public SellerRequest SellerRequest { get; set; }


        public string Error { get; set; }
    }
}
