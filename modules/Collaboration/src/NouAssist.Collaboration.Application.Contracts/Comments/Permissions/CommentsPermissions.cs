namespace NouAssist.Collaboration.Comments.Permissions
{
    public static class CommentsPermissions
    {
        public const string GroupName = "Comments";

        public static class Comments
        {
            public const string Default = GroupName + ".Comments";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Pin = Default + ".Pin";
            public const string React = Default + ".React";
            public const string Moderate = Default + ".Moderate";
        }
    }
}
