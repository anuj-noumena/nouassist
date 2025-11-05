namespace NouAssist.Tasks.Permissions
{
    public static class TasksPermissions
    {
        public const string GroupName = "Tasks";

        public static class Tasks
        {
            public const string Default = GroupName + ".Tasks";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageActionSteps = Default + ".ManageActionSteps";
            public const string ManageCriteria = Default + ".ManageCriteria";
        }

        public static class TaskInstances
        {
            public const string Default = GroupName + ".TaskInstances";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Start = Default + ".Start";
            public const string Complete = Default + ".Complete";
            public const string Delegate = Default + ".Delegate";
            public const string Block = Default + ".Block";
            public const string UpdateProgress = Default + ".UpdateProgress";
        }

        public static class TaskTypes
        {
            public const string Default = GroupName + ".TaskTypes";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageCustomFields = Default + ".ManageCustomFields";
        }
    }
}
