namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Constants
{
    /// <summary>
    /// Constants for the Legal & Compliance domain
    /// </summary>
    public static class LegalComplianceConsts
    {
        // IntellectualPropertyAsset constraints
        public const int MinIPAssetNameLength = 2;
        public const int MaxIPAssetNameLength = 128;
        public const int MaxIPDescriptionLength = 2000;
        public const int MaxIPIdentifierLength = 50;
        
        // LegalAgreement constraints
        public const int MinAgreementTitleLength = 2;
        public const int MaxAgreementTitleLength = 200;
        public const int MaxAgreementDescriptionLength = 2000;
        public const int MaxPartyNameLength = 100;
        
        // Compliance constraints
        public const int MinObligationTitleLength = 2;
        public const int MaxObligationTitleLength = 200;
        public const int MaxObligationDescriptionLength = 2000;
        
        // Corporate Records constraints
        public const int MinRecordTitleLength = 2;
        public const int MaxRecordTitleLength = 200;
        public const int MaxRecordDescriptionLength = 5000;
        
        // Template constraints
        public const int MaxTemplateNameLength = 100;
        public const int MaxTemplateContentLength = 50000;
        
        // Common constraints
        public const int MaxJurisdictionLength = 100;
        public const int MaxNotesLength = 5000;
    }
}
