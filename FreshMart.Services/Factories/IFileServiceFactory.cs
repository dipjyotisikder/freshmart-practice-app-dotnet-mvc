namespace FreshMart.Services.Factories
{
    public interface IFileServiceFactory
    {
        IFileService Create();
        IFileService Create(string type);
    }
}

