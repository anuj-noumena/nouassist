using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Traceability.Artifact.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Traceability.Artifact
{
    /// <summary>
    /// Application service interface for external artifact registry
    /// </summary>
    public interface IArtifactRegistryAppService : IApplicationService
    {
        /// <summary>
        /// Registers a new external artifact source
        /// </summary>
        Task RegisterArtifactSourceAsync(RegisterArtifactSourceDto input);
        
        /// <summary>
        /// Verifies if an artifact exists in its source system
        /// </summary>
        Task<bool> VerifyArtifactExistsAsync(VerifyArtifactDto input);
        
        /// <summary>
        /// Gets information about an artifact from its source system
        /// </summary>
        Task<ArtifactInfoDto> GetArtifactInfoAsync(VerifyArtifactDto input);
    }
}
