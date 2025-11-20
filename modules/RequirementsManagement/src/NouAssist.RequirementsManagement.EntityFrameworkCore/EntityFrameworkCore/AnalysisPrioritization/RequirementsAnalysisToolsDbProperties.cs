namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore
{
    public static class RequirementsAnalysisToolsDbProperties
    {
        public const string ConnectionStringName = "RequirementsAnalysisTools";
        
        public static string DbTablePrefix { get; set; } = "RAT_";
        
        public static string? DbSchema { get; set; } = null;
    }
}
