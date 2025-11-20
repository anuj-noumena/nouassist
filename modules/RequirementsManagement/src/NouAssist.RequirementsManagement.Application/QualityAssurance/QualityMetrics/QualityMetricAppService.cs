using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.QualityAssurance.Entities;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.Permissions;
using NouAssist.RequirementsManagement.QualityAssurance.QualityMetrics.Dtos;
using NouAssist.RequirementsManagement.QualityAssurance.Repositories;
using NouAssist.RequirementsManagement.QualityAssurance.Services;
using NouAssist.RequirementsManagement.QualityAssurance.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Timing;

namespace NouAssist.RequirementsManagement.QualityAssurance.QualityMetrics
{
    [Authorize(QAPermissions.Report.Default)]
    public class QualityMetricAppService : ApplicationService, IQualityMetricAppService
    {
        private readonly IQualityMetricSnapshotRepository _metricRepository;
        private readonly IQualityReportingService _qualityReportingService;
        private readonly ILogger<QualityMetricAppService> _logger;
        private readonly IClock _clock;

        public QualityMetricAppService(
            IQualityMetricSnapshotRepository metricRepository,
            IQualityReportingService qualityReportingService,
            ILogger<QualityMetricAppService> logger,
            IClock clock)
        {
            _metricRepository = metricRepository;
            _qualityReportingService = qualityReportingService;
            _logger = logger;
            _clock = clock;
            LocalizationResource = typeof(QAResource);
        }

