namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates
{
    /// <summary>
    /// Represents a compliance obligation or regulatory requirement
    /// </summary>
    public class ComplianceObligation : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public ObligationDescription Description { get; private set; }
        public Jurisdiction Jurisdiction { get; private set; }
        public StatusValue Status { get; private set; }
        public string AuthorityName { get; private set; }
        public string ReferenceNumber { get; private set; }
        public DateTime? Deadline { get; private set; }
        public string DocumentUrl { get; private set; }
        public bool IsRecurring { get; private set; }
        public string RecurrencePattern { get; private set; }
        
        protected ComplianceObligation() { }

        /// <summary>
        /// Creates a new compliance obligation
        /// </summary>
        public static ComplianceObligation Create(
            Guid id,
            string title,
            ObligationDescription description,
            Jurisdiction jurisdiction,
            string authorityName,
            DateTime? deadline = null,
            string referenceNumber = null,
            bool isRecurring = false,
            string recurrencePattern = null,
            Guid? tenantId = null)
        {
            return new ComplianceObligation(
                id,
                title,
                description,
                jurisdiction,
                authorityName,
                deadline,
                referenceNumber,
                isRecurring,
                recurrencePattern,
                tenantId);
        }

        private ComplianceObligation(
            Guid id,
            string title,
            ObligationDescription description,
            Jurisdiction jurisdiction,
            string authorityName,
            DateTime? deadline = null,
            string referenceNumber = null,
            bool isRecurring = false,
            string recurrencePattern = null,
            Guid? tenantId = null) : base(id)
        {
            TenantId = tenantId;
            SetTitle(title);
            Description = description;
            Jurisdiction = jurisdiction;
            SetAuthorityName(authorityName);
            Deadline = deadline;
            ReferenceNumber = referenceNumber;
            Status = new StatusValue(ComplianceStatus.NotStarted, Clock.Now);
            IsRecurring = isRecurring;
            RecurrencePattern = recurrencePattern;
            
            AddDistributedEvent(new ComplianceObligationCreatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Title = Title,
                Deadline = Deadline
            });
        }

        /// <summary>
        /// Updates the compliance obligation's basic information
        /// </summary>
        public void Update(
            string title,
            ObligationDescription description,
            Jurisdiction jurisdiction,
            string authorityName,
            DateTime? deadline = null,
            string referenceNumber = null,
            bool isRecurring = false,
            string recurrencePattern = null)
        {
            SetTitle(title);
            Description = description;
            Jurisdiction = jurisdiction;
            SetAuthorityName(authorityName);
            Deadline = deadline;
            ReferenceNumber = referenceNumber;
            IsRecurring = isRecurring;
            RecurrencePattern = recurrencePattern;
        }

        /// <summary>
        /// Updates the compliance obligation's document URL
        /// </summary>
        public void SetDocumentUrl(string documentUrl)
        {
            DocumentUrl = documentUrl;
        }

        /// <summary>
        /// Updates the status of the compliance obligation
        /// </summary>
        public void UpdateStatus(ComplianceStatus newStatus, DateTime statusDate, string notes = null)
        {
            var previousStatus = Status.Status;
            Status = new StatusValue(newStatus, statusDate, notes);
            
            AddDistributedEvent(new ComplianceStatusChangedEto
            {
                Id = Id,
                TenantId = TenantId,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                Deadline = Deadline,
                IsOverdue = IsOverdue(statusDate)
            });
        }

        /// <summary>
        /// Checks if the compliance obligation is overdue
        /// </summary>
        public bool IsOverdue(DateTime checkDate)
        {
            return Deadline.HasValue && 
                   Deadline.Value < checkDate && 
                   Status.Status != ComplianceStatus.Completed &&
                   Status.Status != ComplianceStatus.NotApplicable &&
                   Status.Status != ComplianceStatus.Waived;
        }

        /// <summary>
        /// Generates a new recurring compliance obligation
        /// </summary>
        public ComplianceObligation GenerateNextRecurringObligation()
        {
            if (!IsRecurring || !Deadline.HasValue)
            {
                return null;
            }

            DateTime nextDeadline;
            
            // Simple recurrence pattern handling
            if (RecurrencePattern == "Annual")
            {
                nextDeadline = Deadline.Value.AddYears(1);
            }
            else if (RecurrencePattern == "Monthly")
            {
                nextDeadline = Deadline.Value.AddMonths(1);
            }
            else if (RecurrencePattern == "Quarterly")
            {
                nextDeadline = Deadline.Value.AddMonths(3);
            }
            else
            {
                // Default to annual if pattern is not recognized
                nextDeadline = Deadline.Value.AddYears(1);
            }

            return ComplianceObligation.Create(
                GuidGenerator.Create(),
                Title,
                Description,
                Jurisdiction,
                AuthorityName,
                nextDeadline,
                ReferenceNumber,
                IsRecurring,
                RecurrencePattern,
                TenantId);
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                LegalComplianceConsts.MinObligationTitleLength,
                LegalComplianceConsts.MaxObligationTitleLength
            );
        }

        private void SetAuthorityName(string authorityName)
        {
            AuthorityName = Check.NotNullOrWhiteSpace(
                authorityName,
                nameof(authorityName),
                maxLength: LegalComplianceConsts.MaxPartyNameLength
            );
        }
    }
}
