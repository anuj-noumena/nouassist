using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates
{
    /// <summary>
    /// Represents an issuance of securities (stocks, options, etc.)
    /// </summary>
    public class SecurityIssuance : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid CapTableId { get; private set; }
        public SecurityType SecurityType { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime IssuanceDate { get; private set; }
        public ShareQuantity TotalAuthorized { get; private set; }
        public ShareQuantity TotalIssued { get; private set; }
        public ShareQuantity TotalOutstanding { get; private set; }
        public PricePerShare IssuePrice { get; private set; }
        public bool IsActive { get; private set; }
        public Guid? ShareClassId { get; private set; }
        public OptionDetails OptionDetails { get; private set; }

        private readonly List<ShareClass> _shareClasses;
        public IReadOnlyCollection<ShareClass> ShareClasses => _shareClasses.AsReadOnly();

        protected SecurityIssuance() 
        {
            _shareClasses = new List<ShareClass>();
        }

        /// <summary>
        /// Creates a new security issuance for stock
        /// </summary>
        public static SecurityIssuance CreateStockIssuance(
            Guid id,
            Guid capTableId,
            string name,
            ShareQuantity totalAuthorized,
            PricePerShare issuePrice,
            DateTime issuanceDate,
            string description = null,
            Guid? tenantId = null)
        {
            return new SecurityIssuance(
                id,
                capTableId,
                SecurityType.CommonStock,
                name,
                totalAuthorized,
                issuePrice,
                issuanceDate,
                description,
                optionDetails: null,
                tenantId);
        }

        /// <summary>
        /// Creates a new security issuance for options
        /// </summary>
        public static SecurityIssuance CreateOptionIssuance(
            Guid id,
            Guid capTableId,
            string name,
            ShareQuantity totalAuthorized,
            OptionDetails optionDetails,
            DateTime issuanceDate,
            string description = null,
            Guid? tenantId = null)
        {
            var strikePrice = new PricePerShare(optionDetails.StrikePrice, optionDetails.Currency);
            
            return new SecurityIssuance(
                id,
                capTableId,
                SecurityType.StockOption,
                name,
                totalAuthorized,
                strikePrice,
                issuanceDate,
                description,
                optionDetails,
                tenantId);
        }

        private SecurityIssuance(
            Guid id,
            Guid capTableId,
            SecurityType securityType,
            string name,
            ShareQuantity totalAuthorized,
            PricePerShare issuePrice,
            DateTime issuanceDate,
            string description = null,
            OptionDetails optionDetails = null,
            Guid? tenantId = null) : base(id)
        {
            CapTableId = capTableId;
            SecurityType = securityType;
            SetName(name);
            SetDescription(description);
            IssuanceDate = issuanceDate;
            TotalAuthorized = totalAuthorized ?? throw new ArgumentNullException(nameof(totalAuthorized));
            TotalIssued = new ShareQuantity(0);
            TotalOutstanding = new ShareQuantity(0);
            IssuePrice = issuePrice ?? throw new ArgumentNullException(nameof(issuePrice));
            OptionDetails = optionDetails;
            IsActive = true;
            TenantId = tenantId;

            _shareClasses = new List<ShareClass>();
        }

        /// <summary>
        /// Issues securities to a holder
        /// </summary>
        public void Issue(ShareQuantity quantity)
        {
            ValidateIssuance(quantity);

            TotalIssued = TotalIssued.Add(quantity);
            TotalOutstanding = TotalOutstanding.Add(quantity);
        }

        /// <summary>
        /// Cancels previously issued securities
        /// </summary>
        public void Cancel(ShareQuantity quantity)
        {
            if (quantity.IsGreaterThan(TotalOutstanding))
            {
                throw new InsufficientSharesException(quantity.Value, TotalOutstanding.Value);
            }

            TotalOutstanding = TotalOutstanding.Subtract(quantity);
        }

        /// <summary>
        /// Adds a share class to this issuance
        /// </summary>
        public void AddShareClass(
            string name,
            string description,
            decimal parValue = 0.001m,
            int votingRights = 1,
            bool hasDividendRights = true,
            bool hasLiquidationPreference = false,
            decimal liquidationMultiple = 1.0m,
            bool hasAntiDilutionRights = false,
            string antiDilutionType = null)
        {
            // Check if share class with same name already exists
            if (_shareClasses.Any(sc => sc.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException("CapTable:ErrorMessage:DuplicateSecurityHolder")
                    .WithData("name", name)
                    .WithData("type", "ShareClass");
            }

            var shareClass = new ShareClass(
                Guid.NewGuid(),
                name,
                description,
                SecurityType,
                parValue,
                votingRights,
                hasDividendRights,
                hasLiquidationPreference,
                liquidationMultiple,
                hasAntiDilutionRights,
                antiDilutionType
            );

            _shareClasses.Add(shareClass);

            // Set the default share class if this is the first one
            if (ShareClassId == null)
            {
                ShareClassId = shareClass.Id;
            }
        }

        /// <summary>
        /// Gets available shares for issuance
        /// </summary>
        public ShareQuantity GetAvailableShares()
        {
            return TotalAuthorized.Subtract(TotalIssued);
        }

        /// <summary>
        /// Checks if options are expired
        /// </summary>
        public bool IsExpired(DateTime currentDate)
        {
            return SecurityType == SecurityType.StockOption && 
                   OptionDetails?.IsExpired(currentDate) == true;
        }

        /// <summary>
        /// Updates the issuance details
        /// </summary>
        public void UpdateDetails(
            string name,
            string description = null,
            ShareQuantity totalAuthorized = null)
        {
            SetName(name);
            SetDescription(description);

            if (totalAuthorized != null)
            {
                if (totalAuthorized.IsLessThan(TotalIssued))
                {
                    throw new BusinessException("CapTable:ErrorMessage:InvalidValue")
                        .WithData("reason", "Authorized shares cannot be less than issued shares");
                }
                TotalAuthorized = totalAuthorized;
            }
        }

        /// <summary>
        /// Deactivates the security issuance
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        private void ValidateIssuance(ShareQuantity quantity)
        {
            if (!IsActive)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidOperation")
                    .WithData("reason", "Cannot issue from inactive security issuance");
            }

            var newTotalIssued = TotalIssued.Add(quantity);
            if (newTotalIssued.IsGreaterThan(TotalAuthorized))
            {
                throw new InsufficientSharesException(newTotalIssued.Value, TotalAuthorized.Value);
            }
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: CapTableConsts.MaxNameLength
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
