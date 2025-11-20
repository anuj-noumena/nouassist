using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using NouAssist.RequirementsManagement.ValidationVerification.TestCases.Dtos;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCases
{
    /// <summary>
    /// Application service interface for managing test cases
    /// </summary>
    public interface ITestCaseAppService : IApplicationService
    {
        /// <summary>
        /// Gets a test case by ID
        /// </summary>
        Task<TestCaseDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a paged list of test cases
        /// </summary>
        Task<PagedResultDto<TestCaseSummaryDto>> GetListAsync(GetTestCaseListDto input);

        /// <summary>
        /// Creates a new test case
        /// </summary>
        Task<TestCaseDto> CreateAsync(CreateTestCaseDto input);

        /// <summary>
        /// Updates an existing test case
        /// </summary>
        Task<TestCaseDto> UpdateAsync(Guid id, UpdateTestCaseDto input);

        /// <summary>
        /// Deletes a test case
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Adds a test step to a test case
        /// </summary>
        Task<TestCaseDto> AddTestStepAsync(Guid id, CreateTestStepDto input);

        /// <summary>
        /// Updates a test step
        /// </summary>
        Task<TestCaseDto> UpdateTestStepAsync(Guid id, Guid stepId, UpdateTestStepDto input);

        /// <summary>
        /// Removes a test step from a test case
        /// </summary>
        Task<TestCaseDto> RemoveTestStepAsync(Guid id, Guid stepId);

        /// <summary>
        /// Marks a test case as ready for execution
        /// </summary>
        Task<TestCaseDto> MarkAsReadyAsync(Guid id);

        /// <summary>
        /// Starts test execution
        /// </summary>
        Task<TestExecutionLogDto> StartExecutionAsync(Guid id, StartTestExecutionDto input);

        /// <summary>
        /// Completes test execution
        /// </summary>
        Task<TestCaseDto> CompleteExecutionAsync(Guid id, Guid executionId, CompleteTestExecutionDto input);

        /// <summary>
        /// Blocks a test case
        /// </summary>
        Task<TestCaseDto> BlockAsync(Guid id, string reason);

        /// <summary>
        /// Marks a test case as obsolete
        /// </summary>
        Task<TestCaseDto> MarkAsObsoleteAsync(Guid id, string reason);

        /// <summary>
        /// Updates requirement links
        /// </summary>
        Task<TestCaseDto> UpdateRequirementLinksAsync(Guid id, UpdateRequirementLinksDto input);

        /// <summary>
        /// Gets test cases for a specific requirement
        /// </summary>
        Task<PagedResultDto<TestCaseSummaryDto>> GetTestCasesForRequirementAsync(
            Guid requirementId, GetTestCaseListDto input);

        /// <summary>
        /// Gets test cases requiring attention
        /// </summary>
        Task<PagedResultDto<TestCaseSummaryDto>> GetTestCasesRequiringAttentionAsync(GetTestCaseListDto input);
    }
}
