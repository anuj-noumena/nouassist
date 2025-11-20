using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.OpportunityManagement.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.OpportunityManagement
{
    /// <summary>
    /// Application service interface for Opportunity Management
    /// </summary>
    public interface IOpportunityManagementAppService : IApplicationService
    {
        Task<OpportunityManagementRecordDto> GetAsync(Guid id);
        Task<PagedResultDto<OpportunityManagementRecordDto>> GetListAsync(PagedResultRequestDto input);
        Task<OpportunityManagementRecordDto> CreateAsync(CreateOpportunityManagementRecordDto input);
        Task<OpportunityManagementRecordDto> UpdateAsync(Guid id, CreateOpportunityManagementRecordDto input);
        Task DeleteAsync(Guid id);

        // Filter Management
        Task<OpportunityManagementRecordDto> AddFilterRuleAsync(Guid recordId, AddFilterRuleDto input);
        Task<OpportunityManagementRecordDto> ActivateFilterRuleAsync(Guid recordId, Guid ruleId);
        Task<OpportunityManagementRecordDto> DeactivateFilterRuleAsync(Guid recordId, Guid ruleId);

        // Opportunity Evaluation
        Task<decimal> EvaluateOpportunityAsync(Guid recordId, EvaluateOpportunityDto input);
        Task<decimal> CalculateOpportunityCostAsync(Guid recordId, decimal opportunityValue, decimal requiredInvestment, decimal[] alternativeValues);

        // Strategic Experiments
        Task<OpportunityManagementRecordDto> AddStrategicExperimentAsync(Guid recordId, string experimentDescription);

        // Focus Management
        Task<OpportunityManagementRecordDto> UpdateFocusLevelAsync(Guid recordId, FocusLevel newLevel, string reason = null);

        // Reporting
        Task<ListResultDto<OpportunityManagementRecordDto>> GetActiveRecordsAsync();
        Task<ListResultDto<OpportunityManagementRecordDto>> GetRecordsByFocusLevelAsync(FocusLevel focusLevel);
    }
}
