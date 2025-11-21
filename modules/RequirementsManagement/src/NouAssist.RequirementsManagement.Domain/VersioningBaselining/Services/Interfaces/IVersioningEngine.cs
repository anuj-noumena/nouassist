using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Entities;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Services.Interfaces
{
    /// <summary>
    /// Interface for the versioning engine domain service
    /// </summary>
    public interface IVersioningEngine : IDomainService
    {
        /// <summary>
        /// Gets the version history for a requirement, creating it if it doesn't exist
        /// </summary>
        Task<VersionedRequirementHistory> GetOrCreateRequirementHistoryAsync(
            Guid requirementId,
            Guid? tenantId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates a new version of a requirement
        /// </summary>
        Task<RequirementVersionRecord> CreateVersionAsync(
            Guid requirementId,
            string contentSnapshot,
            string changeComment,
            Guid authorId,
            string authorName,
            VersionChangeType changeType,
            DateTime? creationTime = null,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a specific version of a requirement
        /// </summary>
        Task<RequirementVersionRecord> GetVersionAsync(
            Guid requirementId,
            string version,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets the latest version of a requirement
        /// </summary>
        Task<RequirementVersionRecord> GetLatestVersionAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets all versions of a requirement
        /// </summary>
        Task<List<RequirementVersionRecord>> GetVersionHistoryAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Compares two versions of a requirement
        /// </summary>
        Task<VersionComparisonResult> CompareVersionsAsync(
            Guid requirementId,
            string firstVersion,
            string secondVersion,
            CancellationToken cancellationToken = default);
    }
    
    /// <summary>
    /// Enum representing the type of version change
    /// </summary>
    public enum VersionChangeType
    {
        /// <summary>
        /// A patch level change (e.g., 1.0.0 -> 1.0.1)
        /// </summary>
        Patch = 0,
        
        /// <summary>
        /// A minor level change (e.g., 1.0.0 -> 1.1.0)
        /// </summary>
        Minor = 1,
        
        /// <summary>
        /// A major level change (e.g., 1.0.0 -> 2.0.0)
        /// </summary>
        Major = 2
    }
    
    /// <summary>
    /// Result of comparing two requirement versions
    /// </summary>
    public class VersionComparisonResult
    {
        /// <summary>
        /// First version being compared
        /// </summary>
        public string FirstVersion { get; set; }
        
        /// <summary>
        /// Second version being compared
        /// </summary>
        public string SecondVersion { get; set; }
        
        /// <summary>
        /// Flag indicating if the versions are identical
        /// </summary>
        public bool AreIdentical { get; set; }
        
        /// <summary>
        /// Structured representation of differences between versions
        /// </summary>
        public object Differences { get; set; }
    }
}
