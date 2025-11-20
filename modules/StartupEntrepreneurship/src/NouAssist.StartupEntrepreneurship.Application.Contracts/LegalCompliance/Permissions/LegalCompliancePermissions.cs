namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Permissions
{
    public static class LegalCompliancePermissions
    {
        public const string GroupName = "LegalCompliance";

        // IP Asset Management
        public static class IPAsset
        {
            public const string Default = GroupName + ".IPAsset";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageDeadlines = Default + ".ManageDeadlines";
            public const string ManageValuation = Default + ".ManageValuation";
        }

        // Legal Agreement Management
        public static class LegalAgreement
        {
            public const string Default = GroupName + ".LegalAgreement";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageTerms = Default + ".ManageTerms";
            public const string ManageReminders = Default + ".ManageReminders";
            public const string Renew = Default + ".Renew";
            public const string Terminate = Default + ".Terminate";
        }

        // Compliance Management
        public static class Compliance
        {
            public const string Default = GroupName + ".Compliance";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string UpdateStatus = Default + ".UpdateStatus";
        }

        // Corporate Record Management
        public static class CorporateRecord
        {
            public const string Default = GroupName + ".CorporateRecord";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string UpdateStatus = Default + ".UpdateStatus";
        }

        // Document Generation
        public static class DocumentGeneration
        {
            public const string Default = GroupName + ".DocumentGeneration";
            public const string ManageTemplates = Default + ".ManageTemplates";
            public const string GenerateDocuments = Default + ".GenerateDocuments";
        }
    }
}
