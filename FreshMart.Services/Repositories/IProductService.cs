using FreshMart.Models;
using FreshMart.Models.ViewModels;
using System.Collections.Generic;

namespace FreshMart.Services
{
    public interface IProductService
    {
        //Products
        List<Product> GetAllProducts();
        List<Product> GetProductsByCategoryId(long id);
        ProductViewModel GetProductViewModelWithCartCount(long id);


        List<District> GetAllDistricts();

        //Category
        List<Category> GetAllCategories();
        List<string> GetParentCategoryNames();

        Category GetParentCategory(long id);

        ProductViewModel GetProductViewModel();

    }
}
