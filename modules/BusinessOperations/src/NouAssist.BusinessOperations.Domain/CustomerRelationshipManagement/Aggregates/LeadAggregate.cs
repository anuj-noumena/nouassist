namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates
{
    /// <summary>
    /// Lead aggregate root representing a potential customer
    /// </summary>
    public class Lead : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Full name of the lead
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Current status in the sales process
        /// </summary>
        public LeadStatus Status { get; private set; }
        
        /// <summary>
        /// Contact information
        /// </summary>
        public ContactInfo ContactInfo { get; private set; }
        
        /// <summary>
        /// Additional lead details
        /// </summary>
        public LeadDetails Details { get; private set; }
        
        /// <summary>
        /// Lead score for prioritization
        /// </summary>
        public int Score { get; private set; }
        
        /// <summary>
        /// Tags for categorization
        /// </summary>
        private readonly List<string> _tags;
        
        /// <summary>
        /// Read-only access to tags
        /// </summary>
        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();
        
        /// <summary>
        /// Associated source tracking information
        /// </summary>
        public LeadSourceRecord SourceRecord { get; private set; }
        
        /// <summary>
        /// ID of the contact if this lead was converted
        /// </summary>
        public Guid? ConvertedToContactId { get; private set; }
        
        /// <summary>
        /// When the lead was converted to a contact
        /// </summary>
        public DateTime? ConversionDate { get; private set; }

        protected Lead()
        {
            _tags = new List<string>();
        }

        public static Lead Create(
            Guid id,
            string name,
            ContactInfo contactInfo,
            LeadDetails details,
            LeadStatus status = LeadStatus.New,
            int score = 0,
            Guid? tenantId = null)
        {
            return new Lead(id, name, contactInfo, details, status, score, tenantId);
        }

        private Lead(
            Guid id,
            string name,
            ContactInfo contactInfo,
            LeadDetails details,
            LeadStatus status = LeadStatus.New,
            int score = 0,
            Guid? tenantId = null) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), CRMConsts.MaxLengths.Name);
            ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
            Details = details ?? throw new ArgumentNullException(nameof(details));
            Status = status;
            Score = score;
            TenantId = tenantId;
            _tags = new List<string>();
        }

        public void SetSourceRecord(
            LeadSource source,
            DateTime captureTime,
            string campaignId = null,
            string utmSource = null,
            string utmMedium = null,
            string utmCampaign = null,
            string utmTerm = null,
            string utmContent = null,
            string referrerUrl = null,
            string landingPageUrl = null)
        {
            SourceRecord = new LeadSourceRecord(
                Id,
                source,
                captureTime,
                campaignId,
                utmSource,
                utmMedium,
                utmCampaign,
                utmTerm,
                utmContent,
                referrerUrl,
                landingPageUrl);
        }

        public void UpdateName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), CRMConsts.MaxLengths.Name);
        }

        public void UpdateContactInfo(ContactInfo contactInfo)
        {
            ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        }

        public void UpdateDetails(LeadDetails details)
        {
            Details = details ?? throw new ArgumentNullException(nameof(details));
        }

        public void UpdateScore(int score)
        {
            Score = score;
        }

        public void AddTag(string tag)
        {
            if (_tags.Count >= CRMConsts.Defaults.MaxTagsPerEntity)
            {
                throw new BusinessException("Maximum number of tags reached");
            }

            tag = tag.Trim();
            if (tag.Length > CRMConsts.MaxLengths.TagName)
            {
                throw new BusinessException("Tag name is too long");
            }

            if (!_tags.Contains(tag))
            {
                _tags.Add(tag);
            }
        }

        public void RemoveTag(string tag)
        {
            _tags.Remove(tag);
        }

        public void ChangeStatus(LeadStatus newStatus)
        {
            if (Status == LeadStatus.Converted && newStatus != LeadStatus.Converted)
            {
                throw new LeadAlreadyConvertedException(Id);
            }

            if (!IsValidStatusTransition(Status, newStatus))
            {
                throw new InvalidLeadStatusTransitionException(Status, newStatus);
            }

            var previousStatus = Status;
            Status = newStatus;

            AddDistributedEvent(new LeadStatusChangedEto
            {
                Id = Id,
                Name = Name,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                StatusChangeTime = Clock.Now,
                TenantId = TenantId
            });
        }

        public void MarkAsConverted(Guid contactId, DateTime conversionDate)
        {
            if (Status == LeadStatus.Converted)
            {
                throw new LeadAlreadyConvertedException(Id);
            }

            Status = LeadStatus.Converted;
            ConvertedToContactId = contactId;
            ConversionDate = conversionDate;

            AddDistributedEvent(new LeadConvertedEto
            {
                Id = Id,
                ContactId = contactId,
                LeadName = Name,
                ConversionTime = conversionDate,
                TenantId = TenantId
            });
        }

        private bool IsValidStatusTransition(LeadStatus currentStatus, LeadStatus newStatus)
        {
            // Implement status transition rules
            // This is a simple example; actual rules could be more complex
            switch (currentStatus)
            {
                case LeadStatus.New:
                    return newStatus != LeadStatus.Converted;
                    
                case LeadStatus.Contacted:
                    return newStatus != LeadStatus.New;
                    
                case LeadStatus.Qualified:
                    return newStatus != LeadStatus.New;
                    
                case LeadStatus.Unqualified:
                    return newStatus == LeadStatus.Archived;
                    
                case LeadStatus.Converted:
                    return false; // Cannot change status once converted
                    
                case LeadStatus.Archived:
                    return false; // Cannot change status once archived
                    
                default:
                    return true;
            }
        }
    }
}
