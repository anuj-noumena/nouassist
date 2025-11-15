using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.CreditScoreMonitoring.ValueObjects
{
    public class CreditUtilization : ValueObject
    {
        public decimal TotalCreditLimit { get; init; }
        public decimal TotalBalances { get; init; }
        public decimal UtilizationPercentage { get; init; }

        public CreditUtilization(
            decimal totalCreditLimit,
            decimal totalBalances)
        {
            TotalCreditLimit = totalCreditLimit;
            TotalBalances = totalBalances;
            UtilizationPercentage = totalCreditLimit > 0 
                ? (totalBalances / totalCreditLimit) * 100 
                : 0;
        }

        public bool IsHealthy => UtilizationPercentage <= 30;
        public bool NeedsAttention => UtilizationPercentage > 30 && UtilizationPercentage <= 50;
        public bool IsCritical => UtilizationPercentage > 50;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TotalCreditLimit;
            yield return TotalBalances;
            yield return UtilizationPercentage;
        }
    }
}
