using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing conditions for a contract alert
    /// </summary>
    public class AlertCondition : ValueObject
    {
        public DateTime TriggerDate { get; private set; }
        public int DaysInAdvance { get; private set; }
        public bool IsRecurring { get; private set; }
        public string Frequency { get; private set; }  // Daily, Weekly, Monthly, etc.

        protected AlertCondition() { }

        public AlertCondition(DateTime triggerDate, int daysInAdvance, bool isRecurring = false, string frequency = null)
        {
            if (daysInAdvance < 0)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:InvalidValue")
                    .WithData("propertyName", "DaysInAdvance");
            }
            
            TriggerDate = triggerDate;
            DaysInAdvance = daysInAdvance;
            IsRecurring = isRecurring;
            Frequency = isRecurring ? Check.NotNullOrWhiteSpace(frequency, nameof(frequency)) : null;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TriggerDate;
            yield return DaysInAdvance;
            yield return IsRecurring;
            yield return Frequency ?? string.Empty;
        }
    }
}
