using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using NouAssist.RequirementsManagement.ValidationVerification.Aggregates;
using NouAssist.RequirementsManagement.ValidationVerification.Entities;
using NouAssist.RequirementsManagement.ValidationVerification.ValueObjects;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using NouAssist.RequirementsManagement.ValidationVerification.Permissions;
using NouAssist.RequirementsManagement.ValidationVerification.TestCases.Dtos;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCases
{
    /// <summary>
    /// Application service for managing test cases
    /// </summary>
    [Authorize(ValidationVerificationPermissions.TestCases.Default)]
    public class TestCaseAppService : ApplicationService, ITestCaseAppService
    {
        private readonly IRepository<TestCase, Guid> _testCaseRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public TestCaseAppService(
            IRepository<TestCase, Guid> testCaseRepository,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _testCaseRepository = testCaseRepository;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(ValidationVerificationResource);
        }

        public virtual async Task<TestCaseDto> GetAsync(Guid id)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);
                return ObjectMapper.Map<TestCase, TestCaseDto>(testCase);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        public virtual async Task<PagedResultDto<TestCaseSummaryDto>> GetListAsync(GetTestCaseListDto input)
        {
            try
            {
                var queryable = await _testCaseRepository.GetQueryableAsync();

                // Apply filters
                queryable = ApplyFilters(queryable, input);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var entities = await AsyncExecuter.ToListAsync(
                    queryable.OrderBy(input.Sorting ?? nameof(TestCase.Name))
                        .PageBy(input)
                );

                return new PagedResultDto<TestCaseSummaryDto>(
                    totalCount,
                    ObjectMapper.Map<List<TestCase>, List<TestCaseSummaryDto>>(entities)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.Create)]
        public virtual async Task<TestCaseDto> CreateAsync(CreateTestCaseDto input)
        {
            try
            {
                var newId = _guidGenerator.Create();
                var currentTenantId = _currentTenant.Id;

                // Create test case using factory method
                var testCase = TestCase.Create(
                    newId,
                    input.Name,
                    input.Description,
                    input.Priority,
                    input.RequirementIds,
                    input.Preconditions,
                    input.Postconditions,
                    currentTenantId
                );

                testCase.EstimatedDuration = input.EstimatedDuration;

                // Add test steps if provided
                foreach (var stepDto in input.TestSteps)
                {
                    testCase.AddTestStep(
                        stepDto.Instruction,
                        stepDto.ExpectedResult,
                        stepDto.TestData,
                        stepDto.Prerequisites
                    );
                }

                await _testCaseRepository.InsertAsync(testCase);

                return ObjectMapper.Map<TestCase, TestCaseDto>(testCase);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.Update)]
        public virtual async Task<TestCaseDto> UpdateAsync(Guid id, UpdateTestCaseDto input)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);

                // Update properties
                testCase.Update(
                    input.Name,
                    input.Description,
                    input.Priority,
                    input.Preconditions,
                    input.Postconditions,
                    input.EstimatedDuration
                );

                await _testCaseRepository.UpdateAsync(testCase);

                return ObjectMapper.Map<TestCase, TestCaseDto>(testCase);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);

                // Only allow deletion of draft and obsolete test cases
                if (testCase.Status == TestCaseStatus.InExecution)
                {
                    throw new UserFriendlyException(L["ValidationVerification:ErrorMessage:CannotDeleteTestInExecution"]);
                }

                await _testCaseRepository.DeleteAsync(id);
            }
            catch (UserFriendlyException) { throw; }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.ManageSteps)]
        public virtual async Task<TestCaseDto> AddTestStepAsync(Guid id, CreateTestStepDto input)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);

                testCase.AddTestStep(
                    input.Instruction,
                    input.ExpectedResult,
                    input.TestData,
                    input.Prerequisites
                );

                await _testCaseRepository.UpdateAsync(testCase);

                return ObjectMapper.Map<TestCase, TestCaseDto>(testCase);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.ManageSteps)]
        public virtual async Task<TestCaseDto> UpdateTestStepAsync(Guid id, Guid stepId, UpdateTestStepDto input)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);

                testCase.UpdateTestStep(
                    stepId,
                    input.Instruction,
                    input.ExpectedResult,
                    input.TestData,
                    input.Prerequisites
                );

                await _testCaseRepository.UpdateAsync(testCase);

                return ObjectMapper.Map<TestCase, TestCaseDto>(testCase);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.ManageSteps)]
        public virtual async Task<TestCaseDto> RemoveTestStepAsync(Guid id, Guid stepId)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);

                testCase.RemoveTestStep(stepId);

                await _testCaseRepository.UpdateAsync(testCase);

                return ObjectMapper.Map<TestCase, TestCaseDto>(testCase);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.Update)]
        public virtual async Task<TestCaseDto> MarkAsReadyAsync(Guid id)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);

                testCase.MarkAsReady();

                await _testCaseRepository.UpdateAsync(testCase);

                return ObjectMapper.Map<TestCase, TestCaseDto>(testCase);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.Execute)]
        public virtual async Task<TestExecutionLogDto> StartExecutionAsync(Guid id, StartTestExecutionDto input)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);
                var currentUserId = _currentUser.Id;
                var currentTime = _clock.Now;

                if (!currentUserId.HasValue)
                {
                    throw new UserFriendlyException(L["ValidationVerification:ErrorMessage:UserNotAuthenticated"]);
                }

                var executionLog = testCase.StartExecution(currentUserId.Value, currentTime);

                // Set optional execution context
                if (!string.IsNullOrEmpty(input.Environment))
                {
                    executionLog.SetEnvironment(input.Environment);
                }

                if (!string.IsNullOrEmpty(input.TestDataUsed))
                {
                    executionLog.SetTestData(input.TestDataUsed);
                }

                await _testCaseRepository.UpdateAsync(testCase);

                return ObjectMapper.Map<TestExecutionLog, TestExecutionLogDto>(executionLog);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.Execute)]
        public virtual async Task<TestCaseDto> CompleteExecutionAsync(
            Guid id, Guid executionId, CompleteTestExecutionDto input)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);
                var currentTime = _clock.Now;

                // Convert step results to the format expected by the domain
                var stepResults = new Dictionary<Guid, TestExecutionStatus>();
                foreach (var stepResult in input.StepResults)
                {
                    stepResults[stepResult.Key] = stepResult.Value.Status;
                }

                testCase.CompleteExecution(
                    executionId,
                    input.OverallStatus,
                    currentTime,
                    stepResults,
                    input.ExecutionNotes
                );

                // Add comments to test steps if provided
                foreach (var stepResult in input.StepResults.Where(sr => !string.IsNullOrEmpty(sr.Value.Comments)))
                {
                    var step = testCase.TestSteps.FirstOrDefault(s => s.Id == stepResult.Key);
                    step?.AddComments(stepResult.Value.Comments);
                }

                await _testCaseRepository.UpdateAsync(testCase);

                return ObjectMapper.Map<TestCase, TestCaseDto>(testCase);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.Update)]
        public virtual async Task<TestCaseDto> BlockAsync(Guid id, string reason)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);

                testCase.Block(reason);

                await _testCaseRepository.UpdateAsync(testCase);

                return ObjectMapper.Map<TestCase, TestCaseDto>(testCase);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.Update)]
        public virtual async Task<TestCaseDto> MarkAsObsoleteAsync(Guid id, string reason)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);

                testCase.MarkAsObsolete(reason);

                await _testCaseRepository.UpdateAsync(testCase);

                return ObjectMapper.Map<TestCase, TestCaseDto>(testCase);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCases.ManageLinks)]
        public virtual async Task<TestCaseDto> UpdateRequirementLinksAsync(Guid id, UpdateRequirementLinksDto input)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAsync(id);

                // Clear existing links and add new ones
                foreach (var existingId in testCase.RequirementIds.ToList())
                {
                    testCase.RemoveRequirementLink(existingId);
                }

                foreach (var requirementId in input.RequirementIds)
                {
                    testCase.AddRequirementLink(requirementId);
                }

                await _testCaseRepository.UpdateAsync(testCase);

                return ObjectMapper.Map<TestCase, TestCaseDto>(testCase);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        public virtual async Task<PagedResultDto<TestCaseSummaryDto>> GetTestCasesForRequirementAsync(
            Guid requirementId, GetTestCaseListDto input)
        {
            try
            {
                var queryable = await _testCaseRepository.GetQueryableAsync();

                // Filter by requirement
                queryable = queryable.Where(tc => tc.RequirementIds.Contains(requirementId));

                // Apply other filters
                queryable = ApplyFilters(queryable, input);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var entities = await AsyncExecuter.ToListAsync(
                    queryable.OrderBy(input.Sorting ?? nameof(TestCase.Name))
                        .PageBy(input)
                );

                return new PagedResultDto<TestCaseSummaryDto>(
                    totalCount,
                    ObjectMapper.Map<List<TestCase>, List<TestCaseSummaryDto>>(entities)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        public virtual async Task<PagedResultDto<TestCaseSummaryDto>> GetTestCasesRequiringAttentionAsync(
            GetTestCaseListDto input)
        {
            try
            {
                var queryable = await _testCaseRepository.GetQueryableAsync();
                var oneWeekAgo = _clock.Now.AddDays(-7);

                // Filter test cases requiring attention
                queryable = queryable.Where(tc => 
                    tc.Status == TestCaseStatus.Blocked ||
                    (tc.Status == TestCaseStatus.Ready &&
                     tc.ExecutionLogs.Any(el => el.Status == TestExecutionStatus.Fail)) ||
                    (tc.Status == TestCaseStatus.Draft &&
                     tc.CreationTime < oneWeekAgo));

                // Apply other filters
                queryable = ApplyFilters(queryable, input);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var entities = await AsyncExecuter.ToListAsync(
                    queryable.OrderBy(input.Sorting ?? nameof(TestCase.LastModificationTime) + " desc")
                        .PageBy(input)
                );

                return new PagedResultDto<TestCaseSummaryDto>(
                    totalCount,
                    ObjectMapper.Map<List<TestCase>, List<TestCaseSummaryDto>>(entities)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        // Helper methods

        private IQueryable<TestCase> ApplyFilters(IQueryable<TestCase> queryable, GetTestCaseListDto input)
        {
            if (!input.Filter.IsNullOrWhiteSpace())
            {
                queryable = queryable.Where(tc => 
                    tc.Name.Contains(input.Filter) || tc.Description.Contains(input.Filter));
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(tc => tc.Status == input.Status.Value);
            }

            if (input.Priority.HasValue)
            {
                queryable = queryable.Where(tc => tc.Priority == input.Priority.Value);
            }

            if (input.RequirementId.HasValue)
            {
                queryable = queryable.Where(tc => tc.RequirementIds.Contains(input.RequirementId.Value));
            }

            if (input.LastExecutionStatus.HasValue)
            {
                queryable = queryable.Where(tc => 
                    tc.ExecutionLogs.Any(el => el.Status == input.LastExecutionStatus.Value));
            }

            if (input.RequiresAttention.HasValue && input.RequiresAttention.Value)
            {
                var oneWeekAgo = _clock.Now.AddDays(-7);
                queryable = queryable.Where(tc => 
                    tc.Status == TestCaseStatus.Blocked ||
                    (tc.Status == TestCaseStatus.Ready &&
                     tc.ExecutionLogs.Any(el => el.Status == TestExecutionStatus.Fail)) ||
                    (tc.Status == TestCaseStatus.Draft &&
                     tc.CreationTime < oneWeekAgo));
            }

            if (input.CreatedAfter.HasValue)
            {
                queryable = queryable.Where(tc => tc.CreationTime >= input.CreatedAfter.Value);
            }

            if (input.CreatedBefore.HasValue)
            {
                queryable = queryable.Where(tc => tc.CreationTime <= input.CreatedBefore.Value);
            }

            return queryable;
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in TestCaseAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
