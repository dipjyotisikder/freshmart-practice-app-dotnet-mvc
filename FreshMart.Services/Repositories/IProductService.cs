using FreshMart.Models;
using FreshMart.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreshMart.Services
{
    public interface IProductService
    {
        //Products
        List<Product> GetAllProducts();
        List<Product> GetProductsByCategoryId(long id);
        Task<ProductViewModel> GetProductViewModelWithCartCountAsync(long id);


        Task<List<District>> GetAllDistrictsAsync();

        //Category
        Task<List<Category>> GetAllCategoriesAsync();

        List<string> GetParentCategoryNames();

        Task<Category> GetParentCategoryAsync(long id);

        Task<ProductViewModel> GetProductViewModelAsync();

    }
}
