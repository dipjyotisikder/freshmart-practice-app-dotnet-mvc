﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.ViewModels
{
    public class SearchViewModel
    {
        public string Text { get; set; }

        public long? DistrictId { get; set; }

        public List<int> PriceRanges { get; set; }
        public int? PriceRange { get; set; }

    }
}
