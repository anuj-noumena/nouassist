namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Permissions
{
    public static class FundraisingPipelinePermissions
    {
        public const string GroupName = "FundraisingPipeline";

        public static class Prospect
        {
            public const string Default = GroupName + ".Prospect";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
        }

        public static class FundraisingRound
        {
            public const string Default = GroupName + ".FundraisingRound";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
        }

        public static class PitchDeck
        {
            public const string Default = GroupName + ".PitchDeck";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
        }

        public static class TermSheet
        {
            public const string Default = GroupName + ".TermSheet";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
        }
    }
}
