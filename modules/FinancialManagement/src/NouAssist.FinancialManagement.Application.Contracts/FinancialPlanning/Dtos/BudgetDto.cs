using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
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
        public decimal TotalBudget { get; set; }
        public decimal ActualSpent { get; set; }
        public decimal Remaining { get; set; }
        public decimal VariancePercentage { get; set; }
        public string Currency { get; set; }
        public List<CategoryAllocationDto> Allocations { get; set; }
    }

    public class CategoryAllocationDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal BudgetedAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal Variance { get; set; }
        public decimal PercentageUsed { get; set; }
        public bool IsOverBudget { get; set; }
    }

    public class CreateBudgetDto
    {
        public string Name { get; set; }
        public BudgetType Type { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string Currency { get; set; } = "USD";
        public List<CreateCategoryAllocationDto> Allocations { get; set; }
    }

    public class CreateCategoryAllocationDto
    {
        public Guid CategoryId { get; set; }
        public decimal BudgetedAmount { get; set; }
    }

    public class GetBudgetListDto : PagedAndSortedResultRequestDto
    {
        public BudgetType? TypeFilter { get; set; }
        public BudgetStatus? StatusFilter { get; set; }
    }
}
