using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound.Dtos;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound
{
    public interface IFundraisingRoundAppService : IApplicationService
    {
        Task<FundraisingRoundDto> GetAsync(Guid id);
        
        Task<PagedResultDto<FundraisingRoundDto>> GetListAsync(GetFundraisingRoundListDto input);
        
        Task<FundraisingRoundDto> CreateAsync(CreateFundraisingRoundDto input);
        
        Task<FundraisingRoundDto> UpdateAsync(Guid id, UpdateFundraisingRoundDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<FundraisingRoundDto> UpdateStageAsync(Guid id, UpdateRoundStageDto input);
        
        Task<FundraisingRoundDto> UpdateCommittedAmountAsync(Guid id, UpdateCommittedAmountDto input);
        
        Task<PipelineStageDto> AddPipelineStageAsync(Guid id, CreatePipelineStageDto input);
        
        Task<PipelineStageDto> UpdatePipelineStageAsync(Guid id, Guid stageId, UpdatePipelineStageDto input);
        
        Task DeletePipelineStageAsync(Guid id, Guid stageId);
        
        Task<List<PipelineStageDto>> GetPipelineStagesAsync(Guid id);
        
        Task<List<ProspectDto>> GetProspectsAsync(Guid id);
        
        Task<List<FundraisingRoundDto>> GetActiveRoundsAsync();
        
        Task<List<FundraisingRoundDto>> GetAtRiskRoundsAsync();
    }
}
