using Volo.Abp.Modularity;

namespace NouAssist.Tasks;

[DependsOn(
    typeof(TasksDomainModule),
    typeof(TasksTestBaseModule)
)]
public class TasksDomainTestModule : AbpModule
{

}
