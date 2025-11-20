namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants
{
    /// <summary>
    /// Constants for the IRM (Investor Relationship Management) module
    /// </summary>
    public static class IRMConsts
    {
        public const string ModuleName = "IRM";
        
        public static class InvestorProfile
        {
            public const int MaxNameLength = 200;
            public const int MaxEmailLength = 256;
            public const int MaxPhoneLength = 50;
            public const int MaxCompanyNameLength = 300;
            public const int MaxNotesLength = 2000;
            public const int MaxLinkedInLength = 500;
            public const int MaxWebsiteLength = 500;
        }
        
        public static class InvestorUpdate
        {
            public const int MaxTitleLength = 300;
            public const int MaxContentLength = 10000;
            public const int MaxSubjectLength = 500;
        }
        
        public static class DueDiligenceDataRoom
        {
            public const int MaxNameLength = 200;
            public const int MaxDescriptionLength = 1000;
            public const int MaxDocumentNameLength = 500;
            public const int MaxDocumentPathLength = 1000;
        }
        
        public static class InvestorGroup
        {
            public const int MaxNameLength = 200;
            public const int MaxDescriptionLength = 1000;
        }
    }
}
