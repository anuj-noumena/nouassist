using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Constants;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Events;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Exceptions;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Contract
{
    /// <summary>
    /// Represents a contract with its versions, clauses, parties, amendments, and alerts.
    /// </summary>
    public class ContractAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        #region Properties
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public string ReferenceNumber { get; private set; }
        public string Description { get; private set; }
        public ContractStatus Status { get; private set; }
        public DateTime EffectiveDate { get; private set; }
        public DateTime? ExpirationDate { get; private set; }
        public decimal TotalValue { get; private set; }
        public string Currency { get; private set; }
        public ContractTemplateReference TemplateReference { get; private set; }
        public DocumentId MainDocumentId { get; private set; }

        private readonly List<ContractVersion> _versions;
        public IReadOnlyCollection<ContractVersion> Versions => _versions.AsReadOnly();

        private readonly List<ContractClause> _clauses;
        public IReadOnlyCollection<ContractClause> Clauses => _clauses.AsReadOnly();

        private readonly List<ContractParty> _parties;
        public IReadOnlyCollection<ContractParty> Parties => _parties.AsReadOnly();

        private readonly List<ContractAmendment> _amendments;
        public IReadOnlyCollection<ContractAmendment> Amendments => _amendments.AsReadOnly();

        private readonly List<ContractAlert> _alerts;
        public IReadOnlyCollection<ContractAlert> Alerts => _alerts.AsReadOnly();
        #endregion

        #region Constructors
        protected ContractAggregate()
        {
            _versions = new List<ContractVersion>();
            _clauses = new List<ContractClause>();
            _parties = new List<ContractParty>();
            _amendments = new List<ContractAmendment>();
            _alerts = new List<ContractAlert>();
        }

        public static ContractAggregate Create(
            Guid id,
            Guid? tenantId,
            string title,
            string referenceNumber,
            string description,
            DateTime effectiveDate,
            DateTime? expirationDate,
            decimal totalValue,
            string currency,
            ContractTemplateReference templateReference,
            DocumentId mainDocumentId)
        {
            return new ContractAggregate(
                id,
                tenantId,
                title,
                referenceNumber,
                description,
                effectiveDate,
                expirationDate,
                totalValue,
                currency,
                templateReference,
                mainDocumentId);
        }

        private ContractAggregate(
            Guid id,
            Guid? tenantId,
            string title,
            string referenceNumber,
            string description,
            DateTime effectiveDate,
            DateTime? expirationDate,
            decimal totalValue,
            string currency,
            ContractTemplateReference templateReference,
            DocumentId mainDocumentId)
            : base(id)
        {
            TenantId = tenantId;
            SetTitle(title);
            SetReferenceNumber(referenceNumber);
            SetDescription(description);
            SetEffectiveDate(effectiveDate);
            SetExpirationDate(expirationDate);
            SetTotalValue(totalValue, currency);
            TemplateReference = templateReference;
            MainDocumentId = mainDocumentId;
            Status = ContractStatus.Draft;

            _versions = new List<ContractVersion>();
            _clauses = new List<ContractClause>();
            _parties = new List<ContractParty>();
            _amendments = new List<ContractAmendment>();
            _alerts = new List<ContractAlert>();

            // Initialize first version
            var initialVersion = new ContractVersion(
                GuidGenerator.Create(),
                Id,
                new VersionMetadata(1, "Initial Version", null),
                mainDocumentId);
            
            _versions.Add(initialVersion);

            // Add domain event
            AddDistributedEvent(new ContractCreatedEto
            {
                Id = Id,
                Title = Title,
                Status = Status,
                DocumentId = MainDocumentId.Id,
                EffectiveDate = EffectiveDate,
                ExpirationDate = ExpirationDate,
                TenantId = TenantId
            });
        }
        #endregion

        #region Public Methods
        public void UpdateBasicDetails(
            string title,
            string referenceNumber,
            string description,
            DateTime effectiveDate,
            DateTime? expirationDate,
            decimal totalValue,
            string currency)
        {
            if (Status == ContractStatus.Archived || Status == ContractStatus.Terminated)
            {
                throw new InvalidContractStatusTransitionException(Status, Status);
            }

            SetTitle(title);
            SetReferenceNumber(referenceNumber);
            SetDescription(description);
            SetEffectiveDate(effectiveDate);
            SetExpirationDate(expirationDate);
            SetTotalValue(totalValue, currency);
        }

        public void ChangeStatus(ContractStatus newStatus, DateTime statusChangeDate)
        {
            if (!CanTransitionToStatus(newStatus))
            {
                throw new InvalidContractStatusTransitionException(Status, newStatus);
            }

            var oldStatus = Status;
            Status = newStatus;

            // Add domain event
            AddDistributedEvent(new ContractStatusChangedEto
            {
                Id = Id,
                Title = Title,
                Status = Status,
                PreviousStatus = oldStatus,
                StatusChangeDate = statusChangeDate,
                TenantId = TenantId
            });
        }

        public ContractVersion CreateNewVersion(
            string versionLabel,
            string changeReason,
            DocumentId documentId,
            DateTime versionDate)
        {
            if (Status == ContractStatus.Archived || Status == ContractStatus.Terminated)
            {
                throw new InvalidContractStatusTransitionException(Status, Status);
            }

            var latestVersion = GetLatestVersion();
            var newVersionNumber = latestVersion.Metadata.VersionNumber + 1;

            var newVersion = new ContractVersion(
                GuidGenerator.Create(),
                Id,
                new VersionMetadata(newVersionNumber, versionLabel, changeReason),
                documentId);

            _versions.Add(newVersion);

            // Add domain event
            AddDistributedEvent(new ContractVersionedEto
            {
                Id = Id,
                Title = Title,
                Status = Status,
                DocumentId = documentId.Id,
                VersionNumber = newVersionNumber,
                VersionLabel = versionLabel,
                VersionDate = versionDate,
                TenantId = TenantId
            });

            return newVersion;
        }

        public ContractClause AddClause(
            string title,
            string content,
            string reference,
            bool isMandatory)
        {
            if (Status == ContractStatus.Archived || Status == ContractStatus.Terminated)
            {
                throw new InvalidContractStatusTransitionException(Status, Status);
            }

            var clause = new ContractClause(
                GuidGenerator.Create(),
                Id,
                title,
                content,
                reference,
                isMandatory);

            _clauses.Add(clause);
            return clause;
        }

        public void RemoveClause(Guid clauseId)
        {
            if (Status == ContractStatus.Archived || Status == ContractStatus.Terminated)
            {
                throw new InvalidContractStatusTransitionException(Status, Status);
            }

            var clause = _clauses.FirstOrDefault(c => c.Id == clauseId);
            if (clause != null)
            {
                _clauses.Remove(clause);
            }
        }

        public ContractParty AddParty(
            string name,
            string identifier,
            string contactInfo,
            ContractPartyType partyType)
        {
            if (Status == ContractStatus.Archived || Status == ContractStatus.Terminated)
            {
                throw new InvalidContractStatusTransitionException(Status, Status);
            }

            var party = new ContractParty(
                GuidGenerator.Create(),
                Id,
                name,
                identifier,
                contactInfo,
                partyType);

            _parties.Add(party);
            return party;
        }

        public void RemoveParty(Guid partyId)
        {
            if (Status == ContractStatus.Archived || Status == ContractStatus.Terminated)
            {
                throw new InvalidContractStatusTransitionException(Status, Status);
            }

            var party = _parties.FirstOrDefault(p => p.Id == partyId);
            if (party != null)
            {
                _parties.Remove(party);
            }
        }

        public ContractAmendment AddAmendment(
            AmendmentDetails details,
            DocumentId documentId)
        {
            if (Status != ContractStatus.Active)
            {
                throw new BusinessException("ContractPortfolioManagement:ErrorMessage:InvalidOperation")
                    .WithData("operation", "add amendment")
                    .WithData("requiredStatus", "Active");
            }

            var amendment = new ContractAmendment(
                GuidGenerator.Create(),
                Id,
                details,
                documentId);

            _amendments.Add(amendment);
            return amendment;
        }

        public ContractAlert AddAlert(
            string title,
            string description,
            ContractAlertType alertType,
            AlertCondition alertCondition)
        {
            var alert = new ContractAlert(
                GuidGenerator.Create(),
                Id,
                title,
                description,
                alertType,
                alertCondition);

            _alerts.Add(alert);
            return alert;
        }

        public void RemoveAlert(Guid alertId)
        {
            var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert != null)
            {
                _alerts.Remove(alert);
            }
        }

        public ContractVersion GetLatestVersion()
        {
            return _versions.OrderByDescending(v => v.Metadata.VersionNumber).First();
        }

        public bool IsActive(DateTime currentDate)
        {
            return Status == ContractStatus.Active &&
                   EffectiveDate <= currentDate &&
                   (!ExpirationDate.HasValue || ExpirationDate.Value >= currentDate);
        }

        public bool IsExpired(DateTime currentDate)
        {
            return ExpirationDate.HasValue && ExpirationDate.Value < currentDate;
        }

        public bool IsExpiringSoon(DateTime currentDate, int daysThreshold)
        {
            if (!ExpirationDate.HasValue)
            {
                return false;
            }

            var daysUntilExpiration = (ExpirationDate.Value - currentDate).TotalDays;
            return daysUntilExpiration >= 0 && daysUntilExpiration <= daysThreshold;
        }
        #endregion

        #region Private Methods
        private bool CanTransitionToStatus(ContractStatus newStatus)
        {
            switch (Status)
            {
                case ContractStatus.Draft:
                    return newStatus == ContractStatus.UnderReview || 
                           newStatus == ContractStatus.Archived;
                
                case ContractStatus.UnderReview:
                    return newStatus == ContractStatus.Draft || 
                           newStatus == ContractStatus.Negotiation || 
                           newStatus == ContractStatus.Archived;
                
                case ContractStatus.Negotiation:
                    return newStatus == ContractStatus.UnderReview || 
                           newStatus == ContractStatus.PendingSignature || 
                           newStatus == ContractStatus.Archived;
                
                case ContractStatus.PendingSignature:
                    return newStatus == ContractStatus.Negotiation || 
                           newStatus == ContractStatus.Active || 
                           newStatus == ContractStatus.Archived;
                
                case ContractStatus.Active:
                    return newStatus == ContractStatus.Expired || 
                           newStatus == ContractStatus.Terminated || 
                           newStatus == ContractStatus.Archived;
                
                case ContractStatus.Expired:
                case ContractStatus.Terminated:
                    return newStatus == ContractStatus.Archived;
                
                case ContractStatus.Archived:
                    return false;
                
                default:
                    return false;
            }
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: ContractPortfolioManagementConsts.Contract.MaxTitleLength
            );
        }

        private void SetReferenceNumber(string referenceNumber)
        {
            ReferenceNumber = Check.NotNullOrWhiteSpace(
                referenceNumber,
                nameof(referenceNumber),
                maxLength: ContractPortfolioManagementConsts.Contract.MaxReferenceNumberLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: ContractPortfolioManagementConsts.Contract.MaxDescriptionLength
            );
        }

        private void SetEffectiveDate(DateTime effectiveDate)
        {
            EffectiveDate = effectiveDate;
        }

        private void SetExpirationDate(DateTime? expirationDate)
        {
            if (expirationDate.HasValue && expirationDate.Value <= EffectiveDate)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:DateRange")
                    .WithData("property", "ExpirationDate")
                    .WithData("reference", "EffectiveDate");
            }

            ExpirationDate = expirationDate;
        }

        private void SetTotalValue(decimal totalValue, string currency)
        {
            if (totalValue < 0)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:InvalidValue")
                    .WithData("propertyName", "TotalValue");
            }

            TotalValue = totalValue;
            Currency = Check.NotNullOrWhiteSpace(currency, nameof(currency));
        }
        #endregion
    }
}
