using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.TaskManagement;

[DependsOn(
    typeof(TaskManagementDomainModule),
    typeof(TaskManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class TaskManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<TaskManagementApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<TaskManagementApplicationModule>(validate: true);
        });
    }
}
