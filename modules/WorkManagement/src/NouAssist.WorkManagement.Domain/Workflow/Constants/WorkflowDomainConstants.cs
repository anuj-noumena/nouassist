namespace NouAssist.WorkManagement.Workflow.Constants
{
    public static class WorkflowDomainConstants
    {
        public const int MaxTitleLength = 200;
        public const int MaxDescriptionLength = 5000;
        public const int MaxIdLength = 50;
        public const int MaxWorkflowStatusesPerWorkflow = 50;
        public const int MaxTransitionsPerWorkflow = 100;
        public const int MaxActionsPerTransition = 20;
        public const int MaxAllowedRolesPerTransition = 10;
        public const int MaxAllowedUsersPerTransition = 50;
    }
}
