using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Traceability.Localization;
using NouAssist.RequirementsManagement.Traceability.Models;
using NouAssist.RequirementsManagement.Traceability.Permissions;
using NouAssist.RequirementsManagement.Traceability.Services.Interfaces;
using NouAssist.RequirementsManagement.Traceability.TraceMatrix.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Traceability.TraceMatrix
{
    /// <summary>
    /// Application service for traceability matrix operations
    /// </summary>
    [Authorize(TraceabilityManagementPermissions.TraceMatrices.Default)]
    public class TraceMatrixAppService : ApplicationService, ITraceMatrixAppService
    {
        private readonly ITraceabilityQueryService _traceabilityQueryService;
        private readonly ILogger<TraceMatrixAppService> _logger;

        public TraceMatrixAppService(
            ITraceabilityQueryService traceabilityQueryService,
            ILogger<TraceMatrixAppService> logger)
        {
            _traceabilityQueryService = traceabilityQueryService;
            _logger = logger;
            
            LocalizationResource = typeof(TraceabilityManagementResource);
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.TraceMatrices.Generate)]
        public virtual async Task<TraceabilityMatrixDto> GenerateMatrixAsync(GetTraceabilityMatrixDto input)
        {
            try
            {
                // Generate matrix using domain service
                var matrix = await _traceabilityQueryService.GenerateMatrixAsync(
                    input.SourceType,
                    input.TargetType,
                    input.LinkType);
                
                // Map to DTO with additional processing
                var matrixDto = ObjectMapper.Map<TraceabilityMatrix, TraceabilityMatrixDto>(matrix);
                
                // Process source artifacts
                matrixDto.SourceArtifacts = matrix.SourceArtifacts
                    .Select(id => new ArtifactReferenceDto { Id = id, Name = id })
                    .ToList();
                
                // Process target artifacts
                matrixDto.TargetArtifacts = matrix.TargetArtifacts
                    .Select(id => new ArtifactReferenceDto { Id = id, Name = id })
                    .ToList();
                
                // Process cells with links
                foreach (var source in matrix.Links.Keys)
                {
                    foreach (var target in matrix.Links[source].Keys)
                    {
                        var cell = new MatrixCellDto
                        {
                            SourceId = source,
                            TargetId = target,
                            Links = matrix.Links[source][target]
                                .Select(link => new MatrixCellLinkDto
                                {
                                    Type = link.Type,
                                    LinkId = link.LinkId
                                })
                                .ToList()
                        };
                        
                        matrixDto.Cells.Add(cell);
                    }
                }
                
                return matrixDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:TraceMatrix:GenerateFailed"]);
            }
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.TraceMatrices.CheckCoverage)]
        public virtual async Task<CoverageReportDto> CheckCoverageAsync(GetCoverageReportDto input)
        {
            try
            {
                // Check coverage using domain service
                var report = await _traceabilityQueryService.CheckCoverageAsync(
                    input.SourceType,
                    input.TargetType,
                    input.LinkType);
                
                // Map to DTO and return
                return ObjectMapper.Map<CoverageReport, CoverageReportDto>(report);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:TraceMatrix:CoverageFailed"]);
            }
        }

        /// <summary>
        /// Handles and logs errors
        /// </summary>
        private Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "Error in TraceMatrixAppService: {Message}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
