using Microsoft.Extensions.DependencyInjection;
using NouAssist.BusinessOperations.DocumentManagement.EntityFrameworkCore;
using NouAssist.BusinessOperations.DocumentManagement.Interfaces.Services;
using NouAssist.BusinessOperations.DocumentManagement.Storage;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations.DocumentManagement
{
    [DependsOn(
        typeof(DocumentManagementDomainModule),
        typeof(DocumentManagementEntityFrameworkCoreModule),
        typeof(AbpBlobStoringFileSystemModule)
    )]
    public class DocumentManagementInfrastructureModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the domain storage service implementation
            context.Services.AddTransient<IDocumentStorageService, BlobDocumentStorageService>();
            
            // Configure blob storage
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseFileSystem(fileSystem =>
                    {
                        fileSystem.BasePath = "App_Data/DocumentManagement";
                    });
                });
            });
        }
    }
}
