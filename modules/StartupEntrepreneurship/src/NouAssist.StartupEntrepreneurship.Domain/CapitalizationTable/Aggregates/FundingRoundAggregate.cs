using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Events;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates
{
    /// <summary>
    /// Represents a funding round for the company
    /// </summary>
    public class FundingRound : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid CapTableId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public FundingRoundStatus Status { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? CloseDate { get; private set; }
        public Valuation Valuation { get; private set; }
        public InvestmentAmount TargetAmount { get; private set; }
        public InvestmentAmount RaisedAmount { get; private set; }
        public ShareQuantity SharesIssued { get; private set; }
        public PricePerShare PricePerShare { get; private set; }
        public string LeadInvestor { get; private set; }

        protected FundingRound() { }

        /// <summary>
        /// Creates a new funding round
        /// </summary>
        public static FundingRound Create(
            Guid id,
            Guid capTableId,
            string name,
            Valuation valuation,
            InvestmentAmount targetAmount,
            DateTime startDate,
            string description = null,
            string leadInvestor = null,
            Guid? tenantId = null)
        {
            return new FundingRound(id, capTableId, name, valuation, targetAmount, startDate, description, leadInvestor, tenantId);
        }

        private FundingRound(
            Guid id,
            Guid capTableId,
            string name,
            Valuation valuation,
            InvestmentAmount targetAmount,
            DateTime startDate,
            string description = null,
            string leadInvestor = null,
            Guid? tenantId = null) : base(id)
        {
            CapTableId = capTableId;
            SetName(name);
            SetDescription(description);
            Valuation = valuation ?? throw new ArgumentNullException(nameof(valuation));
            TargetAmount = targetAmount ?? throw new ArgumentNullException(nameof(targetAmount));
            RaisedAmount = new InvestmentAmount(0, targetAmount.Currency);
            SharesIssued = new ShareQuantity(0);
            StartDate = startDate;
            Status = FundingRoundStatus.Planning;
            LeadInvestor = leadInvestor;
            TenantId = tenantId;

            // Calculate price per share based on valuation
            // This is a simplified calculation - real implementation would be more complex
            var totalShares = 1000000; // This should come from current cap table
            PricePerShare = new PricePerShare(valuation.PostMoneyValuation / totalShares, valuation.Currency);
        }

        /// <summary>
        /// Activates the funding round
        /// </summary>
        public void Activate()
        {
            if (Status != FundingRoundStatus.Planning)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidOperation")
                    .WithData("currentStatus", Status.ToString())
                    .WithData("requiredStatus", FundingRoundStatus.Planning.ToString());
            }

            Status = FundingRoundStatus.Active;
        }

        /// <summary>
        /// Records an investment in this funding round
        /// </summary>
        public void RecordInvestment(InvestmentAmount amount, ShareQuantity shares)
        {
            if (Status != FundingRoundStatus.Active)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidOperation")
                    .WithData("reason", "Can only record investments in active funding rounds");
            }

            if (amount.Currency != RaisedAmount.Currency)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidValue")
                    .WithData("reason", "Investment currency must match target currency");
            }

            var newRaisedAmount = RaisedAmount.Add(amount);
            if (newRaisedAmount.Value > TargetAmount.Value)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidValue")
                    .WithData("reason", "Investment would exceed target amount");
            }

            RaisedAmount = newRaisedAmount;
            SharesIssued = SharesIssued.Add(shares);
        }

        /// <summary>
        /// Closes the funding round
        /// </summary>
        public void Close(DateTime closeDate)
        {
            if (Status != FundingRoundStatus.Active)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidOperation")
                    .WithData("reason", "Can only close active funding rounds");
            }

            if (closeDate < StartDate)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidValue")
                    .WithData("reason", "Close date cannot be before start date");
            }

            Status = FundingRoundStatus.Closed;
            CloseDate = closeDate;

            AddDistributedEvent(new FundingRoundClosedEto
            {
                Id = Id,
                CapTableId = CapTableId,
                RoundName = Name,
                TotalRaised = RaisedAmount.Value,
                PreMoneyValuation = Valuation.PreMoneyValuation,
                PostMoneyValuation = Valuation.PostMoneyValuation,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Cancels the funding round
        /// </summary>
        public void Cancel()
        {
            if (Status == FundingRoundStatus.Closed)
            {
                throw new FundingRoundClosedException(Id);
            }

            Status = FundingRoundStatus.Cancelled;
        }

        /// <summary>
        /// Updates funding round details
        /// </summary>
        public void UpdateDetails(
            string name,
            string description = null,
            InvestmentAmount targetAmount = null,
            string leadInvestor = null)
        {
            if (Status == FundingRoundStatus.Closed)
            {
                throw new FundingRoundClosedException(Id);
            }

            SetName(name);
            SetDescription(description);
            
            if (targetAmount != null)
            {
                if (targetAmount.Value < RaisedAmount.Value)
                {
                    throw new BusinessException("CapTable:ErrorMessage:InvalidValue")
                        .WithData("reason", "Target amount cannot be less than already raised amount");
                }
                TargetAmount = targetAmount;
            }

            LeadInvestor = leadInvestor;
        }

        /// <summary>
        /// Gets the completion percentage of the funding round
        /// </summary>
        public decimal GetCompletionPercentage()
        {
            if (TargetAmount.Value == 0)
                return 0;

            return (RaisedAmount.Value / TargetAmount.Value) * 100;
        }

        /// <summary>
        /// Gets remaining amount to be raised
        /// </summary>
        public InvestmentAmount GetRemainingAmount()
        {
            return TargetAmount.Subtract(RaisedAmount);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: CapTableConsts.MaxFundingRoundNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: CapTableConsts.MaxDescriptionLength
            );
        }
    }
}
