using FreshMart.Models;
using FreshMart.Models.ViewModels;
using System.Collections.Generic;

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
