using Volo.Abp.Reflection;

namespace NouAssist.IdentityAccess.Permissions;

public class IdentityAccessPermissions
{
    public const string GroupName = "IdentityAccess";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityAccessPermissions));
    }

    // Team Permissions
    public static class Team
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

    // Users Permissions
    public static class Users
    {
        public const string GroupName = "Users";

        public static class UserManagement
        {
            public const string Default = GroupName + ".Users";
            public const string View = Default + ".View";
            public const string Update = Default + ".Update";
            public const string UpdateProfile = Default + ".UpdateProfile";
            public const string ManageSettings = Default + ".ManageSettings";
        }
    }

    // Contacts Permissions
    public static class Contacts
    {
        public const string GroupName = "Contacts";

        public static class ContactManagement
        {
            public const string Default = GroupName + ".Contacts";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Archive = Default + ".Archive";
        }

        public static class Clients
        {
            public const string Default = GroupName + ".Clients";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string ManageStatus = Default + ".ManageStatus";
        }
    }
}
