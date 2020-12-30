using System;
using System.Collections.Generic;
using System.Text;

namespace FreshMart.Core.Constants
{
    public static class Constants
    {
        public enum FileType
        {
            Photo = 1,
            Doc,
            Video
        }


        public enum StorageType
        {
            local = 1,
            amazon
        }
    }
}
