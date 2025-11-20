using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet
{
    public interface ITermSheetAppService : IApplicationService
    {
        Task<TermSheetOfferDto> GetAsync(Guid id);
        
        Task<PagedResultDto<TermSheetOfferDto>> GetListAsync(GetTermSheetListDto input);
        
        Task<TermSheetOfferDto> CreateAsync(CreateTermSheetOfferDto input);
        
        Task<TermSheetOfferDto> UpdateAsync(Guid id, UpdateTermSheetOfferDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<TermSheetOfferDto> UpdateStatusAsync(Guid id, UpdateTermSheetStatusDto input);
        
        Task<TermSheetClauseDto> AddClauseAsync(Guid id, CreateTermSheetClauseDto input);
        
        Task<TermSheetClauseDto> UpdateClauseAsync(Guid id, Guid clauseId, UpdateTermSheetClauseDto input);
        
        Task DeleteClauseAsync(Guid id, Guid clauseId);
        
        Task<List<TermSheetClauseDto>> GetClausesAsync(Guid id);
        
        Task<NegotiationNoteDto> AddNegotiationNoteAsync(Guid id, CreateNegotiationNoteDto input);
        
        Task<NegotiationNoteDto> UpdateNegotiationNoteAsync(Guid id, Guid noteId, UpdateNegotiationNoteDto input);
        
        Task DeleteNegotiationNoteAsync(Guid id, Guid noteId);
        
        Task<List<NegotiationNoteDto>> GetNegotiationNotesAsync(Guid id);
        
        Task<TermSheetComparisonResultDto> CompareTermSheetsAsync(List<Guid> termSheetIds);
        
        Task<TermSheetRankingResultDto> RankTermSheetsAsync(List<Guid> termSheetIds);
        
        Task<List<TermSheetOfferDto>> GetUrgentTermSheetsAsync();
        
        Task<List<TermSheetOfferDto>> GetFavorableTermSheetsAsync();
    }
}
