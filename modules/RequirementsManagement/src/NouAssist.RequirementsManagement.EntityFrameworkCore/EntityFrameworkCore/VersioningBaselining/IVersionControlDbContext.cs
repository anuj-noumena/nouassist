using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore
{
    /// <summary>
    /// DbContext interface for the VersionControl module
    /// </summary>
    [ConnectionStringName(VersionControlDbProperties.ConnectionStringName)]
    public interface IVersionControlDbContext : IEfCoreDbContext
    {
        /// <summary>
        /// DbSet for VersionedRequirementHistory aggregate
        /// </summary>
        DbSet<VersionedRequirementHistory> VersionedRequirementHistories { get; }
        
        /// <summary>
        /// DbSet for BaselineDefinition aggregate
        /// </summary>
        DbSet<BaselineDefinition> BaselineDefinitions { get; }
        
        /// <summary>
        /// DbSet for RequirementBranch aggregate
        /// </summary>
        DbSet<RequirementBranch> RequirementBranches { get; }
    }
}
