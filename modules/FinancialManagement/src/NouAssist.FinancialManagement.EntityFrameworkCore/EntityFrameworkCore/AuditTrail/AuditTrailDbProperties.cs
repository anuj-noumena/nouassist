namespace NouAssist.AuditTrail.EntityFrameworkCore
{
    public static class AuditTrailDbProperties
    {
        public static string DbTablePrefix { get; set; } = "AuditTrail";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "AuditTrail";
    }
}
