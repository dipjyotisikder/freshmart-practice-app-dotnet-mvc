using FreshMart.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FreshMart.Models
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        [DisplayName("Title (Required)")]
        public string Title { get; set; }

        public long SellerId { get; set; }
        [ForeignKey("SellerId")]
        public virtual Seller Seller { get; set; }

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
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

        [DisplayName("Category (Required)")]
        public long CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }


        public long PhotoId { get; set; }
        [ForeignKey("PhotoId")]
        public virtual Document Photo { get; set; }


        [DisplayName("Upload an image (Required)")]
        public string ImagePath { get; set; }

        public float OfferPrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime OfferExpireDate { get; set; }

        public virtual IEnumerable<ProductOrder> ProductOrders { get; set; }
    }
}