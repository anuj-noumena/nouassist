using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.QualityAssurance.Entities;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.Localization;
using NouAssist.RequirementsManagement.QualityAssurance.Permissions;
using NouAssist.RequirementsManagement.QualityAssurance.Repositories;
using NouAssist.RequirementsManagement.QualityAssurance.Services;
using NouAssist.RequirementsManagement.QualityAssurance.TestRuns.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestRuns
{
    [Authorize(QAPermissions.TestCycle.ManageTestRuns)]
    public class TestRunAppService : ApplicationService, ITestRunAppService
    {
        private readonly ITestCycleRepository _testCycleRepository;
        private readonly ITestExecutionManager _testExecutionManager;
        private readonly ILogger<TestRunAppService> _logger;
        private readonly ICurrentUser _currentUser;

        public TestRunAppService(
            ITestCycleRepository testCycleRepository,
            ITestExecutionManager testExecutionManager,
            ILogger<TestRunAppService> logger,
            ICurrentUser currentUser)
        {
            _testCycleRepository = testCycleRepository;
            _testExecutionManager = testExecutionManager;
            _logger = logger;
            _currentUser = currentUser;
            LocalizationResource = typeof(QAResource);
        }

        public virtual async Task<TestRunDto> AddAsync(AddTestRunDto input)
        {
            try
            {
                var testRun = await _testExecutionManager.AddTestCaseAsync(
                    input.TestCycleId,
                    input.TestCaseId,
                    input.TestCaseTitle,
                    input.ExpectedOutcome);
                
                return ObjectMapper.Map<TestRun, TestRunDto>(testRun);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.RecordResults)]
        public virtual async Task<TestRunDto> RecordResultAsync(RecordTestResultDto input)
        {
            try
            {
                await _testExecutionManager.RecordTestResultAsync(
                    input.TestCycleId,
                    input.TestCaseId,
                    input.Result,
                    input.ActualOutcomeDescription,
                    input.ActualOutcomeLogData);
                
                var testCycle = await _testCycleRepository.GetWithDetailsAsync(input.TestCycleId);
                var testRun = testCycle.TestRuns.FirstOrDefault(tr => tr.TestCaseId == input.TestCaseId);
                
                if (testRun == null)
                {
                    throw new UserFriendlyException(L["TestRun:NotFound", input.TestCaseId]);
                }
                
                return ObjectMapper.Map<TestRun, TestRunDto>(testRun);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.RecordResults)]
        public virtual async Task<TestRunDto> SetInProgressAsync(Guid testCycleId, Guid testCaseId)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetWithDetailsAsync(testCycleId);
                var testRun = testCycle.TestRuns.FirstOrDefault(tr => tr.TestCaseId == testCaseId);
                
                if (testRun == null)
                {
                    throw new UserFriendlyException(L["TestRun:NotFound", testCaseId]);
                }
                
                testRun.SetInProgress(_currentUser.Id.Value);
                await _testCycleRepository.UpdateAsync(testCycle);
                
                return ObjectMapper.Map<TestRun, TestRunDto>(testRun);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.RecordResults)]
        public virtual async Task<TestRunDto> ResetAsync(Guid testCycleId, Guid testCaseId)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetWithDetailsAsync(testCycleId);
                var testRun = testCycle.TestRuns.FirstOrDefault(tr => tr.TestCaseId == testCaseId);
                
                if (testRun == null)
                {
                    throw new UserFriendlyException(L["TestRun:NotFound", testCaseId]);
                }
                
                testRun.Reset();
                await _testCycleRepository.UpdateAsync(testCycle);
                
                return ObjectMapper.Map<TestRun, TestRunDto>(testRun);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.View)]
        public virtual async Task<TestRunDto> GetAsync(Guid testCycleId, Guid testCaseId)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetWithDetailsAsync(testCycleId);
                var testRun = testCycle.TestRuns.FirstOrDefault(tr => tr.TestCaseId == testCaseId);
                
                if (testRun == null)
                {
                    throw new UserFriendlyException(L["TestRun:NotFound", testCaseId]);
                }
                
                return ObjectMapper.Map<TestRun, TestRunDto>(testRun);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.View)]
        public virtual async Task<PagedResultDto<TestRunDto>> GetListAsync(GetTestRunListDto input)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetWithDetailsAsync(input.TestCycleId);
                
                // Filter test runs
                var filteredTestRuns = testCycle.TestRuns
                    .WhereIf(input.Result.HasValue, tr => tr.Result == input.Result.Value)
                    .WhereIf(input.ExecutorId.HasValue, tr => tr.ExecutorId == input.ExecutorId.Value)
                    .ToList();
                
                // Get total count
                var totalCount = filteredTestRuns.Count;
                
                // Apply sorting and paging
                var sortedTestRuns = filteredTestRuns;
                if (!input.Sorting.IsNullOrWhiteSpace())
                {
                    sortedTestRuns = input.Sorting.Contains("TestCaseTitle", StringComparison.OrdinalIgnoreCase)
                        ? input.Sorting.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                            ? filteredTestRuns.OrderByDescending(tr => tr.TestCaseTitle).ToList()
                            : filteredTestRuns.OrderBy(tr => tr.TestCaseTitle).ToList()
                        : input.Sorting.Contains("Result", StringComparison.OrdinalIgnoreCase)
                            ? input.Sorting.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                                ? filteredTestRuns.OrderByDescending(tr => tr.Result).ToList()
                                : filteredTestRuns.OrderBy(tr => tr.Result).ToList()
                            : filteredTestRuns;
                }
                
                var pagedTestRuns = sortedTestRuns
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();
                
                // Map to DTOs
                var testRunDtos = ObjectMapper.Map<List<TestRun>, List<TestRunDto>>(pagedTestRuns);
                
                return new PagedResultDto<TestRunDto>(totalCount, testRunDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.View)]
        public virtual async Task<List<TestRunDto>> GetByTestCycleIdAsync(Guid testCycleId)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetWithDetailsAsync(testCycleId);
                return ObjectMapper.Map<List<TestRun>, List<TestRunDto>>(testCycle.TestRuns.ToList());
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.View)]
        public virtual async Task<List<TestRunDto>> GetByResultAsync(Guid testCycleId, TestRunResult result)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetWithDetailsAsync(testCycleId);
                var testRuns = testCycle.TestRuns.Where(tr => tr.Result == result).ToList();
                return ObjectMapper.Map<List<TestRun>, List<TestRunDto>>(testRuns);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "Error in TestRunAppService: {ErrorMessage}", exception.Message);
            
            if (exception is BusinessException businessException)
            {
                throw new UserFriendlyException(
                    L[businessException.Code ?? "Error:UnknownBusinessError"], 
                    businessException.Code, 
                    businessException.Data);
            }
            
            return Task.CompletedTask;
        }
    }
}
