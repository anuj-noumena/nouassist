using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Exceptions;
using NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates
{
    /// <summary>
    /// Represents a corporate governance record such as meeting minutes, resolutions, or certificates
    /// </summary>
    public class CorporateRecord : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public CorporateRecordType Type { get; private set; }
        public RecordType RecordType { get; private set; }
        public DateTime RecordDate { get; private set; }
        public string DocumentUrl { get; private set; }
        public FilingStatus Status { get; private set; }
        public string ReferenceNumber { get; private set; }
        
        protected CorporateRecord() { }

        /// <summary>
        /// Creates a new corporate record
        /// </summary>
        public static CorporateRecord Create(
            Guid id,
            string title,
            string description,
            CorporateRecordType type,
            DateTime recordDate,
            string referenceNumber = null,
            Guid? tenantId = null)
        {
            return new CorporateRecord(
                id,
                title,
                description,
                type,
                recordDate,
                referenceNumber,
                tenantId);
        }

        private CorporateRecord(
            Guid id,
            string title,
            string description,
            CorporateRecordType type,
            DateTime recordDate,
            string referenceNumber = null,
            Guid? tenantId = null) : base(id)
        {
            TenantId = tenantId;
            SetTitle(title);
            SetDescription(description);
            Type = type;
            RecordType = new RecordType(type);
            RecordDate = recordDate;
            ReferenceNumber = referenceNumber;
            Status = new FilingStatus(FilingStatusType.Draft, Clock.Now);
            
            AddDistributedEvent(new CorporateRecordCreatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Title = Title,
                Type = Type,
                RecordDate = RecordDate
            });
        }

        /// <summary>
        /// Updates the corporate record's basic information
        /// </summary>
        public void Update(
            string title,
            string description,
            DateTime recordDate,
            string referenceNumber = null)
        {
            if (Status.Status != FilingStatusType.Draft)
            {
                throw new InvalidAgreementStateException(Id, Status.Status, FilingStatusType.Draft);
            }
            
            SetTitle(title);
            SetDescription(description);
            RecordDate = recordDate;
            ReferenceNumber = referenceNumber;
        }

        /// <summary>
        /// Updates the corporate record's document URL
        /// </summary>
        public void SetDocumentUrl(string documentUrl)
        {
            DocumentUrl = documentUrl;
        }

        /// <summary>
        /// Updates the status of the corporate record
        /// </summary>
        public void UpdateStatus(FilingStatusType newStatus, DateTime statusDate, string notes = null)
        {
            // Basic validation
            if (newStatus == FilingStatusType.Filed && Status.Status != FilingStatusType.Draft)
            {
                throw new InvalidAgreementStateException(Id, Status.Status, FilingStatusType.Draft);
            }
            
            Status = new FilingStatus(newStatus, statusDate, notes);
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                LegalComplianceConsts.MinRecordTitleLength,
                LegalComplianceConsts.MaxRecordTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: LegalComplianceConsts.MaxRecordDescriptionLength
            );
        }
    }
}
