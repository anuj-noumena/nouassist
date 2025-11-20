namespace NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates
{
    /// <summary>
    /// Represents a structured decision model for evaluating options systematically
    /// </summary>
    public class DecisionModelInstance : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        #region Properties
        
        /// <summary>
        /// The tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Title of the decision model
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// Description of the decision context
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Type of decision model (e.g., "WeightedCriteria", "MAUT", "AHP")
        /// </summary>
        public string ModelType { get; private set; }
        
        /// <summary>
        /// Date when the decision was finalized
        /// </summary>
        public DateTime? DecisionDate { get; private set; }
        
        /// <summary>
        /// Criteria used in this decision model
        /// </summary>
        private readonly List<Criterion> _criteria;
        
        /// <summary>
        /// Read-only collection of criteria
        /// </summary>
        public IReadOnlyCollection<Criterion> Criteria => _criteria.AsReadOnly();
        
        /// <summary>
        /// Alternatives being evaluated in this decision model
        /// </summary>
        private readonly List<Alternative> _alternatives;
        
        /// <summary>
        /// Read-only collection of alternatives
        /// </summary>
        public IReadOnlyCollection<Alternative> Alternatives => _alternatives.AsReadOnly();
        
        /// <summary>
        /// ID of the selected alternative, if a decision was made
        /// </summary>
        public Guid? SelectedAlternativeId { get; private set; }
        
        /// <summary>
        /// Whether the decision model has been finalized
        /// </summary>
        public bool IsFinalized { get; private set; }
        
        #endregion

        #region Constructors
        
        protected DecisionModelInstance()
        {
            _criteria = new List<Criterion>();
            _alternatives = new List<Alternative>();
        }
        
        /// <summary>
        /// Creates a new decision model instance
        /// </summary>
        public static DecisionModelInstance Create(
            Guid id,
            string title,
            string description,
            string modelType,
            Guid? tenantId = null)
        {
            return new DecisionModelInstance(
                id,
                title,
                description,
                modelType,
                tenantId);
        }
        
        private DecisionModelInstance(
            Guid id,
            string title,
            string description,
            string modelType,
            Guid? tenantId) : base(id)
        {
            SetTitle(title);
            SetDescription(description);
            SetModelType(modelType);
            TenantId = tenantId;
            IsFinalized = false;
            _criteria = new List<Criterion>();
            _alternatives = new List<Alternative>();
            
            AddDistributedEvent(new DecisionModelCreatedEto
            {
                Id = Id,
                Title = Title,
                TenantId = TenantId,
                ModelType = ModelType
            });
        }
        
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Adds a criterion to the decision model
        /// </summary>
        public Criterion AddCriterion(string name, string description, decimal weight)
        {
            ValidateNotFinalized();
            
            var criterion = new Criterion(
                GuidGenerator.Create(),
                Id,
                name,
                description,
                weight);
                
            _criteria.Add(criterion);
            return criterion;
        }
        
        /// <summary>
        /// Adds an alternative to the decision model
        /// </summary>
        public Alternative AddAlternative(string name, string description)
        {
            ValidateNotFinalized();
            
            var alternative = new Alternative(
                GuidGenerator.Create(),
                Id,
                name,
                description);
                
            _alternatives.Add(alternative);
            return alternative;
        }
        
        /// <summary>
        /// Sets a score for an alternative on a specific criterion
        /// </summary>
        public void SetScore(Guid alternativeId, Guid criterionId, decimal score)
        {
            ValidateNotFinalized();
            
            var alternative = _alternatives.FirstOrDefault(a => a.Id == alternativeId)
                ?? throw new EntityNotFoundException(nameof(Alternative), alternativeId);
                
            var criterion = _criteria.FirstOrDefault(c => c.Id == criterionId)
                ?? throw new EntityNotFoundException(nameof(Criterion), criterionId);
                
            alternative.SetScore(criterionId, score);
        }
        
        /// <summary>
        /// Finalizes the decision model and selects the best alternative
        /// </summary>
        public void FinalizeDecision(Guid selectedAlternativeId, DateTime? decisionTime = null)
        {
            if (IsFinalized)
            {
                throw new DecisionSupportDomainException(
                    "DecisionSupport:ErrorMessage:AlreadyFinalized",
                    "This decision model has already been finalized");
            }
            
            if (!_alternatives.Any(a => a.Id == selectedAlternativeId))
            {
                throw new EntityNotFoundException(nameof(Alternative), selectedAlternativeId);
            }
            
            IsFinalized = true;
            SelectedAlternativeId = selectedAlternativeId;
            DecisionDate = decisionTime ?? DateTime.UtcNow;
            
            AddDistributedEvent(new DecisionFinalizedEto
            {
                Id = Id,
                Title = Title,
                TenantId = TenantId,
                SelectedAlternativeId = selectedAlternativeId,
                DecisionDate = DecisionDate.Value
            });
        }
        
        /// <summary>
        /// Proposes a strategic initiative based on this decision model
        /// </summary>
        public StrategicInitiativeProposal ProposeInitiative(
            string title,
            string description,
            DecisionRationaleText rationale,
            ExpectedOutcomeText expectedOutcomes,
            AlignmentStatus alignment = AlignmentStatus.NotEvaluated)
        {
            if (!IsFinalized)
            {
                throw new DecisionSupportDomainException(
                    "DecisionSupport:ErrorMessage:DecisionNotFinalized",
                    "Cannot propose an initiative from a decision model that has not been finalized");
            }
            
            var initiative = StrategicInitiativeProposal.Create(
                GuidGenerator.Create(),
                title,
                description,
                rationale,
                expectedOutcomes,
                Id,
                alignment,
                TenantId);
                
            // Add reference to the selected alternative
            var selectedAlternative = _alternatives.FirstOrDefault(a => a.Id == SelectedAlternativeId);
            if (selectedAlternative != null)
            {
                initiative.AddSupportingAnalysis(
                    "SelectedAlternative",
                    selectedAlternative.Id.ToString(),
                    $"Selected alternative: {selectedAlternative.Name}");
            }
            
            return initiative;
        }
        
        #endregion

        #region Private Methods
        
        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: DecisionSupportConsts.MaxTitleLength
            );
        }
        
        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: DecisionSupportConsts.MaxDescriptionLength
            );
        }
        
        private void SetModelType(string modelType)
        {
            ModelType = Check.NotNullOrWhiteSpace(
                modelType,
                nameof(modelType),
                maxLength: 50
            );
        }
        
        private void ValidateNotFinalized()
        {
            if (IsFinalized)
            {
                throw new DecisionSupportDomainException(
                    "DecisionSupport:ErrorMessage:CannotModifyFinalized",
                    "Cannot modify a finalized decision model");
            }
        }
        
        #endregion
    }
}
