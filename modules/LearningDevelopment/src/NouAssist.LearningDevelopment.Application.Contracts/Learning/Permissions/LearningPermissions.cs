namespace NouAssist.LearningDevelopment.Learning.Permissions
{
    public static class LearningPermissions
    {
        public const string GroupName = "Learning";

        public static class Topics
        {
            public const string Default = GroupName + ".Topics";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageChildren = Default + ".ManageChildren";
        }

        public static class LearningSessions
        {
            public const string Default = GroupName + ".LearningSessions";
            public const string Start = Default + ".Start";
            public const string Advance = Default + ".Advance";
            public const string Pause = Default + ".Pause";
            public const string Resume = Default + ".Resume";
            public const string RecordData = Default + ".RecordData";
        }

        public static class LearningNotes
        {
            public const string Default = GroupName + ".LearningNotes";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class Mindmaps
        {
            public const string Default = GroupName + ".Mindmaps";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class BookReadingHistories
        {
            public const string Default = GroupName + ".BookReadingHistories";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
    }
}
