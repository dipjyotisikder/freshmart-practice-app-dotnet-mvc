namespace FreshMart.Services.Factories
{
    public interface IFileServiceFactory
    {
        IFileService Create(string type);
    }
}

