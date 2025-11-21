using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Traceability.Artifact.Dtos;
using NouAssist.RequirementsManagement.Traceability.Exceptions;
using NouAssist.RequirementsManagement.Traceability.Localization;
using NouAssist.RequirementsManagement.Traceability.Models;
using NouAssist.RequirementsManagement.Traceability.Permissions;
using NouAssist.RequirementsManagement.Traceability.Services.Interfaces;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Traceability.Artifact
{
    /// <summary>
    /// Application service for external artifact registry
    /// </summary>
    [Authorize(TraceabilityManagementPermissions.ArtifactRegistry.Default)]
    public class ArtifactRegistryAppService : ApplicationService, IArtifactRegistryAppService
    {
        private readonly IExternalArtifactRegistry _externalArtifactRegistry;
        private readonly ILogger<ArtifactRegistryAppService> _logger;

        public ArtifactRegistryAppService(
            IExternalArtifactRegistry externalArtifactRegistry,
            ILogger<ArtifactRegistryAppService> logger)
        {
            _externalArtifactRegistry = externalArtifactRegistry;
            _logger = logger;
            
            LocalizationResource = typeof(TraceabilityManagementResource);
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.ArtifactRegistry.Register)]
        public virtual async Task RegisterArtifactSourceAsync(RegisterArtifactSourceDto input)
        {
            try
            {
                // Register artifact source using domain service
                await _externalArtifactRegistry.RegisterArtifactSourceAsync(
                    input.SourceName,
                    input.ArtifactType,
                    input.BaseUri,
                    input.ConnectionParameters);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:ArtifactRegistry:RegisterFailed"]);
            }
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.ArtifactRegistry.Verify)]
        public virtual async Task<bool> VerifyArtifactExistsAsync(VerifyArtifactDto input)
        {
            try
            {
                // Verify artifact exists using domain service
                return await _externalArtifactRegistry.VerifyArtifactExistsAsync(
                    input.ArtifactId,
                    input.ArtifactType,
                    input.Version);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:ArtifactRegistry:VerifyFailed"]);
            }
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.ArtifactRegistry.Verify)]
        public virtual async Task<ArtifactInfoDto> GetArtifactInfoAsync(VerifyArtifactDto input)
        {
            try
            {
                // Get artifact info from domain service
                var artifactInfo = await _externalArtifactRegistry.GetArtifactInfoAsync(
                    input.ArtifactId,
                    input.ArtifactType,
                    input.Version);
                
                // Map to DTO and set exists flag
                var artifactInfoDto = ObjectMapper.Map<ArtifactInfo, ArtifactInfoDto>(artifactInfo);
                artifactInfoDto.Exists = true;
                
                return artifactInfoDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                
                // If the artifact doesn't exist, return info with exists = false
                if (ex is ArtifactNotFoundException)
                {
                    return new ArtifactInfoDto
                    {
                        Id = input.ArtifactId,
                        Type = input.ArtifactType,
                        Version = input.Version,
                        Exists = false,
                        LastModified = Clock.Now
                    };
                }
                
                throw new UserFriendlyException(L["Error:ArtifactRegistry:GetInfoFailed"]);
            }
        }

        /// <summary>
        /// Handles and logs errors
        /// </summary>
        private Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "Error in ArtifactRegistryAppService: {Message}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
