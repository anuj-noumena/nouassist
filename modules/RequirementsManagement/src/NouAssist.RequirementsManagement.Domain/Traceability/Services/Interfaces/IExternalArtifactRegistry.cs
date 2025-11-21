using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Traceability.Models;

namespace NouAssist.RequirementsManagement.Traceability.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing external artifact sources
    /// </summary>
    public interface IExternalArtifactRegistry
    {
        /// <summary>
        /// Registers a new external artifact source
        /// </summary>
        Task RegisterArtifactSourceAsync(
            string sourceName,
            ArtifactType artifactType,
            string baseUri,
            Dictionary<string, string> connectionParameters);

        /// <summary>
        /// Verifies if an artifact exists in its source system
        /// </summary>
        Task<bool> VerifyArtifactExistsAsync(
            string artifactId,
            ArtifactType artifactType,
            string version = null);

        /// <summary>
        /// Gets the latest modification time for an artifact from its source system
        /// </summary>
        Task<DateTime?> GetArtifactLastModifiedTimeAsync(
            string artifactId,
            ArtifactType artifactType,
            string version = null);
            
        /// <summary>
        /// Gets information about an artifact from its source system
        /// </summary>
        Task<ArtifactInfo> GetArtifactInfoAsync(
            string artifactId,
            ArtifactType artifactType,
            string version = null);
    }
}
