namespace NouAssist.Core.Config
{
    public static class DomainConstants
    {
        // Notification constants
        public const int MaxNotificationTitleLength = 200;
        public const int MaxNotificationMessageLength = 1000;
        public const int NotificationExpiryDays = 30;

        // Resource constants
        public const int MaxFileSizeMB = 100;
        public const int MaxResourceVersions = 10;

        // Search constants
        public const int MaxSearchQueryLength = 500;
        public const int MaxSearchResults = 100;
        public const int MaxSavedSearches = 50;

        // General constants
        public const int MaxTitleLength = 200;
        public const int MaxDescriptionLength = 2000;
        public const int MaxTagLength = 50;
        public const int MaxTagsPerEntity = 20;
        
        // Habits
        public const int TargetSleepHours = 8;
        public const int TargetWaterGlasses = 8;
        public const int DefaultPomodoroMinutes = 25;
        public const int DefaultBreakMinutes = 5;
        public const int MaxHabitStreakDays = 365;
        public const int MaxPomodoroMinutes = 120;
        public const int MinPomodoroMinutes = 1;
        public const int MaxWaterGlasses = 20;
        public const int MaxSleepHours = 24;

        // Reviews
        public const int WeeklyReviewFrequencyDays = 7;
        public const int MonthlyReviewFrequencyDays = 30;
        public const int QuarterlyReviewFrequencyDays = 90;
        public const int AnnualReviewFrequencyDays = 365;
        public const int MaxReviewChecklistItems = 50;
        public const int MaxHorizonRelatedEntities = 100;

        // Business Rules
        public static class BusinessRuleThresholds
        {
            public const int HabitConsistencyThreshold = 70; // 70% consistency = strong habit
            public const int MinReviewCompletenessScore = 60;
            public const double MinActionItemCompletionRate = 0.8;
        }
    }
}
