namespace NouAssist.TaxManagement.EntityFrameworkCore
{
    public static class TaxManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "TaxManagement_";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "TaxManagement";
    }
}
