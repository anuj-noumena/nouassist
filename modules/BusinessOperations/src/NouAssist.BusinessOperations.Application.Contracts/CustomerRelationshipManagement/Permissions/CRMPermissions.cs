namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Permissions
{
    public static class CRMPermissions
    {
        public const string GroupName = "CRM";

        // Permissions for Lead Management
        public static class Lead
        {
            public const string Default = GroupName + ".Lead"; // Group permission
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Convert = Default + ".Convert";
        }

        // Permissions for Contact Management
        public static class Contact
        {
            public const string Default = GroupName + ".Contact";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageGroups = Default + ".ManageGroups";
        }

        // Permissions for Organization Management
        public static class Organization
        {
            public const string Default = GroupName + ".Organization";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
        }

        // Permissions for Interaction Management
        public static class Interaction
        {
            public const string Default = GroupName + ".Interaction";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageFollowUps = Default + ".ManageFollowUps";
        }

        // Permissions for Automation Management
        public static class Automation
        {
            public const string Default = GroupName + ".Automation";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Execute = Default + ".Execute";
        }

        // Permissions for Communication Templates
        public static class CommunicationTemplate
        {
            public const string Default = GroupName + ".CommunicationTemplate";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
        }

        // Permissions for Analytics
        public static class Analytics
        {
            public const string Default = GroupName + ".Analytics";
            public const string ViewLeadMetrics = Default + ".ViewLeadMetrics";
            public const string ViewContactMetrics = Default + ".ViewContactMetrics";
            public const string ViewInteractionMetrics = Default + ".ViewInteractionMetrics";
            public const string ExportReports = Default + ".ExportReports";
        }

        // Permissions for Integration
        public static class Integration
        {
            public const string Default = GroupName + ".Integration";
            public const string ConfigureExternalSystems = Default + ".ConfigureExternalSystems";
            public const string ImportData = Default + ".ImportData";
            public const string ExportData = Default + ".ExportData";
        }
    }
}
