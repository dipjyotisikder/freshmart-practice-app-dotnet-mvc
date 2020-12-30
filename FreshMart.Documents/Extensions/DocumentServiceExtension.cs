using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FreshMart.Documents.Extensions
{
    public static class DocumentServiceExtension
    {
        public static IServiceCollection AddDocumentServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<IFileServiceFactory, FileServiceFactory>();
            return services;
        }
    }
}
