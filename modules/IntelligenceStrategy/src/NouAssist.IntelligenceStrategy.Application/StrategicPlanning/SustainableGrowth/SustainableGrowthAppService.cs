using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Permissions;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories;
using NouAssist.IntelligenceStrategy.StrategicPlanning.SustainableGrowth.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.SustainableGrowth
{
    /// <summary>
    /// Application service for Sustainable Growth Framework management
    /// </summary>
    [Authorize(StrategicPlanningPermissions.SustainableGrowth.Default)]
    public class SustainableGrowthAppService : ApplicationService, ISustainableGrowthAppService
    {
        private readonly ISustainableGrowthFrameworkRepository _frameworkRepository;

        public SustainableGrowthAppService(
            ISustainableGrowthFrameworkRepository frameworkRepository)
        {
            _frameworkRepository = frameworkRepository;
            LocalizationResource = typeof(StrategicPlanningResource);
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.View)]
        public virtual async Task<SustainableGrowthFrameworkDto> GetAsync(Guid id)
        {
            var framework = await _frameworkRepository.GetFrameworkWithEsgGoalsAsync(id);
            var dto = ObjectMapper.Map<SustainableGrowthFramework, SustainableGrowthFrameworkDto>(framework);
            dto.OverallEsgScore = framework.CalculateOverallEsgScore();
            return dto;
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.View)]
        public virtual async Task<PagedResultDto<SustainableGrowthFrameworkDto>> GetListAsync(PagedResultRequestDto input)
        {
            var queryable = await _frameworkRepository.GetQueryableAsync();
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            
            var frameworks = await AsyncExecuter.ToListAsync(
                queryable.OrderBy("FrameworkStartDate desc").PageBy(input)
            );

            var dtos = ObjectMapper.Map<List<SustainableGrowthFramework>, List<SustainableGrowthFrameworkDto>>(frameworks);
            foreach (var dto in dtos)
            {
                var framework = frameworks.First(f => f.Id == dto.Id);
                dto.OverallEsgScore = framework.CalculateOverallEsgScore();
            }

            return new PagedResultDto<SustainableGrowthFrameworkDto>(totalCount, dtos);
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.Create)]
        public virtual async Task<SustainableGrowthFrameworkDto> CreateAsync(CreateSustainableGrowthFrameworkDto input)
        {
            var framework = SustainableGrowthFramework.Create(
                GuidGenerator.Create(),
                input.Name,
                input.Description,
                input.FrameworkStartDate,
                input.FrameworkEndDate,
                CurrentTenant.Id
            );

            await _frameworkRepository.InsertAsync(framework);
            return ObjectMapper.Map<SustainableGrowthFramework, SustainableGrowthFrameworkDto>(framework);
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.Update)]
        public virtual async Task<SustainableGrowthFrameworkDto> UpdateAsync(Guid id, CreateSustainableGrowthFrameworkDto input)
        {
            var framework = await _frameworkRepository.GetAsync(id);
            // Update framework properties - would need update methods in domain aggregate
            await _frameworkRepository.UpdateAsync(framework);
            return ObjectMapper.Map<SustainableGrowthFramework, SustainableGrowthFrameworkDto>(framework);
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _frameworkRepository.DeleteAsync(id);
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.ManageEsgGoals)]
        public virtual async Task<SustainableGrowthFrameworkDto> AddEsgGoalAsync(Guid frameworkId, AddEsgGoalDto input)
        {
            var framework = await _frameworkRepository.GetAsync(frameworkId);
            
            framework.AddEsgGoal(
                input.Category,
                input.Title,
                input.Description,
                input.MetricName,
                input.TargetValue,
                input.Unit,
                input.TargetDate
            );

            await _frameworkRepository.UpdateAsync(framework);
            return await GetAsync(frameworkId);
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.RecordMeasurements)]
        public virtual async Task<SustainableGrowthFrameworkDto> RecordEsgMeasurementAsync(Guid frameworkId, Guid esgGoalId, RecordEsgMeasurementDto input)
        {
            var framework = await _frameworkRepository.GetAsync(frameworkId);
            
            framework.RecordEsgMeasurement(esgGoalId, input.Value, input.MeasurementDate);
            await _frameworkRepository.UpdateAsync(framework);
            
            return await GetAsync(frameworkId);
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.Update)]
        public virtual async Task<SustainableGrowthFrameworkDto> ActivateAsync(Guid id)
        {
            var framework = await _frameworkRepository.GetAsync(id);
            framework.Activate();
            await _frameworkRepository.UpdateAsync(framework);
            return await GetAsync(id);
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.Update)]
        public virtual async Task<SustainableGrowthFrameworkDto> DeactivateAsync(Guid id)
        {
            var framework = await _frameworkRepository.GetAsync(id);
            framework.Deactivate();
            await _frameworkRepository.UpdateAsync(framework);
            return await GetAsync(id);
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.View)]
        public virtual async Task<ListResultDto<SustainableGrowthFrameworkDto>> GetActiveFrameworksAsync()
        {
            var frameworks = await _frameworkRepository.GetActiveFrameworksAsync();
            var dtos = ObjectMapper.Map<List<SustainableGrowthFramework>, List<SustainableGrowthFrameworkDto>>(frameworks);
            
            foreach (var dto in dtos)
            {
                var framework = frameworks.First(f => f.Id == dto.Id);
                dto.OverallEsgScore = framework.CalculateOverallEsgScore();
            }

            return new ListResultDto<SustainableGrowthFrameworkDto>(dtos);
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.View)]
        public virtual async Task<ListResultDto<EsgGoalRecordDto>> GetGoalsBehindScheduleAsync(Guid frameworkId)
        {
            var framework = await _frameworkRepository.GetFrameworkWithEsgGoalsAsync(frameworkId);
            var goalsBehindSchedule = framework.GetGoalsBehindSchedule().ToList();
            return new ListResultDto<EsgGoalRecordDto>(
                ObjectMapper.Map<List<EsgGoalRecord>, List<EsgGoalRecordDto>>(goalsBehindSchedule)
            );
        }

        [Authorize(StrategicPlanningPermissions.SustainableGrowth.View)]
        public virtual async Task<ListResultDto<EsgGoalRecordDto>> GetGoalsByCategoryAsync(Guid frameworkId, EsgCategory category)
        {
            var framework = await _frameworkRepository.GetFrameworkWithEsgGoalsAsync(frameworkId);
            var goalsByCategory = framework.GetEsgGoalsByCategory(category).ToList();
            return new ListResultDto<EsgGoalRecordDto>(
                ObjectMapper.Map<List<EsgGoalRecord>, List<EsgGoalRecordDto>>(goalsByCategory)
            );
        }
    }
}
