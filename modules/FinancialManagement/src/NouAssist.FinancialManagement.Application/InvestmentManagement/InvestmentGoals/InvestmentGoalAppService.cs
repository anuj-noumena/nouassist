using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using NouAssist.FinancialManagement.InvestmentManagement.Data;
using NouAssist.InvestmentManagement.InvestmentGoals.Dtos;
using NouAssist.InvestmentManagement.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.InvestmentManagement.InvestmentGoals
{
    [Authorize(InvestmentManagementPermissions.InvestmentGoals.Default)]
    public class InvestmentGoalAppService : ApplicationService, IInvestmentGoalAppService
    {
        private readonly IInvestmentGoalRepository _investmentGoalRepository;

        public InvestmentGoalAppService(IInvestmentGoalRepository investmentGoalRepository)
        {
            _investmentGoalRepository = investmentGoalRepository;
        }

        public async Task<InvestmentGoalDto> GetAsync(Guid id)
        {
            var goal = await _investmentGoalRepository.GetAsync(id);
            return ObjectMapper.Map<InvestmentGoal, InvestmentGoalDto>(goal);
        }

        public async Task<PagedResultDto<InvestmentGoalDto>> GetListAsync(GetInvestmentGoalListDto input)
        {
            var userId = CurrentUser.GetId();
            
            var goals = input.OnlyActive == true
                ? await _investmentGoalRepository.GetActiveGoalsAsync(userId)
                : await _investmentGoalRepository.GetUserGoalsAsync(userId);

            var totalCount = goals.Count;
            var pagedGoals = goals
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<InvestmentGoalDto>(
                totalCount,
                ObjectMapper.Map<List<InvestmentGoal>, List<InvestmentGoalDto>>(pagedGoals)
            );
        }

        [Authorize(InvestmentManagementPermissions.InvestmentGoals.Create)]
        public async Task<InvestmentGoalDto> CreateAsync(CreateInvestmentGoalDto input)
        {
            var userId = CurrentUser.GetId();

            var goal = new InvestmentGoal(
                GuidGenerator.Create(),
                userId,
                CurrentTenant.Id,
                input.Name,
                input.Description,
                input.TargetAmount,
                input.TargetDate,
                input.RecommendedRiskLevel
            );

            if (input.PortfolioId.HasValue)
            {
                goal.LinkToPortfolio(input.PortfolioId.Value);
            }

            await _investmentGoalRepository.InsertAsync(goal);

            return ObjectMapper.Map<InvestmentGoal, InvestmentGoalDto>(goal);
        }

        [Authorize(InvestmentManagementPermissions.InvestmentGoals.Update)]
        public async Task<InvestmentGoalDto> UpdateAsync(Guid id, UpdateInvestmentGoalDto input)
        {
            var goal = await _investmentGoalRepository.GetAsync(id);

            // Update properties through reflection or add methods to aggregate

            await _investmentGoalRepository.UpdateAsync(goal);

            return ObjectMapper.Map<InvestmentGoal, InvestmentGoalDto>(goal);
        }

        [Authorize(InvestmentManagementPermissions.InvestmentGoals.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _investmentGoalRepository.DeleteAsync(id);
        }

        [Authorize(InvestmentManagementPermissions.InvestmentGoals.Update)]
        public async Task<InvestmentGoalDto> UpdateProgressAsync(Guid id, UpdateGoalProgressDto input)
        {
            var goal = await _investmentGoalRepository.GetAsync(id);

            goal.UpdateProgress(input.CurrentAmount);

            await _investmentGoalRepository.UpdateAsync(goal);

            return ObjectMapper.Map<InvestmentGoal, InvestmentGoalDto>(goal);
        }
    }
}
