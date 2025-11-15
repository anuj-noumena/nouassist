namespace NouAssist.CurrencyManagement.EntityFrameworkCore
{
    public static class CurrencyManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Currency";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "CurrencyManagement";
    }
}
