using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect
{
    public interface IProspectAppService : IApplicationService
    {
        Task<ProspectDto> GetAsync(Guid id);
        
        Task<PagedResultDto<ProspectDto>> GetListAsync(GetProspectListDto input);
        
        Task<ProspectDto> CreateAsync(CreateProspectDto input);
        
        Task<ProspectDto> UpdateAsync(Guid id, UpdateProspectDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<ProspectDto> UpdateStatusAsync(Guid id, UpdateProspectStatusDto input);
        
        Task<InteractionLogDto> AddInteractionAsync(Guid id, CreateInteractionDto input);
        
        Task<InteractionLogDto> UpdateInteractionAsync(Guid id, Guid interactionId, UpdateInteractionDto input);
        
        Task DeleteInteractionAsync(Guid id, Guid interactionId);
        
        Task<List<InteractionLogDto>> GetInteractionsAsync(Guid id);
        
        Task<ProspectDto> UpdateScoresAsync(Guid id);
        
        Task<ProspectDto> UpdateForecastAsync(Guid id, decimal baseAmount);
        
        Task<List<ProspectDto>> GetHighValueProspectsAsync();
        
        Task<List<ProspectDto>> GetProspectsNeedingFollowUpAsync();
    }
}
