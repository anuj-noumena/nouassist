using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore
{
    /// <summary>
    /// DbContext implementation for the VersionControl module
    /// </summary>
    [ConnectionStringName(VersionControlDbProperties.ConnectionStringName)]
    public class VersionControlDbContext : 
        AbpDbContext<VersionControlDbContext>, IVersionControlDbContext
    {
        /// <summary>
        /// DbSet for VersionedRequirementHistory aggregate
        /// </summary>
        public DbSet<VersionedRequirementHistory> VersionedRequirementHistories { get; set; }
        
        /// <summary>
        /// DbSet for BaselineDefinition aggregate
        /// </summary>
        public DbSet<BaselineDefinition> BaselineDefinitions { get; set; }
        
        /// <summary>
        /// DbSet for RequirementBranch aggregate
        /// </summary>
        public DbSet<RequirementBranch> RequirementBranches { get; set; }

        public VersionControlDbContext(DbContextOptions<VersionControlDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply configurations for the VersionControl module
            builder.ConfigureVersionControl();
        }
    }
}
