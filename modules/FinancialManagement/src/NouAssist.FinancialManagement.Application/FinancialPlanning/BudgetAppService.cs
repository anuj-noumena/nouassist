using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    [Authorize(FinancialManagementPermissions.FinancialPlanning.Default)]
    public class BudgetAppService : ApplicationService, IBudgetAppService
    {
        private readonly IRepository<Budget, Guid> _budgetRepository;

        public BudgetAppService(IRepository<Budget, Guid> budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ManageBudgets)]
        public async Task<BudgetDto> GetAsync(Guid id)
        {
            var queryable = await _budgetRepository.WithDetailsAsync();
            var budget = await AsyncExecuter.FirstOrDefaultAsync(
                queryable.Where(b => b.Id == id));

            return ObjectMapper.Map<Budget, BudgetDto>(budget);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ManageBudgets)]
        public async Task<PagedResultDto<BudgetDto>> GetListAsync(GetBudgetListDto input)
        {
            var queryable = await _budgetRepository.GetQueryableAsync();

            queryable = queryable.Where(b => b.UserId == CurrentUser.Id.Value);

            if (input.TypeFilter.HasValue)
            {
                queryable = queryable.Where(b => b.Type == input.TypeFilter.Value);
            }

            if (input.StatusFilter.HasValue)
            {
                queryable = queryable.Where(b => b.Status == input.StatusFilter.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(b => b.PeriodStart)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var budgets = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<BudgetDto>(
                totalCount,
                ObjectMapper.Map<List<Budget>, List<BudgetDto>>(budgets)
            );
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
                input.PeriodEnd,
                input.Currency
            );

            foreach (var allocationDto in input.Allocations)
            {
                var allocation = new CategoryAllocation(
                    allocationDto.CategoryId,
                    allocationDto.CategoryName,
                    new MonetaryValue(allocationDto.BudgetedAmount, input.Currency)
                );
                budget.AddAllocation(allocation);
            }

            await _budgetRepository.InsertAsync(budget);
            return ObjectMapper.Map<Budget, BudgetDto>(budget);
        }
    }
}
