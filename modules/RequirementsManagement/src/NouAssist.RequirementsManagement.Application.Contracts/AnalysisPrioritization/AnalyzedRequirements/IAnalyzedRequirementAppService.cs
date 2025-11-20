using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements
{
    /// <summary>
    /// Application service for managing analyzed requirements
    /// </summary>
    public interface IAnalyzedRequirementAppService : IApplicationService
    {
        /// <summary>
        /// Gets an analyzed requirement by ID
        /// </summary>
        Task<AnalyzedRequirementDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a list of analyzed requirements with filtering
        /// </summary>
        Task<PagedResultDto<AnalyzedRequirementDto>> GetListAsync(GetAnalyzedRequirementListDto input);

        /// <summary>
        /// Creates a new analyzed requirement
        /// </summary>
        Task<AnalyzedRequirementDto> CreateAsync(CreateAnalyzedRequirementDto input);

        /// <summary>
        /// Updates an analyzed requirement
        /// </summary>
        Task<AnalyzedRequirementDto> UpdateAsync(Guid id, UpdateAnalyzedRequirementDto input);

        /// <summary>
        /// Deletes an analyzed requirement
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Sets an attribute on an analyzed requirement
        /// </summary>
        Task SetAttributeAsync(Guid id, SetAttributeDto input);

        /// <summary>
        /// Removes an attribute from an analyzed requirement
        /// </summary>
        Task RemoveAttributeAsync(Guid id, string attributeName);

        /// <summary>
        /// Adds a relationship between requirements
        /// </summary>
        Task AddRelationshipAsync(Guid id, AddRelationshipDto input);

        /// <summary>
        /// Removes a relationship between requirements
        /// </summary>
        Task RemoveRelationshipAsync(Guid id, Guid relationshipId);

        /// <summary>
        /// Sets impact assessment for a requirement
        /// </summary>
        Task SetImpactAssessmentAsync(Guid id, SetImpactAssessmentDto input);

        /// <summary>
        /// Performs impact analysis on a requirement
        /// </summary>
        Task<ImpactAssessmentDto> AnalyzeImpactAsync(Guid id);

        /// <summary>
        /// Gets requirements that need analysis
        /// </summary>
        Task<PagedResultDto<AnalyzedRequirementDto>> GetRequirementsNeedingAnalysisAsync(
            PagedResultRequestDto input);
    }
}
