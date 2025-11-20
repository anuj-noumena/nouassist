using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.QualityAssurance.TestPlans.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestPlans
{
    /// <summary>
    /// Application service for managing test plans
    /// </summary>
    public interface ITestPlanAppService : IApplicationService
    {
        /// <summary>
        /// Creates a new test plan
        /// </summary>
        Task<TestPlanDto> CreateAsync(CreateTestPlanDto input);
        
        /// <summary>
        /// Updates an existing test plan
        /// </summary>
        Task<TestPlanDto> UpdateAsync(Guid id, UpdateTestPlanDto input);
        
        /// <summary>
        /// Deletes a test plan
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Gets a test plan by ID
        /// </summary>
        Task<TestPlanDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a test plan with all details including test cycles and progress
        /// </summary>
        Task<TestPlanDto> GetWithDetailsAsync(Guid id);
        
        /// <summary>
        /// Gets a paged list of test plans
        /// </summary>
        Task<PagedResultDto<TestPlanDto>> GetListAsync(GetTestPlanListDto input);
        
        /// <summary>
        /// Gets test plans for a specific release version
        /// </summary>
        Task<List<TestPlanDto>> GetByReleaseVersionAsync(string releaseVersion);
        
        /// <summary>
        /// Activates a test plan
        /// </summary>
        Task<TestPlanDto> ActivateAsync(Guid id);
        
        /// <summary>
        /// Completes a test plan
        /// </summary>
        Task<TestPlanDto> CompleteAsync(Guid id);
        
        /// <summary>
        /// Cancels a test plan
        /// </summary>
        Task<TestPlanDto> CancelAsync(Guid id, string reason);
        
        /// <summary>
        /// Archives a test plan
        /// </summary>
        Task<TestPlanDto> ArchiveAsync(Guid id);
        
        /// <summary>
        /// Links a requirement to a test plan
        /// </summary>
        Task LinkRequirementAsync(Guid id, Guid requirementId);
        
        /// <summary>
        /// Unlinks a requirement from a test plan
        /// </summary>
        Task UnlinkRequirementAsync(Guid id, Guid requirementId);
        
        /// <summary>
        /// Gets test execution progress for a test plan
        /// </summary>
        Task<TestProgressDto> GetProgressAsync(Guid id);
    }
}
