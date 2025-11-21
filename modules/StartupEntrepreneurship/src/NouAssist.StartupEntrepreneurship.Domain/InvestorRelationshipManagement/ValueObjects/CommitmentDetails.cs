using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Value object representing investment commitment details
    /// </summary>
    public class CommitmentDetails : ValueObject
    {
        public decimal InvestedAmount { get; private set; }
        public decimal CommittedAmount { get; private set; }
        public decimal OwnershipPercentage { get; private set; }
        public string InvestmentRound { get; private set; }
        public DateTime InvestmentDate { get; private set; }

        private CommitmentDetails() { }

        public CommitmentDetails(
            decimal investedAmount,
            decimal committedAmount,
            decimal ownershipPercentage,
            string investmentRound,
            DateTime investmentDate)
        {
            if (investedAmount < 0)
                throw new BusinessException("IRM:ErrorMessage:InvalidOperation", "Invested amount cannot be negative");

            if (committedAmount < 0)
                throw new BusinessException("IRM:ErrorMessage:InvalidOperation", "Committed amount cannot be negative");

            if (ownershipPercentage < 0 || ownershipPercentage > 100)
                throw new BusinessException("IRM:ErrorMessage:InvalidOperation", "Ownership percentage must be between 0 and 100");

            InvestedAmount = investedAmount;
            CommittedAmount = committedAmount;
            OwnershipPercentage = ownershipPercentage;
            InvestmentRound = Check.NotNullOrWhiteSpace(investmentRound, nameof(investmentRound));
            InvestmentDate = investmentDate;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return InvestedAmount;
            yield return CommittedAmount;
            yield return OwnershipPercentage;
            yield return InvestmentRound;
            yield return InvestmentDate;
        }
    }
}
