using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Services;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Data
{
    /// <summary>
    /// Seeds initial data for the Decision Support module
    /// </summary>
    public class DecisionSupportDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IDecisionModelManager _decisionModelManager;
        private readonly IStrategicInitiativeManager _initiativeManager;
        private readonly IScenarioAnalysisManager _scenarioManager;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;

        public DecisionSupportDataSeedContributor(
            IDecisionModelManager decisionModelManager,
            IStrategicInitiativeManager initiativeManager,
            IScenarioAnalysisManager scenarioManager,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _decisionModelManager = decisionModelManager;
            _initiativeManager = initiativeManager;
            _scenarioManager = scenarioManager;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                // Seed only for new systems
                if (context?.TenantId == null) // Host side, only run once
                {
                    await SeedExampleDecisionModelAsync();
                    await SeedExampleStrategicInitiativeAsync();
                    await SeedExampleScenarioAnalysisAsync();
                }
            }
        }

        private async Task SeedExampleDecisionModelAsync()
        {
            // Create example decision model
            await _decisionModelManager.CreateDecisionModelAsync(
                "Example Technology Selection",
                "Choose the best technology stack for our new project",
                "We need to select a technology stack for our upcoming client project that balances performance, development speed, and maintenance costs.",
                DateTime.Now.AddMonths(1));
        }

        private async Task SeedExampleStrategicInitiativeAsync()
        {
            // Create example strategic initiative
            await _initiativeManager.ProposeInitiativeAsync(
                "Digital Transformation Initiative",
                "Organization-wide digital transformation program",
                "Improved operational efficiency, enhanced customer experience, new digital revenue streams",
                "Our current systems are outdated and inefficient. Digital transformation will position us competitively for the future market landscape.");
        }

        private async Task SeedExampleScenarioAnalysisAsync()
        {
            // Create example scenario analysis
            await _scenarioManager.CreateScenarioAnalysisAsync(
                "Market Expansion Scenarios",
                "Evaluate potential outcomes of entering new international markets",
                "5 years",
                "European and Asian markets",
                "Economic conditions, regulatory environment, competitor response, currency fluctuations");
        }
    }
}
