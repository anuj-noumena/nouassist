using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.AnalysisPrioritization.GlossaryTerms.Dtos;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Localization;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Permissions;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Services;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.GlossaryTerms
{
    [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.Default)]
    public class GlossaryTermAppService : ApplicationService, IGlossaryTermAppService
    {
        private readonly IGlossaryTermRepository _glossaryTermRepository;
        private readonly IGlossaryTermManager _glossaryTermManager;

        public GlossaryTermAppService(
            IGlossaryTermRepository glossaryTermRepository,
            IGlossaryTermManager glossaryTermManager)
        {
            _glossaryTermRepository = glossaryTermRepository;
            _glossaryTermManager = glossaryTermManager;

            LocalizationResource = typeof(RequirementsAnalysisToolsResource);
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.View)]
        public virtual async Task<GlossaryTermDto> GetAsync(Guid id)
        {
            try
            {
                var glossaryTerm = await _glossaryTermRepository.GetAsync(id);
                var dto = ObjectMapper.Map<GlossaryTerm, GlossaryTermDto>(glossaryTerm);
                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.View)]
        public virtual async Task<PagedResultDto<GlossaryTermDto>> GetListAsync(GetGlossaryTermListDto input)
        {
            try
            {
                var query = await _glossaryTermRepository.GetQueryableAsync();

                // Apply filters
                query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(input.Filter!) || x.Definition.Contains(input.Filter!));

                if (input.ApprovedOnly.HasValue)
                {
                    query = query.Where(x => x.IsApproved == input.ApprovedOnly.Value);
                }

                if (input.HasSynonyms.HasValue && input.HasSynonyms.Value)
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.Synonyms));
                }

                if (input.HasAcronyms.HasValue && input.HasAcronyms.Value)
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.Acronyms));
                }

                var totalCount = await AsyncExecuter.CountAsync(query);

                var items = new List<GlossaryTerm>();
                if (totalCount > 0)
                {
                    items = await AsyncExecuter.ToListAsync(
                        query.OrderBy(input.Sorting ?? nameof(GlossaryTerm.Name))
                            .PageBy(input)
                    );
                }

                return new PagedResultDto<GlossaryTermDto>(
                    totalCount,
                    ObjectMapper.Map<List<GlossaryTerm>, List<GlossaryTermDto>>(items)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.Create)]
        public virtual async Task<GlossaryTermDto> CreateAsync(CreateGlossaryTermDto input)
        {
            try
            {
                var glossaryTerm = await _glossaryTermManager.CreateAsync(
                    input.Name,
                    input.Definition,
                    input.Context,
                    input.Examples
                );

                return ObjectMapper.Map<GlossaryTerm, GlossaryTermDto>(glossaryTerm);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.Update)]
        public virtual async Task<GlossaryTermDto> UpdateAsync(Guid id, UpdateGlossaryTermDto input)
        {
            try
            {
                var glossaryTerm = await _glossaryTermManager.UpdateDefinitionAsync(id, input.Definition);
                
                glossaryTerm.UpdateContext(input.Context);
                glossaryTerm.UpdateExamples(input.Examples);

                await _glossaryTermRepository.UpdateAsync(glossaryTerm);

                return ObjectMapper.Map<GlossaryTerm, GlossaryTermDto>(glossaryTerm);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var canDelete = await _glossaryTermManager.CanDeleteAsync(id);
                if (!canDelete)
                {
                    throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:CannotDeleteReferencedTerm");
                }

                await _glossaryTermRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.Approve)]
        public virtual async Task ApproveAsync(Guid id)
        {
            try
            {
                await _glossaryTermManager.ApproveAsync(
                    id,
                    CurrentUser.GetId(),
                    Clock.Now
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.Approve)]
        public virtual async Task RevokeApprovalAsync(Guid id)
        {
            try
            {
                await _glossaryTermManager.RevokeApprovalAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.ManageRelatedTerms)]
        public virtual async Task SetRelatedTermsAsync(Guid id, SetRelatedTermsDto input)
        {
            try
            {
                await _glossaryTermManager.SetRelatedTermsAsync(id, input.RelatedTermIds);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.Update)]
        public virtual async Task SetAlternativeFormsAsync(Guid id, SetAlternativeFormsDto input)
        {
            try
            {
                var glossaryTerm = await _glossaryTermRepository.GetAsync(id);
                
                glossaryTerm.SetSynonyms(input.Synonyms);
                glossaryTerm.SetAcronyms(input.Acronyms);

                await _glossaryTermRepository.UpdateAsync(glossaryTerm);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.View)]
        public virtual async Task<List<GlossaryTermDto>> SearchAsync(string searchTerm, bool approvedOnly = true)
        {
            try
            {
                var terms = await _glossaryTermManager.SearchAsync(searchTerm, approvedOnly);
                return ObjectMapper.Map<List<GlossaryTerm>, List<GlossaryTermDto>>(terms);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.View)]
        public virtual async Task<List<GlossaryTermDto>> GetApprovedTermsAsync()
        {
            try
            {
                var terms = await _glossaryTermRepository.GetApprovedTermsAsync();
                return ObjectMapper.Map<List<GlossaryTerm>, List<GlossaryTermDto>>(terms);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.GlossaryTerms.View)]
        public virtual async Task<List<GlossaryTermDto>> GetPendingApprovalAsync()
        {
            try
            {
                var terms = await _glossaryTermRepository.GetPendingApprovalAsync();
                return ObjectMapper.Map<List<GlossaryTerm>, List<GlossaryTermDto>>(terms);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, 
                "Error in GlossaryTermAppService: {ErrorMessage}", 
                exception.Message);
        }
    }
}
