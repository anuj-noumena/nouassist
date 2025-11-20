using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Specification.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore
{
    [ConnectionStringName(RequirementSpecificationManagementDbProperties.ConnectionStringName)]
    public interface IRequirementSpecificationManagementDbContext : IEfCoreDbContext
    {
        DbSet<Requirement> Requirements { get; }
        DbSet<SpecificationStructure> SpecificationStructures { get; }
    }
}
