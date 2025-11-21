using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing a portfolio goal
    /// </summary>
    public class PortfolioGoal : ValueObject
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime TargetDate { get; private set; }
        public bool IsAchieved { get; private set; }

        protected PortfolioGoal() { }

        public PortfolioGoal(string title, string description, DateTime targetDate, bool isAchieved = false)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = description;
            TargetDate = targetDate;
            IsAchieved = isAchieved;
        }

        public void MarkAsAchieved()
        {
            IsAchieved = true;
        }

        public void MarkAsNotAchieved()
        {
            IsAchieved = false;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Description ?? string.Empty;
            yield return TargetDate;
            yield return IsAchieved;
        }
    }
}
