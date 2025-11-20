using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Reports.Dtos
{
    /// <summary>
    /// DTO for scenario analysis
    /// </summary>
    public class ScenarioAnalysisDto
    {
        public Guid CapTableId { get; set; }
        public string CompanyName { get; set; }
        public string ScenarioType { get; set; }
        public List<ScenarioResultDto> Scenarios { get; set; } = new();
        public ScenarioComparisonDto Comparison { get; set; }
    }
    
    /// <summary>
    /// DTO for individual scenario result
    /// </summary>
    public class ScenarioResultDto
    {
        public string ScenarioName { get; set; }
        public decimal InvestmentAmount { get; set; }
        public decimal PreMoneyValuation { get; set; }
        public decimal PostMoneyValuation { get; set; }
        public decimal PricePerShare { get; set; }
        public decimal NewSharesIssued { get; set; }
        public OwnershipReportDto PostInvestmentOwnership { get; set; }
        public DilutionAnalysisDto DilutionAnalysis { get; set; }
    }
    
    /// <summary>
    /// DTO for scenario comparison
    /// </summary>
    public class ScenarioComparisonDto
    {
        public List<ComparisonMetricDto> Metrics { get; set; } = new();
    }
    
    /// <summary>
    /// DTO for comparison metric
    /// </summary>
    public class ComparisonMetricDto
    {
        public string ScenarioName { get; set; }
        public decimal FounderDilution { get; set; }
        public decimal EmployeeDilution { get; set; }
        public decimal PostMoneyValuation { get; set; }
        public decimal OwnershipRetained { get; set; }
    }
}
