namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates
{
    /// <summary>
    /// Aggregate root representing a conversion funnel definition
    /// </summary>
    public class ConversionFunnelDefinition : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Funnel name
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Funnel description
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Whether the funnel is active
        /// </summary>
        public bool IsActive { get; private set; }
        
        /// <summary>
        /// Stages in the funnel
        /// </summary>
        private readonly List<FunnelStage> _stages;
        public IReadOnlyCollection<FunnelStage> Stages => _stages.AsReadOnly();
        
        /// <summary>
        /// Overall conversion rate from top to bottom of funnel
        /// </summary>
        public double OverallConversionRate
        {
            get
            {
                var entryStage = _stages.FirstOrDefault(s => s.IsEntryStage);
                var exitStage = _stages.FirstOrDefault(s => s.IsExitStage);
                
                if (entryStage?.ConversionRate == null || exitStage?.ConversionRate == null || 
                    entryStage.ConversionRate.EnteredCount == 0)
                {
                    return 0;
                }
                
                return (double)exitStage.ConversionRate.CompletedCount / entryStage.ConversionRate.EnteredCount;
            }
        }
        
        protected ConversionFunnelDefinition()
        {
            _stages = new List<FunnelStage>();
        }
        
        /// <summary>
        /// Creates a new conversion funnel
        /// </summary>
        public static ConversionFunnelDefinition Create(
            Guid id,
            string name, 
            string description,
            Guid? tenantId = null)
        {
            var funnel = new ConversionFunnelDefinition
            {
                Id = id,
                TenantId = tenantId,
                IsActive = true
            };
            
            funnel.SetName(name);
            funnel.Description = description;
            
            return funnel;
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: GrowthExperimentationConstants.ConversionFunnels.MaxFunnelNameLength
            );
        }
        
        /// <summary>
        /// Adds a stage to the funnel
        /// </summary>
        public FunnelStage AddStage(
            Guid stageId,
            StageDefinition definition,
            bool isExitStage = false)
        {
            // Check for duplicate stage name
            if (_stages.Any(s => s.Definition.Name == definition.Name))
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:DuplicateStageName")
                    .WithData("stageName", definition.Name);
            }
            
            // Check for duplicate stage order
            if (_stages.Any(s => s.Definition.Order == definition.Order))
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:DuplicateStageOrder")
                    .WithData("order", definition.Order);
            }
            
            // If this is an exit stage, ensure there's no other exit stage
            if (isExitStage && _stages.Any(s => s.IsExitStage))
            {
                var existingExitStage = _stages.First(s => s.IsExitStage);
                existingExitStage.SetAsExitStage(false);
            }
            
            var stage = new FunnelStage(stageId, Id, definition, isExitStage);
            _stages.Add(stage);
            
            return stage;
        }
        
        /// <summary>
        /// Updates a stage's conversion rate
        /// </summary>
        public void UpdateStageConversionRate(Guid stageId, ConversionRatePerStage conversionRate)
        {
            var stage = _stages.FirstOrDefault(s => s.Id == stageId);
            
            if (stage == null)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:StageNotFound")
                    .WithData("stageId", stageId);
            }
            
            stage.UpdateConversionRate(conversionRate);
        }
        
        /// <summary>
        /// Sets the funnel as active or inactive
        /// </summary>
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
        
        /// <summary>
        /// Validates the funnel configuration
        /// </summary>
        public void Validate()
        {
            // Must have at least minimum required stages
            if (_stages.Count < GrowthExperimentationConstants.ConversionFunnels.MinFunnelStages)
            {
                throw new InvalidFunnelConfigurationException(
                    Id, 
                    $"Funnel must have at least {GrowthExperimentationConstants.ConversionFunnels.MinFunnelStages} stages"
                );
            }
            
            // Cannot exceed maximum stages
            if (_stages.Count > GrowthExperimentationConstants.ConversionFunnels.MaxFunnelStages)
            {
                throw new InvalidFunnelConfigurationException(
                    Id, 
                    $"Funnel cannot have more than {GrowthExperimentationConstants.ConversionFunnels.MaxFunnelStages} stages"
                );
            }
            
            // Must have exactly one entry stage (order=1)
            if (_stages.Count(s => s.Definition.Order == 1) != 1)
            {
                throw new InvalidFunnelConfigurationException(Id, "Funnel must have exactly one entry stage (order=1)");
            }
            
            // Must have exactly one exit stage
            if (_stages.Count(s => s.IsExitStage) != 1)
            {
                throw new InvalidFunnelConfigurationException(Id, "Funnel must have exactly one exit stage");
            }
            
            // Stages must have sequential ordering
            var orders = _stages.Select(s => s.Definition.Order).OrderBy(o => o).ToList();
            for (int i = 0; i < orders.Count; i++)
            {
                if (orders[i] != i + 1)
                {
                    throw new InvalidFunnelConfigurationException(Id, "Stages must have sequential ordering");
                }
            }
        }
    }
}
