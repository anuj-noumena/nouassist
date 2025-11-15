namespace NouAssist.CreditScoreMonitoring.EntityFrameworkCore
{
    public static class CreditScoreMonitoringDbProperties
    {
        public static string DbTablePrefix { get; set; } = "CreditScoreMonitoring_";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "CreditScoreMonitoring";
    }
}
