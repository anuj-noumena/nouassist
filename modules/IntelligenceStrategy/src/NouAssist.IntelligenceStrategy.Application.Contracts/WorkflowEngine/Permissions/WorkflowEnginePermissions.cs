namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Permissions
{
    /// <summary>
    /// Defines the permissions for the Workflow Engine module
    /// </summary>
    public static class WorkflowEnginePermissions
    {
        /// <summary>
        /// The group name for workflow engine permissions
        /// </summary>
        public const string GroupName = "WorkflowEngine";

        /// <summary>
        /// Permissions for workflow definitions
        /// </summary>
        public static class WorkflowDefinitions
        {
            /// <summary>
            /// Base permission for workflow definitions
            /// </summary>
            public const string Default = GroupName + ".WorkflowDefinitions";

            /// <summary>
            /// Permission to create workflow definitions
            /// </summary>
            public const string Create = Default + ".Create";

            /// <summary>
            /// Permission to edit workflow definitions
            /// </summary>
            public const string Edit = Default + ".Edit";

            /// <summary>
            /// Permission to delete workflow definitions
            /// </summary>
            public const string Delete = Default + ".Delete";

            /// <summary>
            /// Permission to publish workflow definitions
            /// </summary>
            public const string Publish = Default + ".Publish";

            /// <summary>
            /// Permission to analyze workflow performance
            /// </summary>
            public const string Analyze = Default + ".Analyze";
        }

        /// <summary>
        /// Permissions for workflow templates
        /// </summary>
        public static class WorkflowTemplates
        {
            /// <summary>
            /// Base permission for workflow templates
            /// </summary>
            public const string Default = GroupName + ".WorkflowTemplates";

            /// <summary>
            /// Permission to create workflow templates
            /// </summary>
            public const string Create = Default + ".Create";

            /// <summary>
            /// Permission to edit workflow templates
            /// </summary>
            public const string Edit = Default + ".Edit";

            /// <summary>
            /// Permission to delete workflow templates
            /// </summary>
            public const string Delete = Default + ".Delete";

            /// <summary>
            /// Permission to make templates public
            /// </summary>
            public const string MakePublic = Default + ".MakePublic";
        }

        /// <summary>
        /// Permissions for workflow instances
        /// </summary>
        public static class WorkflowInstances
        {
            /// <summary>
            /// Base permission for workflow instances
            /// </summary>
            public const string Default = GroupName + ".WorkflowInstances";

            /// <summary>
            /// Permission to create workflow instances
            /// </summary>
            public const string Create = Default + ".Create";

            /// <summary>
            /// Permission to manage workflow instances (pause, resume, cancel)
            /// </summary>
            public const string Manage = Default + ".Manage";

            /// <summary>
            /// Permission to assign tasks
            /// </summary>
            public const string AssignTasks = Default + ".AssignTasks";

            /// <summary>
            /// Permission to complete tasks
            /// </summary>
            public const string CompleteTasks = Default + ".CompleteTasks";
        }
    }
}
