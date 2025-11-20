namespace NouAssist.RequirementsManagement.Specification.Constants
{
    /// <summary>
    /// Defines constants for the Requirements Specification Management module
    /// </summary>
    public static class RequirementSpecificationManagementConsts
    {
        public const string ModuleName = "RequirementSpecificationManagement";
        public const string DefaultConnectionStringName = "Default";

        public static class Requirement
        {
            public const int MaxIdentifierLength = 50;
            public const int MaxTitleLength = 200;
            public const int MaxDescriptionLength = 2000;
            public const int MaxVersionLength = 20;
        }

        public static class AcceptanceCriterion
        {
            public const int MaxStatementLength = 1000;
            public const int MaxDescriptionLength = 2000;
        }

        public static class BusinessRule
        {
            public const int MaxNameLength = 200;
            public const int MaxDescriptionLength = 2000;
        }

        public static class Constraint
        {
            public const int MaxNameLength = 200;
            public const int MaxStatementLength = 1000;
        }

        public static class DataRequirement
        {
            public const int MaxFieldNameLength = 100;
            public const int MaxDataTypeLength = 50;
            public const int MaxDescriptionLength = 500;
        }

        public static class ExternalInterface
        {
            public const int MaxNameLength = 200;
            public const int MaxEndpointLength = 500;
            public const int MaxDescriptionLength = 1000;
        }

        public static class SpecificationStructure
        {
            public const int MaxSectionNameLength = 200;
            public const int MaxSectionContentLength = 10000;
        }
    }
}
