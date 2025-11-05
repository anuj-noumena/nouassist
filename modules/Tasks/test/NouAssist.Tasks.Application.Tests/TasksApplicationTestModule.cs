using Volo.Abp.Modularity;

namespace NouAssist.Tasks;

[DependsOn(
    typeof(TasksApplicationModule),
    typeof(TasksDomainTestModule)
    )]
public class TasksApplicationTestModule : AbpModule
{

}
