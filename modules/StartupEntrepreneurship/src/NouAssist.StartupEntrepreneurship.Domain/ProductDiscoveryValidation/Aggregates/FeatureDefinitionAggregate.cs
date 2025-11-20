namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates
{
    /// <summary>
    /// Aggregate root representing a feature definition with prioritization data
    /// </summary>
    public class FeatureDefinition : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string UserStory { get; private set; }
        public string AcceptanceCriteria { get; private set; }
        public FeaturePriority Priority { get; private set; }
        public MvpStatus MvpStatus { get; private set; }
        public Guid? RelatedHypothesisId { get; private set; }
        public Guid? RelatedPrototypeId { get; private set; }
        public string BusinessValue { get; private set; }
        public string TechnicalNotes { get; private set; }

        // Entities within the aggregate
        private readonly List<PriorityScore> _priorityScores;

        // Public read-only access
        public IReadOnlyCollection<PriorityScore> PriorityScores => _priorityScores.AsReadOnly();

        protected FeatureDefinition()
        {
            _priorityScores = new List<PriorityScore>();
        }

        public static FeatureDefinition Create(
            Guid id,
            string name,
            string description,
            string userStory,
            string acceptanceCriteria,
            string businessValue,
            Guid? relatedHypothesisId = null,
            Guid? relatedPrototypeId = null,
            Guid? tenantId = null)
        {
            return new FeatureDefinition(
                id, name, description, userStory, acceptanceCriteria,
                businessValue, relatedHypothesisId, relatedPrototypeId, tenantId);
        }

        private FeatureDefinition(
            Guid id,
            string name,
            string description,
            string userStory,
            string acceptanceCriteria,
            string businessValue,
            Guid? relatedHypothesisId,
            Guid? relatedPrototypeId,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            SetUserStory(userStory);
            SetAcceptanceCriteria(acceptanceCriteria);
            SetBusinessValue(businessValue);
            RelatedHypothesisId = relatedHypothesisId;
            RelatedPrototypeId = relatedPrototypeId;
            TenantId = tenantId;
            Priority = FeaturePriority.Medium;
            MvpStatus = MvpStatus.Candidate;
            _priorityScores = new List<PriorityScore>();
        }

        public void UpdateBasicInfo(
            string name,
            string description,
            string userStory,
            string acceptanceCriteria,
            string businessValue)
        {
            SetName(name);
            SetDescription(description);
            SetUserStory(userStory);
            SetAcceptanceCriteria(acceptanceCriteria);
            SetBusinessValue(businessValue);
        }

        public void UpdateTechnicalNotes(string technicalNotes)
        {
            TechnicalNotes = technicalNotes;
        }

        public void AddPriorityScore(
            PriorityInput input,
            DateTime calculatedAt,
            string calculatedBy)
        {
            var priorityScore = new PriorityScore(
                GuidGenerator.Create(),
                Id,
                input,
                calculatedAt,
                calculatedBy);

            _priorityScores.Add(priorityScore);

            // Update the current priority based on the latest score
            Priority = input.DeterminePriority();

            AddDistributedEvent(new FeatureDefinitionPrioritizedEto
            {
                Id = Id,
                Name = Name,
                Priority = Priority,
                MvpStatus = MvpStatus,
                PrioritizedDate = calculatedAt,
                TenantId = TenantId
            });
        }

        public void UpdateMvpStatus(MvpStatus mvpStatus)
        {
            MvpStatus = mvpStatus;
        }

        public void SetPriorityNotes(Guid scoreId, string notes)
        {
            var score = GetPriorityScore(scoreId);
            score.UpdateNotes(notes);
        }

        public PriorityScore GetLatestPriorityScore()
        {
            return _priorityScores.OrderByDescending(s => s.CalculatedAt).FirstOrDefault();
        }

        public decimal GetAveragePriorityScore()
        {
            if (_priorityScores.Count == 0)
                return 0;

            return _priorityScores.Average(s => s.CalculatedScore);
        }

        public bool IsIncludedInMvp()
        {
            return MvpStatus == MvpStatus.Planned ||
                   MvpStatus == MvpStatus.InDevelopment ||
                   MvpStatus == MvpStatus.Developed;
        }

        public List<PriorityScore> GetPriorityHistory()
        {
            return _priorityScores.OrderBy(s => s.CalculatedAt).ToList();
        }

        private PriorityScore GetPriorityScore(Guid scoreId)
        {
            var score = _priorityScores.FirstOrDefault(s => s.Id == scoreId);
            if (score == null)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:EntityNotFound",
                    "Priority score not found");
            }
            return score;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProductDiscoveryConsts.MaxFeatureNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ProductDiscoveryConsts.MaxFeatureDescriptionLength
            );
        }

        private void SetUserStory(string userStory)
        {
            UserStory = Check.NotNullOrWhiteSpace(
                userStory,
                nameof(userStory),
                maxLength: 1000
            );
        }

        private void SetAcceptanceCriteria(string acceptanceCriteria)
        {
            AcceptanceCriteria = Check.NotNullOrWhiteSpace(
                acceptanceCriteria,
                nameof(acceptanceCriteria),
                maxLength: 2000
            );
        }

        private void SetBusinessValue(string businessValue)
        {
            BusinessValue = Check.NotNullOrWhiteSpace(
                businessValue,
                nameof(businessValue),
                maxLength: 1000
            );
        }
    }
}
