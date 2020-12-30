using FreshMart.Core.Infrastructure;
using static FreshMart.Core.Constants.Constants;

namespace FreshMart.Models
{
    public class Document : BaseEntity
    {
        public string Name { get; set; }

        public string OriginalName { get; set; }

        public string Extension { get; set; }

        public string Path { get; set; }

        public bool IsSaved { get; set; }

        public FileType Type { get; set; }
    }
}
