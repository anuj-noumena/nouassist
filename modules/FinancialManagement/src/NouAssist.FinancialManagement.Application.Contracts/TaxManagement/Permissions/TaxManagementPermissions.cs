namespace NouAssist.TaxManagement.Permissions
{
    public static class TaxManagementPermissions
    {
        public const string GroupName = "TaxManagement";

        public static class TaxReturns
        {
            public const string Default = GroupName + ".TaxReturns";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string File = Default + ".File";
            public const string ViewDocuments = Default + ".ViewDocuments";
        }

        public static class TaxProfiles
        {
            public const string Default = GroupName + ".TaxProfiles";
            public const string Manage = Default + ".Manage";
        }
    }
}
