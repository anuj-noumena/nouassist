using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Analytics.Aggregates;
using NouAssist.Analytics.DTOs.KPI;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.Enums;
using NouAssist.Analytics.Interfaces.Services;
using NouAssist.Analytics.Permissions;
using NouAssist.Analytics.Repositories;
using NouAssist.Analytics.ValueObjects;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Interfaces.Services;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Analytics.Services
{
    [Authorize(AnalyticsPermissions.KPIs.Default)]
    public class KpiAppService : ApplicationService, IKpiAppService
    {
        private readonly IKpiManager _kpiManager;
        private readonly IKpiDefinitionRepository _kpiRepository;
        private readonly IDataSourceRepository _dataSourceRepository;
        
        public KpiAppService(
            IKpiManager kpiManager,
            IKpiDefinitionRepository kpiRepository,
            IDataSourceRepository dataSourceRepository)
        {
            _kpiManager = kpiManager;
            _kpiRepository = kpiRepository;
            _dataSourceRepository = dataSourceRepository;
        }
        
        public async Task<KpiDefinitionDto> GetAsync(Guid id)
        {
            var kpi = await _kpiRepository.GetAsync(id);
            return await MapKpiToDtoAsync(kpi);
        }
        
        public async Task<PagedResultDto<KpiDefinitionDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _kpiRepository.CountAsync();
            
            var kpis = await _kpiRepository.GetPagedListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting ?? "Name"
            );
            
            var kpiDtos = new List<KpiDefinitionDto>();
            foreach (var kpi in kpis)
            {
                kpiDtos.Add(await MapKpiToDtoAsync(kpi));
            }
            
            return new PagedResultDto<KpiDefinitionDto>(totalCount, kpiDtos);
        }
        
        public async Task<List<KpiDefinitionDto>> GetByStatusAsync(KpiStatus status)
        {
            var kpis = await _kpiRepository.GetByStatusAsync(status, CurrentTenant.Id);
            
            var kpiDtos = new List<KpiDefinitionDto>();
            foreach (var kpi in kpis)
            {
                kpiDtos.Add(await MapKpiToDtoAsync(kpi));
            }
            
            return kpiDtos;
        }
        
        [Authorize(AnalyticsPermissions.KPIs.Create)]
        public async Task<KpiDefinitionDto> CreateAsync(CreateKpiDefinitionDto input)
        {
            var kpi = await _kpiManager.CreateKpiDefinitionAsync(
                input.Name,
                input.Description,
                input.CalculationExpression,
                CurrentTenant.Id
            );
            
            return await MapKpiToDtoAsync(kpi);
        }
        
        [Authorize(AnalyticsPermissions.KPIs.Edit)]
        public async Task<KpiDefinitionDto> UpdateAsync(Guid id, UpdateKpiDefinitionDto input)
        {
            var kpi = await _kpiManager.UpdateKpiDefinitionAsync(
                id,
                input.Name,
                input.Description,
                input.CalculationExpression
            );
            
            return await MapKpiToDtoAsync(kpi);
        }
        
        [Authorize(AnalyticsPermissions.KPIs.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _kpiRepository.DeleteAsync(id);
        }
        
        [Authorize(AnalyticsPermissions.KPIs.Edit)]
        public async Task<KpiThresholdDto> AddThresholdAsync(Guid id, AddKpiThresholdDto input)
        {
            var alertCondition = new AlertConditionLogic(
                input.AlertCondition.ConditionExpression,
                input.AlertCondition.ConsecutiveBreaches,
                input.AlertCondition.SuppressMinutes,
                input.AlertCondition.NotifyOnBackToNormal
            );
            
            var threshold = await _kpiManager.AddKpiThresholdAsync(
                id,
                input.Name,
                input.ThresholdValue,
                input.IsUpperBound,
                input.StatusWhenBreached,
                input.Severity,
                alertCondition,
                input.NotificationMessage
            );
            
            return new KpiThresholdDto
            {
                Id = threshold.Id,
                Name = threshold.Name,
                ThresholdValue = threshold.ThresholdValue,
                IsUpperBound = threshold.IsUpperBound,
                StatusWhenBreached = threshold.StatusWhenBreached,
                Severity = threshold.Severity,
                AlertCondition = new AlertConditionLogicDto
                {
                    ConditionExpression = threshold.AlertCondition.ConditionExpression,
                    ConsecutiveBreaches = threshold.AlertCondition.ConsecutiveBreaches,
                    SuppressMinutes = threshold.AlertCondition.SuppressMinutes,
                    NotifyOnBackToNormal = threshold.AlertCondition.NotifyOnBackToNormal
                },
                NotificationMessage = threshold.NotificationMessage
            };
        }
        
        [Authorize(AnalyticsPermissions.KPIs.Edit)]
        public async Task RemoveThresholdAsync(Guid id, Guid thresholdId)
        {
            await _kpiManager.RemoveKpiThresholdAsync(id, thresholdId);
        }
        
        [Authorize(AnalyticsPermissions.KPIs.Edit)]
        public async Task AddDataSourceAsync(Guid id, Guid dataSourceId)
        {
            // Verify data source exists
            await _dataSourceRepository.GetAsync(dataSourceId);
            
            await _kpiManager.AddDataSourceToKpiAsync(id, dataSourceId);
        }
        
        [Authorize(AnalyticsPermissions.KPIs.Edit)]
        public async Task RemoveDataSourceAsync(Guid id, Guid dataSourceId)
        {
            await _kpiManager.RemoveDataSourceFromKpiAsync(id, dataSourceId);
        }
        
        [Authorize(AnalyticsPermissions.KPIs.Edit)]
        public async Task UpdateValueAsync(Guid id, UpdateKpiValueDto input)
        {
            await _kpiManager.UpdateKpiValueAsync(
                id,
                input.NewValue,
                input.UpdateTime
            );
        }
        
        public async Task<(decimal Value, KpiStatus Status, DateTime LastUpdated)> GetCurrentValueAsync(Guid id)
        {
            return await _kpiManager.GetKpiValueAsync(id);
        }
        
        public async Task<List<KpiAlertLogDto>> GetRecentAlertsAsync(Guid id, int maxCount = 10)
        {
            var kpi = await _kpiRepository.GetAsync(id);
            
            var alertDtos = new List<KpiAlertLogDto>();
            foreach (var alert in kpi.AlertLogs.OrderByDescending(a => a.OccurredAt).Take(maxCount))
            {
                var threshold = kpi.Thresholds.FirstOrDefault(t => t.Id == alert.ThresholdId);
                
                alertDtos.Add(new KpiAlertLogDto
                {
                    Id = alert.Id,
                    KpiValue = alert.KpiValue,
                    ThresholdId = alert.ThresholdId,
                    ThresholdName = threshold?.Name ?? "Unknown",
                    ThresholdValue = alert.ThresholdValue,
                    Status = alert.Status,
                    OccurredAt = alert.OccurredAt
                });
            }
            
            return alertDtos;
        }
        
        private async Task<KpiDefinitionDto> MapKpiToDtoAsync(KpiDefinition kpi)
        {
            var dataSourceIds = kpi.DataSources.ToList();
            var dataSources = await _dataSourceRepository.GetListAsync(ds => dataSourceIds.Contains(ds.Id));
            
            return new KpiDefinitionDto
            {
                Id = kpi.Id,
                Name = kpi.Name,
                Description = kpi.Description,
                CalculationExpression = kpi.CalculationExpression,
                CurrentValue = kpi.CurrentValue,
                Status = kpi.Status,
                LastUpdated = kpi.LastUpdated,
                Thresholds = kpi.Thresholds.Select(t => new KpiThresholdDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    ThresholdValue = t.ThresholdValue,
                    IsUpperBound = t.IsUpperBound,
                    StatusWhenBreached = t.StatusWhenBreached,
                    Severity = t.Severity,
                    AlertCondition = new AlertConditionLogicDto
                    {
                        ConditionExpression = t.AlertCondition.ConditionExpression,
                        ConsecutiveBreaches = t.AlertCondition.ConsecutiveBreaches,
                        SuppressMinutes = t.AlertCondition.SuppressMinutes,
                        NotifyOnBackToNormal = t.AlertCondition.NotifyOnBackToNormal
                    },
                    NotificationMessage = t.NotificationMessage
                }).ToList(),
                DataSources = dataSourceIds,
                DataSourceNames = dataSources.Select(ds => ds.Name).ToList(),
                RecentAlerts = kpi.AlertLogs.OrderByDescending(a => a.OccurredAt).Take(5).Select(a => new KpiAlertLogDto
                {
                    Id = a.Id,
                    KpiValue = a.KpiValue,
                    ThresholdId = a.ThresholdId,
                    ThresholdName = kpi.Thresholds.FirstOrDefault(t => t.Id == a.ThresholdId)?.Name ?? "Unknown",
                    ThresholdValue = a.ThresholdValue,
                    Status = a.Status,
                    OccurredAt = a.OccurredAt
                }).ToList(),
                CreationTime = kpi.CreationTime,
                CreatorId = kpi.CreatorId,
                LastModificationTime = kpi.LastModificationTime,
                LastModifierId = kpi.LastModifierId
            };
        }
    }
}
