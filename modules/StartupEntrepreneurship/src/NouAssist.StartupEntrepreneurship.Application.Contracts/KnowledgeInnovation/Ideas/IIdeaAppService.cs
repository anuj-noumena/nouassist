using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Ideas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Ideas
{
    public interface IIdeaAppService : IApplicationService
    {
        Task<IdeaDto> GetAsync(Guid id);
        
        Task<PagedResultDto<IdeaListDto>> GetListAsync(GetIdeaListDto input);
        
        Task<IdeaDto> CreateAsync(CreateIdeaDto input);
        
        Task<IdeaDto> UpdateAsync(Guid id, UpdateIdeaDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<IdeaDto> StartEvaluationAsync(Guid id);
        
        Task<IdeaDto> EvaluateAsync(Guid id, EvaluateIdeaDto input);
        
        Task<IdeaDto> ApproveAsync(Guid id);
        
        Task<IdeaDto> RejectAsync(Guid id);
        
        Task<IdeaDto> StartImplementationAsync(Guid id);
        
        Task<IdeaDto> MarkImplementedAsync(Guid id);
        
        Task<IdeaDto> ArchiveAsync(Guid id);
        
        Task<List<IdeaListDto>> GetIdeasByStatusAsync(IdeaStatus status);
        
        Task<List<IdeaListDto>> GetHighPriorityIdeasAsync();
        
        Task<List<IdeaListDto>> GetIdeasForEvaluationAsync();
    }
}
