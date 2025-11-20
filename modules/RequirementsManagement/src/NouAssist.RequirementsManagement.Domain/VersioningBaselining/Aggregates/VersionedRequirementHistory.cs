namespace NouAssist.RequirementsManagement.VersioningBaselining.Aggregates
{
    /// <summary>
    /// Aggregate root representing the version history of a requirement
    /// </summary>
    public class VersionedRequirementHistory : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// ID of the tenant this requirement belongs to (if multi-tenant)
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// ID of the requirement this history is for
        /// </summary>
        public Guid RequirementId { get; private set; }
        
        /// <summary>
        /// Latest version of the requirement
        /// </summary>
        public VersionIdentifier LatestVersion { get; private set; }
        
        private readonly List<RequirementVersionRecord> _versionRecords;
        
        /// <summary>
        /// Collection of all version records
        /// </summary>
        public IReadOnlyCollection<RequirementVersionRecord> VersionRecords => _versionRecords.AsReadOnly();

        protected VersionedRequirementHistory()
        {
            _versionRecords = new List<RequirementVersionRecord>();
        }

        /// <summary>
        /// Creates a new VersionedRequirementHistory for a requirement
        /// </summary>
        public static VersionedRequirementHistory Create(
            Guid id,
            Guid requirementId,
            Guid? tenantId)
        {
            return new VersionedRequirementHistory(id, requirementId, tenantId);
        }

        private VersionedRequirementHistory(
            Guid id,
            Guid requirementId,
            Guid? tenantId) : base(id)
        {
            RequirementId = requirementId;
            TenantId = tenantId;
            LatestVersion = new VersionIdentifier("0.0.0");
            _versionRecords = new List<RequirementVersionRecord>();
        }

        /// <summary>
        /// Creates a new version of the requirement
        /// </summary>
        public RequirementVersionRecord CreateVersion(
            VersionIdentifier version,
            AuthorRef author,
            ChangeComment changeComment,
            DateTime creationTime,
            string contentSnapshot)
        {
            if (_versionRecords.Any(v => v.Version.Value == version.Value))
            {
                throw new BusinessException("VersionControl:ErrorMessage:VersionAlreadyExists")
                    .WithData("requirementId", RequirementId)
                    .WithData("version", version.Value);
            }

            var versionRecord = new RequirementVersionRecord(
                RequirementId,
                version,
                author,
                changeComment,
                creationTime,
                contentSnapshot);

            _versionRecords.Add(versionRecord);

            // Update latest version if the new version is higher
            if (LatestVersion.CompareTo(version) < 0)
            {
                LatestVersion = version;
            }

            AddDistributedEvent(new RequirementVersionCreatedEto
            {
                TenantId = TenantId,
                RequirementId = RequirementId,
                Version = version.Value,
                AuthorId = author.Id,
                AuthorName = author.Name,
                ChangeComment = changeComment.Text,
                CreationTime = creationTime
            });

            return versionRecord;
        }

        /// <summary>
        /// Gets a specific version of the requirement
        /// </summary>
        public RequirementVersionRecord GetVersion(string version)
        {
            var versionRecord = _versionRecords.FirstOrDefault(v => v.Version.Value == version);
            
            if (versionRecord == null)
            {
                throw new RequirementVersionNotFoundException(RequirementId, version);
            }
            
            return versionRecord;
        }

        /// <summary>
        /// Gets a specific version of the requirement
        /// </summary>
        public RequirementVersionRecord GetVersion(VersionIdentifier version)
        {
            return GetVersion(version.Value);
        }

        /// <summary>
        /// Gets the latest version of the requirement
        /// </summary>
        public RequirementVersionRecord GetLatestVersion()
        {
            if (!_versionRecords.Any())
            {
                throw new BusinessException("VersionControl:ErrorMessage:NoVersionsAvailable")
                    .WithData("requirementId", RequirementId);
            }
            
            return GetVersion(LatestVersion);
        }

        /// <summary>
        /// Creates a new major version of the requirement
        /// </summary>
        public RequirementVersionRecord CreateMajorVersion(
            AuthorRef author,
            ChangeComment changeComment,
            DateTime creationTime,
            string contentSnapshot)
        {
            var newVersion = LatestVersion.IncrementMajor();
            return CreateVersion(newVersion, author, changeComment, creationTime, contentSnapshot);
        }

        /// <summary>
        /// Creates a new minor version of the requirement
        /// </summary>
        public RequirementVersionRecord CreateMinorVersion(
            AuthorRef author,
            ChangeComment changeComment,
            DateTime creationTime,
            string contentSnapshot)
        {
            var newVersion = LatestVersion.IncrementMinor();
            return CreateVersion(newVersion, author, changeComment, creationTime, contentSnapshot);
        }

        /// <summary>
        /// Creates a new patch version of the requirement
        /// </summary>
        public RequirementVersionRecord CreatePatchVersion(
            AuthorRef author,
            ChangeComment changeComment,
            DateTime creationTime,
            string contentSnapshot)
        {
            var newVersion = LatestVersion.IncrementPatch();
            return CreateVersion(newVersion, author, changeComment, creationTime, contentSnapshot);
        }
    }
}
