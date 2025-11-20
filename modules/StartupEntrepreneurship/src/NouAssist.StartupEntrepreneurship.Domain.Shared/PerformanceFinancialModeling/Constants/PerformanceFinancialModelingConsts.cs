namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants
{
    /// <summary>
    /// Constants for Performance & Financial Modeling module
    /// </summary>
    public static class PerformanceFinancialModelingConsts
    {
        // TrackedMetric constants
        public const int MaxMetricNameLength = 100;
        public const int MaxMetricDescriptionLength = 500;
        public const int MaxMetricUnitLength = 20;
        
        // MetricDefinition constants
        public const int MaxDefinitionNameLength = 100;
        public const int MaxDefinitionDescriptionLength = 1000;
        public const int MaxCalculationRuleLength = 2000;
        
        // MetricCategory constants
        public const int MaxCategoryNameLength = 50;
        public const int MaxCategoryDescriptionLength = 200;
        
        // FinancialModel constants
        public const int MaxModelNameLength = 100;
        public const int MaxModelDescriptionLength = 1000;
        public const int MaxScenarioNameLength = 50;
        public const int MaxScenarioDescriptionLength = 500;
        
        // AssumptionSet constants
        public const int MaxAssumptionNameLength = 100;
        public const int MaxAssumptionDescriptionLength = 500;
        
        // General constants
        public const int MaxNotesLength = 2000;
        public const decimal MaxMonetaryValue = 999_999_999_999.99m;
        public const int MaxProjectionYears = 10;
    }
}
