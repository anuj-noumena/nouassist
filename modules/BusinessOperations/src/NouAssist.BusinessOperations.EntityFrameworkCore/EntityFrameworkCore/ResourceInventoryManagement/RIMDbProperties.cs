namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore
{
    /// <summary>
    /// Constants for database properties
    /// </summary>
    public static class RIMDbProperties
    {
        /// <summary>
        /// Connection string name for the RIM module
        /// </summary>
        public const string ConnectionStringName = "RIM";

        /// <summary>
        /// Table prefix for RIM tables
        /// </summary>
        public const string DbTablePrefix = "RIM_";

        /// <summary>
        /// Schema name for RIM tables (null means default schema)
        /// </summary>
        public static string? DbSchema = null;
    }
}
