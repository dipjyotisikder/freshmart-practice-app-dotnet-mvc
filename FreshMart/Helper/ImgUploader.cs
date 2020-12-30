using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Helper
{
    public class ImgUploader
    {


        public string imgString;
        private readonly IHostingEnvironment _env;

        public ImgUploader(IHostingEnvironment environment)
        {
            _env = environment;
        }

        public string ImageUrl(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            string path_Root = _env.ContentRootPath;

            string path_dir = $"/StaticFiles/Images/{file.FileName}";

            string path_to_Images = $"{path_Root}{path_dir}";

            using (var stream = new FileStream(path_to_Images, FileMode.Create))
            {
                file.CopyTo(stream);
                stream.Dispose();
            }
            return path_dir;
        }

    }
}
