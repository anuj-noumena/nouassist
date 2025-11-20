namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates
{
    /// <summary>
    /// Aggregate root representing a solution prototype with its versions
    /// </summary>
    public class SolutionPrototype : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public PrototypeType Type { get; private set; }
        public Guid? RelatedHypothesisId { get; private set; }
        public string Purpose { get; private set; }
        public string TargetAudience { get; private set; }

        // Entities within the aggregate
        private readonly List<PrototypeVersion> _versions;

        // Public read-only access
        public IReadOnlyCollection<PrototypeVersion> Versions => _versions.AsReadOnly();

        protected SolutionPrototype()
        {
            _versions = new List<PrototypeVersion>();
        }

        public static SolutionPrototype Create(
            Guid id,
            string name,
            string description,
            PrototypeType type,
            string purpose,
            string targetAudience,
            Guid? relatedHypothesisId = null,
            Guid? tenantId = null)
        {
            return new SolutionPrototype(
                id, name, description, type, purpose,
                targetAudience, relatedHypothesisId, tenantId);
        }

        private SolutionPrototype(
            Guid id,
            string name,
            string description,
            PrototypeType type,
            string purpose,
            string targetAudience,
            Guid? relatedHypothesisId,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            Type = type;
            SetPurpose(purpose);
            SetTargetAudience(targetAudience);
            RelatedHypothesisId = relatedHypothesisId;
            TenantId = tenantId;
            _versions = new List<PrototypeVersion>();
        }

        public void UpdateBasicInfo(string name, string description, string purpose, string targetAudience)
        {
            SetName(name);
            SetDescription(description);
            SetPurpose(purpose);
            SetTargetAudience(targetAudience);
        }

        public void AddVersion(
            string versionNumber,
            string description,
            PrototypeLink link,
            DateTime createdDate,
            string createdBy)
        {
            // Ensure version number is unique
            if (_versions.Any(v => v.VersionNumber == versionNumber))
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:DuplicateName",
                    $"Version {versionNumber} already exists");
            }

            // Deactivate previous active version if this is not the first version
            if (_versions.Any())
            {
                var activeVersion = _versions.FirstOrDefault(v => v.IsActive);
                activeVersion?.Deactivate();
            }

            var version = new PrototypeVersion(
                GuidGenerator.Create(),
                Id,
                versionNumber,
                description,
                link,
                createdDate,
                createdBy);

            _versions.Add(version);
        }

        public void UpdateVersion(Guid versionId, string description, PrototypeLink link)
        {
            var version = GetVersion(versionId);
            version.UpdateDescription(description);
            version.UpdateLink(link);
        }

        public void ActivateVersion(Guid versionId)
        {
            // Deactivate all versions first
            foreach (var version in _versions)
            {
                version.Deactivate();
            }

            // Activate the specified version
            var targetVersion = GetVersion(versionId);
            targetVersion.Activate();
        }

        public void DeactivateVersion(Guid versionId)
        {
            var version = GetVersion(versionId);
            version.Deactivate();
        }

        public void AddVersionNotes(Guid versionId, string notes)
        {
            var version = GetVersion(versionId);
            version.UpdateNotes(notes);
        }

        public PrototypeVersion GetActiveVersion()
        {
            return _versions.FirstOrDefault(v => v.IsActive);
        }

        public PrototypeVersion GetLatestVersion()
        {
            return _versions.OrderByDescending(v => v.CreatedDate).FirstOrDefault();
        }

        public int GetVersionCount()
        {
            return _versions.Count;
        }

        public List<PrototypeVersion> GetVersionHistory()
        {
            return _versions.OrderBy(v => v.CreatedDate).ToList();
        }

        private PrototypeVersion GetVersion(Guid versionId)
        {
            var version = _versions.FirstOrDefault(v => v.Id == versionId);
            if (version == null)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:EntityNotFound",
                    "Prototype version not found");
            }
            return version;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProductDiscoveryConsts.MaxPrototypeNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ProductDiscoveryConsts.MaxPrototypeDescriptionLength
            );
        }

        private void SetPurpose(string purpose)
        {
            Purpose = Check.NotNullOrWhiteSpace(
                purpose,
                nameof(purpose),
                maxLength: 500
            );
        }

        private void SetTargetAudience(string targetAudience)
        {
            TargetAudience = Check.NotNullOrWhiteSpace(
                targetAudience,
                nameof(targetAudience),
                maxLength: 500
            );
        }
    }
}