        [Authorize(QAPermissions.Report.ManageMetrics)]
        public virtual async Task<QualityMetricDto> CreateAsync(CreateQualityMetricDto input)
        {
            try
            {
                var metricSnapshot = await _qualityReportingService.CreateMetricSnapshotAsync(
                    input.Type,
                    input.Value,
                    input.Unit,
                    input.TestPlanId,
                    input.ReleaseVersion,
                    input.Context);
                
                return ObjectMapper.Map<QualityMetricSnapshot, QualityMetricDto>(metricSnapshot);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Report.View)]
        public virtual async Task<QualityMetricDto> GetAsync(Guid id)
        {
            try
            {
                var metric = await _metricRepository.GetAsync(id);
                return ObjectMapper.Map<QualityMetricSnapshot, QualityMetricDto>(metric);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["QualityMetric:NotFound", id]);
                }
                throw;
            }
        }

        [Authorize(QAPermissions.Report.View)]
        public virtual async Task<PagedResultDto<QualityMetricDto>> GetListAsync(GetQualityMetricListDto input)
        {
            try
            {
                var queryable = await _metricRepository.GetQueryableAsync();
                
                // Apply filters
                queryable = queryable
                    .WhereIf(input.Type.HasValue, 
                        m => EF.Property<string>(m, "Metric_Type") == input.Type.Value.ToString())
                    .WhereIf(input.TestPlanId.HasValue,
                        m => m.TestPlanId == input.TestPlanId.Value)
                    .WhereIf(!input.ReleaseVersion.IsNullOrWhiteSpace(),
                        m => m.ReleaseVersion == input.ReleaseVersion)
                    .WhereIf(input.FromDate.HasValue,
                        m => EF.Property<DateTime>(m, "Metric_Timestamp") >= input.FromDate.Value)
                    .WhereIf(input.ToDate.HasValue,
                        m => EF.Property<DateTime>(m, "Metric_Timestamp") <= input.ToDate.Value);
                
                // Get count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? "Metric_Timestamp desc")
                    .PageBy(input);
                
                // Execute query and map to DTOs
                var metrics = await AsyncExecuter.ToListAsync(queryable);
                var metricDtos = ObjectMapper.Map<List<QualityMetricSnapshot>, List<QualityMetricDto>>(metrics);
                
                return new PagedResultDto<QualityMetricDto>(totalCount, metricDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Report.View)]
        public virtual async Task<List<QualityMetricDto>> GetByTestPlanIdAsync(Guid testPlanId)
        {
            try
            {
                var metrics = await _metricRepository.GetByTestPlanIdAsync(testPlanId);
                return ObjectMapper.Map<List<QualityMetricSnapshot>, List<QualityMetricDto>>(metrics);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Report.View)]
        public virtual async Task<List<QualityMetricDto>> GetByReleaseVersionAsync(string releaseVersion)
        {
            try
            {
                var metrics = await _metricRepository.GetByReleaseVersionAsync(releaseVersion);
                return ObjectMapper.Map<List<QualityMetricSnapshot>, List<QualityMetricDto>>(metrics);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Report.View)]
        public virtual async Task<List<QualityMetricDto>> GetByMetricTypeAsync(MetricType metricType)
        {
            try
            {
                var metrics = await _metricRepository.GetByMetricTypeAsync(metricType);
                return ObjectMapper.Map<List<QualityMetricSnapshot>, List<QualityMetricDto>>(metrics);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Report.View)]
        public virtual async Task<List<QualityMetricDto>> GetByTimeRangeAsync(DateTime startTime, DateTime endTime)
        {
            try
            {
                var metrics = await _metricRepository.GetByTimeRangeAsync(startTime, endTime);
                return ObjectMapper.Map<List<QualityMetricSnapshot>, List<QualityMetricDto>>(metrics);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Report.ManageMetrics)]
        public virtual async Task<QualityMetricDto> CaptureTestPassRateAsync(Guid testPlanId)
        {
            try
            {
                var metric = await _qualityReportingService.CaptureTestPassRateAsync(testPlanId);
                return ObjectMapper.Map<QualityMetricSnapshot, QualityMetricDto>(metric);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Report.ManageMetrics)]
        public virtual async Task<QualityMetricDto> CaptureOpenDefectsAsync(string releaseVersion)
        {
            try
            {
                var metric = await _qualityReportingService.CaptureOpenDefectsAsync(releaseVersion);
                return ObjectMapper.Map<QualityMetricSnapshot, QualityMetricDto>(metric);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Report.View)]
        public virtual async Task<QualityMetricSummaryDto> GetReleaseSummaryAsync(string releaseVersion)
        {
            try
            {
                // Get all metrics for this release
                var allMetrics = await _metricRepository.GetByReleaseVersionAsync(releaseVersion);
                
                // Get the latest metrics of each type
                var testPassRateMetrics = allMetrics
                    .Where(m => m.Metric.Type == MetricType.TestPassRate)
                    .OrderByDescending(m => m.Metric.Timestamp)
                    .ToList();
                
                var defectDensityMetrics = allMetrics
                    .Where(m => m.Metric.Type == MetricType.DefectDensity)
                    .OrderByDescending(m => m.Metric.Timestamp)
                    .ToList();
                
                var openDefectsMetrics = allMetrics
                    .Where(m => m.Metric.Type == MetricType.OpenDefects)
                    .OrderByDescending(m => m.Metric.Timestamp)
                    .ToList();
                
                var criticalDefectsMetrics = allMetrics
                    .Where(m => m.Metric.Type == MetricType.CriticalDefects)
                    .OrderByDescending(m => m.Metric.Timestamp)
                    .ToList();
                
                // Create summary
                var summary = new QualityMetricSummaryDto
                {
                    ReleaseVersion = releaseVersion,
                    TestPassRate = testPassRateMetrics.FirstOrDefault()?.Metric.Value ?? 0,
                    DefectDensity = defectDensityMetrics.FirstOrDefault()?.Metric.Value ?? 0,
                    OpenDefectsCount = Convert.ToInt32(openDefectsMetrics.FirstOrDefault()?.Metric.Value ?? 0),
                    CriticalDefectsCount = Convert.ToInt32(criticalDefectsMetrics.FirstOrDefault()?.Metric.Value ?? 0),
                    TestPassRateHistory = ObjectMapper.Map<List<QualityMetricSnapshot>, List<QualityMetricDto>>(testPassRateMetrics),
                    OpenDefectsHistory = ObjectMapper.Map<List<QualityMetricSnapshot>, List<QualityMetricDto>>(openDefectsMetrics)
                };
                
                return summary;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "Error in QualityMetricAppService: {ErrorMessage}", exception.Message);
            
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
