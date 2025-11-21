namespace NouAssist.IntelligenceStrategy.DecisionSupport.Settings
{
    /// <summary>
    /// Constants for setting names in the Decision Support module
    /// </summary>
    public static class DecisionSupportSettings
    {
        private const string Prefix = "DecisionSupport";

        /// <summary>
        /// Maximum number of alternatives allowed per decision model
        /// </summary>
        public const string MaxAlternativesPerDecisionModel = Prefix + ".MaxAlternativesPerDecisionModel";

        /// <summary>
        /// Maximum number of criteria allowed per decision model
        /// </summary>
        public const string MaxCriteriaPerDecisionModel = Prefix + ".MaxCriteriaPerDecisionModel";

        /// <summary>
        /// Maximum number of scenario models allowed per analysis
        /// </summary>
        public const string MaxScenarioModelsPerAnalysis = Prefix + ".MaxScenarioModelsPerAnalysis";

        /// <summary>
        /// Whether to require rationale for scores
        /// </summary>
        public const string RequireRationaleForScores = Prefix + ".RequireRationaleForScores";

        /// <summary>
        /// Whether to automatically normalize weights to ensure they sum to 1
        /// </summary>
        public const string AutoNormalizeWeights = Prefix + ".AutoNormalizeWeights";

        /// <summary>
        /// Group for feature toggles within the Decision Support module
        /// </summary>
        public static class EnabledFeatures
        {
            private const string FeaturesPrefix = Prefix + ".EnabledFeatures";

            /// <summary>
            /// Whether the scenario analysis feature is enabled
            /// </summary>
            public const string ScenarioAnalysis = FeaturesPrefix + ".ScenarioAnalysis";

            /// <summary>
            /// Whether the strategic initiatives feature is enabled
            /// </summary>
            public const string StrategicInitiatives = FeaturesPrefix + ".StrategicInitiatives";
        }
    }
}
