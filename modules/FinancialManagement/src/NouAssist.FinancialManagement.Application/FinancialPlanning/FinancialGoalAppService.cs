using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    [Authorize(FinancialManagementPermissions.FinancialPlanning.Default)]
    public class FinancialGoalAppService : ApplicationService, IFinancialGoalAppService
    {
        private readonly IRepository<FinancialGoal, Guid> _goalRepository;
        private readonly IFinancialGoalRepository _customGoalRepository;

        public FinancialGoalAppService(
            IRepository<FinancialGoal, Guid> goalRepository,
            IFinancialGoalRepository customGoalRepository)
        {
            _goalRepository = goalRepository;
            _customGoalRepository = customGoalRepository;
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ViewGoals)]
        public async Task<FinancialGoalDto> GetAsync(Guid id)
        {
            var goal = await _goalRepository.GetAsync(id);
            return ObjectMapper.Map<FinancialGoal, FinancialGoalDto>(goal);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ViewGoals)]
        public async Task<PagedResultDto<FinancialGoalDto>> GetListAsync(GetFinancialGoalListDto input)
        {
            var queryable = await _goalRepository.GetQueryableAsync();

            queryable = queryable.Where(g => g.UserId == CurrentUser.Id.Value);

            if (input.TypeFilter.HasValue)
            {
                queryable = queryable.Where(g => g.Type == input.TypeFilter.Value);
            }

            if (input.StatusFilter.HasValue)
            {
                queryable = queryable.Where(g => g.Status == input.StatusFilter.Value);
            }

            if (input.ActiveOnly == true)
            {
                queryable = queryable.Where(g => g.Status == Enums.GoalStatus.Active);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderBy(g => g.TargetDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var goals = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<FinancialGoalDto>(
                totalCount,
                ObjectMapper.Map<List<FinancialGoal>, List<FinancialGoalDto>>(goals)
            );
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.CreateGoals)]
        public async Task<FinancialGoalDto> CreateAsync(CreateFinancialGoalDto input)
        {
            var goal = new FinancialGoal(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                CurrentUser.Id.Value,
                input.Name,
                input.Type,
                new MonetaryValue(input.TargetAmount, input.Currency),
                null,
                input.TargetDate,
                input.Description
            );

            await _goalRepository.InsertAsync(goal, autoSave: true);

            return ObjectMapper.Map<FinancialGoal, FinancialGoalDto>(goal);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.UpdateGoals)]
        public async Task<FinancialGoalDto> UpdateProgressAsync(UpdateFinancialGoalProgressDto input)
        {
            var goal = await _goalRepository.GetAsync(input.GoalId);
            var amount = new MonetaryValue(input.Amount, goal.CurrentAmount.Currency);
            goal.UpdateProgress(amount);
            await _goalRepository.UpdateAsync(goal, autoSave: true);

            return ObjectMapper.Map<FinancialGoal, FinancialGoalDto>(goal);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.UpdateGoals)]
        public async Task<FinancialGoalDto> CompleteGoalAsync(Guid id)
        {
            var goal = await _goalRepository.GetAsync(id);
            goal.Complete();
            await _goalRepository.UpdateAsync(goal, autoSave: true);

            return ObjectMapper.Map<FinancialGoal, FinancialGoalDto>(goal);
        }
    }
}
