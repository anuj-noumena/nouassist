namespace NouAssist.FinancialManagement.EntityFrameworkCore
{
    public static class FinancialManagementDbProperties
    {
        public const string ConnectionStringName = "FinancialManagement";
        public static string DbTablePrefix { get; set; } = "FM_";
        public static string DbSchema { get; set; } = "financial";
    }
}
