namespace NouAssist.RequirementsManagement.Specification.Permissions
{
    public static class RequirementSpecificationManagementPermissions
    {
        public const string GroupName = "RequirementSpecificationManagement";

        // Requirements Management
        public static class Requirements
        {
            public const string Default = GroupName + ".Requirements";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ChangeStatus = Default + ".ChangeStatus";
            public const string ManageAcceptanceCriteria = Default + ".ManageAcceptanceCriteria";
            public const string ManageBusinessRules = Default + ".ManageBusinessRules";
            public const string ManageConstraints = Default + ".ManageConstraints";
            public const string ManageDataRequirements = Default + ".ManageDataRequirements";
            public const string ManageInterfaces = Default + ".ManageInterfaces";
            public const string LinkToObjectives = Default + ".LinkToObjectives";
            public const string SetScope = Default + ".SetScope";
        }

        // Specification Structure Management
        public static class SpecificationStructures
        {
            public const string Default = GroupName + ".SpecificationStructures";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageSections = Default + ".ManageSections";
            public const string GenerateDocuments = Default + ".GenerateDocuments";
        }

        // Reporting
        public static class Reporting
        {
            public const string Default = GroupName + ".Reporting";
            public const string ViewReports = Default + ".ViewReports";
            public const string ExportData = Default + ".ExportData";
        }
    }
}
