using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.AnalysisPrioritization.GlossaryTerms.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.GlossaryTerms
{
    /// <summary>
    /// Application service for managing glossary terms
    /// </summary>
    public interface IGlossaryTermAppService : IApplicationService
    {
        /// <summary>
        /// Gets a glossary term by ID
        /// </summary>
        Task<GlossaryTermDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a list of glossary terms with filtering
        /// </summary>
        Task<PagedResultDto<GlossaryTermDto>> GetListAsync(GetGlossaryTermListDto input);

        /// <summary>
        /// Creates a new glossary term
        /// </summary>
        Task<GlossaryTermDto> CreateAsync(CreateGlossaryTermDto input);

        /// <summary>
        /// Updates a glossary term
        /// </summary>
        Task<GlossaryTermDto> UpdateAsync(Guid id, UpdateGlossaryTermDto input);

        /// <summary>
        /// Deletes a glossary term
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Approves a glossary term
        /// </summary>
        Task ApproveAsync(Guid id);

        /// <summary>
        /// Revokes approval of a glossary term
        /// </summary>
        Task RevokeApprovalAsync(Guid id);

        /// <summary>
        /// Sets related terms for a glossary term
        /// </summary>
        Task SetRelatedTermsAsync(Guid id, SetRelatedTermsDto input);

        /// <summary>
        /// Sets alternative forms (synonyms and acronyms) for a glossary term
        /// </summary>
        Task SetAlternativeFormsAsync(Guid id, SetAlternativeFormsDto input);

        /// <summary>
        /// Searches glossary terms by name or definition
        /// </summary>
        Task<List<GlossaryTermDto>> SearchAsync(string searchTerm, bool approvedOnly = true);

        /// <summary>
        /// Gets approved terms only
        /// </summary>
        Task<List<GlossaryTermDto>> GetApprovedTermsAsync();

        /// <summary>
        /// Gets terms pending approval
        /// </summary>
        Task<List<GlossaryTermDto>> GetPendingApprovalAsync();
    }
}
