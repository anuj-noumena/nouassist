namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants
{
    /// <summary>
    /// Constants for Business Model Management module
    /// </summary>
    public static class BusinessModelManagementConsts
    {
        public const string ModuleName = "BusinessModelManagement";
        
        public static class BusinessModelCanvas
        {
            public const int MaxTitleLength = 200;
            public const int MaxDescriptionLength = 2000;
            public const int MaxVersionNameLength = 100;
            public const int MaxVersionNotesLength = 1000;
            public const int MaxComponentNameLength = 100;
            public const int MaxComponentContentLength = 5000;
        }
        
        public static class RevenueModel
        {
            public const int MaxNameLength = 200;
            public const int MaxDescriptionLength = 2000;
            public const int MaxPricingModelNameLength = 100;
            public const int MaxScenarioNameLength = 100;
            public const int MaxScenarioDescriptionLength = 1000;
        }
        
        public static class StrategicPivot
        {
            public const int MaxReasonLength = 2000;
            public const int MaxDescriptionLength = 5000;
            public const int MaxExpectedOutcomeLength = 2000;
            public const int MaxActualOutcomeLength = 2000;
        }
        
        public static class Hypothesis
        {
            public const int MaxAssumptionLength = 1000;
            public const int MaxTestDescriptionLength = 2000;
            public const int MaxResultDescriptionLength = 2000;
        }
        
        public static class GoToMarket
        {
            public const int MaxStrategyNameLength = 200;
            public const int MaxChannelNameLength = 100;
            public const int MaxChannelDescriptionLength = 1000;
            public const int MaxTargetAudienceLength = 500;
        }
    }
}
