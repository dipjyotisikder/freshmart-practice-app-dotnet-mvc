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
            string extension = Path.GetExtension(file.FileName);


            //if (extension.ToString() != ".jpg" || extension.ToString() != ".JPG" || extension.ToString() != ".jpeg" || extension.ToString() != ".JPEG")
            //{
            //    return null;
            //}

            string path_Root = _env.WebRootPath;

            string path_to_Images = path_Root + "\\Assets\\images\\products\\" + file.FileName;

            using (var stream = new FileStream(path_to_Images, FileMode.Create))
            {

                file.CopyTo(stream);
                string revUrl = Reverse.reverse(path_to_Images);
                int count = 0;
                int flag = 0;

                for (int i = 0; i < revUrl.Length; i++)
                {
                    if (revUrl[i] == '\\')
                    {
                        count++;

                    }
                    if (count == 4)
                    {
                        flag = i;
                        break;
                    }
                }

                string sub = revUrl.Substring(0, flag + 1);
                string finalString = Reverse.reverse(sub);

                string f = finalString.Replace("\\", "/");
                imgString = f;
            }

            return imgString;


        }

    }
}
