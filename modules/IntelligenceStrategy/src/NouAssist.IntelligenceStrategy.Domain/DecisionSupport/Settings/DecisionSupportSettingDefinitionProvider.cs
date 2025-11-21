using NouAssist.IntelligenceStrategy.DecisionSupport.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Settings
{
    /// <summary>
    /// Defines settings for the Decision Support module
    /// </summary>
    public class DecisionSupportSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    DecisionSupportSettings.MaxAlternativesPerDecisionModel,
                    "10",
                    L("Settings:MaxAlternativesPerDecisionModel"),
                    L("Settings:MaxAlternativesPerDecisionModelDescription"),
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    DecisionSupportSettings.MaxCriteriaPerDecisionModel,
                    "10",
                    L("Settings:MaxCriteriaPerDecisionModel"),
                    L("Settings:MaxCriteriaPerDecisionModelDescription"),
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    DecisionSupportSettings.MaxScenarioModelsPerAnalysis,
                    "5",
                    L("Settings:MaxScenarioModelsPerAnalysis"),
                    L("Settings:MaxScenarioModelsPerAnalysisDescription"),
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    DecisionSupportSettings.RequireRationaleForScores,
                    "false",
                    L("Settings:RequireRationaleForScores"),
                    L("Settings:RequireRationaleForScoresDescription"),
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    DecisionSupportSettings.AutoNormalizeWeights,
                    "true",
                    L("Settings:AutoNormalizeWeights"),
                    L("Settings:AutoNormalizeWeightsDescription"),
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    DecisionSupportSettings.EnabledFeatures.ScenarioAnalysis,
                    "true",
                    L("Settings:EnabledFeatures.ScenarioAnalysis"),
                    L("Settings:EnabledFeatures.ScenarioAnalysisDescription"),
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    DecisionSupportSettings.EnabledFeatures.StrategicInitiatives,
                    "true",
                    L("Settings:EnabledFeatures.StrategicInitiatives"),
                    L("Settings:EnabledFeatures.StrategicInitiativesDescription"),
                    isVisibleToClients: true
                )
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DecisionSupportResource>(name);
        }
    }
}
