using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Permissions;
using NouAssist.RequirementsManagement.QualityAssurance.Repositories;
using NouAssist.RequirementsManagement.QualityAssurance.Services;
using NouAssist.RequirementsManagement.QualityAssurance.TestCycles.Dtos;
using NouAssist.RequirementsManagement.QualityAssurance.TestPlans.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestCycles
{
    [Authorize(QAPermissions.TestCycle.Default)]
    public class TestCycleAppService : ApplicationService, ITestCycleAppService
    {
        private readonly ITestCycleRepository _testCycleRepository;
        private readonly ITestExecutionManager _testExecutionManager;
        private readonly ILogger<TestCycleAppService> _logger;
        private readonly ICurrentUser _currentUser;

        public TestCycleAppService(
            ITestCycleRepository testCycleRepository,
            ITestExecutionManager testExecutionManager,
            ILogger<TestCycleAppService> logger,
            ICurrentUser currentUser)
        {
            _testCycleRepository = testCycleRepository;
            _testExecutionManager = testExecutionManager;
            _logger = logger;
            _currentUser = currentUser;
            LocalizationResource = typeof(QAResource);
        }

        [Authorize(QAPermissions.TestCycle.Create)]
        public virtual async Task<TestCycleDto> CreateAsync(CreateTestCycleDto input)
        {
            try
            {
                var testCycle = await _testExecutionManager.CreateTestCycleAsync(
                    input.TestPlanId,
                    input.Name,
                    input.Description,
                    input.BuildInformation,
                    input.StartDate,
                    input.EndDate,
                    input.AssigneeId);
                
                return ObjectMapper.Map<TestCycle, TestCycleDto>(testCycle);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.Update)]
        public virtual async Task<TestCycleDto> UpdateAsync(Guid id, UpdateTestCycleDto input)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetAsync(id);
                
                testCycle.UpdateBasicInfo(
                    input.Name,
                    input.Description,
                    input.BuildInformation,
                    input.StartDate,
                    input.EndDate,
                    input.AssigneeId);
                
                await _testCycleRepository.UpdateAsync(testCycle);
                
                return ObjectMapper.Map<TestCycle, TestCycleDto>(testCycle);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _testCycleRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.View)]
        public virtual async Task<TestCycleDto> GetAsync(Guid id)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetAsync(id);
                return ObjectMapper.Map<TestCycle, TestCycleDto>(testCycle);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["TestCycle:NotFound", id]);
                }
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.View)]
        public virtual async Task<TestCycleDto> GetWithDetailsAsync(Guid id)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetWithDetailsAsync(id);
                return ObjectMapper.Map<TestCycle, TestCycleDto>(testCycle, 
                    opt => opt.IncludeMembers(tc => tc.TestRuns));
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["TestCycle:NotFound", id]);
                }
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.View)]
        public virtual async Task<PagedResultDto<TestCycleDto>> GetListAsync(GetTestCycleListDto input)
        {
            try
            {
                var queryable = await _testCycleRepository.GetQueryableAsync();
                
                // Apply filters
                queryable = queryable
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(), 
                        tc => tc.Name.Contains(input.Filter) || tc.Description.Contains(input.Filter))
                    .WhereIf(input.TestPlanId.HasValue,
                        tc => tc.TestPlanId == input.TestPlanId.Value)
                    .WhereIf(input.Status.HasValue,
                        tc => tc.Status == input.Status.Value)
                    .WhereIf(input.AssigneeId.HasValue,
                        tc => tc.AssigneeId == input.AssigneeId.Value);
                
                // Get count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(TestCycle.CreationTime) + " desc")
                    .PageBy(input);
                
                // Execute query and map to DTOs
                var testCycles = await AsyncExecuter.ToListAsync(queryable);
                var testCycleDtos = ObjectMapper.Map<List<TestCycle>, List<TestCycleDto>>(testCycles);
                
                return new PagedResultDto<TestCycleDto>(totalCount, testCycleDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.View)]
        public virtual async Task<List<TestCycleDto>> GetByTestPlanIdAsync(Guid testPlanId)
        {
            try
            {
                var testCycles = await _testCycleRepository.GetByTestPlanIdAsync(testPlanId);
                return ObjectMapper.Map<List<TestCycle>, List<TestCycleDto>>(testCycles);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.View)]
        public virtual async Task<List<TestCycleDto>> GetMyActiveTestCyclesAsync()
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    return new List<TestCycleDto>();
                }
                
                var testCycles = await _testCycleRepository.GetActiveByAssigneeIdAsync(_currentUser.Id.Value);
                return ObjectMapper.Map<List<TestCycle>, List<TestCycleDto>>(testCycles);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.Start)]
        public virtual async Task<TestCycleDto> StartAsync(Guid id)
        {
            try
            {
                var testCycle = await _testExecutionManager.StartTestCycleAsync(id);
                return ObjectMapper.Map<TestCycle, TestCycleDto>(testCycle);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.Complete)]
        public virtual async Task<TestCycleDto> CompleteAsync(Guid id)
        {
            try
            {
                var testCycle = await _testExecutionManager.CompleteTestCycleAsync(id);
                return ObjectMapper.Map<TestCycle, TestCycleDto>(testCycle);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.Start)]
        public virtual async Task<TestCycleDto> PauseAsync(Guid id)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetAsync(id);
                testCycle.Pause();
                await _testCycleRepository.UpdateAsync(testCycle);
                
                return ObjectMapper.Map<TestCycle, TestCycleDto>(testCycle);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.Start)]
        public virtual async Task<TestCycleDto> ResumeAsync(Guid id)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetAsync(id);
                testCycle.Resume();
                await _testCycleRepository.UpdateAsync(testCycle);
                
                return ObjectMapper.Map<TestCycle, TestCycleDto>(testCycle);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.Complete)]
        public virtual async Task<TestCycleDto> CancelAsync(Guid id, string reason)
        {
            try
            {
                Check.NotNullOrWhiteSpace(reason, nameof(reason));
                
                var testCycle = await _testCycleRepository.GetAsync(id);
                testCycle.Cancel(reason);
                await _testCycleRepository.UpdateAsync(testCycle);
                
                return ObjectMapper.Map<TestCycle, TestCycleDto>(testCycle);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestCycle.View)]
        public virtual async Task<TestProgressDto> GetProgressAsync(Guid id)
        {
            try
            {
                var progress = await _testExecutionManager.GetTestCycleProgressAsync(id);
                return ObjectMapper.Map<(int, int, int, int, int), TestProgressDto>(progress);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "Error in TestCycleAppService: {ErrorMessage}", exception.Message);
            
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
