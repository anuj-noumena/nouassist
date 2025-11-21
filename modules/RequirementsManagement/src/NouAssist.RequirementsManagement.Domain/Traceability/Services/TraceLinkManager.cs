using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.Events;
using NouAssist.RequirementsManagement.Traceability.Exceptions;
using NouAssist.RequirementsManagement.Traceability.Repositories;
using NouAssist.RequirementsManagement.Traceability.Services.Interfaces;
using NouAssist.RequirementsManagement.Traceability.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace NouAssist.RequirementsManagement.Traceability.Services
{
    /// <summary>
    /// Implementation of the trace link management service
    /// </summary>
    public class TraceLinkManager : DomainService, ITraceLinkManager
    {
        private readonly ITraceLinkRepository _traceLinkRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IClock _clock;
        private readonly ILogger<TraceLinkManager> _logger;

        public TraceLinkManager(
            ITraceLinkRepository traceLinkRepository,
            IGuidGenerator guidGenerator,
            IClock clock,
            ILogger<TraceLinkManager> logger)
        {
            _traceLinkRepository = traceLinkRepository;
            _guidGenerator = guidGenerator;
            _clock = clock;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TraceLink> CreateAsync(
            ArtifactIdentifier sourceArtifactId,
            string sourceArtifactName,
            ArtifactIdentifier targetArtifactId,
            string targetArtifactName,
            LinkType linkType,
            LinkDirection linkDirection,
            string customLinkType = null,
            string description = null)
        {
            // Check if a link with the same artifacts and type already exists
            bool exists = await _traceLinkRepository.ExistsAsync(
                sourceArtifactId.Id,
                targetArtifactId.Id,
                linkType);

            if (exists)
            {
                throw new DuplicateLinkException(sourceArtifactId.Id, targetArtifactId.Id, linkType);
            }

            // Create the trace link
            var traceLink = TraceLink.Create(
                _guidGenerator.Create(),
                sourceArtifactId,
                sourceArtifactName,
                targetArtifactId,
                targetArtifactName,
                linkType,
                linkDirection,
                customLinkType,
                description,
                CurrentTenant.Id);

            // Save the trace link
            await _traceLinkRepository.InsertAsync(traceLink, autoSave: true);

            // Add distributed event
            traceLink.AddDistributedEvent(new TraceLinkCreatedEto
            {
                Id = traceLink.Id,
                SourceArtifactId = sourceArtifactId.Id,
                TargetArtifactId = targetArtifactId.Id,
                LinkType = linkType,
                LinkDirection = linkDirection,
                TenantId = traceLink.TenantId,
                CreationTime = _clock.Now
            });

            _logger.LogInformation(
                "Created trace link {LinkId} from {SourceId} to {TargetId} of type {LinkType}",
                traceLink.Id,
                sourceArtifactId.Id,
                targetArtifactId.Id,
                linkType);

            return traceLink;
        }

        /// <inheritdoc />
        public async Task<TraceLink> UpdateAsync(
            Guid id,
            LinkType linkType,
            LinkDirection linkDirection,
            string customLinkType = null,
            string description = null)
        {
            var traceLink = await _traceLinkRepository.GetAsync(id);
            
            if (traceLink == null)
            {
                throw new TraceLinkNotFoundException(id);
            }
            
            traceLink.Update(
                linkType,
                linkDirection,
                customLinkType,
                description);
                
            await _traceLinkRepository.UpdateAsync(traceLink, autoSave: true);
            
            _logger.LogInformation(
                "Updated trace link {LinkId} of type {LinkType}",
                traceLink.Id,
                linkType);
                
            return traceLink;
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id)
        {
            var traceLink = await _traceLinkRepository.GetAsync(id);
            
            if (traceLink == null)
            {
                throw new TraceLinkNotFoundException(id);
            }
            
            // Capture details for event before deletion
            var sourceArtifactId = traceLink.SourceArtifact.ArtifactIdentifier.Id;
            var targetArtifactId = traceLink.TargetArtifact.ArtifactIdentifier.Id;
            var tenantId = traceLink.TenantId;
            
            await _traceLinkRepository.DeleteAsync(traceLink, autoSave: true);
            
            // Publish event about deleted link
            await DistributedEventBus.PublishAsync(new TraceLinkDeletedEto
            {
                Id = id,
                SourceArtifactId = sourceArtifactId,
                TargetArtifactId = targetArtifactId,
                TenantId = tenantId
            });
            
            _logger.LogInformation(
                "Deleted trace link {LinkId} from {SourceId} to {TargetId}",
                id,
                sourceArtifactId,
                targetArtifactId);
        }

        /// <inheritdoc />
        public async Task<TraceLink> VerifyAsync(Guid id, Guid userId)
        {
            var traceLink = await _traceLinkRepository.GetAsync(id);
            
            if (traceLink == null)
            {
                throw new TraceLinkNotFoundException(id);
            }
            
            traceLink.MarkAsVerified(_clock.Now, userId);
            
            await _traceLinkRepository.UpdateAsync(traceLink, autoSave: true);
            
            _logger.LogInformation(
                "Verified trace link {LinkId} by user {UserId}",
                traceLink.Id,
                userId);
                
            return traceLink;
        }

        /// <inheritdoc />
        public async Task UpdateArtifactModificationAsync(
            string artifactId,
            ArtifactType artifactType,
            DateTime modificationTime)
        {
            // Get all links where this artifact is either source or target
            var links = await _traceLinkRepository.GetLinksForArtifactAsync(artifactId, artifactType);
            
            foreach (var link in links)
            {
                // Update the appropriate artifact reference based on whether it's source or target
                if (link.SourceArtifact.ArtifactIdentifier.Id == artifactId && 
                    link.SourceArtifact.ArtifactIdentifier.Type == artifactType)
                {
                    link.UpdateSourceArtifactModificationInfo(modificationTime);
                }
                else if (link.TargetArtifact.ArtifactIdentifier.Id == artifactId && 
                         link.TargetArtifact.ArtifactIdentifier.Type == artifactType)
                {
                    link.UpdateTargetArtifactModificationInfo(modificationTime);
                }
                
                await _traceLinkRepository.UpdateAsync(link);
            }
            
            await _traceLinkRepository.UnitOfWork.SaveChangesAsync();
            
            _logger.LogInformation(
                "Updated modification info for artifact {ArtifactId} of type {ArtifactType}, affecting {LinkCount} trace links",
                artifactId,
                artifactType,
                links.Count);
        }

        /// <inheritdoc />
        public async Task<List<TraceLink>> GetLinksForArtifactAsync(
            string artifactId,
            ArtifactType artifactType = ArtifactType.Other)
        {
            return await _traceLinkRepository.GetLinksForArtifactAsync(
                artifactId,
                artifactType);
        }

        /// <inheritdoc />
        public async Task<List<TraceLink>> GetSuspectLinksAsync(int maxResultCount = 50)
        {
            return await _traceLinkRepository.GetSuspectLinksAsync(maxResultCount);
        }
    }
}
