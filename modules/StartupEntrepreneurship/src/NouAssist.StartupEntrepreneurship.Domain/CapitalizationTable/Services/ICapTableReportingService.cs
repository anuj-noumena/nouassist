using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Services
{
    /// <summary>
    /// Domain service for cap table reporting and analysis
    /// </summary>
    public interface ICapTableReportingService
    {
        /// <summary>
        /// Generates ownership report as of a specific date
        /// </summary>
        Task<OwnershipReport> GenerateOwnershipReportAsync(
            Guid capTableId,
            DateTime asOfDate,
            bool includeOptions = false);

        /// <summary>
        /// Generates fully diluted ownership report
        /// </summary>
        Task<OwnershipReport> GenerateFullyDilutedReportAsync(
            Guid capTableId,
            DateTime asOfDate);

        /// <summary>
        /// Generates valuation report
        /// </summary>
        Task<ValuationReport> GenerateValuationReportAsync(
            Guid capTableId);

        /// <summary>
        /// Generates funding round summary
        /// </summary>
        Task<FundingRoundSummary> GenerateFundingRoundSummaryAsync(
            Guid fundingRoundId);

        /// <summary>
        /// Generates option pool summary
        /// </summary>
        Task<OptionPoolSummary> GenerateOptionPoolSummaryAsync(
            Guid optionPoolId,
            DateTime asOfDate);

        /// <summary>
        /// Generates waterfall analysis for a liquidity event
        /// </summary>
        Task<WaterfallAnalysis> GenerateWaterfallAnalysisAsync(
            Guid capTableId,
            decimal liquidationValue,
            DateTime liquidationDate);
    }

    /// <summary>
    /// Ownership report data
    /// </summary>
    public class OwnershipReport
    {
        public Guid CapTableId { get; set; }
        public DateTime AsOfDate { get; set; }
        public ShareQuantity TotalShares { get; set; }
        public List<OwnershipEntry> Entries { get; set; } = new();
        public bool IncludesOptions { get; set; }

        public class OwnershipEntry
        {
            public Guid HolderId { get; set; }
            public string HolderName { get; set; }
            public string HolderType { get; set; }
            public ShareQuantity Shares { get; set; }
            public OwnershipPercentage OwnershipPercentage { get; set; }
            public ShareQuantity VestedShares { get; set; }
            public ShareQuantity UnvestedShares { get; set; }
        }
    }

    /// <summary>
    /// Valuation report data
    /// </summary>
    public class ValuationReport
    {
        public Guid CapTableId { get; set; }
        public Valuation CurrentValuation { get; set; }
        public List<ValuationEntry> History { get; set; } = new();

        public class ValuationEntry
        {
            public DateTime Date { get; set; }
            public Valuation Valuation { get; set; }
            public string Source { get; set; }
            public string Notes { get; set; }
        }
    }

    /// <summary>
    /// Funding round summary data
    /// </summary>
    public class FundingRoundSummary
    {
        public Guid FundingRoundId { get; set; }
        public string RoundName { get; set; }
        public string Status { get; set; }
        public InvestmentAmount TargetAmount { get; set; }
        public InvestmentAmount RaisedAmount { get; set; }
        public decimal CompletionPercentage { get; set; }
        public Valuation Valuation { get; set; }
        public ShareQuantity SharesIssued { get; set; }
        public PricePerShare PricePerShare { get; set; }
        public string LeadInvestor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CloseDate { get; set; }
    }

    /// <summary>
    /// Option pool summary data
    /// </summary>
    public class OptionPoolSummary
    {
        public Guid OptionPoolId { get; set; }
        public string PoolName { get; set; }
        public ShareQuantity TotalReserved { get; set; }
        public ShareQuantity TotalGranted { get; set; }
        public ShareQuantity TotalExercised { get; set; }
        public ShareQuantity TotalForfeited { get; set; }
        public ShareQuantity AvailableOptions { get; set; }
        public ShareQuantity OutstandingOptions { get; set; }
        public OwnershipPercentage PoolPercentage { get; set; }
        public DateTime AsOfDate { get; set; }
    }

    /// <summary>
    /// Waterfall analysis data for liquidity events
    /// </summary>
    public class WaterfallAnalysis
    {
        public Guid CapTableId { get; set; }
        public decimal LiquidationValue { get; set; }
        public DateTime LiquidationDate { get; set; }
        public List<WaterfallEntry> Waterfall { get; set; } = new();
        public decimal TotalDistributed { get; set; }

        public class WaterfallEntry
        {
            public Guid HolderId { get; set; }
            public string HolderName { get; set; }
            public string SecurityType { get; set; }
            public ShareQuantity Shares { get; set; }
            public decimal LiquidationPreference { get; set; }
            public decimal DistributionAmount { get; set; }
            public decimal PercentageOfTotal { get; set; }
            public int Priority { get; set; }
        }
    }
}
