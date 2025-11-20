using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Reports.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Reports
{
    /// <summary>
    /// Application service interface for CapTable reporting
    /// </summary>
    public interface ICapTableReportAppService : IApplicationService
    {
        // Ownership reports
        Task<OwnershipReportDto> GetOwnershipReportAsync(Guid capTableId, DateTime asOfDate, bool includeOptions = false);
        Task<OwnershipReportDto> GetFullyDilutedOwnershipReportAsync(Guid capTableId, DateTime asOfDate);
        
        // Valuation reports
        Task<ValuationReportDto> GetValuationReportAsync(Guid capTableId);
        
        // Dilution analysis
        Task<DilutionAnalysisDto> CalculateDilutionImpactAsync(Guid capTableId, decimal newSharesIssued, DateTime effectiveDate);
        Task<DilutionAnalysisDto> CalculateFundingRoundDilutionAsync(Guid capTableId, Guid fundingRoundId);
        
        // Scenario modeling
        Task<ScenarioAnalysisDto> ModelFundingRoundScenariosAsync(Guid capTableId, List<FundingRoundScenarioInputDto> scenarios);
        Task<ScenarioAnalysisDto> ModelOptionGrantScenariosAsync(Guid capTableId, List<OptionGrantScenarioInputDto> scenarios);
    }
    
    /// <summary>
    /// DTO for funding round scenario input
    /// </summary>
    public class FundingRoundScenarioInputDto
    {
        public string ScenarioName { get; set; }
        public decimal InvestmentAmount { get; set; }
        public decimal PreMoneyValuation { get; set; }
        public string Currency { get; set; } = "USD";
    }
    
    /// <summary>
    /// DTO for option grant scenario input
    /// </summary>
    public class OptionGrantScenarioInputDto
    {
        public string ScenarioName { get; set; }
        public decimal OptionsToGrant { get; set; }
    }
}
