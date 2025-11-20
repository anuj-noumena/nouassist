using System;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class ProjectedRevenueDataDto
    {
        public decimal MonthlyRecurringRevenue { get; set; }
        public decimal AnnualRecurringRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal NetRevenue { get; set; }
        public int CustomerCount { get; set; }
        public decimal CustomerAcquisitionCost { get; set; }
        public decimal LifetimeValue { get; set; }
        public double ChurnRate { get; set; }
        public DateTime ProjectionDate { get; set; }
    }
}
