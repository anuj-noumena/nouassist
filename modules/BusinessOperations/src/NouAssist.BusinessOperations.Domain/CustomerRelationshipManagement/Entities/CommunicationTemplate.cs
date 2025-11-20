namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities
{
    /// <summary>
    /// Template for standardized communications
    /// </summary>
    public class CommunicationTemplate : Entity, IMultiTenant
    {
        /// <summary>
        /// Tenant ID
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Unique code for the template
        /// </summary>
        public string Code { get; private set; }
        
        /// <summary>
        /// Name of the template
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Type of communication this template is for
        /// </summary>
        public InteractionType Type { get; private set; }
        
        /// <summary>
        /// Subject line for the template
        /// </summary>
        public string Subject { get; private set; }
        
        /// <summary>
        /// Content of the template
        /// </summary>
        public string Content { get; private set; }
        
        /// <summary>
        /// Whether this template is active
        /// </summary>
        public bool IsActive { get; private set; }
        
        /// <summary>
        /// Optional category for organization
        /// </summary>
        public string Category { get; private set; }
        
        /// <summary>
        /// Description of the template's purpose
        /// </summary>
        public string Description { get; private set; }

        protected CommunicationTemplate()
        {
        }

        public CommunicationTemplate(
            Guid id,
            string code,
            string name,
            InteractionType type,
            string subject,
            string content,
            string description = null,
            string category = null,
            Guid? tenantId = null)
            : base(id)
        {
            Code = Check.NotNullOrWhiteSpace(code, nameof(code), CRMConsts.MaxLengths.Code);
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), CRMConsts.MaxLengths.Name);
            Type = type;
            Subject = Check.NotNullOrWhiteSpace(subject, nameof(subject));
            Content = Check.NotNullOrWhiteSpace(content, nameof(content));
            Description = description;
            Category = category;
            IsActive = true;
            TenantId = tenantId;
        }

        public void Update(
            string name = null,
            string subject = null,
            string content = null,
            string description = null,
            string category = null)
        {
            if (name != null)
            {
                Name = Check.NotNullOrWhiteSpace(name, nameof(name), CRMConsts.MaxLengths.Name);
            }

            if (subject != null)
            {
                Subject = Check.NotNullOrWhiteSpace(subject, nameof(subject));
            }

            if (content != null)
            {
                Content = Check.NotNullOrWhiteSpace(content, nameof(content));
            }

            if (description != null)
            {
                Description = description;
            }

            if (category != null)
            {
                Category = category;
            }
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public string RenderContent(Dictionary<string, string> variables)
        {
            // Simple template rendering logic
            var renderedContent = Content;
            foreach (var variable in variables)
            {
                renderedContent = renderedContent.Replace($"{{{{{variable.Key}}}}}", variable.Value);
            }
            return renderedContent;
        }

        public string RenderSubject(Dictionary<string, string> variables)
        {
            // Simple template rendering logic
            var renderedSubject = Subject;
            foreach (var variable in variables)
            {
                renderedSubject = renderedSubject.Replace($"{{{{{variable.Key}}}}}", variable.Value);
            }
            return renderedSubject;
        }
    }
}
