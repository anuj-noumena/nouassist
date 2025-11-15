namespace NouAssist.DebtManagement.EntityFrameworkCore
{
    public static class DebtManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "DebtManagement_";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "DebtManagement";
    }
}
