using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.VersioningBaselining.Baselining.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Baselining
{
    /// <summary>
    /// Application service for managing baselines
    /// </summary>
    public interface IBaseliningAppService : IApplicationService
    {
        /// <summary>
        /// Creates a new baseline
        /// </summary>
        Task<BaselineDto> CreateAsync(CreateBaselineDto input);
        
        /// <summary>
        /// Updates an existing baseline
        /// </summary>
        Task<BaselineDto> UpdateAsync(Guid id, UpdateBaselineDto input);
        
        /// <summary>
        /// Gets a baseline by ID
        /// </summary>
        Task<BaselineDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a list of baselines
        /// </summary>
        Task<PagedResultDto<BaselineDto>> GetListAsync(GetBaselineListDto input);
        
        /// <summary>
        /// Deletes a baseline
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Adds a requirement to a baseline
        /// </summary>
        Task<BaselineDto> AddRequirementAsync(Guid id, AddRequirementToBaselineDto input);
        
        /// <summary>
        /// Removes a requirement from a baseline
        /// </summary>
        Task<BaselineDto> RemoveRequirementAsync(Guid id, Guid requirementId);
        
        /// <summary>
        /// Updates a requirement's version in a baseline
        /// </summary>
        Task<BaselineDto> UpdateRequirementAsync(Guid id, UpdateRequirementInBaselineDto input);
        
        /// <summary>
        /// Activates a baseline
        /// </summary>
        Task<BaselineDto> ActivateAsync(Guid id);
        
        /// <summary>
        /// Archives a baseline
        /// </summary>
        Task<BaselineDto> ArchiveAsync(Guid id);
    }
}
