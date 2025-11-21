using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects
{
    /// <summary>
    /// Value object representing psychographic information for customer segments
    /// </summary>
    public class Psychographics : ValueObject
    {
        public string Interests { get; private set; }
        public string Values { get; private set; }
        public string Lifestyle { get; private set; }
        public string Personality { get; private set; }
        public string Attitudes { get; private set; }

        private Psychographics() { }

        public Psychographics(
            string interests = null,
            string values = null,
            string lifestyle = null,
            string personality = null,
            string attitudes = null)
        {
            Interests = interests?.Trim();
            Values = values?.Trim();
            Lifestyle = lifestyle?.Trim();
            Personality = personality?.Trim();
            Attitudes = attitudes?.Trim();

            ValidateData();
        }

        private void ValidateData()
        {
            var maxLength = MarketIntelligenceConsts.CustomerSegment.MaxPsychographicValueLength;

            if (!string.IsNullOrEmpty(Interests) && Interests.Length > maxLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Interests")
                    .WithData("min", 0)
                    .WithData("max", maxLength);
            }

            if (!string.IsNullOrEmpty(Values) && Values.Length > maxLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Values")
                    .WithData("min", 0)
                    .WithData("max", maxLength);
            }

            if (!string.IsNullOrEmpty(Lifestyle) && Lifestyle.Length > maxLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Lifestyle")
                    .WithData("min", 0)
                    .WithData("max", maxLength);
            }

            if (!string.IsNullOrEmpty(Personality) && Personality.Length > maxLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Personality")
                    .WithData("min", 0)
                    .WithData("max", maxLength);
            }

            if (!string.IsNullOrEmpty(Attitudes) && Attitudes.Length > maxLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Attitudes")
                    .WithData("min", 0)
                    .WithData("max", maxLength);
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Interests;
            yield return Values;
            yield return Lifestyle;
            yield return Personality;
            yield return Attitudes;
        }
    }
}
