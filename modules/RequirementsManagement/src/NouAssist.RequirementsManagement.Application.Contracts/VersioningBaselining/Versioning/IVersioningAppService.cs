using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.VersioningBaselining.Versioning.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Versioning
{
    /// <summary>
    /// Application service for managing requirement versions
    /// </summary>
    public interface IVersioningAppService : IApplicationService
    {
        /// <summary>
        /// Creates a new version of a requirement
        /// </summary>
        Task<VersionDto> CreateVersionAsync(CreateVersionDto input);
        
        /// <summary>
        /// Gets a specific version of a requirement
        /// </summary>
        Task<VersionDto> GetVersionAsync(Guid requirementId, string version);
        
        /// <summary>
        /// Gets the latest version of a requirement
        /// </summary>
        Task<VersionDto> GetLatestVersionAsync(Guid requirementId);
        
        /// <summary>
        /// Gets the version history of a requirement
        /// </summary>
        Task<VersionHistoryDto> GetVersionHistoryAsync(GetVersionHistoryDto input);
        
        /// <summary>
        /// Compares two versions of a requirement
        /// </summary>
        Task<VersionComparisonResultDto> CompareVersionsAsync(CompareVersionsDto input);
    }
}
