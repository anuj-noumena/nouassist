using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.FinancialPlanning.Entities
{
    public class GoalContribution : Entity
    {
        public Guid GoalId { get; private set; }
        public MonetaryValue Amount { get; private set; }
        public DateTime ContributionDate { get; private set; }
        public string Notes { get; private set; }

        protected GoalContribution() { }

        public GoalContribution(
            Guid goalId,
            MonetaryValue amount,
            DateTime contributionDate,
            string notes = null)
        {
            GoalId = goalId;
            Amount = amount;
            ContributionDate = contributionDate;
            Notes = notes;
        }

        public override object[] GetKeys()
        {
            return new object[] { GoalId, ContributionDate };
        }
    }
}
