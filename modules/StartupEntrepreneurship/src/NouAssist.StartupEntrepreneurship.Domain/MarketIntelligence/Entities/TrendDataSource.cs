using System;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities
{
    /// <summary>
    /// Represents a data source for market trend information
    /// </summary>
    public class TrendDataSource : Entity<Guid>
    {
        public Guid MarketTrendId { get; private set; }
        public string SourceName { get; private set; }
        public string SourceUrl { get; private set; }
        public string SourceType { get; private set; }
        public decimal Reliability { get; private set; }
        public DateTime CollectedAt { get; private set; }
        public string DataQuality { get; private set; }
        public string ExtractedData { get; private set; }

        protected TrendDataSource() { }

        internal TrendDataSource(
            Guid id,
            Guid marketTrendId,
            string sourceName,
            string sourceUrl,
            string sourceType,
            decimal reliability,
            DateTime collectedAt,
            string dataQuality = null,
            string extractedData = null) : base(id)
        {
            MarketTrendId = marketTrendId;
            SetSourceName(sourceName);
            SetSourceUrl(sourceUrl);
            SourceType = sourceType?.Trim();
            SetReliability(reliability);
            CollectedAt = collectedAt;
            DataQuality = dataQuality?.Trim();
            ExtractedData = extractedData?.Trim();
        }

        internal void UpdateSource(
            string sourceName,
            string sourceUrl,
            decimal reliability,
            string dataQuality = null)
        {
            SetSourceName(sourceName);
            SetSourceUrl(sourceUrl);
            SetReliability(reliability);
            DataQuality = dataQuality?.Trim();
        }

        private void SetSourceName(string sourceName)
        {
            SourceName = Check.NotNullOrWhiteSpace(
                sourceName,
                nameof(sourceName),
                maxLength: MarketIntelligenceConsts.MarketTrend.MaxTitleLength
            );
        }

        private void SetSourceUrl(string sourceUrl)
        {
            SourceUrl = Check.NotNullOrWhiteSpace(
                sourceUrl,
                nameof(sourceUrl),
                maxLength: MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength
            );
        }

        private void SetReliability(decimal reliability)
        {
            if (reliability < MarketIntelligenceConsts.Validation.MinConfidenceScore ||
                reliability > MarketIntelligenceConsts.Validation.MaxConfidenceScore)
            {
                throw new BusinessException("MarketIntelligence:Validation:InvalidScore")
                    .WithData("min", MarketIntelligenceConsts.Validation.MinConfidenceScore)
                    .WithData("max", MarketIntelligenceConsts.Validation.MaxConfidenceScore);
            }

            Reliability = reliability;
        }
    }
}
