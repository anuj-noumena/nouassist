using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Permissions;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot
{
    [Authorize(BusinessModelManagementPermissions.StrategicPivot.Default)]
    public class StrategicPivotAppService : ApplicationService, IStrategicPivotAppService
    {
        private readonly IStrategicPivotRepository _pivotRepository;
        private readonly IPivotLogger _pivotLogger;
        private readonly IImpactMeasurementService _impactMeasurementService;
        private readonly ICurrentUser _currentUser;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public StrategicPivotAppService(
            IStrategicPivotRepository pivotRepository,
            IPivotLogger pivotLogger,
            IImpactMeasurementService impactMeasurementService,
            ICurrentUser currentUser,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _pivotRepository = pivotRepository;
            _pivotLogger = pivotLogger;
            _impactMeasurementService = impactMeasurementService;
            _currentUser = currentUser;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(BusinessModelManagementResource);
        }

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.View)]
        public virtual async Task<StrategicPivotDto> GetAsync(Guid id)
        {
            try
            {
                var pivot = await _pivotRepository.GetWithImpactMetricsAsync(id);
                await CheckPivotAccessAsync(pivot);

                return await MapPivotToDtoAsync(pivot);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.View)]
        public virtual async Task<PagedResultDto<StrategicPivotDto>> GetListAsync(GetStrategicPivotListDto input)
        {
            try
            {
                var query = await _pivotRepository.GetQueryableAsync();

                // Apply filters
                query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Description.Contains(input.Filter) || x.Rationale.Reason.Contains(input.Filter));

                if (input.Type.HasValue)
                {
                    query = query.Where(x => x.Type == input.Type.Value);
                }

                if (input.Status.HasValue)
                {
                    query = query.Where(x => x.Status == input.Status.Value);
                }

                if (input.InitiatorId.HasValue)
                {
                    query = query.Where(x => x.InitiatedBy == input.InitiatorId.Value);
                }

                if (input.InitiatedAfter.HasValue)
                {
                    query = query.Where(x => x.InitiatedDate >= input.InitiatedAfter.Value);
                }

                if (input.InitiatedBefore.HasValue)
                {
                    query = query.Where(x => x.InitiatedDate <= input.InitiatedBefore.Value);
                }

                if (input.IncludeOverdue == true)
                {
                    var currentDate = _clock.Now;
                    query = query.Where(x => x.Status != PivotStatus.Completed && 
                                           x.Status != PivotStatus.Abandoned && 
                                           x.TargetCompletionDate < currentDate);
                }

                var totalCount = await AsyncExecuter.CountAsync(query);

                var sorting = input.Sorting.IsNullOrWhiteSpace() ? nameof(Aggregates.StrategicPivot.InitiatedDate) + " desc" : input.Sorting;
                var items = await AsyncExecuter.ToListAsync(
                    query.OrderBy(sorting).PageBy(input));

                var dtos = new List<StrategicPivotDto>();
                foreach (var item in items)
                {
                    dtos.Add(await MapPivotToDtoAsync(item));
                }

                return new PagedResultDto<StrategicPivotDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.Create)]
        public virtual async Task<StrategicPivotDto> CreateAsync(CreateStrategicPivotDto input)
        {
            try
            {
                var rationale = ObjectMapper.Map<PivotRationaleDto, PivotRationale>(input.Rationale);

                var pivot = await _pivotLogger.InitiatePivotAsync(
                    input.Type,
                    rationale,
                    input.Description,
                    _currentUser.GetId(),
                    _clock.Now,
                    input.TargetCompletionDate,
                    CurrentTenant.Id
                );

                return await MapPivotToDtoAsync(pivot);
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

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.Update)]
        public virtual async Task<StrategicPivotDto> UpdateAsync(Guid id, UpdateStrategicPivotDto input)
        {
            try
            {
                var pivot = await _pivotRepository.GetAsync(id);
                await CheckPivotAccessAsync(pivot);

                pivot.UpdateDescription(input.Description);
                pivot.UpdateTargetCompletionDate(input.TargetCompletionDate);

                await _pivotRepository.UpdateAsync(pivot);

                return await MapPivotToDtoAsync(pivot);
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

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var pivot = await _pivotRepository.GetAsync(id);
                await CheckPivotAccessAsync(pivot);

                // Only allow deletion of planned or abandoned pivots
                if (pivot.Status != PivotStatus.Planned && pivot.Status != PivotStatus.Abandoned)
                {
                    throw new UserFriendlyException(L["CannotDeleteActivePivot"]);
                }

                await _pivotRepository.DeleteAsync(pivot);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.ManageLifecycle)]
        public virtual async Task<StrategicPivotDto> StartImplementationAsync(Guid id)
        {
            try
            {
                var pivot = await _pivotRepository.GetAsync(id);
                await CheckPivotAccessAsync(pivot);

                pivot.StartImplementation();
                await _pivotRepository.UpdateAsync(pivot);

                return await MapPivotToDtoAsync(pivot);
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

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.ManageLifecycle)]
        public virtual async Task<StrategicPivotDto> CompleteAsync(Guid id, CompletePivotDto input)
        {
            try
            {
                var pivot = await _pivotRepository.GetAsync(id);
                await CheckPivotAccessAsync(pivot);

                var completionDate = input.CompletionDate ?? _clock.Now;
                pivot.Complete(input.ActualOutcome, input.WasSuccessful, completionDate);

                await _pivotRepository.UpdateAsync(pivot);

                return await MapPivotToDtoAsync(pivot);
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

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.ManageLifecycle)]
        public virtual async Task<StrategicPivotDto> PutOnHoldAsync(Guid id)
        {
            try
            {
                var pivot = await _pivotRepository.GetAsync(id);
                await CheckPivotAccessAsync(pivot);

                pivot.PutOnHold();
                await _pivotRepository.UpdateAsync(pivot);

                return await MapPivotToDtoAsync(pivot);
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

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.ManageLifecycle)]
        public virtual async Task<StrategicPivotDto> ResumeAsync(Guid id)
        {
            try
            {
                var pivot = await _pivotRepository.GetAsync(id);
                await CheckPivotAccessAsync(pivot);

                pivot.Resume();
                await _pivotRepository.UpdateAsync(pivot);

                return await MapPivotToDtoAsync(pivot);
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

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.ManageLifecycle)]
        public virtual async Task<StrategicPivotDto> AbandonAsync(Guid id)
        {
            try
            {
                var pivot = await _pivotRepository.GetAsync(id);
                await CheckPivotAccessAsync(pivot);

                pivot.Abandon();
                await _pivotRepository.UpdateAsync(pivot);

                return await MapPivotToDtoAsync(pivot);
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

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.ManageImpactMetrics)]
        public virtual async Task<ImpactMetricDefinitionDto> CreateImpactMetricAsync(Guid pivotId, CreateImpactMetricDto input)
        {
            try
            {
                var pivot = await _pivotRepository.GetWithImpactMetricsAsync(pivotId);
                await CheckPivotAccessAsync(pivot);

                var metric = await _impactMeasurementService.DefineMetricAsync(
                    pivot,
                    input.MetricName,
                    input.Description,
                    input.Unit,
                    input.MeasurementMethod,
                    input.BaselineValue,
                    input.TargetValue
                );

                return await MapMetricToDtoAsync(metric);
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

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.ManageImpactMetrics)]
        public virtual async Task DeleteImpactMetricAsync(Guid pivotId, Guid metricId)
        {
            try
            {
                var pivot = await _pivotRepository.GetWithImpactMetricsAsync(pivotId);
                await CheckPivotAccessAsync(pivot);

                pivot.RemoveImpactMetric(metricId);
                await _pivotRepository.UpdateAsync(pivot);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.View)]
        public virtual async Task<List<ImpactMetricDefinitionDto>> GetImpactMetricsAsync(Guid pivotId)
        {
            try
            {
                var pivot = await _pivotRepository.GetWithImpactMetricsAsync(pivotId);
                await CheckPivotAccessAsync(pivot);

                var dtos = new List<ImpactMetricDefinitionDto>();
                foreach (var metric in pivot.ImpactMetrics)
                {
                    dtos.Add(await MapMetricToDtoAsync(metric));
                }

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.ManageImpactMetrics)]
        public virtual async Task RecordMeasurementAsync(Guid pivotId, Guid metricId, RecordMeasurementDto input)
        {
            try
            {
                var pivot = await _pivotRepository.GetWithImpactMetricsAsync(pivotId);
                await CheckPivotAccessAsync(pivot);

                var metric = pivot.GetImpactMetric(metricId);
                if (metric == null)
                {
                    throw new UserFriendlyException(L["MetricNotFound"]);
                }

                await _impactMeasurementService.RecordMeasurementAsync(
                    metric,
                    input.Value,
                    input.MeasuredAt,
                    input.MeasurementMethod,
                    input.Notes
                );

                await _pivotRepository.UpdateAsync(pivot);
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

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.View)]
        public virtual async Task<List<ImpactValueDto>> GetTrendDataAsync(Guid pivotId, Guid metricId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var pivot = await _pivotRepository.GetWithImpactMetricsAsync(pivotId);
                await CheckPivotAccessAsync(pivot);

                var metric = pivot.GetImpactMetric(metricId);
                if (metric == null)
                {
                    throw new UserFriendlyException(L["MetricNotFound"]);
                }

                var trendData = await _impactMeasurementService.GetTrendDataAsync(metric, fromDate, toDate);

                return trendData
                    .Select(td => ObjectMapper.Map<ImpactValue, ImpactValueDto>(td))
                    .ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.ViewAnalytics)]
        public virtual async Task<PivotAnalysisDto> AnalyzeImpactAsync(Guid id)
        {
            try
            {
                var pivot = await _pivotRepository.GetWithImpactMetricsAsync(id);
                await CheckPivotAccessAsync(pivot);

                var analysis = await _impactMeasurementService.AnalyzeImpactAsync(pivot);

                return new PivotAnalysisDto
                {
                    TotalMetricsDefined = (int)analysis["TotalMetricsDefined"],
                    MetricsWithData = (int)analysis["MetricsWithData"],
                    OverallSuccessRate = (double)analysis["OverallSuccessRate"],
                    PivotStatus = (string)analysis["PivotStatus"],
                    IsOverdue = (bool)analysis["IsOverdue"],
                    Duration = analysis.ContainsKey("Duration") ? (double?)analysis["Duration"] : null,
                    MetricAnalysis = ((List<object>)analysis["MetricAnalysis"])
                        .Cast<dynamic>()
                        .Select(ma => new MetricAnalysisDto
                        {
                            MetricName = ma.MetricName,
                            BaselineValue = ma.BaselineValue,
                            TargetValue = ma.TargetValue,
                            LatestValue = ma.LatestValue,
                            ProgressToTarget = ma.ProgressToTarget,
                            TrendDirection = ma.TrendDirection,
                            MeasurementCount = ma.MeasurementCount
                        }).ToList()
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.View)]
        public virtual async Task<List<StrategicPivotDto>> GetOverduePivotsAsync()
        {
            try
            {
                var overduePivots = await _pivotRepository.GetOverduePivotsAsync(_clock.Now, CurrentTenant.Id);

                var dtos = new List<StrategicPivotDto>();
                foreach (var pivot in overduePivots)
                {
                    dtos.Add(await MapPivotToDtoAsync(pivot));
                }

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.View)]
        public virtual async Task<List<StrategicPivotDto>> GetActivePivotsAsync()
        {
            try
            {
                var activePivots = await _pivotRepository.GetActivePivotsAsync(CurrentTenant.Id);

                var dtos = new List<StrategicPivotDto>();
                foreach (var pivot in activePivots)
                {
                    dtos.Add(await MapPivotToDtoAsync(pivot));
                }

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.StrategicPivot.View)]
        public virtual async Task<List<ImpactMetricDefinitionDto>> GetSuggestedMetricsAsync(Guid id)
        {
            try
            {
                var pivot = await _pivotRepository.GetAsync(id);
                await CheckPivotAccessAsync(pivot);

                var suggestions = await _pivotLogger.SuggestImpactMetricsAsync(pivot.Type, pivot.Description);

                var dtos = new List<ImpactMetricDefinitionDto>();
                foreach (var suggestion in suggestions)
                {
                    dtos.Add(await MapMetricToDtoAsync(suggestion));
                }

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        // Helper methods
        private async Task CheckPivotAccessAsync(Aggregates.StrategicPivot pivot)
        {
            if (pivot.InitiatedBy != _currentUser.GetId() && pivot.TenantId != CurrentTenant.Id)
            {
                throw new UserFriendlyException(L["AccessDenied"]);
            }
        }

        private async Task<StrategicPivotDto> MapPivotToDtoAsync(Aggregates.StrategicPivot pivot)
        {
            var dto = ObjectMapper.Map<Aggregates.StrategicPivot, StrategicPivotDto>(pivot);
            dto.OverallSuccessRate = pivot.CalculateOverallSuccessRate();
            dto.IsOverdue = pivot.IsOverdue();
            dto.DurationInDays = pivot.GetDuration()?.TotalDays;

            // Map impact metrics with calculated values
            dto.ImpactMetrics = new List<ImpactMetricDefinitionDto>();
            foreach (var metric in pivot.ImpactMetrics)
            {
                dto.ImpactMetrics.Add(await MapMetricToDtoAsync(metric));
            }

            return dto;
        }

        private async Task<ImpactMetricDefinitionDto> MapMetricToDtoAsync(Entities.ImpactMetricDefinition metric)
        {
            var dto = ObjectMapper.Map<Entities.ImpactMetricDefinition, ImpactMetricDefinitionDto>(metric);
            dto.LatestValue = metric.GetLatestMeasurement();
            dto.ProgressToTarget = metric.CalculateProgressToTarget();
            return dto;
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in {ServiceName}: {ErrorMessage}", 
                GetType().FullName, exception.Message);
            await Task.CompletedTask;
        }
    }
}
