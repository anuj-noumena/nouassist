using System;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore
{
    public static class ContractPortfolioManagementDbProperties
    {
        /// <summary>
        /// Default connection string name for the module's database
        /// </summary>
        public const string ConnectionStringName = "ContractPortfolioManagement";

        /// <summary>
        /// Default database table prefix
        /// </summary>
        public const string DbTablePrefix = "CPM_";

        /// <summary>
        /// Database schema name
        /// </summary>
        public const string? DbSchema = null; // Uses the default schema (usually "public" for PostgreSQL)
    }
}
