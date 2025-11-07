namespace NouAssist.CollaborationHub.UserProfiles.Permissions
{
    public static class UsersPermissions
    {
        public const string GroupName = "Users";

        public static class Users
        {
            public const string Default = GroupName + ".Users";
            public const string View = Default + ".View";
            public const string Update = Default + ".Update";
            public const string UpdateProfile = Default + ".UpdateProfile";
            public const string ManageSettings = Default + ".ManageSettings";
        }
    }
}
