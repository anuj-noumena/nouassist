namespace NouAssist.Projects.Constants
{
    public static class ProjectDomainConstants
    {
        // Limits
        public const int MaxTitleLength = 200;
        public const int MaxDescriptionLength = 5000;
        public const int MaxSummaryLength = 500;
        public const int MaxSuccessCriteria = 20;
        public const int MaxCollaboratingTeams = 10;
        public const int MaxIdLength = 50;

        // Priority
        public const double MinImportance = 0.0;
        public const double MaxImportance = 1.0;
        public const double MinUrgency = 0.0;
        public const double MaxUrgency = 1.0;
        public const double DefaultImportance = 0.5;
        public const double DefaultUrgency = 0.5;

        // Duration
        public const int MaxProjectDurationDays = 3650; // ~10 years
        public const int MinSprintDurationDays = 7;
        public const int MaxSprintDurationDays = 30;
        public const int DefaultSprintDurationDays = 14;

        // Story Points
        public const int MaxStoryPoints = 100;
        public const int DefaultSprintVelocity = 20;

        // Health Thresholds
        public const double ProjectHealthyThreshold = 0.7;
        public const double ProjectAtRiskThreshold = 0.4;
    }
}
