namespace NouAssist.DocumentManagement.Permissions
{
    public static class DocumentManagementPermissions
    {
        public const string GroupName = "DocumentManagement";

        public static class Documents
        {
            public const string Default = GroupName + ".Documents";
            public const string View = Default + ".View";
            public const string Upload = Default + ".Upload";
            public const string Download = Default + ".Download";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Archive = Default + ".Archive";
            public const string ProcessOCR = Default + ".ProcessOCR";
        }

        public static class Folders
        {
            public const string Default = GroupName + ".Folders";
            public const string View = Default + ".View";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Manage = Default + ".Manage";
        }
    }
}
