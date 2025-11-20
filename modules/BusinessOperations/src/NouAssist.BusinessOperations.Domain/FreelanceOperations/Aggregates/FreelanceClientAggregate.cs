namespace NouAssist.BusinessOperations.FreelanceOperations.Aggregates
{
    /// <summary>
    /// Aggregate root representing a freelance client
    /// </summary>
    public class FreelanceClient : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the client
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Company name if applicable
        /// </summary>
        public string CompanyName { get; private set; }
        
        /// <summary>
        /// Contact information for the client
        /// </summary>
        public ClientContactInfo ContactInfo { get; private set; }
        
        /// <summary>
        /// Current status of the client
        /// </summary>
        public ClientStatus Status { get; private set; }
        
        /// <summary>
        /// Notes about the client
        /// </summary>
        public string Notes { get; private set; }
        
        /// <summary>
        /// Source of the client (e.g., Referral, Website, LinkedIn)
        /// </summary>
        public string Source { get; private set; }
        
        /// <summary>
        /// Date when client relationship started
        /// </summary>
        public DateTime? RelationshipStartDate { get; private set; }
        
        /// <summary>
        /// Collection of communication records with this client
        /// </summary>
        public IReadOnlyCollection<ClientCommunicationRecord> CommunicationRecords => _communicationRecords.AsReadOnly();
        private readonly List<ClientCommunicationRecord> _communicationRecords;

        protected FreelanceClient()
        {
            _communicationRecords = new List<ClientCommunicationRecord>();
        }

        /// <summary>
        /// Creates a new freelance client
        /// </summary>
        public static FreelanceClient Create(
            Guid id,
            string name,
            ClientContactInfo contactInfo,
            string companyName = null,
            ClientStatus initialStatus = ClientStatus.Lead,
            string source = null,
            string notes = null,
            Guid? tenantId = null)
        {
            var client = new FreelanceClient(
                id,
                name,
                contactInfo,
                companyName,
                initialStatus,
                source,
                notes,
                tenantId);
                
            return client;
        }

        private FreelanceClient(
            Guid id,
            string name,
            ClientContactInfo contactInfo,
            string companyName = null,
            ClientStatus initialStatus = ClientStatus.Lead,
            string source = null,
            string notes = null,
            Guid? tenantId = null) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: FreelanceOperationsConsts.Client.MaxNameLength
            );
            
            ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
            CompanyName = !string.IsNullOrWhiteSpace(companyName) ? 
                companyName.Truncate(FreelanceOperationsConsts.Client.MaxCompanyNameLength) : null;
            Status = initialStatus;
            Source = source;
            Notes = notes;
            TenantId = tenantId;
            _communicationRecords = new List<ClientCommunicationRecord>();
            RelationshipStartDate = initialStatus == ClientStatus.Active ? DateTime.UtcNow : null;
            
            AddDistributedEvent(new FreelanceClientCreatedEto
            {
                Id = Id,
                Name = Name,
                Email = ContactInfo.Email,
                CompanyName = CompanyName,
                Status = Status,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Update the basic information for this client
        /// </summary>
        public void UpdateBasicInfo(
            string name,
            string companyName,
            string notes)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: FreelanceOperationsConsts.Client.MaxNameLength
            );
            
            CompanyName = !string.IsNullOrWhiteSpace(companyName) ? 
                companyName.Truncate(FreelanceOperationsConsts.Client.MaxCompanyNameLength) : null;
            
            Notes = notes;
            
            AddDistributedEvent(new FreelanceClientUpdatedEto
            {
                Id = Id,
                Name = Name,
                Email = ContactInfo.Email,
                Status = Status,
                UpdatedAt = DateTime.UtcNow,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Update the contact information for this client
        /// </summary>
        public void UpdateContactInfo(ClientContactInfo contactInfo)
        {
            ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
            
            AddDistributedEvent(new FreelanceClientUpdatedEto
            {
                Id = Id,
                Name = Name,
                Email = ContactInfo.Email,
                Status = Status,
                UpdatedAt = DateTime.UtcNow,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Change the status of this client
        /// </summary>
        public void ChangeStatus(ClientStatus newStatus)
        {
            // When transitioning to Active status, set relationship start date if not already set
            if (newStatus == ClientStatus.Active && Status != ClientStatus.Active && !RelationshipStartDate.HasValue)
            {
                RelationshipStartDate = DateTime.UtcNow;
            }
            
            Status = newStatus;
            
            AddDistributedEvent(new FreelanceClientUpdatedEto
            {
                Id = Id,
                Name = Name,
                Email = ContactInfo.Email,
                Status = Status,
                UpdatedAt = DateTime.UtcNow,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Add a communication record for this client
        /// </summary>
        public ClientCommunicationRecord AddCommunicationRecord(
            Guid communicationId,
            CommunicationLog communicationDetails,
            Guid? projectId = null,
            bool requiresFollowUp = false,
            DateTime? followUpDate = null)
        {
            var record = new ClientCommunicationRecord(
                communicationId,
                Id,
                communicationDetails,
                projectId,
                requiresFollowUp,
                followUpDate
            );
            
            _communicationRecords.Add(record);
            
            return record;
        }

        /// <summary>
        /// Update a communication record's follow-up status
        /// </summary>
        public void UpdateCommunicationFollowUp(
            Guid communicationId,
            bool requiresFollowUp,
            DateTime? followUpDate = null)
        {
            var record = GetCommunicationRecord(communicationId);
            record.SetFollowUpRequirement(requiresFollowUp, followUpDate);
        }

        /// <summary>
        /// Mark a communication follow-up as completed
        /// </summary>
        public void MarkCommunicationFollowUpCompleted(Guid communicationId)
        {
            var record = GetCommunicationRecord(communicationId);
            record.MarkFollowUpCompleted();
        }

        /// <summary>
        /// Remove a communication record
        /// </summary>
        public void RemoveCommunicationRecord(Guid communicationId)
        {
            var record = GetCommunicationRecord(communicationId);
            _communicationRecords.Remove(record);
        }

        /// <summary>
        /// Get a specific communication record
        /// </summary>
        private ClientCommunicationRecord GetCommunicationRecord(Guid communicationId)
        {
            var record = _communicationRecords.FirstOrDefault(r => r.Id == communicationId);
            
            if (record == null)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:EntityNotFound", "Communication record");
            }
            
            return record;
        }

        /// <summary>
        /// Get all communication records that require follow-up
        /// </summary>
        public IReadOnlyList<ClientCommunicationRecord> GetPendingFollowUps()
        {
            return _communicationRecords
                .Where(r => r.RequiresFollowUp)
                .OrderBy(r => r.FollowUpDate)
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// Check if the client has any overdue follow-ups
        /// </summary>
        public bool HasOverdueFollowUps()
        {
            return _communicationRecords.Any(r => 
                r.RequiresFollowUp && 
                r.FollowUpDate.HasValue && 
                r.FollowUpDate.Value < DateTime.UtcNow);
        }
    }
}
