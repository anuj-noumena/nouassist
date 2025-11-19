namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement
{
    public static class TasksDbConsts
    {
        // Task
        public const int MaxTitleLength = 500;
        public const int MaxDescriptionLength = 4000;
        public const int MaxExpectedResultLength = 2000;
        
        // ChecklistItem (replaces TaskActionStep)
        public const int MaxChecklistItemTitleLength = 500;
        public const int MaxChecklistItemDescriptionLength = 2000;
        
        // AcceptanceCriterion
        public const int MaxCriterionDescriptionLength = 1000;
        public const int MaxGivenContextLength = 500;
        public const int MaxWhenActionLength = 500;
        public const int MaxThenOutcomeLength = 500;
        public const int MaxTestScenarioLength = 2000;
        public const int MaxCriterionNotesLength = 2000;
        
        // TaskType
        public const int MaxTaskTypeNameLength = 200;
        public const int MaxTaskTypeDescriptionLength = 1000;
        public const int MaxColorLength = 50;
        
        // TaskInstance
        public const int MaxResolutionLength = 200;
        public const int MaxResolutionNoteLength = 2000;
        public const int MaxBlockedReasonLength = 1000;
        public const int MaxFlaggedReasonLength = 1000;
        public const int MaxCancellationReasonLength = 1000;
        
        // TaskActionStep
        public const int MaxStepTitleLength = 500;
        public const int MaxStepNotesLength = 2000;
    }
}
