using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore
{
    public static class MarketIntelligenceDbProperties
    {
        public const string ConnectionStringName = "MarketIntelligence";
        public static string DbTablePrefix { get; set; } = MarketIntelligenceConsts.DbTablePrefix;
        public static string? DbSchema { get; set; } = MarketIntelligenceConsts.DbSchema;
    }
}
