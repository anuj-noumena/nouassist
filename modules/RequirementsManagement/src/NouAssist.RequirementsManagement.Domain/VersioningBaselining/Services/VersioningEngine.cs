namespace NouAssist.RequirementsManagement.VersioningBaselining.Services
{
    /// <summary>
    /// Implementation of the versioning engine domain service
    /// </summary>
    public class VersioningEngine : DomainService, IVersioningEngine
    {
        private readonly IVersionedRequirementHistoryRepository _historyRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<VersioningEngine> _logger;

        public VersioningEngine(
            IVersionedRequirementHistoryRepository historyRepository,
            IGuidGenerator guidGenerator,
            ILogger<VersioningEngine> logger)
        {
            _historyRepository = historyRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<VersionedRequirementHistory> GetOrCreateRequirementHistoryAsync(
            Guid requirementId,
            Guid? tenantId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var history = await _historyRepository.GetByRequirementIdAsync(requirementId, cancellationToken);
                return history;
            }
            catch (RequirementVersionNotFoundException)
            {
                _logger.LogInformation("Creating new version history for requirement {RequirementId}", requirementId);
                
                var history = VersionedRequirementHistory.Create(
                    _guidGenerator.Create(),
                    requirementId,
                    tenantId);
                
                await _historyRepository.InsertAsync(history, true, cancellationToken);
                return history;
            }
        }

        /// <inheritdoc/>
        public async Task<RequirementVersionRecord> CreateVersionAsync(
            Guid requirementId,
            string contentSnapshot,
            string changeComment,
            Guid authorId,
            string authorName,
            VersionChangeType changeType,
            DateTime? creationTime = null,
            CancellationToken cancellationToken = default)
        {
            var history = await GetOrCreateRequirementHistoryAsync(requirementId, CurrentTenant.Id, cancellationToken);
            
            var timestamp = creationTime ?? Clock.Now;
            var author = new AuthorRef(authorId, authorName);
            var comment = new ChangeComment(changeComment);
            
            RequirementVersionRecord versionRecord;
            
            switch (changeType)
            {
                case VersionChangeType.Major:
                    versionRecord = history.CreateMajorVersion(author, comment, timestamp, contentSnapshot);
                    break;
                case VersionChangeType.Minor:
                    versionRecord = history.CreateMinorVersion(author, comment, timestamp, contentSnapshot);
                    break;
                case VersionChangeType.Patch:
                default:
                    versionRecord = history.CreatePatchVersion(author, comment, timestamp, contentSnapshot);
                    break;
            }
            
            await _historyRepository.UpdateAsync(history, cancellationToken: cancellationToken);
            
            _logger.LogInformation(
                "Created version {Version} for requirement {RequirementId}",
                versionRecord.Version.Value,
                requirementId);
            
            return versionRecord;
        }

        /// <inheritdoc/>
        public async Task<RequirementVersionRecord> GetVersionAsync(
            Guid requirementId,
            string version,
            CancellationToken cancellationToken = default)
        {
            var history = await _historyRepository.GetByRequirementIdAsync(requirementId, cancellationToken);
            return history.GetVersion(version);
        }

        /// <inheritdoc/>
        public async Task<RequirementVersionRecord> GetLatestVersionAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var history = await _historyRepository.GetByRequirementIdAsync(requirementId, cancellationToken);
            return history.GetLatestVersion();
        }

        /// <inheritdoc/>
        public async Task<List<RequirementVersionRecord>> GetVersionHistoryAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var history = await _historyRepository.GetByRequirementIdAsync(requirementId, cancellationToken);
            return history.VersionRecords.OrderByDescending(v => v.CreationTime).ToList();
        }

        /// <inheritdoc/>
        public async Task<VersionComparisonResult> CompareVersionsAsync(
            Guid requirementId,
            string firstVersion,
            string secondVersion,
            CancellationToken cancellationToken = default)
        {
            var history = await _historyRepository.GetByRequirementIdAsync(requirementId, cancellationToken);
            
            var firstVersionRecord = history.GetVersion(firstVersion);
            var secondVersionRecord = history.GetVersion(secondVersion);
            
            // Simple comparison - in a real implementation, you would use a diff algorithm
            // to calculate structured differences between the content snapshots
            var areIdentical = firstVersionRecord.ContentSnapshot == secondVersionRecord.ContentSnapshot;
            
            return new VersionComparisonResult
            {
                FirstVersion = firstVersion,
                SecondVersion = secondVersion,
                AreIdentical = areIdentical,
                Differences = areIdentical ? null : new { /* Structured differences */ }
            };
        }
    }
}
