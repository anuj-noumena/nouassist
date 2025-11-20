namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Entities
{
    /// <summary>
    /// Represents a step in a crisis response protocol
    /// </summary>
    public class CrisisProtocolStep : Entity<Guid>
    {
        public Guid CrisisManagementPlanId { get; private set; }
        public string StepName { get; private set; }
        public string Description { get; private set; }
        public int OrderSequence { get; private set; }
        public EmergencyProcedure Procedure { get; private set; }
        public List<string> RequiredRoles { get; private set; }
        public TimeSpan MaxExecutionTime { get; private set; }
        public bool IsAutomated { get; private set; }

        protected CrisisProtocolStep()
        {
            RequiredRoles = new List<string>();
        }

        internal CrisisProtocolStep(
            Guid id,
            Guid crisisManagementPlanId,
            string stepName,
            string description,
            int orderSequence,
            EmergencyProcedure procedure,
            TimeSpan maxExecutionTime,
            bool isAutomated = false) : base(id)
        {
            CrisisManagementPlanId = crisisManagementPlanId;
            SetStepName(stepName);
            SetDescription(description);
            OrderSequence = orderSequence;
            Procedure = procedure;
            MaxExecutionTime = maxExecutionTime;
            IsAutomated = isAutomated;
            RequiredRoles = new List<string>();
        }

        internal void AddRequiredRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role cannot be empty");
            
            if (!RequiredRoles.Contains(role))
                RequiredRoles.Add(role);
        }

        internal void UpdateExecutionTime(TimeSpan newTime)
        {
            MaxExecutionTime = newTime;
        }

        private void SetStepName(string stepName)
        {
            StepName = Check.NotNullOrWhiteSpace(stepName, nameof(stepName), maxLength: 100);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 1000);
        }
    }
}
