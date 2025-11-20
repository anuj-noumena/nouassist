using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.ChangeManagement.EntityFrameworkCore
{
    /// <summary>
    /// DbContext implementation for Change Management module
    /// </summary>
    [ConnectionStringName(ChangeManagementDbProperties.ConnectionStringName)]
    public class ChangeManagementDbContext 
        : AbpDbContext<ChangeManagementDbContext>, IChangeManagementDbContext
    {
        public DbSet<RequirementChangeRequest> ChangeRequests { get; set; }
        public DbSet<ChangeRationale> ChangeRationales { get; set; }
        public DbSet<ChangeImpactStatement> ChangeImpactStatements { get; set; }
        public DbSet<ApprovalStep> ApprovalSteps { get; set; }

        public ChangeManagementDbContext(DbContextOptions<ChangeManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call base method first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations for Change Management module
            builder.ConfigureChangeManagement(this);
        }
    }
}
