﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MediatR;
using System.Collections.Generic;
using FreshMart.ViewModels;

namespace FreshMart.Models.Queries
{
    public class GetAllProductQuery : IRequest<ProductViewModel>
    {
        public string Text { get; set; }

        public long? DistrictId { get; set; }

        public List<int> PriceRanges { get; set; }

        public int? PriceRange { get; set; }
    }
}
