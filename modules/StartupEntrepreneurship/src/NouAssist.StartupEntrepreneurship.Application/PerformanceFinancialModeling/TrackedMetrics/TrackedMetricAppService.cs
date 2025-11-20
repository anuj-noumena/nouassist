using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Permissions;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrackedMetrics.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrackedMetrics
{
    [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.Default)]
    public class TrackedMetricAppService : ApplicationService, ITrackedMetricAppService
    {
        private readonly ITrackedMetricRepository _trackedMetricRepository;
        private readonly IRepository<MetricDefinition, Guid> _metricDefinitionRepository;
        private readonly IRepository<MetricCategory, Guid> _metricCategoryRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public TrackedMetricAppService(
            ITrackedMetricRepository trackedMetricRepository,
            IRepository<MetricDefinition, Guid> metricDefinitionRepository,
            IRepository<MetricCategory, Guid> metricCategoryRepository,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _trackedMetricRepository = trackedMetricRepository;
            _metricDefinitionRepository = metricDefinitionRepository;
            _metricCategoryRepository = metricCategoryRepository;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(PerformanceFinancialModelingResource);
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<TrackedMetricDto> GetAsync(Guid id)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(id, includeDetails: false);
                CheckResourceAccess(metric);

                var dto = ObjectMapper.Map<TrackedMetric, TrackedMetricDto>(metric);
                await EnrichTrackedMetricDto(dto, metric);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<TrackedMetricDetailDto> GetDetailAsync(Guid id)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(id, includeDetails: true);
                CheckResourceAccess(metric);

                var dto = ObjectMapper.Map<TrackedMetric, TrackedMetricDetailDto>(metric);
                await EnrichTrackedMetricDetailDto(dto, metric);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<PagedResultDto<TrackedMetricDto>> GetListAsync(GetTrackedMetricListDto input)
        {
            try
            {
                var query = await _trackedMetricRepository.GetQueryableAsync();

                // Apply tenant filter
                query = query.Where(m => m.TenantId == _currentTenant.Id);

                // Apply filters
                query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(input.Filter) || x.Description.Contains(input.Filter));

                query = query.WhereIf(input.Type.HasValue, x => x.Type == input.Type.Value);
                query = query.WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value);
                query = query.WhereIf(input.DefinitionId.HasValue, x => x.DefinitionId == input.DefinitionId.Value);
                query = query.WhereIf(input.LastUpdatedSince.HasValue, x => x.LastUpdatedAt >= input.LastUpdatedSince.Value);

                // Apply sorting
                if (!input.Sorting.IsNullOrWhiteSpace())
                {
                    query = query.OrderBy(input.Sorting);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);

                // Apply paging
                var entities = await AsyncExecuter.ToListAsync(
                    query.PageBy(input)
                );

                // Map to DTOs
                var dtos = ObjectMapper.Map<List<TrackedMetric>, List<TrackedMetricDto>>(entities);

                // Enrich DTOs
                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var metric = entities.First(e => e.Id == dto.Id);
                    await EnrichTrackedMetricDto(dto, metric);
                }));

                return new PagedResultDto<TrackedMetricDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.Create)]
        public virtual async Task<TrackedMetricDto> CreateAsync(CreateTrackedMetricDto input)
        {
            try
            {
                // Validate definition exists
                var definition = await _metricDefinitionRepository.GetAsync(input.DefinitionId);
                if (!definition.IsActive)
                {
                    throw new UserFriendlyException(L["ErrorMessage:InactiveDefinition"]);
                }

                // Check for duplicates
                var existingMetric = await _trackedMetricRepository.FindByNameAsync(input.Name, _currentTenant.Id);
                if (existingMetric != null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:DuplicateMetricName", input.Name]);
                }

                // Create the metric
                var metricId = _guidGenerator.Create();
                var metric = TrackedMetric.Create(
                    metricId,
                    input.Name,
                    input.Description,
                    input.Type,
                    input.DataType,
                    input.Frequency,
                    input.DefinitionId,
                    input.Unit,
                    _currentTenant.Id
                );

                await _trackedMetricRepository.InsertAsync(metric, autoSave: true);

                var dto = ObjectMapper.Map<TrackedMetric, TrackedMetricDto>(metric);
                await EnrichTrackedMetricDto(dto, metric);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:DefinitionNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.Update)]
        public virtual async Task<TrackedMetricDto> UpdateAsync(Guid id, UpdateTrackedMetricDto input)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(id);
                CheckResourceAccess(metric);

                // Check for duplicate names (excluding current metric)
                var existingMetric = await _trackedMetricRepository.FindByNameAsync(input.Name, _currentTenant.Id);
                if (existingMetric != null && existingMetric.Id != id)
                {
                    throw new UserFriendlyException(L["ErrorMessage:DuplicateMetricName", input.Name]);
                }

                metric.UpdateBasicInfo(input.Name, input.Description, input.Unit);
                await _trackedMetricRepository.UpdateAsync(metric, autoSave: true);

                var dto = ObjectMapper.Map<TrackedMetric, TrackedMetricDto>(metric);
                await EnrichTrackedMetricDto(dto, metric);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(id);
                CheckResourceAccess(metric);

                await _trackedMetricRepository.DeleteAsync(metric, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.Update)]
        public virtual async Task<TrackedMetricDto> ActivateAsync(Guid id)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(id);
                CheckResourceAccess(metric);

                metric.Activate();
                await _trackedMetricRepository.UpdateAsync(metric, autoSave: true);

                var dto = ObjectMapper.Map<TrackedMetric, TrackedMetricDto>(metric);
                await EnrichTrackedMetricDto(dto, metric);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.Update)]
        public virtual async Task<TrackedMetricDto> DeactivateAsync(Guid id)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(id);
                CheckResourceAccess(metric);

                metric.Deactivate();
                await _trackedMetricRepository.UpdateAsync(metric, autoSave: true);

                var dto = ObjectMapper.Map<TrackedMetric, TrackedMetricDto>(metric);
                await EnrichTrackedMetricDto(dto, metric);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.ManageDataPoints)]
        public virtual async Task AddDataPointAsync(Guid metricId, AddDataPointDto input)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId);
                CheckResourceAccess(metric);

                metric.AddDataPoint(input.Value, input.Timestamp, input.Notes);
                await _trackedMetricRepository.UpdateAsync(metric, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.ManageDataPoints)]
        public virtual async Task BulkAddDataPointsAsync(Guid metricId, BulkAddDataPointsDto input)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId);
                CheckResourceAccess(metric);

                foreach (var dataPoint in input.DataPoints)
                {
                    metric.AddDataPoint(dataPoint.Value, dataPoint.Timestamp, dataPoint.Notes);
                }

                await _trackedMetricRepository.UpdateAsync(metric, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<MetricDataPointDto>> GetDataPointsAsync(Guid metricId, GetDataPointsDto input)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId, includeDetails: true);
                CheckResourceAccess(metric);

                var startDate = input.StartDate ?? DateTime.UtcNow.AddDays(-30);
                var endDate = input.EndDate ?? DateTime.UtcNow;

                var dataPoints = metric.GetDataPointsInPeriod(startDate, endDate);

                if (input.MaxCount.HasValue && dataPoints.Count > input.MaxCount.Value)
                {
                    dataPoints = dataPoints.TakeLast(input.MaxCount.Value).ToList();
                }

                return ObjectMapper.Map<List<MetricDataPoint>, List<MetricDataPointDto>>(dataPoints);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.ManageGoals)]
        public virtual async Task<MetricGoalDto> SetGoalAsync(Guid metricId, SetMetricGoalDto input)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId);
                CheckResourceAccess(metric);

                var targetValue = ObjectMapper.Map<MetricValueDto, MetricValue>(input.TargetValue);
                var secondaryValue = input.SecondaryValue != null 
                    ? ObjectMapper.Map<MetricValueDto, MetricValue>(input.SecondaryValue) 
                    : null;

                metric.SetGoal(targetValue, input.TargetDate, input.ThresholdType, input.Description, secondaryValue);
                await _trackedMetricRepository.UpdateAsync(metric, autoSave: true);

                var goal = metric.GetActiveGoal();
                var dto = ObjectMapper.Map<MetricGoal, MetricGoalDto>(goal);
                await EnrichMetricGoalDto(dto, goal, metric);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.ManageGoals)]
        public virtual async Task<MetricGoalDto> UpdateGoalAsync(Guid goalId, UpdateMetricGoalDto input)
        {
            try
            {
                var metrics = await _trackedMetricRepository.GetListAsync(
                    await _trackedMetricRepository.WithDetailsAsync()
                );

                var metric = metrics.FirstOrDefault(m => m.Goals.Any(g => g.Id == goalId));
                if (metric == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:GoalNotFound"]);
                }

                CheckResourceAccess(metric);

                var goal = metric.Goals.First(g => g.Id == goalId);
                goal.UpdateDescription(input.Description);

                await _trackedMetricRepository.UpdateAsync(metric, autoSave: true);

                var dto = ObjectMapper.Map<MetricGoal, MetricGoalDto>(goal);
                await EnrichMetricGoalDto(dto, goal, metric);

                return dto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.ManageGoals)]
        public virtual async Task DeactivateGoalAsync(Guid goalId)
        {
            try
            {
                var metrics = await _trackedMetricRepository.GetListAsync(
                    await _trackedMetricRepository.WithDetailsAsync()
                );

                var metric = metrics.FirstOrDefault(m => m.Goals.Any(g => g.Id == goalId));
                if (metric == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:GoalNotFound"]);
                }

                CheckResourceAccess(metric);

                var goal = metric.Goals.First(g => g.Id == goalId);
                goal.Deactivate();

                await _trackedMetricRepository.UpdateAsync(metric, autoSave: true);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<MetricGoalDto>> GetGoalsAsync(Guid metricId)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId, includeDetails: true);
                CheckResourceAccess(metric);

                var dtos = ObjectMapper.Map<List<MetricGoal>, List<MetricGoalDto>>(metric.Goals.ToList());

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var goal = metric.Goals.First(g => g.Id == dto.Id);
                    await EnrichMetricGoalDto(dto, goal, metric);
                }));

                return dtos;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<decimal?> GetTrendAsync(Guid metricId, int periodDays = 30)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId, includeDetails: true);
                CheckResourceAccess(metric);

                return metric.GetTrend(periodDays);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<MetricValueDto> GetAverageValueAsync(Guid metricId, int periodDays = 30)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId, includeDetails: true);
                CheckResourceAccess(metric);

                var averageValue = metric.GetAverageValue(periodDays);
                return averageValue != null ? ObjectMapper.Map<MetricValue, MetricValueDto>(averageValue) : null;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<TrackedMetricDto>> GetByTypeAsync(MetricType type)
        {
            try
            {
                var metrics = await _trackedMetricRepository.GetByTypeAsync(type, _currentTenant.Id);
                var dtos = ObjectMapper.Map<List<TrackedMetric>, List<TrackedMetricDto>>(metrics);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var metric = metrics.First(m => m.Id == dto.Id);
                    await EnrichTrackedMetricDto(dto, metric);
                }));

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<TrackedMetricDto>> GetActiveMetricsAsync()
        {
            try
            {
                var metrics = await _trackedMetricRepository.GetActiveMetricsAsync(_currentTenant.Id);
                var dtos = ObjectMapper.Map<List<TrackedMetric>, List<TrackedMetricDto>>(metrics);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var metric = metrics.First(m => m.Id == dto.Id);
                    await EnrichTrackedMetricDto(dto, metric);
                }));

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<TrackedMetricDto>> GetMetricsDueForUpdateAsync()
        {
            try
            {
                var metrics = await _trackedMetricRepository.GetMetricsDueForUpdateAsync(_currentTenant.Id);
                var dtos = ObjectMapper.Map<List<TrackedMetric>, List<TrackedMetricDto>>(metrics);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var metric = metrics.First(m => m.Id == dto.Id);
                    await EnrichTrackedMetricDto(dto, metric);
                }));

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // Helper methods
        protected virtual void CheckResourceAccess(TrackedMetric metric)
        {
            if (metric.TenantId != _currentTenant.Id)
            {
                throw new EntityNotFoundException(typeof(TrackedMetric), metric.Id);
            }
        }

        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in TrackedMetricAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
        }

        protected virtual async Task EnrichTrackedMetricDto(TrackedMetricDto dto, TrackedMetric metric)
        {
            dto.DataPointCount = metric.DataPoints.Count;
            dto.ActiveGoalCount = metric.Goals.Count(g => g.IsActive);
            dto.TrendPercentage = metric.GetTrend();

            await Task.CompletedTask;
        }

        protected virtual async Task EnrichTrackedMetricDetailDto(TrackedMetricDetailDto dto, TrackedMetric metric)
        {
            await EnrichTrackedMetricDto(dto, metric);

            // Get definition name
            try
            {
                var definition = await _metricDefinitionRepository.FindAsync(metric.DefinitionId);
                dto.DefinitionName = definition?.Name;

                if (definition?.CategoryId.HasValue == true)
                {
                    var category = await _metricCategoryRepository.FindAsync(definition.CategoryId.Value);
                    dto.CategoryName = category?.Name;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to enrich TrackedMetricDetailDto for metric {MetricId}", metric.Id);
            }
        }

        protected virtual async Task EnrichMetricGoalDto(MetricGoalDto dto, MetricGoal goal, TrackedMetric metric)
        {
            dto.DaysRemaining = goal.Target.GetDaysRemaining();
            dto.IsOnTrack = goal.IsOnTrack(metric.LastValue ?? new MetricValue(0, metric.DataType));
            
            if (metric.LastValue != null)
            {
                dto.ProgressPercentage = goal.GetProgress(metric.LastValue);
            }

            if (goal.Target.IsOverdue())
            {
                dto.Status = "Overdue";
            }
            else if (goal.IsAchieved)
            {
                dto.Status = "Achieved";
            }
            else if (dto.IsOnTrack)
            {
                dto.Status = "On Track";
            }
            else
            {
                dto.Status = "At Risk";
            }

            await Task.CompletedTask;
        }
    }
}
