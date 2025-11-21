using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects
{
    /// <summary>
    /// Projected revenue data for a specific time period
    /// </summary>
    public class ProjectedRevenueData : ValueObject
    {
        public decimal MonthlyRecurringRevenue { get; private set; }
        public decimal AnnualRecurringRevenue { get; private set; }
        public decimal TotalRevenue { get; private set; }
        public decimal NetRevenue { get; private set; }
        public int CustomerCount { get; private set; }
        public decimal CustomerAcquisitionCost { get; private set; }
        public decimal LifetimeValue { get; private set; }
        public double ChurnRate { get; private set; }
        public DateTime ProjectionDate { get; private set; }

        protected ProjectedRevenueData() { }

        public ProjectedRevenueData(
            decimal monthlyRecurringRevenue,
            decimal annualRecurringRevenue,
            decimal totalRevenue,
            decimal netRevenue,
            int customerCount,
            decimal customerAcquisitionCost,
            decimal lifetimeValue,
            double churnRate,
            DateTime projectionDate)
        {
            MonthlyRecurringRevenue = monthlyRecurringRevenue;
            AnnualRecurringRevenue = annualRecurringRevenue;
            TotalRevenue = totalRevenue;
            NetRevenue = netRevenue;
            CustomerCount = customerCount;
            CustomerAcquisitionCost = customerAcquisitionCost;
            LifetimeValue = lifetimeValue;
            ChurnRate = churnRate;
            ProjectionDate = projectionDate;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MonthlyRecurringRevenue;
            yield return AnnualRecurringRevenue;
            yield return TotalRevenue;
            yield return NetRevenue;
            yield return CustomerCount;
            yield return CustomerAcquisitionCost;
            yield return LifetimeValue;
            yield return ChurnRate;
            yield return ProjectionDate.Date;
        }
    }
}
