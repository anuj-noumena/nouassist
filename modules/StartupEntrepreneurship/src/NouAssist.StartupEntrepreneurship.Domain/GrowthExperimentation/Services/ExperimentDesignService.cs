namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services
{
    /// <summary>
    /// Implementation of the experiment design service
    /// </summary>
    public class ExperimentDesignService : DomainService, IExperimentDesignService
    {
        private readonly IGrowthExperimentRepository _experimentRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<ExperimentDesignService> _logger;

        public ExperimentDesignService(
            IGrowthExperimentRepository experimentRepository,
            IGuidGenerator guidGenerator,
            ILogger<ExperimentDesignService> logger)
        {
            _experimentRepository = experimentRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<GrowthExperiment> CreateExperimentAsync(
            string name,
            string description,
            ExperimentGoal goal,
            TargetMetric primaryMetric,
            IEnumerable<TargetMetric> secondaryMetrics = null)
        {
            try
            {
                var id = _guidGenerator.Create();
                var experiment = GrowthExperiment.Create(
                    id,
                    name,
                    description,
                    goal,
                    CurrentTenant.Id);

                // Add primary metric
                experiment.AddTargetMetric(primaryMetric);

                // Add secondary metrics if provided
                if (secondaryMetrics != null)
                {
                    foreach (var metric in secondaryMetrics)
                    {
                        experiment.AddTargetMetric(metric);
                    }
                }

                await _experimentRepository.InsertAsync(experiment);
                
                _logger.LogInformation(
                    "Created new experiment {ExperimentId} with name {ExperimentName}",
                    experiment.Id,
                    experiment.Name);

                return experiment;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create experiment with name {ExperimentName}",
                    name);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AddHypothesisAsync(Guid experimentId, string statement, string rationale)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(experimentId);
                experiment.AddHypothesis(statement, rationale);
                await _experimentRepository.UpdateAsync(experiment);

                _logger.LogInformation(
                    "Added hypothesis to experiment {ExperimentId}",
                    experimentId);
            }
            catch (EntityNotFoundException)
            {
                throw new ExperimentNotFoundException(experimentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to add hypothesis to experiment {ExperimentId}",
                    experimentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ExperimentVariant> AddControlVariantAsync(
            Guid experimentId,
            string name,
            string description,
            double trafficAllocation,
            string configuration,
            string url = null)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(experimentId);
                var variantSetup = new VariantSetup(trafficAllocation, configuration, url);
                var variant = experiment.AddVariant(
                    _guidGenerator.Create(),
                    name,
                    description,
                    VariantType.Control,
                    variantSetup);

                await _experimentRepository.UpdateAsync(experiment);

                _logger.LogInformation(
                    "Added control variant {VariantId} to experiment {ExperimentId}",
                    variant.Id,
                    experimentId);

                return variant;
            }
            catch (EntityNotFoundException)
            {
                throw new ExperimentNotFoundException(experimentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to add control variant to experiment {ExperimentId}",
                    experimentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ExperimentVariant> AddTreatmentVariantAsync(
            Guid experimentId,
            string name,
            string description,
            double trafficAllocation,
            string configuration,
            string url = null)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(experimentId);
                var variantSetup = new VariantSetup(trafficAllocation, configuration, url);
                var variant = experiment.AddVariant(
                    _guidGenerator.Create(),
                    name,
                    description,
                    VariantType.Treatment,
                    variantSetup);

                await _experimentRepository.UpdateAsync(experiment);

                _logger.LogInformation(
                    "Added treatment variant {VariantId} to experiment {ExperimentId}",
                    variant.Id,
                    experimentId);

                return variant;
            }
            catch (EntityNotFoundException)
            {
                throw new ExperimentNotFoundException(experimentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to add treatment variant to experiment {ExperimentId}",
                    experimentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task SetExperimentReadyAsync(Guid experimentId)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(experimentId);
                experiment.SetReady();
                await _experimentRepository.UpdateAsync(experiment);

                _logger.LogInformation(
                    "Set experiment {ExperimentId} as ready to run",
                    experimentId);
            }
            catch (EntityNotFoundException)
            {
                throw new ExperimentNotFoundException(experimentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to set experiment {ExperimentId} as ready",
                    experimentId);
                throw;
            }
        }
    }
}
