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
    [Authorize(FinancialManagementPermissions.FinancialPlanning.Default)]
    public class BudgetAppService : ApplicationService, IBudgetAppService
    {
        private readonly IBudgetRepository _budgetRepository;

        public BudgetAppService(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<BudgetDto> GetAsync(Guid id)
        {
            var budget = await _budgetRepository.GetWithAllocationsAsync(id);
            return MapToDto(budget);
        }

        public async Task<PagedResultDto<BudgetDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var userId = CurrentUser.Id.Value;
            var queryable = await _budgetRepository.GetQueryableAsync();
            queryable = queryable.Where(b => b.UserId == userId);

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(b => b.PeriodStart)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var budgets = await AsyncExecuter.ToListAsync(queryable);
            var dtos = new List<BudgetDto>();

            foreach (var budget in budgets)
            {
                var fullBudget = await _budgetRepository.GetWithAllocationsAsync(budget.Id);
                dtos.Add(MapToDto(fullBudget));
            }

            return new PagedResultDto<BudgetDto>(totalCount, dtos);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ManageBudgets)]
        public async Task<BudgetDto> CreateAsync(CreateBudgetDto input)
        {
            var budget = new Budget(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                CurrentUser.Id.Value,
                input.Name,
                input.Type,
                input.PeriodStart,
                input.PeriodEnd);

            foreach (var allocationDto in input.Allocations)
            {
                var amount = new MonetaryValue(allocationDto.BudgetedAmount, allocationDto.Currency);
                var allocation = new CategoryAllocation(
                    budget.Id,
                    allocationDto.CategoryId,
                    amount);

                budget.AddAllocation(allocation);
            }

            await _budgetRepository.InsertAsync(budget);

            return MapToDto(budget);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ManageBudgets)]
        public async Task<BudgetDto> ActivateAsync(Guid id)
        {
            var budget = await _budgetRepository.GetWithAllocationsAsync(id);
            budget.Activate();
            await _budgetRepository.UpdateAsync(budget);
            return MapToDto(budget);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ManageBudgets)]
        public async Task<BudgetDto> CloseAsync(Guid id)
        {
            var budget = await _budgetRepository.GetWithAllocationsAsync(id);
            budget.Close();
            await _budgetRepository.UpdateAsync(budget);
            return MapToDto(budget);
        }

        public async Task<BudgetDto> GetActiveBudgetAsync(Guid userId)
        {
            var budgets = await _budgetRepository.GetActiveBudgetsAsync(userId);
            var currentBudget = budgets.FirstOrDefault(b => 
                b.PeriodStart <= DateTime.Today && b.PeriodEnd >= DateTime.Today);

            if (currentBudget == null)
            {
                throw new Volo.Abp.UserFriendlyException("No active budget found for the current period");
            }

            return MapToDto(currentBudget);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ManageBudgets)]
        public async Task DeleteAsync(Guid id)
        {
            await _budgetRepository.DeleteAsync(id);
        }

        private BudgetDto MapToDto(Budget budget)
        {
            var dto = ObjectMapper.Map<Budget, BudgetDto>(budget);
            
            if (budget.TotalBudget.Amount > 0)
            {
                dto.VariancePercentage = ((budget.ActualSpent.Amount - budget.TotalBudget.Amount) 
                    / budget.TotalBudget.Amount) * 100;
            }

            // Map allocations with additional calculated fields
            dto.Allocations = budget.Allocations.Select(a => new CategoryAllocationDto
            {
                CategoryId = a.CategoryId,
                BudgetedAmount = a.BudgetedAmount,
                ActualAmount = a.ActualAmount,
                Variance = new MonetaryValue(
                    a.ActualAmount.Amount - a.BudgetedAmount.Amount,
                    a.BudgetedAmount.Currency),
                PercentageUsed = a.BudgetedAmount.Amount > 0 
                    ? (a.ActualAmount.Amount / a.BudgetedAmount.Amount) * 100 
                    : 0
            }).ToList();

            return dto;
        }
    }
}
