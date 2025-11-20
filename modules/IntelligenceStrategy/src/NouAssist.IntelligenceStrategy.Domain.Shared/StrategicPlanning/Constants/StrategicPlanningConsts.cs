namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Constants
{
    /// <summary>
    /// Constants for Strategic Planning domain
    /// </summary>
    public static class StrategicPlanningConsts
    {
        // Strategic Goal Constants
        public const int MaxGoalTitleLength = 200;
        public const int MaxGoalDescriptionLength = 2000;
        public const int MaxTargetDescriptionLength = 500;
        
        // Crisis Management Constants
        public const int MaxPlanNameLength = 150;
        public const int MaxProtocolStepLength = 1000;
        public const int MaxContingencyDescriptionLength = 2000;
        
        // Sustainable Growth Constants
        public const int MaxFrameworkNameLength = 100;
        public const int MaxEsgGoalDescriptionLength = 1500;
        public const int MaxMetricNameLength = 100;
        
        // Opportunity Management Constants
        public const int MaxOpportunityTitleLength = 200;
        public const int MaxFilterCriterionLength = 500;
        public const int MaxExperimentDescriptionLength = 1000;
        
        // Transition Planning Constants
        public const int MaxTransitionPlanNameLength = 150;
        public const int MaxScenarioDescriptionLength = 2000;
        public const int MaxMilestoneDescriptionLength = 500;
        
        // Planning Horizon Constants
        public const int MinHorizonMonths = 1;
        public const int MaxHorizonMonths = 120; // 10 years
        
        // General Constants
        public const int MaxNotesLength = 3000;
        public const int MaxTagLength = 50;
        public const int MaxTagsPerEntity = 20;
    }
}
