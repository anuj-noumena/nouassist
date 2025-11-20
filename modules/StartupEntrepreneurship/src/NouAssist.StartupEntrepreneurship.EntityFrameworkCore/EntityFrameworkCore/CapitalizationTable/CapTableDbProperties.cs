namespace NouAssist.StartupEntrepreneurship.CapitalizationTable
{
    /// <summary>
    /// Database properties for CapTable module
    /// </summary>
    public static class CapTableDbProperties
    {
        public static string DbTablePrefix { get; set; } = "CapTable";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "CapTable";
    }
}
