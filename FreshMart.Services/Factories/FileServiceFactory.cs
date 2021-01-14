using FreshMart.Database;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using static FreshMart.Core.Constants.Constants;

namespace FreshMart.Services.Factories
{

    public class FileServiceFactory : IFileServiceFactory
    {
        private IFileService amazonS3FileService = null;
        private IFileService localFileService = null;

        private readonly IHostingEnvironment _env;
        private readonly IEncryptionServices _encryptionService;
        private readonly AppDbContext _context;

        public FileServiceFactory(IHostingEnvironment env, IEncryptionServices encryptionService, AppDbContext context)
        {
            _env = env;
            _encryptionService = encryptionService;
            _context = context;
        }


        public IFileService Create(string type)
        {
            if (type == StorageType.amazon.ToString())
            {
                if (amazonS3FileService == null)
                {
                    amazonS3FileService = new AmazonS3FileService();
                    return amazonS3FileService;
                }
                else
                {
                    return amazonS3FileService;
                }
            }
            else if (type == StorageType.local.ToString())
            {
                if (localFileService == null)
                {
                    localFileService = new LocalFileService(_env, _encryptionService, _context);
                    return localFileService;
                }
                else
                {
                    return localFileService;
                }
            }

            return null;
        }

        public IFileService Create()
        {
            if (localFileService == null)
            {
                localFileService = new LocalFileService(_env, _encryptionService, _context);
                return localFileService;
            }
            else
            {
                return localFileService;
            }
        }
    }
}
