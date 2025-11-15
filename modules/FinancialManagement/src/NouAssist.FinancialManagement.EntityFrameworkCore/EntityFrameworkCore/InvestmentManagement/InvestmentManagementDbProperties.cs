namespace NouAssist.InvestmentManagement.EntityFrameworkCore
{
    public static class InvestmentManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "InvestmentManagement_";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "InvestmentManagement";
    }
}
