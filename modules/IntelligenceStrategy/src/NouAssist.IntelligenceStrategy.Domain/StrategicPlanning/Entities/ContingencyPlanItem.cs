namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Entities
{
    /// <summary>
    /// Represents a specific contingency plan item within a crisis management plan
    /// </summary>
    public class ContingencyPlanItem : Entity<Guid>
    {
        public Guid CrisisManagementPlanId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public RiskScenario RiskScenario { get; private set; }
        public List<string> ActionSteps { get; private set; }
        public List<string> RequiredResources { get; private set; }
        public Guid? ResponsibleUserId { get; private set; }
        public int Priority { get; private set; }

        protected ContingencyPlanItem()
        {
            ActionSteps = new List<string>();
            RequiredResources = new List<string>();
        }

        internal ContingencyPlanItem(
            Guid id,
            Guid crisisManagementPlanId,
            string title,
            string description,
            RiskScenario riskScenario,
            int priority) : base(id)
        {
            CrisisManagementPlanId = crisisManagementPlanId;
            SetTitle(title);
            SetDescription(description);
            RiskScenario = riskScenario;
            Priority = priority;
            ActionSteps = new List<string>();
            RequiredResources = new List<string>();
        }

        internal void AddActionStep(string step)
        {
            if (string.IsNullOrWhiteSpace(step))
                throw new ArgumentException("Action step cannot be empty");

            ActionSteps.Add(step.Trim());
        }

        internal void AddRequiredResource(string resource)
        {
            if (string.IsNullOrWhiteSpace(resource))
                throw new ArgumentException("Required resource cannot be empty");

            RequiredResources.Add(resource.Trim());
        }

        internal void AssignResponsible(Guid userId)
        {
            ResponsibleUserId = userId;
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: StrategicPlanningConsts.MaxGoalTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: StrategicPlanningConsts.MaxContingencyDescriptionLength
            );
        }
    }
}
