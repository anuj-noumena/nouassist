namespace NouAssist.Reviews.Permissions
{
    public static class ReviewsPermissions
    {
        public const string GroupName = "Reviews";

        public static class Reviews
        {
            public const string Default = GroupName + ".Reviews";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Complete = Default + ".Complete";
            public const string ViewOwn = Default + ".ViewOwn";
        }

        public static class Templates
        {
            public const string Default = GroupName + ".Templates";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string ViewSystem = Default + ".ViewSystem";
        }

        public static class Analytics
        {
            public const string Default = GroupName + ".Analytics";
            public const string ViewOwn = Default + ".ViewOwn";
        }
    }
}
