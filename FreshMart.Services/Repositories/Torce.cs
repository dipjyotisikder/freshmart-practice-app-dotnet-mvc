using System;
using System.Collections.Generic;
using System.Linq;
using FreshMart.Database;
using FreshMart.Models;
using Microsoft.EntityFrameworkCore;

namespace FreshMart.Services
{
    public class Torce
    {
        private readonly AppDbContext _context;

        public Torce(AppDbContext _con)
        {
            _context = _con;
        }

        public List<Product> SearchProducts()
        {
            var products = _context.Products
                .Include(c => c.Category)
                .Include(c => c.District)
                .Include(p => p.Seller)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
            return products;
            ;
        }

        public List<Product> SearchProducts(string text)
        {
            var products = _context.Products
                .Include(c => c.Category)
                .Include(c => c.District)
                .Include(p => p.Seller)
                .Where(p => p.Title.Contains(text))
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
            return products;
            ;
        }


        public List<Product> SearchProducts(string text, int? range)
        {
            if (range == 1)
            {
                var products = _context.Products
                    .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Title.Contains(text))
                    .Where(p => p.Price > 0 && p.Price <= 100.0)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }

            if (range == 2)
            {
                var products = _context.Products
                   .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Title.Contains(text))
                    .Where(p => p.Price > 100.0 && p.Price <= 200.0)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            if (range == 3)
            {
                var products = _context.Products
                   .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Title.Contains(text))
                    .Where(p => p.Price > 200.0 && p.Price <= 300.0)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            if (range == 4)
            {
                var products = _context.Products
                   .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Title.Contains(text))
                    .Where(p => p.Price > 300.0 && p.Price <= 400.0)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            else
            {
                return this.SearchProducts();
            }
        }



        public List<Product> SearchProducts(string text, int? range, long? district)
        {

            if (range == 1)
            {
                List<Product> products = _context.Products
                    .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Title.Contains(text))
                    .Where(p => p.Price > 0 && p.Price <= 100.0)
                    .Where(p => p.District.Id == district)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }

            if (range == 2)
            {
                List<Product> products = _context.Products
                    .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Title.Contains(text))
                    .Where(p => p.Price > 100.0 && p.Price <= 200.0)
                    .Where(p => p.District.Id == district)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            if (range == 3)
            {
                List<Product> products = _context.Products
                    .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Title.Contains(text))
                    .Where(p => p.Price > 200.0 && p.Price <= 300.0)
                    .Where(p => p.District.Id == district)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            if (range == 4)
            {
                List<Product> products = _context.Products
                    .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Title.Contains(text))
                    .Where(p => p.Price > 300.0 && p.Price <= 400.0)
                    .Where(p => p.District.Id == district)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            else
            {
                return this.SearchProducts();
            }

        }



        public List<Product> SearchProducts2(int? range, long? district)
        {
            if (range == 1)
            {
                List<Product> products = _context.Products
                    .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Price > 0 && p.Price <= 100.0)
                    .Where(p => p.District.Id == district)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }

            if (range == 2)
            {
                List<Product> products = _context.Products
                   .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Price > 100.0 && p.Price <= 200.0)
                    .Where(p => p.District.Id == district)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            if (range == 3)
            {
                List<Product> products = _context.Products
                   .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Price > 200.0 && p.Price <= 300.0)
                    .Where(p => p.District.Id == district)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            if (range == 4)
            {
                List<Product> products = _context.Products
                   .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Price > 300.0 && p.Price <= 400.0)
                    .Where(p => p.District.Id == district)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            else
            {
                return this.SearchProducts();
            }
        }

        public List<Product> SearchProducts2(int? range)
        {
            if (range == 1)
            {
                List<Product> products = _context.Products
                    .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Price > 0 && p.Price <= 100.0)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }

            if (range == 2)
            {
                List<Product> products = _context.Products
                   .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Price > 100.0 && p.Price <= 200.0)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            if (range == 3)
            {
                List<Product> products = _context.Products
                   .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Price > 200.0 && p.Price <= 300.0)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            if (range == 4)
            {
                List<Product> products = _context.Products
                   .Include(c => c.Category)
                    .Include(c => c.District)
                    .Include(p => p.Seller)
                    .Where(p => p.Price > 300.0 && p.Price <= 400.0)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                return products;
            }
            else
            {
                return this.SearchProducts();
            }
        }


        public List<Product> SearchProducts3(long? dist)
        {
            List<Product> products = _context.Products
                .Include(c => c.Category)
                .Include(c => c.District)
                .Include(p => p.Seller)
                .Where(p => p.District.Id == dist)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
            return products;
            ;
        }


    }
}
