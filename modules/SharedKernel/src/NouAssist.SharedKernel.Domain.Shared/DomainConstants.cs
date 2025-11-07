namespace NouAssist.SharedKernel
{
    public static class DomainConstants
    {
        // Validation Limits
        public const int MaxTitleLength = 255;
        public const int MaxDescriptionLength = 5000;
        public const int MaxNotesLength = 10000;
        public const int MaxBioLength = 500;
        
        // Score Ranges
        public const double MinImportance = 0.0;
        public const double MaxImportance = 1.0;
        public const double MinUrgency = 0.0;
        public const double MaxUrgency = 1.0;
        public const double DefaultImportance = 0.5;
        public const double DefaultUrgency = 0.5;
        
        // Thresholds
        public const double HighImportanceThreshold = 0.75;
        public const double LowImportanceThreshold = 0.25;
        public const double HighUrgencyThreshold = 0.75;
        public const double LowUrgencyThreshold = 0.25;
        
        // Pomodoro
        public const int MaxPomodoroMinutes = 60;
        public const int MinPomodoroMinutes = 5;
        public const int DefaultPomodoroMinutes = 25;
        public const int DefaultBreakMinutes = 5;
        public const int DefaultLongBreakMinutes = 15;
        
        // Tags
        public const int MaxTagsPerEntity = 10;
        public const int PopularTagThreshold = 10;
        
        // Reviews
        public const int StaleDaysThreshold = 7;
        public const int ReviewDaysThreshold = 90;
        
        // Alignment
        public const int MinAlignmentStrength = 1;
        public const int MaxAlignmentStrength = 5;
    }
}
