using FreshMart.Core.Infrastructure;
using FreshMart.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FreshMart.ViewModels
{
    public class CreateProductViewModel
    {
        [DisplayName("Title (Required)")]
        public string Title { get; set; }


        [DisplayName("Price (Required)")]
        public float? Price { get; set; }


        [DisplayName("Description")]
        public string Description { get; set; }


        [DisplayName("Unit (Required)")]
        public string Unit { get; set; }


        [DisplayName("Item in Stock (Required)")]
        public int? ItemInStock { get; set; }

        [DisplayName("Category (Required)")]
        public long? CategoryId { get; set; }


        [DisplayName("Upload an image (Required)")]
        public string ImagePath { get; set; }


        public float? OfferPrice { get; set; }
    }
}