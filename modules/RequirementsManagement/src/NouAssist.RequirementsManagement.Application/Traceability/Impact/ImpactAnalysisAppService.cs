using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.Impact.Dtos;
using NouAssist.RequirementsManagement.Traceability.Localization;
using NouAssist.RequirementsManagement.Traceability.Models;
using NouAssist.RequirementsManagement.Traceability.Permissions;
using NouAssist.RequirementsManagement.Traceability.Services.Interfaces;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Traceability.Impact
{
    /// <summary>
    /// Application service for impact analysis
    /// </summary>
    [Authorize(TraceabilityManagementPermissions.ImpactAnalysis.Default)]
    public class ImpactAnalysisAppService : ApplicationService, IImpactAnalysisAppService
    {
        private readonly ITraceabilityQueryService _traceabilityQueryService;
        private readonly ILogger<ImpactAnalysisAppService> _logger;

        public ImpactAnalysisAppService(
            ITraceabilityQueryService traceabilityQueryService,
            ILogger<ImpactAnalysisAppService> logger)
        {
            _traceabilityQueryService = traceabilityQueryService;
            _logger = logger;
            
            LocalizationResource = typeof(TraceabilityManagementResource);
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.ImpactAnalysis.Analyze)]
        public virtual async Task<ImpactAnalysisResultDto> AnalyzeImpactAsync(GetImpactAnalysisDto input)
        {
            try
            {
                // Perform impact analysis using domain service
                var result = await _traceabilityQueryService.PerformImpactAnalysisAsync(
                    input.ArtifactId,
                    input.ArtifactType,
                    input.MaxDepth);
                
                // Map to DTO with additional processing
                var resultDto = ObjectMapper.Map<ImpactAnalysisResult, ImpactAnalysisResultDto>(result);
                
                // Map impacted artifacts
                resultDto.ImpactedArtifacts = ObjectMapper.Map<List<ImpactedArtifact>, List<ImpactedArtifactDto>>(
                    result.ImpactedArtifacts);
                
                // Generate impact summary by type
                resultDto.ImpactSummaryByType = result.ImpactedArtifacts
                    .GroupBy(a => a.ArtifactType)
                    .ToDictionary(g => g.Key, g => g.Count());
                
                // Generate impact summary by depth
                resultDto.ImpactSummaryByDepth = result.ImpactedArtifacts
                    .GroupBy(a => a.ImpactDepth)
                    .ToDictionary(g => g.Key, g => g.Count());
                
                return resultDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:ImpactAnalysis:AnalyzeFailed"]);
            }
        }

        /// <summary>
        /// Handles and logs errors
        /// </summary>
        private Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "Error in ImpactAnalysisAppService: {Message}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
