namespace NouAssist.IntelligenceStrategy.DecisionSupport.Entities
{
    /// <summary>
    /// Represents an alternative in a decision model
    /// </summary>
    public class Alternative : Entity<Guid>
    {
        /// <summary>
        /// The decision model this alternative belongs to
        /// </summary>
        public Guid DecisionModelId { get; private set; }
        
        /// <summary>
        /// Name of the alternative
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of the alternative
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Scores for this alternative on different criteria
        /// </summary>
        private readonly List<AlternativeScore> _scores;
        
        /// <summary>
        /// Read-only collection of scores
        /// </summary>
        public IReadOnlyCollection<AlternativeScore> Scores => _scores.AsReadOnly();

        protected Alternative() 
        {
            _scores = new List<AlternativeScore>();
        }

        /// <summary>
        /// Creates a new instance of <see cref="Alternative"/>
        /// </summary>
        internal Alternative(
            Guid id,
            Guid decisionModelId,
            string name,
            string description) : base(id)
        {
            DecisionModelId = decisionModelId;
            SetName(name);
            SetDescription(description);
            _scores = new List<AlternativeScore>();
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: 100
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
        
        /// <summary>
        /// Sets a score for this alternative on a specific criterion
        /// </summary>
        internal void SetScore(Guid criterionId, decimal score)
        {
            if (score < 0 || score > 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(score),
                    "Score must be between 0 and 1");
            }
            
            var existingScore = _scores.FirstOrDefault(s => s.CriterionId == criterionId);
            
            if (existingScore != null)
            {
                existingScore.UpdateScore(score);
            }
            else
            {
                _scores.Add(new AlternativeScore(Id, criterionId, score));
            }
        }
    }
}
