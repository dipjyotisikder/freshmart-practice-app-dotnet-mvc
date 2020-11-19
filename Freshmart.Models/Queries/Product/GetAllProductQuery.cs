using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MediatR;
using System.Collections.Generic;
using FreshMart.Models.ViewModels;

namespace FreshMart.Models.Queries
{
    public class GetAllProductQuery : IRequest<ProductViewModel>
    {
        public string Text { get; set; }

        public int? DistrictId { get; set; }

        public List<int> PriceRanges { get; set; }

        public int? PriceRange { get; set; }
    }
}
