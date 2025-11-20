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
using NouAssist.RequirementsManagement.QualityAssurance.TestPlans.Dtos;
using NouAssist.RequirementsManagement.QualityAssurance.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestPlans
{
    [Authorize(QAPermissions.TestPlan.Default)]
    public class TestPlanAppService : ApplicationService, ITestPlanAppService
    {
        private readonly ITestPlanRepository _testPlanRepository;
        private readonly ITestPlanManager _testPlanManager;
        private readonly ITestExecutionManager _testExecutionManager;
        private readonly ILogger<TestPlanAppService> _logger;
        private readonly IClock _clock;

        public TestPlanAppService(
            ITestPlanRepository testPlanRepository,
            ITestPlanManager testPlanManager,
            ITestExecutionManager testExecutionManager,
            ILogger<TestPlanAppService> logger,
            IClock clock)
        {
            _testPlanRepository = testPlanRepository;
            _testPlanManager = testPlanManager;
            _testExecutionManager = testExecutionManager;
            _logger = logger;
            _clock = clock;
            LocalizationResource = typeof(QAResource);
        }

        [Authorize(QAPermissions.TestPlan.Create)]
        public virtual async Task<TestPlanDto> CreateAsync(CreateTestPlanDto input)
        {
            try
            {
                // Create strategy and environment value objects
                var strategy = new TestingStrategy(
                    input.StrategyApproach,
                    input.StrategyCoverageDescription,
                    input.StrategyOutOfScope);
                
                var environment = new EnvironmentConfiguration(
                    input.EnvironmentName,
                    input.EnvironmentDescription,
                    input.EnvironmentUrl);
                
                // Use domain service to create test plan
                var testPlan = await _testPlanManager.CreateAsync(
                    input.Name,
                    input.Description,
                    input.ReleaseVersion,
                    strategy,
                    environment,
                    input.StartDate,
                    input.EndDate);
                
                return ObjectMapper.Map<TestPlan, TestPlanDto>(testPlan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.Update)]
        public virtual async Task<TestPlanDto> UpdateAsync(Guid id, UpdateTestPlanDto input)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetAsync(id);
                
                // Create strategy and environment value objects
                var strategy = new TestingStrategy(
                    input.StrategyApproach,
                    input.StrategyCoverageDescription,
                    input.StrategyOutOfScope);
                
                var environment = new EnvironmentConfiguration(
                    input.EnvironmentName,
                    input.EnvironmentDescription,
                    input.EnvironmentUrl);
                
                // Update basic info using domain entity method
                testPlan.UpdateBasicInfo(
                    input.Name,
                    input.Description,
                    input.ReleaseVersion,
                    strategy,
                    environment,
                    input.StartDate,
                    input.EndDate);
                
                await _testPlanRepository.UpdateAsync(testPlan);
                
                return ObjectMapper.Map<TestPlan, TestPlanDto>(testPlan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _testPlanRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.View)]
        public virtual async Task<TestPlanDto> GetAsync(Guid id)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetAsync(id);
                return ObjectMapper.Map<TestPlan, TestPlanDto>(testPlan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["TestPlan:NotFound", id]);
                }
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.View)]
        public virtual async Task<TestPlanDto> GetWithDetailsAsync(Guid id)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetWithDetailsAsync(id);
                var testPlanDto = ObjectMapper.Map<TestPlan, TestPlanDto>(testPlan, 
                    opt => opt.IncludeMembers(tp => tp.TestCycles));
                
                // Add test progress information
                var progress = testPlan.GetTestExecutionProgress();
                testPlanDto.Progress = ObjectMapper.Map<(int, int, int, int, int), TestProgressDto>(progress);
                
                return testPlanDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["TestPlan:NotFound", id]);
                }
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.View)]
        public virtual async Task<PagedResultDto<TestPlanDto>> GetListAsync(GetTestPlanListDto input)
        {
            try
            {
                var queryable = await _testPlanRepository.GetQueryableAsync();
                
                // Apply filters
                queryable = queryable
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(), 
                        tp => tp.Name.Contains(input.Filter) || tp.Description.Contains(input.Filter))
                    .WhereIf(!input.ReleaseVersion.IsNullOrWhiteSpace(),
                        tp => tp.ReleaseVersion == input.ReleaseVersion)
                    .WhereIf(input.Status.HasValue,
                        tp => tp.Status == input.Status.Value);
                
                // Apply special filter for requirement ID if provided
                if (input.RequirementId.HasValue)
                {
                    var testPlansWithRequirement = await _testPlanRepository.GetByRequirementIdAsync(input.RequirementId.Value);
                    var testPlanIds = testPlansWithRequirement.Select(tp => tp.Id).ToList();
                    queryable = queryable.Where(tp => testPlanIds.Contains(tp.Id));
                }
                
                // Get count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(TestPlan.CreationTime) + " desc")
                    .PageBy(input);
                
                // Execute query and map to DTOs
                var testPlans = await AsyncExecuter.ToListAsync(queryable);
                var testPlanDtos = ObjectMapper.Map<List<TestPlan>, List<TestPlanDto>>(testPlans);
                
                return new PagedResultDto<TestPlanDto>(totalCount, testPlanDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.View)]
        public virtual async Task<List<TestPlanDto>> GetByReleaseVersionAsync(string releaseVersion)
        {
            try
            {
                var testPlans = await _testPlanRepository.GetByReleaseVersionAsync(releaseVersion);
                return ObjectMapper.Map<List<TestPlan>, List<TestPlanDto>>(testPlans);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.Activate)]
        public virtual async Task<TestPlanDto> ActivateAsync(Guid id)
        {
            try
            {
                var testPlan = await _testPlanManager.ActivateAsync(id);
                return ObjectMapper.Map<TestPlan, TestPlanDto>(testPlan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.Complete)]
        public virtual async Task<TestPlanDto> CompleteAsync(Guid id)
        {
            try
            {
                var testPlan = await _testPlanManager.CompleteAsync(id);
                return ObjectMapper.Map<TestPlan, TestPlanDto>(testPlan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.Archive)]
        public virtual async Task<TestPlanDto> CancelAsync(Guid id, string reason)
        {
            try
            {
                Check.NotNullOrWhiteSpace(reason, nameof(reason));
                
                var testPlan = await _testPlanManager.CancelAsync(id, reason);
                return ObjectMapper.Map<TestPlan, TestPlanDto>(testPlan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.Archive)]
        public virtual async Task<TestPlanDto> ArchiveAsync(Guid id)
        {
            try
            {
                var testPlan = await _testPlanManager.ArchiveAsync(id);
                return ObjectMapper.Map<TestPlan, TestPlanDto>(testPlan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.Update)]
        public virtual async Task LinkRequirementAsync(Guid id, Guid requirementId)
        {
            try
            {
                await _testPlanManager.LinkRequirementAsync(id, requirementId);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.Update)]
        public virtual async Task UnlinkRequirementAsync(Guid id, Guid requirementId)
        {
            try
            {
                await _testPlanManager.UnlinkRequirementAsync(id, requirementId);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.TestPlan.View)]
        public virtual async Task<TestProgressDto> GetProgressAsync(Guid id)
        {
            try
            {
                var progress = await _testExecutionManager.GetTestPlanProgressAsync(id);
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
            _logger.Log(logLevel, exception, "Error in TestPlanAppService: {ErrorMessage}", exception.Message);
            
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
