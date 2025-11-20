using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords
{
    public interface IDecisionRecordAppService : IApplicationService
    {
        Task<DecisionRecordDto> GetAsync(Guid id);
        
        Task<PagedResultDto<DecisionRecordListDto>> GetListAsync(GetDecisionRecordListDto input);
        
        Task<DecisionRecordDto> CreateAsync(CreateDecisionRecordDto input);
        
        Task<DecisionRecordDto> UpdateAsync(Guid id, UpdateDecisionRecordDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<DecisionRecordDto> UpdateContextAsync(Guid id, UpdateDecisionContextDto input);
        
        Task<DecisionAlternativeDto> AddAlternativeAsync(Guid id, AddAlternativeDto input);
        
        Task<DecisionRecordDto> MakeDecisionAsync(Guid id, MakeDecisionDto input);
        
        Task<DecisionRecordDto> ImplementDecisionAsync(Guid id, ImplementDecisionDto input);
        
        Task<DecisionRecordDto> EvaluateOutcomeAsync(Guid id, EvaluateOutcomeDto input);
        
        Task<DecisionRecordDto> ReverseDecisionAsync(Guid id, string reason);
        
        Task<List<DecisionRecordListDto>> GetDecisionsByStatusAsync(DecisionStatus status);
        
        Task<List<DecisionRecordListDto>> GetRecentDecisionsAsync(DateTime sinceDate);
    }
}
