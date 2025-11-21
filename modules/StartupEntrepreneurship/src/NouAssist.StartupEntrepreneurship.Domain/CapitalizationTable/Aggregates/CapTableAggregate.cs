using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Events;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates
{
    /// <summary>
    /// Represents the capitalization table for a company
    /// Maintains the authoritative record of ownership and equity distribution
    /// </summary>
    public class CapTable : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string CompanyName { get; private set; }
        public string CompanyIdentifier { get; private set; }
        public DateTime AsOfDate { get; private set; }
        public ShareQuantity TotalAuthorizedShares { get; private set; }
        public ShareQuantity TotalIssuedShares { get; private set; }
        public ShareQuantity TotalOutstandingShares { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<TransactionRecord> _transactions;
        private readonly List<ValuationHistory> _valuationHistory;

        public IReadOnlyCollection<TransactionRecord> Transactions => _transactions.AsReadOnly();
        public IReadOnlyCollection<ValuationHistory> ValuationHistory => _valuationHistory.AsReadOnly();

        protected CapTable() 
        {
            _transactions = new List<TransactionRecord>();
            _valuationHistory = new List<ValuationHistory>();
        }

        /// <summary>
        /// Creates a new cap table for a company
        /// </summary>
        public static CapTable Create(
            Guid id,
            string companyName,
            ShareQuantity totalAuthorizedShares,
            DateTime asOfDate,
            string companyIdentifier = null,
            Guid? tenantId = null)
        {
            return new CapTable(id, companyName, totalAuthorizedShares, asOfDate, companyIdentifier, tenantId);
        }

        private CapTable(
            Guid id,
            string companyName,
            ShareQuantity totalAuthorizedShares,
            DateTime asOfDate,
            string companyIdentifier = null,
            Guid? tenantId = null) : base(id)
        {
            SetCompanyName(companyName);
            TotalAuthorizedShares = totalAuthorizedShares ?? throw new ArgumentNullException(nameof(totalAuthorizedShares));
            TotalIssuedShares = new ShareQuantity(0);
            TotalOutstandingShares = new ShareQuantity(0);
            AsOfDate = asOfDate;
            CompanyIdentifier = companyIdentifier;
            IsActive = true;
            TenantId = tenantId;

            _transactions = new List<TransactionRecord>();
            _valuationHistory = new List<ValuationHistory>();

            AddDistributedEvent(new CapTableCreatedEto
            {
                Id = Id,
                CompanyName = CompanyName,
                TotalShares = TotalAuthorizedShares.Value,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Records a share issuance transaction
        /// </summary>
        public void IssueShares(
            Guid toHolderId,
            Guid securityIssuanceId,
            Guid shareClassId,
            ShareQuantity quantity,
            PricePerShare pricePerShare,
            DateTime transactionDate,
            string memo = null)
        {
            ValidateShareIssuance(quantity);

            var transaction = new TransactionRecord(
                Guid.NewGuid(),
                Id,
                TransactionType.IssuanceOfShares,
                transactionDate,
                quantity,
                pricePerShare,
                memo,
                toHolderId: toHolderId,
                securityIssuanceId: securityIssuanceId,
                shareClassId: shareClassId,
                tenantId: TenantId
            );

            _transactions.Add(transaction);
            TotalIssuedShares = TotalIssuedShares.Add(quantity);
            TotalOutstandingShares = TotalOutstandingShares.Add(quantity);

            AddDistributedEvent(new SecurityIssuedEto
            {
                Id = Guid.NewGuid(),
                CapTableId = Id,
                SecurityHolderId = toHolderId,
                SecurityType = SecurityType.CommonStock, // Should come from share class
                Quantity = quantity.Value,
                PricePerShare = pricePerShare.Value,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Records a share transfer transaction
        /// </summary>
        public void TransferShares(
            Guid fromHolderId,
            Guid toHolderId,
            Guid shareClassId,
            ShareQuantity quantity,
            PricePerShare pricePerShare,
            DateTime transactionDate,
            string memo = null)
        {
            var transaction = new TransactionRecord(
                Guid.NewGuid(),
                Id,
                TransactionType.Transfer,
                transactionDate,
                quantity,
                pricePerShare,
                memo,
                fromHolderId,
                toHolderId,
                shareClassId: shareClassId,
                tenantId: TenantId
            );

            _transactions.Add(transaction);

            AddDistributedEvent(new SharesTransferredEto
            {
                Id = Guid.NewGuid(),
                CapTableId = Id,
                FromHolderId = fromHolderId,
                ToHolderId = toHolderId,
                Quantity = quantity.Value,
                PricePerShare = pricePerShare.Value,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Records an option grant transaction
        /// </summary>
        public void GrantOptions(
            Guid toHolderId,
            Guid securityIssuanceId,
            ShareQuantity quantity,
            OptionDetails optionDetails,
            DateTime grantDate,
            string memo = null)
        {
            var strikePrice = new PricePerShare(optionDetails.StrikePrice, optionDetails.Currency);
            
            var transaction = new TransactionRecord(
                Guid.NewGuid(),
                Id,
                TransactionType.OptionGrant,
                grantDate,
                quantity,
                strikePrice,
                memo,
                toHolderId: toHolderId,
                securityIssuanceId: securityIssuanceId,
                tenantId: TenantId
            );

            _transactions.Add(transaction);

            AddDistributedEvent(new OptionGrantedEto
            {
                Id = Guid.NewGuid(),
                CapTableId = Id,
                SecurityHolderId = toHolderId,
                Quantity = quantity.Value,
                StrikePrice = optionDetails.StrikePrice,
                GrantDate = grantDate,
                ExpiryDate = optionDetails.ExpiryDate,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Records an option exercise transaction
        /// </summary>
        public void ExerciseOptions(
            Guid holderId,
            Guid securityIssuanceId,
            Guid shareClassId,
            ShareQuantity quantity,
            PricePerShare strikePrice,
            DateTime exerciseDate,
            string memo = null)
        {
            ValidateShareIssuance(quantity);

            var transaction = new TransactionRecord(
                Guid.NewGuid(),
                Id,
                TransactionType.OptionExercise,
                exerciseDate,
                quantity,
                strikePrice,
                memo,
                toHolderId: holderId,
                securityIssuanceId: securityIssuanceId,
                shareClassId: shareClassId,
                tenantId: TenantId
            );

            _transactions.Add(transaction);
            TotalIssuedShares = TotalIssuedShares.Add(quantity);
            TotalOutstandingShares = TotalOutstandingShares.Add(quantity);
        }

        /// <summary>
        /// Records valuation at a specific point in time
        /// </summary>
        public void RecordValuation(
            DateTime valuationDate,
            Valuation valuation,
            string source,
            string notes = null,
            Guid? fundingRoundId = null)
        {
            var valuationEntry = new ValuationHistory(
                Guid.NewGuid(),
                Id,
                valuationDate,
                valuation,
                source,
                notes,
                fundingRoundId,
                TenantId
            );

            _valuationHistory.Add(valuationEntry);
        }

        /// <summary>
        /// Updates the as-of date for the cap table
        /// </summary>
        public void UpdateAsOfDate(DateTime newAsOfDate)
        {
            if (newAsOfDate <= AsOfDate)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidValue")
                    .WithData("reason", "New as-of date must be later than current date");
            }

            AsOfDate = newAsOfDate;
        }

        /// <summary>
        /// Calculates total outstanding shares including options
        /// </summary>
        public ShareQuantity CalculateFullyDilutedShares()
        {
            var optionShares = _transactions
                .Where(t => t.TransactionType == TransactionType.OptionGrant)
                .Sum(t => t.Quantity.Value);

            var exercisedOptions = _transactions
                .Where(t => t.TransactionType == TransactionType.OptionExercise)
                .Sum(t => t.Quantity.Value);

            return new ShareQuantity(TotalOutstandingShares.Value + optionShares - exercisedOptions);
        }

        /// <summary>
        /// Gets the latest valuation
        /// </summary>
        public ValuationHistory GetLatestValuation()
        {
            return _valuationHistory
                .OrderByDescending(v => v.ValuationDate)
                .FirstOrDefault();
        }

        private void ValidateShareIssuance(ShareQuantity quantity)
        {
            var newTotalIssued = TotalIssuedShares.Add(quantity);
            if (newTotalIssued.IsGreaterThan(TotalAuthorizedShares))
            {
                throw new BusinessException("CapTable:ErrorMessage:InsufficientShares")
                    .WithData("requested", newTotalIssued.Value)
                    .WithData("authorized", TotalAuthorizedShares.Value);
            }
        }

        private void SetCompanyName(string companyName)
        {
            CompanyName = Check.NotNullOrWhiteSpace(
                companyName,
                nameof(companyName),
                maxLength: CapTableConsts.MaxCompanyNameLength
            );
        }

        /// <summary>
        /// Deactivates the cap table
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Reactivates the cap table
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }
    }
}
