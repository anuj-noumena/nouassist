namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects
{
    /// <summary>
    /// Value object representing a detailed trend description
    /// </summary>
    public class TrendDescription : ValueObject
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Keywords { get; private set; }
        public string SourceUrl { get; private set; }

        private TrendDescription() { }

        public TrendDescription(string title, string description, string keywords = null, string sourceUrl = null)
        {
            SetTitle(title);
            SetDescription(description);
            Keywords = keywords?.Trim();
            SourceUrl = sourceUrl?.Trim();

            ValidateData();
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
            if (!string.IsNullOrEmpty(Keywords) && Keywords.Length > MarketIntelligenceConsts.MarketTrend.MaxKeywordsLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Keywords")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.MarketTrend.MaxKeywordsLength);
            }

            if (!string.IsNullOrEmpty(SourceUrl) && SourceUrl.Length > MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Source URL")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength);
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Description;
            yield return Keywords;
            yield return SourceUrl;
        }
    }
}
