using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Entities;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Events.IntellectualProperty;
using NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates
{
    /// <summary>
    /// Represents an intellectual property asset such as a patent, trademark, or copyright
    /// </summary>
    public class IntellectualPropertyAsset : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IntellectualPropertyType Type { get; private set; }
        public IPIdentifier Identifier { get; private set; }
        public Jurisdiction Jurisdiction { get; private set; }
        public FilingStatus Status { get; private set; }
        public DateTime FilingDate { get; private set; }
        public DateTime? RegistrationDate { get; private set; }
        public DateTime? ExpirationDate { get; private set; }
        public IPValuationEstimate Valuation { get; private set; }
        
        private readonly List<FilingDeadline> _filingDeadlines;
        public IReadOnlyCollection<FilingDeadline> FilingDeadlines => _filingDeadlines.AsReadOnly();

        protected IntellectualPropertyAsset()
        {
            _filingDeadlines = new List<FilingDeadline>();
        }

        /// <summary>
        /// Creates a new intellectual property asset
        /// </summary>
        public static IntellectualPropertyAsset Create(
            Guid id,
            string name,
            string description,
            IntellectualPropertyType type,
            IPIdentifier identifier,
            Jurisdiction jurisdiction,
            DateTime filingDate,
            FilingStatusType initialStatus,
            DateTime? registrationDate,
            DateTime? expirationDate,
            Guid? tenantId)
        {
            return new IntellectualPropertyAsset(
                id,
                name,
                description,
                type,
                identifier,
                jurisdiction,
                filingDate,
                initialStatus,
                registrationDate,
                expirationDate,
                tenantId);
        }

        private IntellectualPropertyAsset(
            Guid id,
            string name,
            string description,
            IntellectualPropertyType type,
            IPIdentifier identifier,
            Jurisdiction jurisdiction,
            DateTime filingDate,
            FilingStatusType initialStatus,
            DateTime? registrationDate,
            DateTime? expirationDate,
            Guid? tenantId) : base(id)
        {
            TenantId = tenantId;
            SetName(name);
            SetDescription(description);
            Type = type;
            Identifier = identifier;
            Jurisdiction = jurisdiction;
            FilingDate = filingDate;
            Status = new FilingStatus(initialStatus, filingDate);
            RegistrationDate = registrationDate;
            ExpirationDate = expirationDate;
            _filingDeadlines = new List<FilingDeadline>();
            
            AddDistributedEvent(new IPAssetCreatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Name = Name,
                Type = Type,
                Identifier = Identifier.Value,
                FilingDate = FilingDate,
                Status = Status.Status,
                Jurisdiction = Jurisdiction.ToString()
            });
        }

        /// <summary>
        /// Updates the intellectual property asset's basic information
        /// </summary>
        public void Update(
            string name,
            string description,
            Jurisdiction jurisdiction)
        {
            SetName(name);
            SetDescription(description);
            Jurisdiction = jurisdiction;
            
            AddDistributedEvent(new IPAssetUpdatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Name = Name,
                Type = Type,
                Status = Status.Status
            });
        }

        /// <summary>
        /// Updates the filing status of the intellectual property asset
        /// </summary>
        public void UpdateStatus(FilingStatusType newStatus, DateTime statusDate, string notes = null)
        {
            Status = Status.UpdateStatus(newStatus, statusDate, notes);
            
            if (newStatus == FilingStatusType.Registered && !RegistrationDate.HasValue)
            {
                RegistrationDate = statusDate;
            }
            
            AddDistributedEvent(new IPAssetUpdatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Name = Name,
                Type = Type,
                Status = Status.Status
            });
        }

        /// <summary>
        /// Sets or updates the valuation estimate for the intellectual property asset
        /// </summary>
        public void SetValuation(
            decimal estimatedValue,
            string currency,
            DateTime valuationDate,
            string valuationMethod,
            string valuationNotes = null)
        {
            Valuation = new IPValuationEstimate(
                estimatedValue,
                currency,
                valuationDate,
                valuationMethod,
                valuationNotes);
        }

        /// <summary>
        /// Adds a filing deadline for the intellectual property asset
        /// </summary>
        public void AddFilingDeadline(
            string title,
            DateTime deadline,
            string description = null,
            bool isRecurring = false,
            string recurrencePattern = null)
        {
            var deadlineEntity = new FilingDeadline(
                Id,
                title,
                deadline,
                description,
                isRecurring,
                recurrencePattern);
            
            _filingDeadlines.Add(deadlineEntity);
        }

        /// <summary>
        /// Removes a filing deadline
        /// </summary>
        public void RemoveFilingDeadline(Guid deadlineId)
        {
            var deadline = _filingDeadlines.FirstOrDefault(d => d.Id == deadlineId);
            if (deadline != null)
            {
                _filingDeadlines.Remove(deadline);
            }
        }

        /// <summary>
        /// Renews the intellectual property asset
        /// </summary>
        public void Renew(DateTime renewalDate, DateTime newExpirationDate, string notes = null)
        {
            if (Status.Status == FilingStatusType.Expired)
            {
                UpdateStatus(FilingStatusType.Renewed, renewalDate, notes);
            }
            
            ExpirationDate = newExpirationDate;
            
            AddDistributedEvent(new IPAssetUpdatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Name = Name,
                Type = Type,
                Status = Status.Status
            });
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                LegalComplianceConsts.MinIPAssetNameLength,
                LegalComplianceConsts.MaxIPAssetNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: LegalComplianceConsts.MaxIPDescriptionLength
            );
        }
    }
}
