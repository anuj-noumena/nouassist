using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    [Authorize(FinancialManagementPermissions.FinancialPlanning.Default)]
    public class ExpenseAppService : ApplicationService, IExpenseAppService
    {
        private readonly IRepository<Expense, Guid> _expenseRepository;

        public ExpenseAppService(IRepository<Expense, Guid> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ViewExpenses)]
        public async Task<ExpenseDto> GetAsync(Guid id)
        {
            var expense = await _expenseRepository.GetAsync(id);
            return ObjectMapper.Map<Expense, ExpenseDto>(expense);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ViewExpenses)]
        public async Task<PagedResultDto<ExpenseDto>> GetListAsync(GetExpenseListDto input)
        {
            var queryable = await _expenseRepository.GetQueryableAsync();

            if (input.UserFilter.HasValue)
            {
                queryable = queryable.Where(e => e.UserId == input.UserFilter.Value);
            }

            if (input.CategoryFilter.HasValue)
            {
                queryable = queryable.Where(e => e.CategoryId == input.CategoryFilter.Value);
            }

            if (input.StatusFilter.HasValue)
            {
                queryable = queryable.Where(e => e.Status == input.StatusFilter.Value);
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
            var expense = new Expense(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                CurrentUser.Id.Value,
                new MonetaryValue(input.Amount, input.Currency),
                input.ExpenseDate,
                input.CategoryId,
                input.PayeeId,
                input.Description,
                input.IsReimbursable,
                input.IsBillable,
                input.ReceiptDocumentId,
                input.ProjectId
            );

            await _expenseRepository.InsertAsync(expense);
            return ObjectMapper.Map<Expense, ExpenseDto>(expense);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.CreateExpenses)]
        public async Task<ExpenseDto> SubmitAsync(SubmitExpenseDto input)
        {
            var expense = await _expenseRepository.GetAsync(input.ExpenseId);
            expense.Submit();
            await _expenseRepository.UpdateAsync(expense);
            return ObjectMapper.Map<Expense, ExpenseDto>(expense);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ApproveExpenses)]
        public async Task<ExpenseDto> ApproveAsync(ApproveExpenseDto input)
        {
            var expense = await _expenseRepository.GetAsync(input.ExpenseId);

            if (input.IsApproved)
            {
                expense.Approve(CurrentUser.Id.Value);
            }
            else
            {
                expense.Reject(CurrentUser.Id.Value, input.Comments);
            }

            await _expenseRepository.UpdateAsync(expense);
            return ObjectMapper.Map<Expense, ExpenseDto>(expense);
        }
    }
}
