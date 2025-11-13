using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.FinancialPlanning.Dtos
{
    public class BudgetDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public BudgetType Type { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public BudgetStatus Status { get; set; }
        public MonetaryValue TotalBudget { get; set; }
        public MonetaryValue ActualSpent { get; set; }
        public decimal VariancePercentage { get; set; }
        public List<CategoryAllocationDto> Allocations { get; set; }
    }

    public class CategoryAllocationDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public MonetaryValue BudgetedAmount { get; set; }
        public MonetaryValue ActualAmount { get; set; }
        public MonetaryValue Variance { get; set; }
        public decimal PercentageUsed { get; set; }
    }

    public class CreateBudgetDto
    {
        public string Name { get; set; }
        public BudgetType Type { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public List<CreateCategoryAllocationDto> Allocations { get; set; }
    }

    public class CreateCategoryAllocationDto
    {
        public Guid CategoryId { get; set; }
        public decimal BudgetedAmount { get; set; }
        public string Currency { get; set; }
    }
}
