using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MediatR;
using FreshMart.Models.ViewModels;

namespace FreshMart.Models.Commands
{
    public class CreateProductCommand : IRequest<ProductViewModel>
    {
        public long Id { get; set; }

        [DisplayName("Title (Required)")]
        public string Title { get; set; }

        public long SellerId { get; set; }

        [DisplayName("Price (Required)")]
        public float Price { get; set; }

        public string Description { get; set; }

        [DisplayName("Unit (Required)")]
        public string Unit { get; set; }

        public bool IsPublished { get; set; }

        [DisplayName("Item in Stock (Required)")]
        public int ItemInStock { get; set; }

        [DisplayName("District (Required)")]
        public long DistrictId { get; set; }

        [DisplayName("Category (Required)")]
        public long CategoryId { get; set; }

        //IMAGE
        [DisplayName("Upload an image (Required)")]
        public string ImagePath { get; set; }

        public float OfferPrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime OfferExpireDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }
    }
}
