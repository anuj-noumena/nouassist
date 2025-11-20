using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.QualityAssurance.TestCycles.Dtos;
using NouAssist.RequirementsManagement.QualityAssurance.TestPlans.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestCycles
{
    /// <summary>
    /// Application service for managing test cycles
    /// </summary>
    public interface ITestCycleAppService : IApplicationService
    {
        /// <summary>
        /// Creates a new test cycle
        /// </summary>
        Task<TestCycleDto> CreateAsync(CreateTestCycleDto input);
        
        /// <summary>
        /// Updates an existing test cycle
        /// </summary>
        Task<TestCycleDto> UpdateAsync(Guid id, UpdateTestCycleDto input);
        
        /// <summary>
        /// Deletes a test cycle
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Gets a test cycle by ID
        /// </summary>
        Task<TestCycleDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a test cycle with all details including test runs
        /// </summary>
        Task<TestCycleDto> GetWithDetailsAsync(Guid id);
        
        /// <summary>
        /// Gets a paged list of test cycles
        /// </summary>
        Task<PagedResultDto<TestCycleDto>> GetListAsync(GetTestCycleListDto input);
        
        /// <summary>
        /// Gets test cycles for a specific test plan
        /// </summary>
        Task<List<TestCycleDto>> GetByTestPlanIdAsync(Guid testPlanId);
        
        /// <summary>
        /// Gets active test cycles assigned to the current user
        /// </summary>
        Task<List<TestCycleDto>> GetMyActiveTestCyclesAsync();
        
        /// <summary>
        /// Starts a test cycle
        /// </summary>
        Task<TestCycleDto> StartAsync(Guid id);
        
        /// <summary>
        /// Completes a test cycle
        /// </summary>
        Task<TestCycleDto> CompleteAsync(Guid id);
        
        /// <summary>
        /// Pauses a test cycle
        /// </summary>
        Task<TestCycleDto> PauseAsync(Guid id);
        
        /// <summary>
        /// Resumes a test cycle
        /// </summary>
        Task<TestCycleDto> ResumeAsync(Guid id);
        
        /// <summary>
        /// Cancels a test cycle
        /// </summary>
        Task<TestCycleDto> CancelAsync(Guid id, string reason);
        
        /// <summary>
        /// Gets test execution progress for a test cycle
        /// </summary>
        Task<TestProgressDto> GetProgressAsync(Guid id);
    }
}
