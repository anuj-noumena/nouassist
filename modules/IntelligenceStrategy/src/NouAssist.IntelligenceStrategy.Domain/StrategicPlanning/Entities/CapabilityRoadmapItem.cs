namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Entities
{
    /// <summary>
    /// Represents an item in a capability development roadmap
    /// </summary>
    public class CapabilityRoadmapItem : Entity<Guid>
    {
        public Guid TransitionPlanId { get; private set; }
        public SkillName Skill { get; private set; }
        public CapabilityLevel CurrentLevel { get; private set; }
        public CapabilityLevel TargetLevel { get; private set; }
        public DateTime TargetDate { get; private set; }
        public string DevelopmentPath { get; private set; }
        public List<string> RequiredActions { get; private set; }
        public decimal InvestmentRequired { get; private set; }

        protected CapabilityRoadmapItem()
        {
            RequiredActions = new List<string>();
        }

        internal CapabilityRoadmapItem(
            Guid id,
            Guid transitionPlanId,
            SkillName skill,
            CapabilityLevel currentLevel,
            CapabilityLevel targetLevel,
            DateTime targetDate,
            decimal investmentRequired = 0) : base(id)
        {
            TransitionPlanId = transitionPlanId;
            Skill = skill;
            CurrentLevel = currentLevel;
            SetTargetLevel(targetLevel);
            TargetDate = targetDate;
            InvestmentRequired = investmentRequired;
            RequiredActions = new List<string>();
        }

        internal void AddRequiredAction(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("Required action cannot be empty");
            
            RequiredActions.Add(action.Trim());
        }

        internal void UpdateProgress(CapabilityLevel newCurrentLevel)
        {
            if (newCurrentLevel > TargetLevel)
                throw new BusinessException("Current level cannot exceed target level");
            
            CurrentLevel = newCurrentLevel;
        }

        internal void SetDevelopmentPath(string path)
        {
            DevelopmentPath = Check.Length(path, nameof(path), 1000);
        }

        public bool IsCompleted => CurrentLevel >= TargetLevel;

        public decimal ProgressPercentage
        {
            get
            {
                var totalLevels = (int)TargetLevel - (int)CapabilityLevel.Beginner;
                var currentProgress = (int)CurrentLevel - (int)CapabilityLevel.Beginner;
                return totalLevels > 0 ? (decimal)currentProgress / totalLevels * 100 : 0;
            }
        }

        private void SetTargetLevel(CapabilityLevel targetLevel)
        {
            if (targetLevel <= CurrentLevel)
                throw new ArgumentException("Target level must be higher than current level");
            
            TargetLevel = targetLevel;
        }
    }
}
