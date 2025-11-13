using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using NouAssist.FinancialManagement.FinancialPlanning.Services;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    [Authorize(FinancialManagementPermissions.FinancialPlanning.ViewExpenses)]
    public class ExpenseAppService : ApplicationService, IExpenseAppService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly BudgetTrackingService _budgetTrackingService;

        public ExpenseAppService(
            IExpenseRepository expenseRepository,
            IBudgetRepository budgetRepository,
            BudgetTrackingService budgetTrackingService)
        {
            _expenseRepository = expenseRepository;
            _budgetRepository = budgetRepository;
            _budgetTrackingService = budgetTrackingService;
        }

        public async Task<ExpenseDto> GetAsync(Guid id)
        {
            var expense = await _expenseRepository.GetAsync(id);
            return ObjectMapper.Map<Expense, ExpenseDto>(expense);
        }

        public async Task<PagedResultDto<ExpenseDto>> GetListAsync(GetExpenseListDto input)
        {
            var queryable = await _expenseRepository.GetQueryableAsync();

            var userId = input.UserId ?? CurrentUser.Id.Value;
            queryable = queryable.Where(e => e.UserId == userId);

            if (input.StartDate.HasValue)
            {
                queryable = queryable.Where(e => e.ExpenseDate >= input.StartDate.Value);
            }

            if (input.EndDate.HasValue)
            {
                queryable = queryable.Where(e => e.ExpenseDate <= input.EndDate.Value);
            }

            if (input.CategoryId.HasValue)
            {
                queryable = queryable.Where(e => e.CategoryId == input.CategoryId.Value);
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(e => e.Status == input.Status.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(e => e.ExpenseDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var expenses = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<ExpenseDto>(
                totalCount,
                ObjectMapper.Map<List<Expense>, List<ExpenseDto>>(expenses)
            );
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.CreateExpenses)]
        public async Task<ExpenseDto> CreateAsync(CreateExpenseDto input)
        {
            var amount = new MonetaryValue(input.Amount, input.Currency);

            var expense = new Expense(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                CurrentUser.Id.Value,
                amount,
                input.ExpenseDate,
                input.CategoryId,
                input.PayeeId,
                input.Description,
                input.IsReimbursable,
                input.IsBillable,
                input.ProjectId,
                input.ReceiptDocumentId);

            await _expenseRepository.InsertAsync(expense);

            return ObjectMapper.Map<Expense, ExpenseDto>(expense);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.CreateExpenses)]
        public async Task<ExpenseDto> SubmitAsync(Guid id)
        {
            var expense = await _expenseRepository.GetAsync(id);
            expense.Submit();
            await _expenseRepository.UpdateAsync(expense);

            return ObjectMapper.Map<Expense, ExpenseDto>(expense);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ApproveExpenses)]
        public async Task<ExpenseDto> ApproveAsync(Guid id, ApproveExpenseDto input)
        {
            var expense = await _expenseRepository.GetAsync(id);

            if (input.IsApproved)
            {
                expense.Approve(CurrentUser.Id.Value, input.Comments);

                // Track against budgets
                await _budgetTrackingService.TrackExpenseAgainstBudgetsAsync(
                    expense.UserId,
                    expense.CategoryId,
                    expense.Amount,
                    expense.ExpenseDate);
            }
            else
            {
                expense.Reject(CurrentUser.Id.Value, input.Comments);
            }

            await _expenseRepository.UpdateAsync(expense);

            return ObjectMapper.Map<Expense, ExpenseDto>(expense);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.CreateExpenses)]
        public async Task DeleteAsync(Guid id)
        {
            await _expenseRepository.DeleteAsync(id);
        }
    }
}
