using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.DebtManagement.Aggregates;
using NouAssist.DebtManagement.Data;
using NouAssist.DebtManagement.PayoffPlans.Dtos;
using NouAssist.DebtManagement.Permissions;
using NouAssist.DebtManagement.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.DebtManagement.PayoffPlans
{
    [Authorize(DebtManagementPermissions.PayoffPlans.Default)]
    public class DebtPayoffPlanAppService : ApplicationService, IDebtPayoffPlanAppService
    {
        private readonly IDebtPayoffPlanRepository _payoffPlanRepository;
        private readonly IDebtAccountRepository _debtAccountRepository;
        private readonly DebtPayoffPlanningService _planningService;
        private readonly DebtConsolidationService _consolidationService;

        public DebtPayoffPlanAppService(
            IDebtPayoffPlanRepository payoffPlanRepository,
            IDebtAccountRepository debtAccountRepository,
            DebtPayoffPlanningService planningService,
            DebtConsolidationService consolidationService)
        {
            _payoffPlanRepository = payoffPlanRepository;
            _debtAccountRepository = debtAccountRepository;
            _planningService = planningService;
            _consolidationService = consolidationService;
        }

        public async Task<DebtPayoffPlanDto> GetAsync(Guid id)
        {
            var plan = await _payoffPlanRepository.GetWithDebtsAsync(id);
            var dto = ObjectMapper.Map<DebtPayoffPlan, DebtPayoffPlanDto>(plan);

            // Get debt details and calculate analysis
            var debtIds = plan.Debts.Select(d => d.DebtAccountId).ToList();
            var debts = await _debtAccountRepository.GetListAsync(d => debtIds.Contains(d.Id));
            
            var analysis = _planningService.AnalyzePayoffPlan(plan, debts);
            dto.Analysis = new PayoffAnalysisDto
            {
                TotalDebt = analysis.TotalDebt,
                TotalInterest = analysis.TotalInterest,
                MonthlyPayment = analysis.MonthlyPayment,
                MonthsToPayoff = analysis.MonthsToPayoff,
                ProjectedPayoffDate = analysis.ProjectedPayoffDate
            };

            return dto;
        }

        public async Task<PagedResultDto<DebtPayoffPlanDto>> GetListAsync(
            PagedAndSortedResultRequestDto input)
        {
            var userId = CurrentUser.GetId();
            var plans = await _payoffPlanRepository.GetActiveUserPlansAsync(userId);

            var totalCount = plans.Count;
            var pagedPlans = plans
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<DebtPayoffPlanDto>(
                totalCount,
                ObjectMapper.Map<List<DebtPayoffPlan>, List<DebtPayoffPlanDto>>(pagedPlans)
            );
        }

        [Authorize(DebtManagementPermissions.PayoffPlans.Create)]
        public async Task<DebtPayoffPlanDto> CreateAsync(CreateDebtPayoffPlanDto input)
        {
            var userId = CurrentUser.GetId();

            // Load debt accounts
            var debts = await _debtAccountRepository.GetListAsync(
                d => input.DebtAccountIds.Contains(d.Id) && d.UserId == userId);

            if (debts.Count != input.DebtAccountIds.Count)
            {
                throw new Volo.Abp.BusinessException("DebtManagement:InvalidDebtAccounts");
            }

            // Create optimized plan
            var plan = _planningService.CreateOptimizedPayoffPlan(
                userId,
                CurrentTenant.Id,
                input.PlanName,
                input.Strategy,
                input.MonthlyPaymentAmount,
                debts
            );

            await _payoffPlanRepository.InsertAsync(plan);

            return ObjectMapper.Map<DebtPayoffPlan, DebtPayoffPlanDto>(plan);
        }

        [Authorize(DebtManagementPermissions.PayoffPlans.Update)]
        public async Task<DebtPayoffPlanDto> UpdateAsync(Guid id, UpdateDebtPayoffPlanDto input)
        {
            var plan = await _payoffPlanRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.PlanName))
            {
                // Update plan name
            }

            if (input.MonthlyPaymentAmount.HasValue)
            {
                plan.UpdateMonthlyPayment(input.MonthlyPaymentAmount.Value);
            }

            if (input.Strategy.HasValue)
            {
                plan.UpdateStrategy(input.Strategy.Value);
            }

            await _payoffPlanRepository.UpdateAsync(plan);

            return ObjectMapper.Map<DebtPayoffPlan, DebtPayoffPlanDto>(plan);
        }

        [Authorize(DebtManagementPermissions.PayoffPlans.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _payoffPlanRepository.DeleteAsync(id);
        }

        public async Task<ConsolidationAnalysisDto> AnalyzeConsolidationAsync(
            decimal consolidationRate,
            int termMonths,
            List<Guid> debtAccountIds)
        {
            var userId = CurrentUser.GetId();
            var debts = await _debtAccountRepository.GetListAsync(
                d => debtAccountIds.Contains(d.Id) && d.UserId == userId);

            var analysis = _consolidationService.AnalyzeConsolidationOpportunity(
                debts,
                consolidationRate,
                termMonths
            );

            return new ConsolidationAnalysisDto
            {
                TotalDebtToConsolidate = analysis.TotalDebtToConsolidate,
                CurrentWeightedAverageRate = analysis.CurrentWeightedAverageRate,
                ConsolidationRate = analysis.ConsolidationRate,
                CurrentTotalMonthlyPayment = analysis.CurrentTotalMonthlyPayment,
                ConsolidatedMonthlyPayment = analysis.ConsolidatedMonthlyPayment,
                CurrentTotalInterest = analysis.CurrentTotalInterest,
                ConsolidatedTotalInterest = analysis.ConsolidatedTotalInterest,
                InterestSavings = analysis.InterestSavings,
                MonthlyPaymentSavings = analysis.MonthlyPaymentSavings,
                IsWorthwhile = analysis.IsWorthwhile
            };
        }
    }
}
