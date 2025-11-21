using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.ValueObjects;

namespace NouAssist.RequirementsManagement.Traceability.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing trace links
    /// </summary>
    public interface ITraceLinkManager
    {
        /// <summary>
        /// Creates a new trace link between two artifacts
        /// </summary>
        Task<TraceLink> CreateAsync(
            ArtifactIdentifier sourceArtifactId,
            string sourceArtifactName,
            ArtifactIdentifier targetArtifactId,
            string targetArtifactName,
            LinkType linkType,
            LinkDirection linkDirection,
            string customLinkType = null,
            string description = null);

        /// <summary>
        /// Updates an existing trace link
        /// </summary>
        Task<TraceLink> UpdateAsync(
            Guid id,
            LinkType linkType,
            LinkDirection linkDirection,
            string customLinkType = null,
            string description = null);

        /// <summary>
        /// Deletes a trace link
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Marks a trace link as verified
        /// </summary>
        Task<TraceLink> VerifyAsync(Guid id, Guid userId);

        /// <summary>
        /// Updates artifact modification information and marks related links as suspect if needed
        /// </summary>
        Task UpdateArtifactModificationAsync(
            string artifactId,
            ArtifactType artifactType,
            DateTime modificationTime);
            
        /// <summary>
        /// Gets all trace links for a specific artifact
        /// </summary>
        Task<List<TraceLink>> GetLinksForArtifactAsync(
            string artifactId,
            ArtifactType artifactType = ArtifactType.Other);
            
        /// <summary>
        /// Gets all suspect links that need verification
        /// </summary>
        Task<List<TraceLink>> GetSuspectLinksAsync(int maxResultCount = 50);
    }
}
