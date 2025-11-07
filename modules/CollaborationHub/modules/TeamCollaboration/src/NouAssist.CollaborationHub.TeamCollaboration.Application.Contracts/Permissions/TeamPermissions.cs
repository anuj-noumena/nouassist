namespace NouAssist.CollaborationHub.TeamCollaboration.Permissions
{
    public static class TeamPermissions
    {
        public const string GroupName = "Team";

        public static class Teams
        {
            public const string Default = GroupName + ".Teams";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageMembers = Default + ".ManageMembers";
        }

        public static class Roles
        {
            public const string Default = GroupName + ".Roles";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string AssignToUsers = Default + ".AssignToUsers";
        }

        public static class Members
        {
            public const string Default = GroupName + ".Members";
            public const string Add = Default + ".Add";
            public const string Remove = Default + ".Remove";
            public const string UpdateRole = Default + ".UpdateRole";
        }
    }
}
