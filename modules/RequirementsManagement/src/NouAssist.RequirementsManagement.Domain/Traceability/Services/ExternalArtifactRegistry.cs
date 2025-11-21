using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Traceability.Models;
using NouAssist.RequirementsManagement.Traceability.Services.Interfaces;
using Volo.Abp.Domain.Services;
using Volo.Abp.Timing;

namespace NouAssist.RequirementsManagement.Traceability.Services
{
    /// <summary>
    /// Service for managing external artifact sources
    /// </summary>
    public class ExternalArtifactRegistry : DomainService, IExternalArtifactRegistry
    {
        private readonly ILogger<ExternalArtifactRegistry> _logger;
        private readonly IClock _clock;
        
        // In a real implementation, this would be persisted to a database
        private readonly Dictionary<string, ArtifactSourceConfiguration> _artifactSources;

        public ExternalArtifactRegistry(
            ILogger<ExternalArtifactRegistry> logger,
            IClock clock)
        {
            _logger = logger;
            _clock = clock;
            _artifactSources = new Dictionary<string, ArtifactSourceConfiguration>();
        }

        /// <inheritdoc />
        public Task RegisterArtifactSourceAsync(
            string sourceName,
            ArtifactType artifactType,
            string baseUri,
            Dictionary<string, string> connectionParameters)
        {
            var key = GetSourceKey(artifactType);
            
            _artifactSources[key] = new ArtifactSourceConfiguration
            {
                SourceName = sourceName,
                ArtifactType = artifactType,
                BaseUri = baseUri,
                ConnectionParameters = connectionParameters,
                RegisteredAt = _clock.Now
            };
            
            _logger.LogInformation(
                "Registered artifact source {SourceName} for type {ArtifactType} with URI {BaseUri}",
                sourceName,
                artifactType,
                baseUri);
                
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<bool> VerifyArtifactExistsAsync(
            string artifactId,
            ArtifactType artifactType,
            string version = null)
        {
            // In a real implementation, this would contact the external system to verify
            // For this example, we'll just check if we have a source registered
            var key = GetSourceKey(artifactType);
            var exists = _artifactSources.ContainsKey(key);
            
            _logger.LogInformation(
                "Verified artifact {ArtifactId} of type {ArtifactType}{Version} exists: {Exists}",
                artifactId,
                artifactType,
                version != null ? $" version {version}" : "",
                exists);
                
            return Task.FromResult(exists);
        }

        /// <inheritdoc />
        public Task<DateTime?> GetArtifactLastModifiedTimeAsync(
            string artifactId,
            ArtifactType artifactType,
            string version = null)
        {
            // In a real implementation, this would query the external system
            // For this example, we'll return the current time
            var lastModified = _clock.Now.AddDays(-1);
            
            _logger.LogInformation(
                "Retrieved last modified time for artifact {ArtifactId} of type {ArtifactType}{Version}: {LastModified}",
                artifactId,
                artifactType,
                version != null ? $" version {version}" : "",
                lastModified);
                
            return Task.FromResult<DateTime?>(lastModified);
        }

        /// <inheritdoc />
        public Task<ArtifactInfo> GetArtifactInfoAsync(
            string artifactId,
            ArtifactType artifactType,
            string version = null)
        {
            // In a real implementation, this would query the external system
            // For this example, we'll create a mock artifact info
            var artifactInfo = new ArtifactInfo
            {
                Id = artifactId,
                Type = artifactType,
                Name = $"Sample {artifactType} {artifactId}",
                Version = version,
                LastModified = _clock.Now.AddDays(-1),
                Uri = $"https://example.com/{artifactType.ToString().ToLower()}/{artifactId}"
            };
            
            _logger.LogInformation(
                "Retrieved info for artifact {ArtifactId} of type {ArtifactType}{Version}",
                artifactId,
                artifactType,
                version != null ? $" version {version}" : "");
                
            return Task.FromResult(artifactInfo);
        }

        private string GetSourceKey(ArtifactType artifactType)
        {
            return artifactType.ToString();
        }
    }
}
