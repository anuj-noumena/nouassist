using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck
{
    public interface IPitchDeckAppService : IApplicationService
    {
        Task<PitchDeckDto> GetAsync(Guid id);
        
        Task<PagedResultDto<PitchDeckDto>> GetListAsync(GetPitchDeckListDto input);
        
        Task<PitchDeckDto> CreateAsync(CreatePitchDeckDto input);
        
        Task<PitchDeckDto> UpdateAsync(Guid id, UpdatePitchDeckDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<PitchVersionDto> AddVersionAsync(Guid id, CreatePitchVersionDto input);
        
        Task<PitchVersionDto> UpdateVersionAsync(Guid id, Guid versionId, UpdatePitchVersionDto input);
        
        Task<List<PitchVersionDto>> GetVersionsAsync(Guid id);
        
        Task<PitchPresentationDto> AddPresentationAsync(Guid id, CreatePresentationDto input);
        
        Task<PitchPresentationDto> UpdatePresentationAsync(Guid id, Guid presentationId, UpdatePresentationDto input);
        
        Task DeletePresentationAsync(Guid id, Guid presentationId);
        
        Task<List<PitchPresentationDto>> GetPresentationsAsync(Guid id);
        
        Task<FeedbackRecordDto> AddFeedbackAsync(Guid id, CreateFeedbackDto input);
        
        Task<FeedbackRecordDto> UpdateFeedbackAsync(Guid id, Guid feedbackId, UpdateFeedbackDto input);
        
        Task DeleteFeedbackAsync(Guid id, Guid feedbackId);
        
        Task<List<FeedbackRecordDto>> GetFeedbacksAsync(Guid id);
        
        Task<PitchEffectivenessDto> AnalyzePitchEffectivenessAsync(Guid id);
    }
}
