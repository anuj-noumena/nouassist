namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates
{
    /// <summary>
    /// Organization aggregate root representing a company or business entity
    /// </summary>
    public class Organization : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the organization
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Contact information
        /// </summary>
        public ContactInfo ContactInfo { get; private set; }
        
        /// <summary>
        /// Detailed information about the organization
        /// </summary>
        public OrganizationInfo Details { get; private set; }
        
        /// <summary>
        /// Unique identifier like tax ID or business registration number
        /// </summary>
        public string RegistrationNumber { get; private set; }
        
        /// <summary>
        /// Parent organization ID for hierarchical relationships
        /// </summary>
        public Guid? ParentOrganizationId { get; private set; }
        
        /// <summary>
        /// Whether this organization is active
        /// </summary>
        public bool IsActive { get; private set; }
        
        /// <summary>
        /// Tags for categorization
        /// </summary>
        private readonly List<string> _tags;
        
        /// <summary>
        /// Read-only access to tags
        /// </summary>
        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

        protected Organization()
        {
            _tags = new List<string>();
        }

        public static Organization Create(
            Guid id,
            string name,
            ContactInfo contactInfo,
            OrganizationInfo details,
            string registrationNumber = null,
            Guid? parentOrganizationId = null,
            Guid? tenantId = null)
        {
            return new Organization(
                id,
                name,
                contactInfo,
                details,
                registrationNumber,
                parentOrganizationId,
                tenantId);
        }

        private Organization(
            Guid id,
            string name,
            ContactInfo contactInfo,
            OrganizationInfo details,
            string registrationNumber = null,
            Guid? parentOrganizationId = null,
            Guid? tenantId = null) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), CRMConsts.MaxLengths.Name);
            ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
            Details = details ?? throw new ArgumentNullException(nameof(details));
            RegistrationNumber = registrationNumber;
            ParentOrganizationId = parentOrganizationId;
            IsActive = true;
            TenantId = tenantId;
            _tags = new List<string>();
        }

        public void UpdateName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), CRMConsts.MaxLengths.Name);
        }

        public void UpdateContactInfo(ContactInfo contactInfo)
        {
            ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        }

        public void UpdateDetails(OrganizationInfo details)
        {
            Details = details ?? throw new ArgumentNullException(nameof(details));
        }

        public void UpdateRegistrationNumber(string registrationNumber)
        {
            RegistrationNumber = registrationNumber;
        }

        public void SetParentOrganization(Guid parentOrganizationId)
        {
            if (parentOrganizationId == Id)
            {
                throw new BusinessException("An organization cannot be its own parent");
            }
            
            ParentOrganizationId = parentOrganizationId;
        }

        public void RemoveParentOrganization()
        {
            ParentOrganizationId = null;
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

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
