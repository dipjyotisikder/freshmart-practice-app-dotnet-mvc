using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FreshMart.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Title (Required)")]
        public string Title { get; set; }

        public int SellerId { get; set; }
        [ForeignKey("SellerId")]
        public Seller Seller { get; set; }

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
        [ForeignKey("DistrictId")]
        public District District { get; set; }

        [DisplayName("Category (Required)")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

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

        public ICollection<ProductOrder> ProductOrder { get; set; }
    }
}