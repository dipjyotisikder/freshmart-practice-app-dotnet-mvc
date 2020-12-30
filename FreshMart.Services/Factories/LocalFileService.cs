using FreshMart.Core;
using FreshMart.Core.Utilities;
using FreshMart.Database;
using FreshMart.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Services.Factories
{
    public class LocalFileService : IFileService
    {
        private readonly IHostingEnvironment _env;
        private readonly IEncryptionServices _encryptionService;
        private readonly AppDbContext _context;

        public LocalFileService(IHostingEnvironment env, IEncryptionServices encryptionService, AppDbContext context)
        {
            _env = env;
            _encryptionService = encryptionService;
            _context = context;
        }

        public async Task<bool> Delete(long id)
        {
            var document = await _context.Documents.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (document != null)
            {
                string path_Root = _env.WebRootPath;
                string path = Path.Combine(path_Root, document.Path);

                if (File.Exists(path))
                {
                    await Task.Run(() => File.Delete(path));

                    document.IsDeleted = true;
                    return await _context.SaveChangesAsync() >= 0;
                }
            }
            return false;
        }

        public string GetDocumentName(IFormFile file)
        {
            //get name
            var name = Path.GetFileNameWithoutExtension(file.FileName);
            name = name.Replace(" ", "_");
            return name;
        }

        public Document Upload(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            //DRILL ORIGINAL NAME
            var originalName = GetDocumentName(file);

            //DRILL EXTENSION
            var extension = Path.GetExtension(file.FileName).ToLower();

            //GENERATE UNIQUE NAME
            var currentName = _encryptionService.GetUniqueKey(20);

            //ROOT PATH
            string rootPath = _env.ContentRootPath;

            //APPLICATION LOCAL DIRECTORY PATH
            string localDirectory = $"/StaticFiles/Images/";

            //FILE PATH
            string imagePath = $"{rootPath}{localDirectory}{currentName}{extension}";

            //SAVE STREAM
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                file.CopyTo(stream);
                stream.Dispose();
            }

            return new Document
            {
                Id = NumberUtilities.GetUniqueNumber(),
                OriginalName = originalName,
                Name = currentName,
                CreatedAt = DateTime.UtcNow,
                Extension = extension,
                Path = Path.Combine(localDirectory, (currentName + extension)),
                Type = FileUtilities.GetType(extension)
            };
        }



    }
}
