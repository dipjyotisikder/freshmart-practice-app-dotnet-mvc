using System.Collections.Generic;
using static FreshMart.Core.Constants.Constants;

namespace FreshMart.Services
{

    public class FileServiceFactory : IFileServiceFactory
    {
        IFileService photoFileService = null;
        IFileService docFileService = null;
        IFileService videoFileService = null;

        public IFileService Create(string extension)
        {
            var type = GetType(extension);

            if (type == FileTypes.photo)
            {
                if (photoFileService == null)
                {
                    photoFileService = new PhotoFileService();
                    return photoFileService;
                }
                else
                {
                    return photoFileService;
                }
            }
            else if (type == FileTypes.docs)
            {
                if (docFileService == null)
                {
                    docFileService = new DocFileService();
                    return docFileService;
                }
                else
                {
                    return docFileService;
                }
            }
            else if (type == FileTypes.video)
            {
                if (videoFileService == null)
                {
                    videoFileService = new DocFileService();
                    return videoFileService;
                }
                else
                {
                    return videoFileService;
                }
            }

            return null;
        }



        public FileTypes GetType(string extension)
        {
            return GetMimeTypes()[extension];
        }



        private Dictionary<string, FileTypes> GetMimeTypes()
        {
            return new Dictionary<string, FileTypes>
            {
                {".txt", FileTypes.docs},
                {".pdf", FileTypes.docs},
                {".doc", FileTypes.docs},
                {".docx", FileTypes.docs},
                {".xls", FileTypes.docs},
                {".xlsx", FileTypes.docs},
                {".zip", FileTypes.docs},
                {".png", FileTypes.photo},
                {".jpg", FileTypes.photo},
                {".jpeg",FileTypes.photo},
                {".gif", FileTypes.photo},
                {".csv", FileTypes.docs}
            };
        }
    }
}
