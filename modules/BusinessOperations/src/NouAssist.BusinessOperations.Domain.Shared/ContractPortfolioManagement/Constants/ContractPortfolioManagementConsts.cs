namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Constants
{
    /// <summary>
    /// Constants for Contract & Portfolio Management module
    /// </summary>
    public static class ContractPortfolioManagementConsts
    {
        /// <summary>
        /// Default name of the module
        /// </summary>
        public const string ModuleName = "ContractPortfolioManagement";

        public static class Contract
        {
            public const int MaxTitleLength = 256;
            public const int MaxReferenceNumberLength = 64;
            public const int MaxDescriptionLength = 2000;
        }

        public static class ContractClause
        {
            public const int MaxTitleLength = 128;
            public const int MaxContentLength = 4000;
        }

        public static class ContractParty
        {
            public const int MaxNameLength = 256;
            public const int MaxIdentifierLength = 64;
            public const int MaxContactInfoLength = 1000;
        }

        public static class Portfolio
        {
            public const int MaxNameLength = 128;
            public const int MaxDescriptionLength = 1000;
        }

        public static class PortfolioCategory
        {
            public const int MaxNameLength = 64;
            public const int MaxDescriptionLength = 500;
        }

        public static class PortfolioRoadmap
        {
            public const int MaxTitleLength = 128;
            public const int MaxDescriptionLength = 1000;
        }
    }
}
