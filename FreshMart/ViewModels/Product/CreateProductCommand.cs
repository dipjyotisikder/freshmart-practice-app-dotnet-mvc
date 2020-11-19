using FreshMart.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Areas.Admin.Models;
using System.ComponentModel;

namespace FreshMart.ViewModels
{
    public class CreateProductCommand
    {
        public int Id { get; set; }

        [DisplayName("Title (Required)")]
        public string Title { get; set; }

        public int SellerId { get; set; }

        [DisplayName("Price (Required)")]
        public float Price { get; set; }

        public string Description { get; set; }

        [DisplayName("Unit (Required)")]
        public string Unit { get; set; }

        public bool IsPublished { get; set; }

        [DisplayName("Item in Stock (Required)")]
        public int ItemInStock { get; set; }

        [DisplayName("District (Required)")]
        public int DistrictId { get; set; }

        [DisplayName("Category (Required)")]
        public int CategoryId { get; set; }

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
