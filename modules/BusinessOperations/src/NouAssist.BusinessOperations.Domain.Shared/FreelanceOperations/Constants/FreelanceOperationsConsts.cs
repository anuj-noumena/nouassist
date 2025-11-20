namespace NouAssist.BusinessOperations.FreelanceOperations.Constants
{
    /// <summary>
    /// Contains constant values used throughout the FreelanceOperations module
    /// </summary>
    public static class FreelanceOperationsConsts
    {
        /// <summary>
        /// Name of the module
        /// </summary>
        public const string ModuleName = "FreelanceOperations";

        public static class Project
        {
            public const int MaxNameLength = 128;
            public const int MaxDescriptionLength = 1000;
        }

        public static class Client
        {
            public const int MaxNameLength = 100;
            public const int MaxEmailLength = 256;
            public const int MaxPhoneLength = 20;
            public const int MaxCompanyNameLength = 200;
            public const int MaxNotesLength = 1000;
        }

        public static class Proposal
        {
            public const int MaxTitleLength = 200;
            public const int MaxSummaryLength = 500;
            public const int MaxVersionNameLength = 50;
        }

        public static class TimeEntry
        {
            public const int MaxDescriptionLength = 500;
        }

        public static class Upwork
        {
            public const int MaxJobIdLength = 50;
            public const int MaxContractIdLength = 50;
            public const int MaxCredentialsLength = 256;
        }
    }
}
