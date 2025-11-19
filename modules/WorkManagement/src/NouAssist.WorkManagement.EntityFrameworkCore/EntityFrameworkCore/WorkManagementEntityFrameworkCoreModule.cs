using Microsoft.Extensions.DependencyInjection;
using NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.EntityFrameworkCore;

[DependsOn(
    typeof(WorkManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class WorkManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<WorkManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IWorkManagementDbContext>(includeAllEntities: true);

            // Task aggregate repositories
            options.AddRepository<TaskManagement.Aggregates.Task.TaskAggregate, TaskManagement.Repositories.EfCoreTaskRepository>();
            options.AddRepository<TaskManagement.Aggregates.Task.TaskDefinitionAggregate, TaskManagement.Repositories.EfCoreTaskDefinitionRepository>();
            options.AddRepository<TaskManagement.Aggregates.Task.TaskDefaultsAggregate, TaskManagement.Repositories.EfCoreTaskDefaultsRepository>();
            options.AddRepository<TaskManagement.Aggregates.Task.TaskRecurrenceAggregate, TaskManagement.Repositories.EfCoreTaskRecurrenceRepository>();
            options.AddRepository<TaskManagement.Aggregates.Task.TaskHierarchyAggregate, TaskManagement.Repositories.EfCoreTaskHierarchyRepository>();

            // TaskInstance aggregate repositories
            options.AddRepository<TaskManagement.Aggregates.TaskInstance.TaskInstance, TaskManagement.Repositories.EfCoreTaskInstanceRepository>();
            options.AddRepository<TaskManagement.Aggregates.TaskInstance.TaskChecklistAggregate, TaskManagement.Repositories.EfCoreTaskChecklistRepository>();
            options.AddRepository<TaskManagement.Aggregates.TaskInstance.TaskOrganizationAggregate, TaskManagement.Repositories.EfCoreTaskOrganizationRepository>();
            options.AddRepository<TaskManagement.Aggregates.TaskInstance.TaskInstanceWorkAggregate, TaskManagement.Repositories.EfCoreTaskInstanceWorkRepository>();
            options.AddRepository<TaskManagement.Aggregates.TaskInstance.TaskCollaborationAggregate, TaskManagement.Repositories.EfCoreTaskCollaborationRepository>();
            options.AddRepository<TaskManagement.Aggregates.TaskInstance.TaskFlowControlAggregate, TaskManagement.Repositories.EfCoreTaskFlowControlRepository>();
        });
    }
}
