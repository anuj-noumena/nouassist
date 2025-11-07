namespace NouAssist.Infrastructure.SearchEngine.Permissions
{
    public static class SearchPermissions
    {
        public const string GroupName = "Search";

        public static class Search
        {
            public const string Default = GroupName + ".Search";
            public const string SaveSearches = Default + ".SaveSearches";
            public const string ViewHistory = Default + ".ViewHistory";
            public const string ViewAnalytics = Default + ".ViewAnalytics";
        }
    }
}
