using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace NouAssist.DecisionSupport.Settings
{
    /// <summary>
    /// Options class for Decision Support module settings
    /// </summary>
    public class DecisionSupportOptions : ISingletonDependency
    {
        private readonly ISettingProvider _settingProvider;

        public DecisionSupportOptions(ISettingProvider settingProvider)
        {
            _settingProvider = settingProvider;
        }

        /// <summary>
        /// Maximum number of alternatives allowed per decision model
        /// </summary>
        public async Task<int> GetMaxAlternativesPerDecisionModelAsync()
        {
            return Convert.ToInt32(await _settingProvider.GetOrNullAsync(DecisionSupportSettings.MaxAlternativesPerDecisionModel) ?? "10");
        }

        /// <summary>
        /// Maximum number of criteria allowed per decision model
        /// </summary>
        public async Task<int> GetMaxCriteriaPerDecisionModelAsync()
        {
            return Convert.ToInt32(await _settingProvider.GetOrNullAsync(DecisionSupportSettings.MaxCriteriaPerDecisionModel) ?? "10");
        }

        /// <summary>
        /// Maximum number of scenario models allowed per analysis
        /// </summary>
        public async Task<int> GetMaxScenarioModelsPerAnalysisAsync()
        {
            return Convert.ToInt32(await _settingProvider.GetOrNullAsync(DecisionSupportSettings.MaxScenarioModelsPerAnalysis) ?? "5");
        }

        /// <summary>
        /// Whether to require rationale for scores
        /// </summary>
        public async Task<bool> GetRequireRationaleForScoresAsync()
        {
            return Convert.ToBoolean(await _settingProvider.GetOrNullAsync(DecisionSupportSettings.RequireRationaleForScores) ?? "false");
        }

        /// <summary>
        /// Whether to automatically normalize weights to ensure they sum to 1
        /// </summary>
        public async Task<bool> GetAutoNormalizeWeightsAsync()
        {
            return Convert.ToBoolean(await _settingProvider.GetOrNullAsync(DecisionSupportSettings.AutoNormalizeWeights) ?? "true");
        }

        /// <summary>
        /// Whether the scenario analysis feature is enabled
        /// </summary>
        public async Task<bool> IsScenarioAnalysisEnabledAsync()
        {
            return Convert.ToBoolean(await _settingProvider.GetOrNullAsync(DecisionSupportSettings.EnabledFeatures.ScenarioAnalysis) ?? "true");
        }

        /// <summary>
        /// Whether the strategic initiatives feature is enabled
        /// </summary>
        public async Task<bool> IsStrategicInitiativesEnabledAsync()
        {
            return Convert.ToBoolean(await _settingProvider.GetOrNullAsync(DecisionSupportSettings.EnabledFeatures.StrategicInitiatives) ?? "true");
        }
    }
}
