using System;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities
{
    /// <summary>
    /// Represents specific characteristics of a customer segment
    /// </summary>
    public class SegmentCharacteristics : Entity<Guid>
    {
        public Guid CustomerSegmentProfileId { get; private set; }
        public string CharacteristicName { get; private set; }
        public string CharacteristicValue { get; private set; }
        public string Category { get; private set; }
        public decimal Importance { get; private set; }
        public string Notes { get; private set; }

        protected SegmentCharacteristics() { }

        internal SegmentCharacteristics(
            Guid id,
            Guid customerSegmentProfileId,
            string characteristicName,
            string characteristicValue,
            string category,
            decimal importance = 0.5m,
            string notes = null) : base(id)
        {
            CustomerSegmentProfileId = customerSegmentProfileId;
            SetCharacteristicName(characteristicName);
            SetCharacteristicValue(characteristicValue);
            Category = category?.Trim();
            SetImportance(importance);
            Notes = notes?.Trim();
        }

        internal void UpdateCharacteristic(
            string characteristicName,
            string characteristicValue,
            string category,
            decimal importance,
            string notes = null)
        {
            SetCharacteristicName(characteristicName);
            SetCharacteristicValue(characteristicValue);
            Category = category?.Trim();
            SetImportance(importance);
            Notes = notes?.Trim();
        }

        private void SetCharacteristicName(string characteristicName)
        {
            CharacteristicName = Check.NotNullOrWhiteSpace(
                characteristicName,
                nameof(characteristicName),
                maxLength: MarketIntelligenceConsts.CustomerSegment.MaxCharacteristicNameLength
            );
        }

        private void SetCharacteristicValue(string characteristicValue)
        {
            CharacteristicValue = Check.NotNullOrWhiteSpace(
                characteristicValue,
                nameof(characteristicValue),
                maxLength: MarketIntelligenceConsts.CustomerSegment.MaxCharacteristicValueLength
            );
        }

        private void SetImportance(decimal importance)
        {
            if (importance < MarketIntelligenceConsts.Validation.MinConfidenceScore ||
                importance > MarketIntelligenceConsts.Validation.MaxConfidenceScore)
            {
                throw new BusinessException("MarketIntelligence:Validation:InvalidScore")
                    .WithData("min", MarketIntelligenceConsts.Validation.MinConfidenceScore)
                    .WithData("max", MarketIntelligenceConsts.Validation.MaxConfidenceScore);
            }

            Importance = importance;
        }
    }
}
