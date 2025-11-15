namespace NouAssist.DocumentManagement.EntityFrameworkCore
{
    public static class DocumentManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Document";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "DocumentManagement";
    }
}
