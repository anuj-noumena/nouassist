using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.DebtManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.DebtManagement.PayoffPlans.Dtos
{
    public class DebtPayoffPlanDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string PlanName { get; set; }
        public PaymentStrategy Strategy { get; set; }
        public decimal MonthlyPaymentAmount { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public List<PayoffPlanDebtDto> Debts { get; set; }
        public PayoffAnalysisDto Analysis { get; set; }
    }

    public class PayoffPlanDebtDto
    {
        public Guid DebtAccountId { get; set; }
        public string DebtName { get; set; }
        public int PayoffOrder { get; set; }
        public decimal AllocatedMonthlyPayment { get; set; }
        public bool IsIncluded { get; set; }
    }

    public class CreateDebtPayoffPlanDto
    {
        public string PlanName { get; set; }
        public PaymentStrategy Strategy { get; set; }
        public decimal MonthlyPaymentAmount { get; set; }
        public List<Guid> DebtAccountIds { get; set; }
    }

    public class UpdateDebtPayoffPlanDto
    {
        public string PlanName { get; set; }
        public decimal? MonthlyPaymentAmount { get; set; }
        public PaymentStrategy? Strategy { get; set; }
    }

    public class PayoffAnalysisDto
    {
        public decimal TotalDebt { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal MonthlyPayment { get; set; }
        public int MonthsToPayoff { get; set; }
        public DateTime ProjectedPayoffDate { get; set; }
    }

    public class ConsolidationAnalysisDto
    {
        public decimal TotalDebtToConsolidate { get; set; }
        public decimal CurrentWeightedAverageRate { get; set; }
        public decimal ConsolidationRate { get; set; }
        public decimal CurrentTotalMonthlyPayment { get; set; }
        public decimal ConsolidatedMonthlyPayment { get; set; }
        public decimal CurrentTotalInterest { get; set; }
        public decimal ConsolidatedTotalInterest { get; set; }
        public decimal InterestSavings { get; set; }
        public decimal MonthlyPaymentSavings { get; set; }
        public bool IsWorthwhile { get; set; }
    }
}
