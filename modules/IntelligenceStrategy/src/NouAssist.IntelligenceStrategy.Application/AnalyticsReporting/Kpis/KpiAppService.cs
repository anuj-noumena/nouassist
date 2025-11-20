using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Permissions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Services;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis
{
    /// <summary>
    /// Application service implementation for KPI management
    /// </summary>
    [Authorize(AnalyticsReportingPermissions.Kpi.Default)]
    public class KpiAppService : ApplicationService, IKpiAppService
    {
        private readonly IKpiManager _kpiManager;
        private readonly IKpiDefinitionRepository _kpiRepository;
        private readonly IKpiAlertLogRepository _alertLogRepository;
        private readonly IDataSourceRepository _dataSourceRepository;
        
        public KpiAppService(
            IKpiManager kpiManager,
            IKpiDefinitionRepository kpiRepository,
            IKpiAlertLogRepository alertLogRepository,
            IDataSourceRepository dataSourceRepository)
        {
            _kpiManager = kpiManager;
            _kpiRepository = kpiRepository;
            _alertLogRepository = alertLogRepository;
            _dataSourceRepository = dataSourceRepository;
            
            // Set resource for localization
            LocalizationResource = typeof(AnalyticsReportingResource);
        }
        
        /// <summary>
        /// Gets a KPI by ID
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Kpi.View)]
        public virtual async Task<KpiDefinitionDto> GetAsync(Guid id)
        {
            try
            {
                var kpi = await _kpiRepository.GetAsync(id);
                
                // Check tenant
                if (kpi.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Enrich with data source information
                var kpiDto = ObjectMapper.Map<KpiDefinition, KpiDefinitionDto>(kpi);
                kpiDto.DataSources = await GetDataSourcesInfoAsync(kpi.DataSourceIds);
                
                return kpiDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Gets a paginated list of KPIs
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Kpi.View)]
        public virtual async Task<PagedResultDto<KpiDefinitionDto>> GetListAsync(GetKpiListDto input)
        {
            try
            {
                // Get queryable repository to apply filters
                var queryable = await _kpiRepository.GetQueryableAsync();
                
                // Apply filters
                queryable = await ApplyListFiltersAsync(queryable, input);
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(KpiDefinition.Name))
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);
                
                // Get KPIs
                var kpis = await AsyncExecuter.ToListAsync(queryable);
                
                // Map to DTOs
                var kpiDtos = ObjectMapper.Map<List<KpiDefinition>, List<KpiDefinitionDto>>(kpis);
                
                // Enrich with data source information if details are requested
                if (input.IncludeDetails && kpis.Any())
                {
                    foreach (var kpi in kpis)
                    {
                        var index = kpis.IndexOf(kpi);
                        kpiDtos[index].DataSources = await GetDataSourcesInfoAsync(kpi.DataSourceIds);
                    }
                }
                
                return new PagedResultDto<KpiDefinitionDto>(totalCount, kpiDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Creates a new KPI
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Kpi.Create)]
        public virtual async Task<KpiDefinitionDto> CreateAsync(CreateKpiDefinitionDto input)
        {
            try
            {
                // Map refresh interval value object
                var refreshInterval = ObjectMapper.Map<CreateKpiDefinitionDto, RefreshInterval>(input);
                
                // Create KPI using domain service
                var kpi = await _kpiManager.CreateKpiAsync(
                    GuidGenerator.Create(),
                    input.Name,
                    input.Description,
                    input.Category,
                    input.Unit,
                    input.DataSourceIds,
                    input.CalculationDefinition,
                    refreshInterval,
                    input.DisplayFormat,
                    input.IsActive,
                    CurrentTenant.Id
                );
                
                // Map to DTO
                var kpiDto = ObjectMapper.Map<KpiDefinition, KpiDefinitionDto>(kpi);
                
                // Enrich with data source information
                kpiDto.DataSources = await GetDataSourcesInfoAsync(kpi.DataSourceIds);
                
                return kpiDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates an existing KPI
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Kpi.Update)]
        public virtual async Task<KpiDefinitionDto> UpdateAsync(Guid id, UpdateKpiDefinitionDto input)
        {
            try
            {
                // Get the KPI
                var kpi = await _kpiRepository.GetAsync(id);
                
                // Check tenant
                if (kpi.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Update basic properties
                kpi.SetName(input.Name);
                kpi.SetDescription(input.Description);
                kpi.SetCategory(input.Category);
                kpi.SetUnit(input.Unit);
                kpi.UpdateDataSources(input.DataSourceIds);
                kpi.UpdateCalculationDefinition(input.CalculationDefinition);
                kpi.SetDisplayFormat(input.DisplayFormat);
                kpi.SetActive(input.IsActive);
                kpi.SetTags(input.Tags);
                
                // Update refresh interval
                var refreshInterval = ObjectMapper.Map<UpdateKpiDefinitionDto, RefreshInterval>(input);
                kpi.UpdateRefreshInterval(refreshInterval);
                
                // Save changes
                await _kpiRepository.UpdateAsync(kpi);
                
                // Map to DTO
                var kpiDto = ObjectMapper.Map<KpiDefinition, KpiDefinitionDto>(kpi);
                
                // Enrich with data source information
                kpiDto.DataSources = await GetDataSourcesInfoAsync(kpi.DataSourceIds);
                
                return kpiDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Deletes a KPI
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Kpi.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                // Get the KPI
                var kpi = await _kpiRepository.GetAsync(id);
                
                // Check tenant
                if (kpi.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Delete the KPI
                await _kpiRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Adds a threshold to a KPI
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Kpi.ManageThresholds)]
        public virtual async Task<KpiThresholdDto> AddThresholdAsync(Guid kpiId, AddKpiThresholdDto input)
        {
            try
            {
                // Map to alert condition logic value object if expression provided
                AlertConditionLogic alertCondition = null;
                if (!string.IsNullOrEmpty(input.AlertExpression))
                {
                    alertCondition = ObjectMapper.Map<AddKpiThresholdDto, AlertConditionLogic>(input);
                }
                
                // Add threshold using domain service
                var threshold = await _kpiManager.AddThresholdToKpiAsync(
                    GuidGenerator.Create(),
                    kpiId,
                    input.Value,
                    input.Operator,
                    input.Status,
                    alertCondition,
                    input.SendNotification,
                    input.NotificationRecipients
                );
                
                // Map to DTO
                return ObjectMapper.Map<KpiThreshold, KpiThresholdDto>(threshold);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates a threshold on a KPI
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Kpi.ManageThresholds)]
        public virtual async Task<KpiThresholdDto> UpdateThresholdAsync(Guid kpiId, Guid thresholdId, UpdateKpiThresholdDto input)
        {
            try
            {
                // Get the KPI
                var kpi = await _kpiRepository.GetAsync(kpiId);
                
                // Check tenant
                if (kpi.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Get the threshold
                var threshold = kpi.Thresholds.FirstOrDefault(t => t.Id == thresholdId);
                if (threshold == null)
                {
                    throw new EntityNotFoundException("Threshold", thresholdId);
                }
                
                // Map to alert condition logic value object if any properties specified
                AlertConditionLogic alertCondition = null;
                if (!string.IsNullOrEmpty(input.AlertExpression) || !string.IsNullOrEmpty(input.AlertDescription) || 
                    input.RequireConsecutiveOccurrences.HasValue || input.ConsecutiveOccurrencesRequired.HasValue)
                {
                    // Store existing alert condition for mapping context
                    var existingCondition = threshold.AlertCondition;
                    var context = new Dictionary<string, object>
                    {
                        ["ExistingAlertCondition"] = existingCondition
                    };
                    
                    // Map input to new alert condition
                    alertCondition = ObjectMapper.Map<UpdateKpiThresholdDto, AlertConditionLogic>(input, context);
                }
                
                // Update threshold
                kpi.UpdateThreshold(
                    thresholdId,
                    input.Value,
                    input.Operator,
                    input.Status,
                    alertCondition,
                    input.SendNotification,
                    input.NotificationRecipients
                );
                
                // Save changes
                await _kpiRepository.UpdateAsync(kpi);
                
                // Get updated threshold
                var updatedThreshold = kpi.Thresholds.First(t => t.Id == thresholdId);
                
                // Map to DTO
                return ObjectMapper.Map<KpiThreshold, KpiThresholdDto>(updatedThreshold);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Removes a threshold from a KPI
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Kpi.ManageThresholds)]
        public virtual async Task DeleteThresholdAsync(Guid kpiId, Guid thresholdId)
        {
            try
            {
                // Get the KPI
                var kpi = await _kpiRepository.GetAsync(kpiId);
                
                // Check tenant
                if (kpi.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Remove threshold
                kpi.RemoveThreshold(thresholdId);
                
                // Save changes
                await _kpiRepository.UpdateAsync(kpi);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates the value of a KPI
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Kpi.Update)]
        public virtual async Task<KpiValueDto> UpdateValueAsync(Guid kpiId, UpdateKpiValueDto input)
        {
            try
            {
                // Get timestamp or use current time
                var timestamp = input.Timestamp ?? Clock.Now;
                
                // Update KPI value using domain service
                await _kpiManager.UpdateKpiValueAsync(
                    kpiId,
                    input.Value,
                    timestamp
                );
                
                // Get the updated KPI to return the current value
                var kpi = await _kpiRepository.GetAsync(kpiId);
                
                // Map to DTO
                return ObjectMapper.Map<KpiValue, KpiValueDto>(kpi.CurrentValue);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Gets KPI alerts for a KPI
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Kpi.View)]
        public virtual async Task<PagedResultDto<KpiAlertLogDto>> GetAlertsAsync(Guid kpiId, GetKpiAlertListDto input)
        {
            try
            {
                // Get the KPI
                var kpi = await _kpiRepository.GetAsync(kpiId);
                
                // Check tenant
                if (kpi.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Get queryable repository
                var queryable = await _alertLogRepository.GetQueryableAsync();
                
                // Filter to specific KPI
                queryable = queryable.Where(a => a.KpiDefinitionId == kpiId);
                
                // Apply additional filters
                queryable = ApplyAlertFilters(queryable, input);
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderByDescending(a => a.Timestamp)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);
                
                // Get alerts
                var alerts = await AsyncExecuter.ToListAsync(queryable);
                
                // Map to DTOs
                var alertDtos = ObjectMapper.Map<List<KpiAlertLog>, List<KpiAlertLogDto>>(alerts);
                
                // Enrich with KPI name
                foreach (var alertDto in alertDtos)
                {
                    alertDto.KpiName = kpi.Name;
                    
                    // Add formatted value if KPI has display format
                    if (!string.IsNullOrEmpty(kpi.DisplayFormat))
                    {
                        alertDto.FormattedValue = string.Format(kpi.DisplayFormat, alertDto.Value);
                    }
                }
                
                return new PagedResultDto<KpiAlertLogDto>(totalCount, alertDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Gets KPI alerts across all KPIs
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Kpi.View)]
        public virtual async Task<PagedResultDto<KpiAlertLogDto>> GetAllAlertsAsync(GetKpiAlertListDto input)
        {
            try
            {
                // Get queryable repository
                var queryable = await _alertLogRepository.GetQueryableAsync();
                
                // Get KPI repository for name lookups
                var kpiQueryable = await _kpiRepository.GetQueryableAsync();
                
                // Filter by tenant
                queryable = queryable.Where(a => kpiQueryable.Any(k => k.Id == a.KpiDefinitionId && k.TenantId == CurrentTenant.Id));
                
                // Apply additional filters
                if (!string.IsNullOrWhiteSpace(input.KpiId) && Guid.TryParse(input.KpiId, out var kpiId))
                {
                    queryable = queryable.Where(a => a.KpiDefinitionId == kpiId);
                }
                
                queryable = ApplyAlertFilters(queryable, input);
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderByDescending(a => a.Timestamp)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);
                
                // Get alerts
                var alerts = await AsyncExecuter.ToListAsync(queryable);
                
                // Map to DTOs
                var alertDtos = ObjectMapper.Map<List<KpiAlertLog>, List<KpiAlertLogDto>>(alerts);
                
                // Create lookup of KPI IDs to names
                var kpiIds = alerts.Select(a => a.KpiDefinitionId).Distinct().ToList();
                var kpis = await AsyncExecuter.ToListAsync(kpiQueryable.Where(k => kpiIds.Contains(k.Id)));
                var kpiLookup = kpis.ToDictionary(k => k.Id, k => k);
                
                // Enrich with KPI names and formatted values
                foreach (var alertDto in alertDtos)
                {
                    if (kpiLookup.TryGetValue(alertDto.KpiDefinitionId, out var kpi))
                    {
                        alertDto.KpiName = kpi.Name;
                        
                        // Add formatted value if KPI has display format
                        if (!string.IsNullOrEmpty(kpi.DisplayFormat))
                        {
                            alertDto.FormattedValue = string.Format(kpi.DisplayFormat, alertDto.Value);
                        }
                    }
                    else
                    {
                        alertDto.KpiName = "Unknown";
                    }
                }
                
                return new PagedResultDto<KpiAlertLogDto>(totalCount, alertDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Applies filters to KPI list query
        /// </summary>
        private async Task<IQueryable<KpiDefinition>> ApplyListFiltersAsync(IQueryable<KpiDefinition> queryable, GetKpiListDto input)
        {
            // Filter by only active
            if (input.OnlyActive == true)
            {
                queryable = queryable.Where(k => k.IsActive);
            }
            
            // Filter by text
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(k => 
                    k.Name.Contains(input.Filter) || 
                    (k.Description != null && k.Description.Contains(input.Filter)));
            }
            
            // Filter by category
            if (!string.IsNullOrWhiteSpace(input.Category))
            {
                queryable = queryable.Where(k => k.Category == input.Category);
            }
            
            // Filter by status
            if (!string.IsNullOrWhiteSpace(input.Status) && Enum.TryParse<KpiStatus>(input.Status, true, out var status))
            {
                queryable = queryable.Where(k => 
                    k.CurrentValue != null && 
                    EF.Property<string>(k, "CurrentValue_Status") == status.ToString());
            }
            
            // Filter by tag
            if (!string.IsNullOrWhiteSpace(input.Tag))
            {
                // This is a simplification - in actual implementation
                // you might need a more complex query to search within the tags array
                queryable = queryable.Where(k => EF.Functions.JsonContains(k.Tags, input.Tag));
            }
            
            // Apply tenant filter
            queryable = queryable.Where(k => k.TenantId == CurrentTenant.Id);
            
            return queryable;
        }
        
        /// <summary>
        /// Applies filters to alert list query
        /// </summary>
        private IQueryable<KpiAlertLog> ApplyAlertFilters(IQueryable<KpiAlertLog> queryable, GetKpiAlertListDto input)
        {
            // Filter by status
            if (!string.IsNullOrWhiteSpace(input.Status) && Enum.TryParse<KpiStatus>(input.Status, true, out var status))
            {
                queryable = queryable.Where(a => a.Status == status);
            }
            
            // Filter by time range
            if (!string.IsNullOrWhiteSpace(input.TimestampMin) && DateTime.TryParse(input.TimestampMin, out var timestampMin))
            {
                queryable = queryable.Where(a => a.Timestamp >= timestampMin);
            }
            
            if (!string.IsNullOrWhiteSpace(input.TimestampMax) && DateTime.TryParse(input.TimestampMax, out var timestampMax))
            {
                queryable = queryable.Where(a => a.Timestamp <= timestampMax);
            }
            
            // Filter by notification sent
            if (input.NotificationSent.HasValue)
            {
                queryable = queryable.Where(a => a.NotificationSent == input.NotificationSent.Value);
            }
            
            return queryable;
        }
        
        /// <summary>
        /// Gets data source information for display
        /// </summary>
        private async Task<KpiDataSourceDto[]> GetDataSourcesInfoAsync(Guid[] dataSourceIds)
        {
            if (dataSourceIds == null || dataSourceIds.Length == 0)
            {
                return Array.Empty<KpiDataSourceDto>();
            }
            
            var result = new List<KpiDataSourceDto>();
            
            foreach (var dataSourceId in dataSourceIds)
            {
                try
                {
                    var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                    
                    result.Add(new KpiDataSourceDto
                    {
                        Id = dataSource.Id,
                        Name = dataSource.Name,
                        Type = dataSource.Type.ToString()
                    });
                }
                catch
                {
                    // If data source not found, add with unknown name
                    result.Add(new KpiDataSourceDto
                    {
                        Id = dataSourceId,
                        Name = "Unknown",
                        Type = "Unknown"
                    });
                }
            }
            
            return result.ToArray();
        }
        
        /// <summary>
        /// Error handling helper method
        /// </summary>
        private async Task HandleErrorAsync(Exception ex, LogLevel logLevel = LogLevel.Error)
        {
            // Log the error
            Logger.Log(logLevel, ex, "Error in KpiAppService: {Message}", ex.Message);
            
            // Handle specific exceptions
            if (ex is EntityNotFoundException)
            {
                throw new UserFriendlyException(
                    L["AnalyticsReporting:ErrorMessage:EntityNotFound", L["AnalyticsReporting:Entity:KPI"]],
                    null,
                    ex);
            }
            
            if (ex is AnalyticsReportingDomainException domainException)
            {
                throw new UserFriendlyException(
                    L[domainException.Code, domainException.Message],
                    domainException.Code,
                    ex);
            }
            
            // For other exceptions, throw a generic error
            throw new UserFriendlyException(
                L["AnalyticsReporting:ErrorMessage:GenericError"],
                null,
                ex);
        }
    }
}
