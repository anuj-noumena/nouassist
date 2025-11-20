using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.ChangeManagement.EntityFrameworkCore
{
    /// <summary>
    /// DbContext interface for Change Management module
    /// </summary>
    [ConnectionStringName(ChangeManagementDbProperties.ConnectionStringName)]
    public interface IChangeManagementDbContext : IEfCoreDbContext
    {
        /// <summary>
        /// DbSet for RequirementChangeRequest aggregates
        /// </summary>
        DbSet<RequirementChangeRequest> ChangeRequests { get; }

        /// <summary>
        /// DbSet for ChangeRationale entities
        /// </summary>
        DbSet<ChangeRationale> ChangeRationales { get; }

        /// <summary>
        /// DbSet for ChangeImpactStatement entities
        /// </summary>
        DbSet<ChangeImpactStatement> ChangeImpactStatements { get; }

        /// <summary>
        /// DbSet for ApprovalStep entities
        /// </summary>
        DbSet<ApprovalStep> ApprovalSteps { get; }
    }
}
