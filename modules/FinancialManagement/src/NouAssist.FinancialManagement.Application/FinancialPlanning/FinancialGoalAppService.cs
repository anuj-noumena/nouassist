using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    [Authorize(FinancialManagementPermissions.FinancialPlanning.ManageGoals)]
    public class FinancialGoalAppService : ApplicationService, IFinancialGoalAppService
    {
        private readonly IFinancialGoalRepository _goalRepository;

        public FinancialGoalAppService(IFinancialGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public async Task<FinancialGoalDto> GetAsync(Guid id)
        {
            var goal = await _goalRepository.GetAsync(id);
            return MapToDto(goal);
        }

        public async Task<PagedResultDto<FinancialGoalDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var userId = CurrentUser.Id.Value;
            var goals = await _goalRepository.GetByUserAsync(userId);

            var totalCount = goals.Count;
            var pagedGoals = goals
                .OrderBy(g => g.TargetDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<FinancialGoalDto>(
                totalCount,
                pagedGoals.Select(MapToDto).ToList()
            );
        }

        public async Task<FinancialGoalDto> CreateAsync(CreateFinancialGoalDto input)
        {
            var targetAmount = new MonetaryValue(input.TargetAmount, input.Currency);

            var goal = new FinancialGoal(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                CurrentUser.Id.Value,
                input.Name,
                input.Type,
                targetAmount,
                input.TargetDate,
                input.Description);

            await _goalRepository.InsertAsync(goal);

            return MapToDto(goal);
        }

        public async Task<FinancialGoalDto> AddContributionAsync(Guid id, AddGoalContributionDto input)
        {
            var goal = await _goalRepository.GetAsync(id);
            var amount = new MonetaryValue(input.Amount, input.Currency);

            var contribution = new GoalContribution(
                goal.Id,
                amount,
                input.ContributionDate,
                input.Notes);

            goal.AddContribution(contribution);

            await _goalRepository.UpdateAsync(goal);

            return MapToDto(goal);
        }

        public async Task<FinancialGoalDto> MarkAsAchievedAsync(Guid id)
        {
            var goal = await _goalRepository.GetAsync(id);
            goal.MarkAsAchieved();
            await _goalRepository.UpdateAsync(goal);
            return MapToDto(goal);
        }

        public async Task DeleteAsync(Guid id)
        {
            var goal = await _goalRepository.GetAsync(id);
            goal.Abandon("Deleted by user");
            await _goalRepository.UpdateAsync(goal);
        }

        private FinancialGoalDto MapToDto(FinancialGoal goal)
        {
            var dto = ObjectMapper.Map<FinancialGoal, FinancialGoalDto>(goal);
            
            if (goal.TargetAmount.Amount > 0)
            {
                dto.ProgressPercentage = (goal.CurrentAmount.Amount / goal.TargetAmount.Amount) * 100;
            }

            return dto;
        }
    }
}
