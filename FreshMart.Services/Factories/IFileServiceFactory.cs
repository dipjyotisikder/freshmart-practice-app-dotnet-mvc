using static FreshMart.Core.Constants.Constants;

namespace FreshMart.Services
{
    public interface IFileServiceFactory
    {
        IFileService Create(string ext);
    }
}
