namespace NouAssist.RequirementsManagement.Specification.Aggregates
{
    /// <summary>
    /// Represents a specification structure (e.g., SRS document structure)
    /// </summary>
    public class SpecificationStructure : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string DocumentType { get; private set; }
        public string Description { get; private set; }

        private readonly List<SpecificationSection> _sections;
        public IReadOnlyCollection<SpecificationSection> Sections => _sections.AsReadOnly();

        protected SpecificationStructure()
        {
            _sections = new List<SpecificationSection>();
        }

        /// <summary>
        /// Factory method to create a new specification structure
        /// </summary>
        public static SpecificationStructure Create(
            Guid id,
            string name,
            string documentType,
            string description,
            Guid? tenantId)
        {
            return new SpecificationStructure(id, name, documentType, description, tenantId);
        }

        private SpecificationStructure(
            Guid id,
            string name,
            string documentType,
            string description,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDocumentType(documentType);
            SetDescription(description);
            TenantId = tenantId;
            _sections = new List<SpecificationSection>();

            AddDistributedEvent(new SpecificationStructureCreatedEto
            {
                Id = Id,
                Name = Name,
                DocumentType = DocumentType,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the basic information
        /// </summary>
        public void UpdateInfo(string name, string documentType, string description)
        {
            SetName(name);
            SetDocumentType(documentType);
            SetDescription(description);
        }

        /// <summary>
        /// Adds a section to the structure
        /// </summary>
        public SpecificationSection AddSection(string sectionName, string content, int order, Guid? parentSectionId = null)
        {
            var section = new SpecificationSection(
                GuidGenerator.Create(),
                Id,
                sectionName,
                content,
                order,
                parentSectionId
            );
            _sections.Add(section);
            return section;
        }

        /// <summary>
        /// Removes a section and all its subsections
        /// </summary>
        public void RemoveSection(Guid sectionId)
        {
            var section = _sections.FirstOrDefault(s => s.Id == sectionId);
            if (section == null) return;

            // Remove all subsections first
            var subsections = _sections.Where(s => s.ParentSectionId == sectionId).ToList();
            foreach (var subsection in subsections)
            {
                RemoveSection(subsection.Id);
            }

            _sections.Remove(section);
        }

        /// <summary>
        /// Gets all root sections (sections without parent)
        /// </summary>
        public IReadOnlyCollection<SpecificationSection> GetRootSections()
        {
            return _sections.Where(s => s.ParentSectionId == null).OrderBy(s => s.Order).ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets subsections of a specific section
        /// </summary>
        public IReadOnlyCollection<SpecificationSection> GetSubsections(Guid parentSectionId)
        {
            return _sections.Where(s => s.ParentSectionId == parentSectionId).OrderBy(s => s.Order).ToList().AsReadOnly();
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: RequirementSpecificationManagementConsts.SpecificationStructure.MaxSectionNameLength
            );
        }

        private void SetDocumentType(string documentType)
        {
            DocumentType = Check.NotNullOrWhiteSpace(documentType, nameof(documentType), maxLength: 100);
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(description, nameof(description), maxLength: 2000);
        }
    }

    /// <summary>
    /// Represents a section within a specification structure
    /// </summary>
    public class SpecificationSection : Entity<Guid>
    {
        public Guid StructureId { get; private set; }
        public string SectionName { get; private set; }
        public string Content { get; private set; }
        public int Order { get; private set; }
        public Guid? ParentSectionId { get; private set; }

        protected SpecificationSection() { }

        internal SpecificationSection(
            Guid id,
            Guid structureId,
            string sectionName,
            string content,
            int order,
            Guid? parentSectionId = null) : base(id)
        {
            StructureId = structureId;
            SetSectionName(sectionName);
            SetContent(content);
            SetOrder(order);
            ParentSectionId = parentSectionId;
        }

        internal void UpdateContent(string sectionName, string content)
        {
            SetSectionName(sectionName);
            SetContent(content);
        }

        internal void UpdateOrder(int order)
        {
            SetOrder(order);
        }

        private void SetSectionName(string sectionName)
        {
            SectionName = Check.NotNullOrWhiteSpace(
                sectionName,
                nameof(sectionName),
                maxLength: RequirementSpecificationManagementConsts.SpecificationStructure.MaxSectionNameLength
            );
        }

        private void SetContent(string content)
        {
            Content = Check.Length(
                content,
                nameof(content),
                maxLength: RequirementSpecificationManagementConsts.SpecificationStructure.MaxSectionContentLength
            );
        }

        private void SetOrder(int order)
        {
            if (order < 0)
            {
                throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:InvalidSectionOrder");
            }
            Order = order;
        }
    }
}
