using System.Collections.Generic;
using static FreshMart.Core.Constants.Constants;

namespace FreshMart.Core.Utilities
{
    public static class FileUtilities
    {
        public static FileType GetType(string extension)
        {
            return GetMimeTypes()[extension];
        }

        private static Dictionary<string, FileType> GetMimeTypes()
        {
            return new Dictionary<string, FileType>
            {
                {".txt", FileType.Doc},
                {".pdf", FileType.Doc},
                {".doc", FileType.Doc},
                {".docx", FileType.Doc},
                {".xls", FileType.Doc},
                {".xlsx", FileType.Doc},
                {".zip", FileType.Doc},
                {".png", FileType.Photo},
                {".jpg", FileType.Photo},
                {".jpeg",FileType.Photo},
                {".gif", FileType.Photo},
                {".csv", FileType.Doc}
            };
        }
    }
}
