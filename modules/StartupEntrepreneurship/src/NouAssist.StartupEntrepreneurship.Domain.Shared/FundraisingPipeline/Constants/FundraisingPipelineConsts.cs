namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants
{
    /// <summary>
    /// Constants used across the Fundraising Pipeline module
    /// </summary>
    public static class FundraisingPipelineConsts
    {
        public const string ModuleName = "FundraisingPipeline";
        
        public static class Prospect
        {
            public const int MaxNameLength = 128;
            public const int MaxCompanyLength = 128;
            public const int MaxTitleLength = 128;
            public const int MaxDescriptionLength = 1000;
        }
        
        public static class Contact
        {
            public const int MaxEmailLength = 256;
            public const int MaxPhoneLength = 20;
            public const int MaxAddressLength = 500;
        }
        
        public static class FundraisingRound
        {
            public const int MaxNameLength = 128;
            public const int MaxDescriptionLength = 1000;
        }
        
        public static class PitchDeck
        {
            public const int MaxTitleLength = 256;
            public const int MaxDescriptionLength = 1000;
        }
        
        public static class TermSheet
        {
            public const int MaxTitleLength = 256;
            public const int MaxClauseNameLength = 128;
            public const int MaxClauseValueLength = 2000;
        }
    }
}
