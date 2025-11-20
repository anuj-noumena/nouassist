using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Specification.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore
{
    [ConnectionStringName(RequirementSpecificationManagementDbProperties.ConnectionStringName)]
    public class RequirementSpecificationManagementDbContext 
        : AbpDbContext<RequirementSpecificationManagementDbContext>, IRequirementSpecificationManagementDbContext
    {
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<SpecificationStructure> SpecificationStructures { get; set; }

        public RequirementSpecificationManagementDbContext(DbContextOptions<RequirementSpecificationManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureRequirementSpecificationManagement(this);
        }
    }
}
