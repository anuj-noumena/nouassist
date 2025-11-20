namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialCalculations.Dtos
{
    /// <summary>
    /// Input DTO for sensitivity analysis
    /// </summary>
    public class PerformSensitivityAnalysisDto
    {
        [Required]
        public string VariableToAnalyze { get; set; }

        [Range(1, 100)]
        public decimal VariancePercentage { get; set; } = 20;

        [StringLength(PerformanceFinancialModelingConsts.MaxNotesLength)]
        public string Notes { get; set; }
    }

    /// <summary>
    /// Input DTO for multi-variable sensitivity analysis
    /// </summary>
    public class PerformMultiVariableSensitivityDto
    {
        public Dictionary<string, decimal> VariablesToAnalyze { get; set; } = new Dictionary<string, decimal>();

        [StringLength(PerformanceFinancialModelingConsts.MaxNotesLength)]
        public string Notes { get; set; }
    }

    /// <summary>
    /// Input DTO for runway analysis
    /// </summary>
    public class PerformRunwayAnalysisDto
    {
        [Range(1, 60)]
        public int ProjectionMonths { get; set; } = 12;

        [Range(6, 60)]
        public int TargetRunwayMonths { get; set; } = 18;

        public bool IncludeOptimization { get; set; } = true;

        [StringLength(PerformanceFinancialModelingConsts.MaxNotesLength)]
        public string Notes { get; set; }
    }

    /// <summary>
    /// Input DTO for break-even analysis
    /// </summary>
    public class PerformBreakEvenAnalysisDto
    {
        public decimal? FixedCosts { get; set; }
        public decimal? VariableCostPerUnit { get; set; }
        public decimal? PricePerUnit { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxNotesLength)]
        public string Notes { get; set; }
    }
}
