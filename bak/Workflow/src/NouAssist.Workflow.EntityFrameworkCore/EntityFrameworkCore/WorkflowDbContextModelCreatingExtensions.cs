using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.Workflow.EntityFrameworkCore;

public static class WorkflowDbContextModelCreatingExtensions
{
    public static void ConfigureWorkflow(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ApplyConfigurationsFromAssembly(typeof(WorkflowDbContext).Assembly);
    }
}
