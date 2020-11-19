using FreshMart.Data;
using FreshMart.Models;
using FreshMart.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Services
{
    public interface IProductService
    {
        //Products
        List<Product> GetAllProducts();
        List<Product> GetProductByCategoryID(int id);
        ProductViewModel ProductVMWithCartCount(int id);


        List<District> GetAllDistricts();

        //Category
        List<Category> GetAllCategories();
        List<string> GetCategoryByDomain();

        string GetCategoryByDomainID(int id);

        ProductViewModel GetProductViewModel();

    }
}
