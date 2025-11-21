using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Services
{
    /// <summary>
    /// Domain service for what-if scenario modeling
    /// </summary>
    public interface IScenarioModelingService
    {
        /// <summary>
        /// Models the impact of a hypothetical funding round
        /// </summary>
        Task<FundingRoundScenario> ModelFundingRoundAsync(
            Guid capTableId,
            InvestmentAmount investment,
            Valuation preMoneyValuation,
            string scenarioName);

        /// <summary>
        /// Models the impact of option grants
        /// </summary>
        Task<OptionGrantScenario> ModelOptionGrantsAsync(
            Guid capTableId,
            ShareQuantity optionsToGrant,
            string scenarioName);

        /// <summary>
        /// Models multiple scenarios for comparison
        /// </summary>
        Task<ScenarioComparison> ModelMultipleScenariosAsync(
            Guid capTableId,
            List<ScenarioDefinition> scenarios);

        /// <summary>
        /// Models exit scenarios with different valuation multiples
        /// </summary>
        Task<ExitScenario> ModelExitScenariosAsync(
            Guid capTableId,
            List<decimal> exitValuations,
            string scenarioName);
    }

    /// <summary>
    /// Definition of a scenario to model
    /// </summary>
    public class ScenarioDefinition
    {
        public string Name { get; set; }
        public InvestmentAmount Investment { get; set; }
        public Valuation PreMoneyValuation { get; set; }
        public ShareQuantity OptionGrants { get; set; }
    }

    /// <summary>
    /// Funding round scenario results
    /// </summary>
    public class FundingRoundScenario
    {
        public string ScenarioName { get; set; }
        public Guid CapTableId { get; set; }
        public InvestmentAmount Investment { get; set; }
        public Valuation Valuation { get; set; }
        public PricePerShare PricePerShare { get; set; }
        public ShareQuantity NewSharesIssued { get; set; }
        public Dictionary<Guid, DilutionImpactResult> DilutionImpact { get; set; }
        public OwnershipReport PostInvestmentOwnership { get; set; }
    }

    /// <summary>
    /// Option grant scenario results
    /// </summary>
    public class OptionGrantScenario
    {
        public string ScenarioName { get; set; }
        public Guid CapTableId { get; set; }
        public ShareQuantity OptionsGranted { get; set; }
        public Dictionary<Guid, DilutionImpactResult> DilutionImpact { get; set; }
        public OwnershipReport FullyDilutedOwnership { get; set; }
    }

    /// <summary>
    /// Comparison of multiple scenarios
    /// </summary>
    public class ScenarioComparison
    {
        public Guid CapTableId { get; set; }
        public List<FundingRoundScenario> Scenarios { get; set; } = new();
        public Dictionary<string, ComparisonMetrics> Comparison { get; set; } = new();

        public class ComparisonMetrics
        {
            public string ScenarioName { get; set; }
            public decimal FounderDilution { get; set; }
            public decimal EmployeeDilution { get; set; }
            public decimal PostMoneyValuation { get; set; }
            public decimal OwnershipRetained { get; set; }
        }
    }

    /// <summary>
    /// Exit scenario analysis
    /// </summary>
    public class ExitScenario
    {
        public string ScenarioName { get; set; }
        public Guid CapTableId { get; set; }
        public List<ExitValuationResult> Results { get; set; } = new();

        public class ExitValuationResult
        {
            public decimal ExitValuation { get; set; }
            public WaterfallAnalysis WaterfallAnalysis { get; set; }
            public Dictionary<Guid, decimal> HolderReturns { get; set; }
        }
    }
}
