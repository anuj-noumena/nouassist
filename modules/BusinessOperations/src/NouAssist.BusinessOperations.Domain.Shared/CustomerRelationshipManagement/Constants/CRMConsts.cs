namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Constants
{
    /// <summary>
    /// Contains constants for the CRM module
    /// </summary>
    public static class CRMConsts
    {
        /// <summary>
        /// Maximum lengths for string properties
        /// </summary>
        public static class MaxLengths
        {
            public const int Name = 128;
            public const int Title = 64;
            public const int Email = 256;
            public const int Phone = 32;
            public const int Address = 512;
            public const int Description = 2000;
            public const int Note = 1000;
            public const int Url = 2048;
            public const int Code = 64;
            public const int TagName = 64;
        }
        
        /// <summary>
        /// Default values for the module
        /// </summary>
        public static class Defaults
        {
            public const int LeadScore = 0;
            public const int MaxContactsPerGroup = 1000;
            public const int MaxTagsPerEntity = 20;
        }
    }
}
