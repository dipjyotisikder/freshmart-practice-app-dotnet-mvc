using FreshMart.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FreshMart.Services.Factories
{
    public interface IFileService
    {
        Document Upload(IFormFile file);

        Task<bool> Delete(long id);

        string GetDocumentName(IFormFile file);
    }
}
