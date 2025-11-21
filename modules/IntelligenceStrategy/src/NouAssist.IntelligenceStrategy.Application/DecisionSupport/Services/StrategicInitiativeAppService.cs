using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.DecisionSupport.Aggregates;
using NouAssist.DecisionSupport.Dtos.StrategicInitiative;
using NouAssist.DecisionSupport.Interfaces.Repositories;
using NouAssist.DecisionSupport.Interfaces.Services;
using NouAssist.DecisionSupport.Permissions;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Services;
using NouAssist.IntelligenceStrategy.DecisionSupport.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.DecisionSupport.Services
{
    [Authorize(DecisionSupportPermissions.StrategicInitiatives.Default)]
    public class StrategicInitiativeAppService :
        CrudAppService<
            StrategicInitiativeProposal,
            StrategicInitiativeDto,
            Guid,
            GetStrategicInitiativesInput,
            CreateStrategicInitiativeDto,
            UpdateStrategicInitiativeDto>,
        IStrategicInitiativeAppService
    {
        private readonly IStrategicInitiativeRepository _initiativeRepository;
        private readonly IStrategicInitiativeManager _initiativeManager;

        public StrategicInitiativeAppService(
            IStrategicInitiativeRepository initiativeRepository,
            IStrategicInitiativeManager initiativeManager)
            : base(initiativeRepository)
        {
            _initiativeRepository = initiativeRepository;
            _initiativeManager = initiativeManager;
            
            GetPolicyName = DecisionSupportPermissions.StrategicInitiatives.Default;
            GetListPolicyName = DecisionSupportPermissions.StrategicInitiatives.Default;
            CreatePolicyName = DecisionSupportPermissions.StrategicInitiatives.Create;
            UpdatePolicyName = DecisionSupportPermissions.StrategicInitiatives.Edit;
            DeletePolicyName = DecisionSupportPermissions.StrategicInitiatives.Delete;
        }

        protected override async Task<IQueryable<StrategicInitiativeProposal>> CreateFilteredQueryAsync(GetStrategicInitiativesInput input)
        {
            var query = await base.CreateFilteredQueryAsync(input);
            
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(x => x.Title.Contains(input.Filter) || 
                                         x.Description.Contains(input.Filter));
            }
            
            if (input.Status.HasValue)
            {
                query = query.Where(x => x.Status == input.Status.Value);
            }
            
            return query;
        }

        [Authorize(DecisionSupportPermissions.StrategicInitiatives.Create)]
        public override async Task<StrategicInitiativeDto> CreateAsync(CreateStrategicInitiativeDto input)
        {
            var initiative = await _initiativeManager.ProposeInitiativeAsync(
                input.Title,
                input.Description,
                input.ExpectedOutcomes,
                input.Rationale,
                input.DecisionModelId);
                
            return ObjectMapper.Map<StrategicInitiativeProposal, StrategicInitiativeDto>(initiative);
        }

        [Authorize(DecisionSupportPermissions.StrategicInitiatives.Approve)]
        public async Task<StrategicInitiativeDto> ApproveAsync(Guid id, ApproveInitiativeDto input)
        {
            var initiative = await _initiativeManager.ApproveInitiativeAsync(
                id,
                input.ProjectId,
                CurrentUser.Id.Value);
                
            return ObjectMapper.Map<StrategicInitiativeProposal, StrategicInitiativeDto>(initiative);
        }

        [Authorize(DecisionSupportPermissions.StrategicInitiatives.Reject)]
        public async Task<StrategicInitiativeDto> RejectAsync(Guid id, RejectInitiativeDto input)
        {
            var initiative = await _initiativeManager.RejectInitiativeAsync(
                id,
                input.Reason,
                CurrentUser.Id.Value);
                
            return ObjectMapper.Map<StrategicInitiativeProposal, StrategicInitiativeDto>(initiative);
        }

        public async Task<PagedResultDto<StrategicInitiativeDto>> GetByDecisionModelAsync(
            Guid decisionModelId, 
            PagedAndSortedResultRequestDto input)
        {
            var initiatives = await _initiativeRepository.GetByDecisionModelIdAsync(decisionModelId);
            
            var totalCount = initiatives.Count;
            
            var items = initiatives
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();
                
            return new PagedResultDto<StrategicInitiativeDto>(
                totalCount,
                ObjectMapper.Map<List<StrategicInitiativeProposal>, List<StrategicInitiativeDto>>(items));
        }
    }
}
