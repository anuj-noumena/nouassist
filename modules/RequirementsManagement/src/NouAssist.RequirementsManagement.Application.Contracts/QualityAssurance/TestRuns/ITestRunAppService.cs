using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.QualityAssurance.TestRuns.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestRuns
{
    /// <summary>
    /// Application service for managing test runs
    /// </summary>
    public interface ITestRunAppService : IApplicationService
    {
        /// <summary>
        /// Adds a test run to a test cycle
        /// </summary>
        Task<TestRunDto> AddAsync(AddTestRunDto input);
        
        /// <summary>
        /// Records a test result
        /// </summary>
        Task<TestRunDto> RecordResultAsync(RecordTestResultDto input);
        
        /// <summary>
        /// Marks a test run as in progress
        /// </summary>
        Task<TestRunDto> SetInProgressAsync(Guid testCycleId, Guid testCaseId);
        
        /// <summary>
        /// Resets a test run back to not run state
        /// </summary>
        Task<TestRunDto> ResetAsync(Guid testCycleId, Guid testCaseId);
        
        /// <summary>
        /// Gets a test run by its test cycle ID and test case ID
        /// </summary>
        Task<TestRunDto> GetAsync(Guid testCycleId, Guid testCaseId);
        
        /// <summary>
        /// Gets a paged list of test runs
        /// </summary>
        Task<PagedResultDto<TestRunDto>> GetListAsync(GetTestRunListDto input);
        
        /// <summary>
        /// Gets all test runs for a test cycle
        /// </summary>
        Task<List<TestRunDto>> GetByTestCycleIdAsync(Guid testCycleId);
        
        /// <summary>
        /// Gets test runs by result type
        /// </summary>
        Task<List<TestRunDto>> GetByResultAsync(Guid testCycleId, TestRunResult result);
    }
}
