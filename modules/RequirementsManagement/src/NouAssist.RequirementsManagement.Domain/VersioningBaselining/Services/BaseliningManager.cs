namespace NouAssist.RequirementsManagement.VersioningBaselining.Services
{
    /// <summary>
    /// Implementation of the baselining manager domain service
    /// </summary>
    public class BaseliningManager : DomainService, IBaseliningManager
    {
        private readonly IBaselineDefinitionRepository _baselineRepository;
        private readonly IVersioningEngine _versioningEngine;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<BaseliningManager> _logger;

        public BaseliningManager(
            IBaselineDefinitionRepository baselineRepository,
            IVersioningEngine versioningEngine,
            IGuidGenerator guidGenerator,
            ILogger<BaseliningManager> logger)
        {
            _baselineRepository = baselineRepository;
            _versioningEngine = versioningEngine;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<BaselineDefinition> CreateBaselineAsync(
            string name,
            string description,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            if (await _baselineRepository.ExistsByNameAsync(name, null, cancellationToken))
            {
                throw new BusinessException("VersionControl:ErrorMessage:BaselineNameAlreadyExists")
                    .WithData("baselineName", name);
            }
            
            var baseline = BaselineDefinition.Create(
                _guidGenerator.Create(),
                name,
                description,
                tenantId ?? CurrentTenant.Id);
            
            await _baselineRepository.InsertAsync(baseline, true, cancellationToken);
            
            _logger.LogInformation("Created baseline '{Name}' with ID {Id}", name, baseline.Id);
            
            return baseline;
        }

        /// <inheritdoc/>
        public async Task AddRequirementToBaselineAsync(
            Guid baselineId,
            Guid requirementId,
            string version,
            CancellationToken cancellationToken = default)
        {
            var baseline = await _baselineRepository.GetAsync(baselineId, cancellationToken);
            
            // Verify the requirement version exists
            await _versioningEngine.GetVersionAsync(requirementId, version, cancellationToken);
            
            var versionIdentifier = new VersionIdentifier(version);
            baseline.AddItem(requirementId, versionIdentifier, Clock.Now);
            
            await _baselineRepository.UpdateAsync(baseline, cancellationToken: cancellationToken);
            
            _logger.LogInformation(
                "Added requirement {RequirementId} version {Version} to baseline {BaselineId}",
                requirementId,
                version,
                baselineId);
        }

        /// <inheritdoc/>
        public async Task RemoveRequirementFromBaselineAsync(
            Guid baselineId,
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var baseline = await _baselineRepository.GetAsync(baselineId, cancellationToken);
            
            baseline.RemoveItem(requirementId);
            
            await _baselineRepository.UpdateAsync(baseline, cancellationToken: cancellationToken);
            
            _logger.LogInformation(
                "Removed requirement {RequirementId} from baseline {BaselineId}",
                requirementId,
                baselineId);
        }

        /// <inheritdoc/>
        public async Task UpdateRequirementVersionInBaselineAsync(
            Guid baselineId,
            Guid requirementId,
            string newVersion,
            CancellationToken cancellationToken = default)
        {
            var baseline = await _baselineRepository.GetAsync(baselineId, cancellationToken);
            
            // Verify the requirement version exists
            await _versioningEngine.GetVersionAsync(requirementId, newVersion, cancellationToken);
            
            var versionIdentifier = new VersionIdentifier(newVersion);
            baseline.UpdateItem(requirementId, versionIdentifier, Clock.Now);
            
            await _baselineRepository.UpdateAsync(baseline, cancellationToken: cancellationToken);
            
            _logger.LogInformation(
                "Updated requirement {RequirementId} to version {Version} in baseline {BaselineId}",
                requirementId,
                newVersion,
                baselineId);
        }

        /// <inheritdoc/>
        public async Task ActivateBaselineAsync(
            Guid baselineId,
            CancellationToken cancellationToken = default)
        {
            var baseline = await _baselineRepository.GetAsync(baselineId, cancellationToken);
            
            baseline.Activate(Clock.Now);
            
            await _baselineRepository.UpdateAsync(baseline, cancellationToken: cancellationToken);
            
            _logger.LogInformation("Activated baseline {BaselineId}", baselineId);
        }

        /// <inheritdoc/>
        public async Task ArchiveBaselineAsync(
            Guid baselineId,
            CancellationToken cancellationToken = default)
        {
            var baseline = await _baselineRepository.GetAsync(baselineId, cancellationToken);
            
            baseline.Archive(Clock.Now);
            
            await _baselineRepository.UpdateAsync(baseline, cancellationToken: cancellationToken);
            
            _logger.LogInformation("Archived baseline {BaselineId}", baselineId);
        }
    }
}
