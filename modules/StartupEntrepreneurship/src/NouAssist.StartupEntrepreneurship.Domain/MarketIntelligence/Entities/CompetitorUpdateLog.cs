using System;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities
{
    /// <summary>
    /// Represents a log entry for competitor updates and monitoring activities
    /// </summary>
    public class CompetitorUpdateLog : Entity<Guid>
    {
        public Guid CompetitorId { get; private set; }
        public UpdateType Type { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Source { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public ImpactLevel ImpactLevel { get; private set; }
        public string Tags { get; private set; }
        public string AdditionalData { get; private set; }

        protected CompetitorUpdateLog() { }

        internal CompetitorUpdateLog(
            Guid id,
            Guid competitorId,
            UpdateType type,
            string title,
            string description,
            DateTime updateDate,
            ImpactLevel impactLevel = ImpactLevel.Low,
            string source = null,
            string tags = null,
            string additionalData = null) : base(id)
        {
            CompetitorId = competitorId;
            Type = type;
            SetTitle(title);
            SetDescription(description);
            Source = source?.Trim();
            UpdateDate = updateDate;
            ImpactLevel = impactLevel;
            Tags = tags?.Trim();
            AdditionalData = additionalData?.Trim();

            ValidateData();
        }

        internal void UpdateDetails(
            string title,
            string description,
            ImpactLevel impactLevel,
            string source = null,
            string tags = null)
        {
            SetTitle(title);
            SetDescription(description);
            ImpactLevel = impactLevel;
            Source = source?.Trim();
            Tags = tags?.Trim();
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: MarketIntelligenceConsts.MarketTrend.MaxTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: MarketIntelligenceConsts.MarketTrend.MaxDescriptionLength
            );
        }

        private void ValidateData()
        {
            if (!string.IsNullOrEmpty(Source) && Source.Length > MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Source")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength);
            }

            if (!string.IsNullOrEmpty(Tags) && Tags.Length > MarketIntelligenceConsts.Validation.MaxTagsLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Tags")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.Validation.MaxTagsLength);
            }
        }
    }
}
