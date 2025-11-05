using Volo.Abp.Identity;

namespace NouAssist;

public static class NouAssistConsts
{
    public const string DbTablePrefix = "App";
    public const string? DbSchema = null;
    public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
    public const string AdminPasswordDefaultValue = IdentityDataSeedContributor.AdminPasswordDefaultValue;
    
    // Task Constants
    public const int MaxTitleLength = 500;
    public const int MaxDescriptionLength = 4000;
    public const int MaxNotesLength = 10000;
    public const int MaxColorLength = 7; // #RRGGBB
        
    public const int MaxActionSteps = 50;
    public const int MaxAcceptanceCriteria = 20;
    public const int MaxSubtasks = 100;
    public const int MaxWatchers = 50;
    public const int MaxLabels = 20;
        
    public const double MinImportance = 0.0;
    public const double MaxImportance = 1.0;
    public const double DefaultImportance = 0.5;
        
    public const double MinUrgency = 0.0;
    public const double MaxUrgency = 1.0;
    public const double DefaultUrgency = 0.5;
        
    public const int CriticalTaskDaysRemaining = 3;
    public const int HighPriorityTaskDaysRemaining = 7;
        
    public const int MaxRecurrenceCount = 365;
    public const int MaxDependencies = 50;
        
    // Custom Field Limits
    public const int MaxCustomFieldsPerTaskType = 50;
    public const int MaxCustomFieldValueLength = 4000;
    
}
