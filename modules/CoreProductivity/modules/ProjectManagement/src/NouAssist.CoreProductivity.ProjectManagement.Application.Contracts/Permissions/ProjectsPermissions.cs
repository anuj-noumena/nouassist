namespace NouAssist.CoreProductivity.ProjectManagement.Permissions
{
    public static class ProjectsPermissions
    {
        public const string GroupName = "Projects";

        public static class Projects
        {
            public const string Default = GroupName + ".Projects";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Complete = Default + ".Complete";
            public const string Cancel = Default + ".Cancel";
            public const string Archive = Default + ".Archive";
        }

        public static class Requirements
        {
            public const string Default = GroupName + ".Requirements";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
        
        public static class Sprints
        {
            public const string Default = GroupName + ".Sprints";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Start = Default + ".Start";
            public const string Complete = Default + ".Complete";
        }

        public static class Dependencies
        {
            public const string Default = GroupName + ".Dependencies";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Resolve = Default + ".Resolve";
        }

        public static class SuccessCriteria
        {
            public const string Default = GroupName + ".SuccessCriteria";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string MarkAsMet = Default + ".MarkAsMet";
        }

        public static class Events
        {
            public const string Default = GroupName + ".Events";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
    }
}
