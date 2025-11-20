using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations.DocumentManagement.Storage
{
    public class DocumentManagementBlobContainerConfiguration : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.Configure<DocumentManagementBlobContainer>(container =>
                {
                    // Use FileSystem provider for development
                    container.UseFileSystem(fileSystem =>
                    {
                        fileSystem.BasePath = "App_Data/DocumentManagement";
                    });
                    
                    // In production, you might want to use Azure Blob Storage or another provider
                    // container.UseAzure(azure =>
                    // {
                    //     azure.ConnectionString = "your connection string";
                    //     azure.ContainerName = "documents";
                    // });
                });
            });
        }
    }
}
