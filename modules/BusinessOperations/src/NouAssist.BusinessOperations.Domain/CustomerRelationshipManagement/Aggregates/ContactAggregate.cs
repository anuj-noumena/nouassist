namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates
{
    /// <summary>
    /// Contact aggregate root representing a customer or person
    /// </summary>
    public class Contact : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Full name of the contact
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Contact's job title
        /// </summary>
        public string JobTitle { get; private set; }
        
        /// <summary>
        /// Contact information
        /// </summary>
        public ContactInfo ContactInfo { get; private set; }
        
        /// <summary>
        /// Organization this contact belongs to
        /// </summary>
        public Guid? OrganizationId { get; private set; }
        
        /// <summary>
        /// Whether this is a primary contact for the organization
        /// </summary>
        public bool IsPrimaryContact { get; private set; }
        
        /// <summary>
        /// Lead this contact was converted from
        /// </summary>
        public Guid? ConvertedFromLeadId { get; private set; }
        
        /// <summary>
        /// Notes about this contact
        /// </summary>
        public string Notes { get; private set; }
        
        /// <summary>
        /// Whether this contact is active
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
        
        /// <summary>
        /// Group IDs this contact belongs to
        /// </summary>
        private readonly List<Guid> _groupIds;
        
        /// <summary>
        /// Read-only access to group IDs
        /// </summary>
        public IReadOnlyCollection<Guid> GroupIds => _groupIds.AsReadOnly();

        protected Contact()
        {
            _tags = new List<string>();
            _groupIds = new List<Guid>();
        }

        public static Contact Create(
            Guid id,
            string name,
            ContactInfo contactInfo,
            string jobTitle = null,
            Guid? organizationId = null,
            bool isPrimaryContact = false,
            Guid? convertedFromLeadId = null,
            string notes = null,
            Guid? tenantId = null)
        {
            return new Contact(
                id,
                name,
                contactInfo,
                jobTitle,
                organizationId,
                isPrimaryContact,
                convertedFromLeadId,
                notes,
                tenantId);
        }

        public static Contact CreateFromLead(
            Guid id,
            Lead lead,
            Guid? organizationId = null,
            bool isPrimaryContact = false)
        {
            return new Contact(
                id,
                lead.Name,
                lead.ContactInfo,
                lead.Details.JobTitle,
                organizationId,
                isPrimaryContact,
                lead.Id,
                lead.Details.Notes,
                lead.TenantId);
        }

        private Contact(
            Guid id,
            string name,
            ContactInfo contactInfo,
            string jobTitle = null,
            Guid? organizationId = null,
            bool isPrimaryContact = false,
            Guid? convertedFromLeadId = null,
            string notes = null,
            Guid? tenantId = null) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), CRMConsts.MaxLengths.Name);
            ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
            JobTitle = jobTitle;
            OrganizationId = organizationId;
            IsPrimaryContact = isPrimaryContact;
            ConvertedFromLeadId = convertedFromLeadId;
            Notes = notes;
            IsActive = true;
            TenantId = tenantId;
            _tags = new List<string>();
            _groupIds = new List<Guid>();
        }

        public void UpdateName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), CRMConsts.MaxLengths.Name);
        }

        public void UpdateContactInfo(ContactInfo contactInfo)
        {
            ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        }

        public void UpdateJobTitle(string jobTitle)
        {
            JobTitle = jobTitle;
        }

        public void SetOrganization(Guid organizationId, bool isPrimaryContact = false)
        {
            OrganizationId = organizationId;
            IsPrimaryContact = isPrimaryContact;
        }

        public void RemoveFromOrganization()
        {
            OrganizationId = null;
            IsPrimaryContact = false;
        }

        public void UpdateNotes(string notes)
        {
            Notes = notes;
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

        public void AddToGroup(Guid groupId)
        {
            if (!_groupIds.Contains(groupId))
            {
                _groupIds.Add(groupId);
            }
        }

        public void RemoveFromGroup(Guid groupId)
        {
            _groupIds.Remove(groupId);
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
