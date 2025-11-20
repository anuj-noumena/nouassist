namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates
{
    /// <summary>
    /// Represents a legal agreement such as a contract, NDA, or license
    /// </summary>
    public class LegalAgreement : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public LegalAgreementType Type { get; private set; }
        public FilingStatus Status { get; private set; }
        public string DocumentUrl { get; private set; }
        public string CounterpartyName { get; private set; }
        public DateTime EffectiveDate { get; private set; }
        public DateTime? ExpirationDate { get; private set; }
        public string ReferenceNumber { get; private set; }
        public Jurisdiction Jurisdiction { get; private set; }
        public decimal? Value { get; private set; }
        public string Currency { get; private set; }

        private readonly List<AgreementTerms> _terms;
        public IReadOnlyCollection<AgreementTerms> Terms => _terms.AsReadOnly();

        private readonly List<AgreementReminder> _reminders;
        public IReadOnlyCollection<AgreementReminder> Reminders => _reminders.AsReadOnly();

        protected LegalAgreement()
        {
            _terms = new List<AgreementTerms>();
            _reminders = new List<AgreementReminder>();
        }

        /// <summary>
        /// Creates a new legal agreement
        /// </summary>
        public static LegalAgreement Create(
            Guid id,
            string title,
            string description,
            LegalAgreementType type,
            string counterpartyName,
            DateTime effectiveDate,
            DateTime? expirationDate,
            string referenceNumber,
            Jurisdiction jurisdiction,
            decimal? value = null,
            string currency = null,
            Guid? tenantId = null)
        {
            return new LegalAgreement(
                id,
                title,
                description,
                type,
                counterpartyName,
                effectiveDate,
                expirationDate,
                referenceNumber,
                jurisdiction,
                value,
                currency,
                tenantId);
        }

        private LegalAgreement(
            Guid id,
            string title,
            string description,
            LegalAgreementType type,
            string counterpartyName,
            DateTime effectiveDate,
            DateTime? expirationDate,
            string referenceNumber,
            Jurisdiction jurisdiction,
            decimal? value = null,
            string currency = null,
            Guid? tenantId = null) : base(id)
        {
            TenantId = tenantId;
            SetTitle(title);
            SetDescription(description);
            Type = type;
            SetCounterpartyName(counterpartyName);
            
            if (expirationDate.HasValue && expirationDate.Value <= effectiveDate)
            {
                throw new BusinessException("LegalCompliance:Validation:DateRange", 
                    "Expiration date must be after effective date");
            }
            
            EffectiveDate = effectiveDate;
            ExpirationDate = expirationDate;
            ReferenceNumber = referenceNumber;
            Jurisdiction = jurisdiction;
            Value = value;
            Currency = currency;
            Status = new FilingStatus(FilingStatusType.Draft, Clock.Now);
            
            _terms = new List<AgreementTerms>();
            _reminders = new List<AgreementReminder>();
            
            AddDistributedEvent(new LegalAgreementCreatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Title = Title,
                Type = Type,
                EffectiveDate = EffectiveDate,
                ExpirationDate = ExpirationDate,
                CounterpartyName = CounterpartyName,
                Status = Status.Status
            });
        }

        /// <summary>
        /// Updates the agreement's basic information
        /// </summary>
        public void Update(
            string title,
            string description,
            string counterpartyName,
            string referenceNumber,
            Jurisdiction jurisdiction,
            decimal? value = null,
            string currency = null)
        {
            if (Status.Status != FilingStatusType.Draft)
            {
                throw new InvalidAgreementStateException(Id, Status.Status, FilingStatusType.Draft);
            }
            
            SetTitle(title);
            SetDescription(description);
            SetCounterpartyName(counterpartyName);
            ReferenceNumber = referenceNumber;
            Jurisdiction = jurisdiction;
            Value = value;
            Currency = currency;
            
            AddDistributedEvent(new LegalAgreementUpdatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Title = Title,
                Type = Type,
                Status = Status.Status
            });
        }

        /// <summary>
        /// Updates the agreement's document URL
        /// </summary>
        public void SetDocumentUrl(string documentUrl)
        {
            DocumentUrl = documentUrl;
        }

        /// <summary>
        /// Updates the agreement's status
        /// </summary>
        public void UpdateStatus(FilingStatusType newStatus, DateTime statusDate, string notes = null)
        {
            // Validate status transitions
            if (newStatus == FilingStatusType.Filed && Status.Status != FilingStatusType.Draft)
            {
                throw new InvalidAgreementStateException(Id, Status.Status, FilingStatusType.Draft);
            }
            
            if (newStatus == FilingStatusType.Approved && Status.Status != FilingStatusType.Filed && 
                Status.Status != FilingStatusType.Pending)
            {
                throw new InvalidAgreementStateException(Id, Status.Status, FilingStatusType.Filed);
            }
            
            Status = Status.UpdateStatus(newStatus, statusDate, notes);
            
            AddDistributedEvent(new LegalAgreementUpdatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Title = Title,
                Type = Type,
                Status = Status.Status
            });
        }

        /// <summary>
        /// Adds a term to the agreement
        /// </summary>
        public void AddTerm(
            string title,
            string description,
            bool isCritical = false,
            DateTime? deadlineDate = null)
        {
            if (Status.Status != FilingStatusType.Draft)
            {
                throw new InvalidAgreementStateException(Id, Status.Status, FilingStatusType.Draft);
            }
            
            var term = new AgreementTerms(Id, title, description, isCritical, deadlineDate);
            _terms.Add(term);
        }

        /// <summary>
        /// Removes a term from the agreement
        /// </summary>
        public void RemoveTerm(Guid termId)
        {
            if (Status.Status != FilingStatusType.Draft)
            {
                throw new InvalidAgreementStateException(Id, Status.Status, FilingStatusType.Draft);
            }
            
            var term = _terms.FirstOrDefault(t => t.Id == termId);
            if (term != null)
            {
                _terms.Remove(term);
            }
        }

        /// <summary>
        /// Adds a reminder for the agreement
        /// </summary>
        public void AddReminder(
            string title,
            DateTime reminderDate,
            string description = null,
            int? daysBeforeEvent = null,
            bool isRecurring = false,
            string recurrencePattern = null)
        {
            var reminder = new AgreementReminder(
                Id,
                title,
                reminderDate,
                description,
                daysBeforeEvent,
                isRecurring,
                recurrencePattern);
            
            _reminders.Add(reminder);
        }

        /// <summary>
        /// Removes a reminder from the agreement
        /// </summary>
        public void RemoveReminder(Guid reminderId)
        {
            var reminder = _reminders.FirstOrDefault(r => r.Id == reminderId);
            if (reminder != null)
            {
                _reminders.Remove(reminder);
            }
        }

        /// <summary>
        /// Renews the agreement with a new expiration date
        /// </summary>
        public void Renew(DateTime newEffectiveDate, DateTime? newExpirationDate, string notes = null)
        {
            if (Status.Status != FilingStatusType.Approved && Status.Status != FilingStatusType.Expired)
            {
                throw new InvalidAgreementStateException(
                    Id, 
                    Status.Status, 
                    FilingStatusType.Approved);
            }
            
            if (newExpirationDate.HasValue && newExpirationDate.Value <= newEffectiveDate)
            {
                throw new BusinessException("LegalCompliance:Validation:DateRange", 
                    "New expiration date must be after new effective date");
            }
            
            EffectiveDate = newEffectiveDate;
            ExpirationDate = newExpirationDate;
            Status = Status.UpdateStatus(FilingStatusType.Renewed, Clock.Now, notes);
            
            AddDistributedEvent(new LegalAgreementUpdatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Title = Title,
                Type = Type,
                Status = Status.Status,
                EffectiveDate = EffectiveDate,
                ExpirationDate = ExpirationDate
            });
        }

        /// <summary>
        /// Terminates the agreement early
        /// </summary>
        public void Terminate(DateTime terminationDate, string reason)
        {
            if (Status.Status != FilingStatusType.Approved && Status.Status != FilingStatusType.Renewed)
            {
                throw new InvalidAgreementStateException(
                    Id, 
                    Status.Status, 
                    FilingStatusType.Approved);
            }
            
            ExpirationDate = terminationDate;
            Status = Status.UpdateStatus(FilingStatusType.Expired, terminationDate, reason);
            
            AddDistributedEvent(new LegalAgreementUpdatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Title = Title,
                Type = Type,
                Status = Status.Status,
                ExpirationDate = ExpirationDate
            });
        }

        /// <summary>
        /// Checks if the agreement is active
        /// </summary>
        public bool IsActive(DateTime checkDate)
        {
            return (Status.Status == FilingStatusType.Approved || Status.Status == FilingStatusType.Renewed) &&
                   EffectiveDate <= checkDate &&
                   (!ExpirationDate.HasValue || ExpirationDate.Value > checkDate);
        }

        /// <summary>
        /// Checks if the agreement is expired
        /// </summary>
        public bool IsExpired(DateTime checkDate)
        {
            return Status.Status == FilingStatusType.Expired ||
                   (ExpirationDate.HasValue && ExpirationDate.Value <= checkDate);
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                LegalComplianceConsts.MinAgreementTitleLength,
                LegalComplianceConsts.MaxAgreementTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: LegalComplianceConsts.MaxAgreementDescriptionLength
            );
        }

        private void SetCounterpartyName(string counterpartyName)
        {
            CounterpartyName = Check.NotNullOrWhiteSpace(
                counterpartyName,
                nameof(counterpartyName),
                maxLength: LegalComplianceConsts.MaxPartyNameLength
            );
        }
    }
}
