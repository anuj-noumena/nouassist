namespace NouAssist.Contacts.Permissions
{
    public static class ContactsPermissions
    {
        public const string GroupName = "Contacts";

        public static class Contacts
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
