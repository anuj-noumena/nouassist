namespace NouAssist.WorkflowOrchestration.GTDSystem.Permissions
{
    public static class GTDSystemPermissions
    {
        public const string GroupName = "GTD";

        public static class Horizons
        {
            public const string Default = GroupName + ".Horizons";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Review = Default + ".Review";
            public const string Publish = Default + ".Publish";
        }

        public static class Ticklers
        {
            public const string Default = GroupName + ".Ticklers";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Surface = Default + ".Surface";
            public const string Process = Default + ".Process";
        }

        public static class SomedayMaybe
        {
            public const string Default = GroupName + ".SomedayMaybe";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Promote = Default + ".Promote";
            public const string Review = Default + ".Review";
        }

        public static class WaitingFor
        {
            public const string Default = GroupName + ".WaitingFor";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Resolve = Default + ".Resolve";
        }
    }
}
