using FreshMart.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FreshMart.Services.Factories
{
    public class AmazonS3FileService : IFileService
    {
        public Task<bool> Delete(long id)
        {
            throw new System.NotImplementedException();
        }

        public string GetDocumentName(IFormFile file)
        {
            throw new System.NotImplementedException();
        }

        public Document Upload(IFormFile file)
        {
            throw new System.NotImplementedException();
        }
    }
}
