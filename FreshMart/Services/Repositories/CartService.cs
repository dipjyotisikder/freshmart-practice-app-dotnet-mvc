using FreshMart.Helper;
using FreshMart.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using FreshMart.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace FreshMart.Services
{
    public class CartService : ICartService
    {
        private int cartCount;
        private float total;
        IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public CartService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }


        public int GetCartCount()
        {

            if (SessionHelper.GetObjectFromJson<List<CartItem>>(_httpContextAccessor.HttpContext.Session, "cart") == null)
            {
                this.cartCount = 0;
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(_httpContextAccessor.HttpContext.Session, "cart");
                this.cartCount = cart.Count;
            }

            return cartCount;
        }


        public float GetCartTotalPrice()
        {
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(_httpContextAccessor.HttpContext.Session, "cart") == null)
            {
                this.total = 0;
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(_httpContextAccessor.HttpContext.Session, "cart");
                this.total = cart.Sum(c => c.Product.Price * c.Quantity);
            }

            return this.total;
        }

        //public GetCartItems()
        //{

        //}

    }

}
