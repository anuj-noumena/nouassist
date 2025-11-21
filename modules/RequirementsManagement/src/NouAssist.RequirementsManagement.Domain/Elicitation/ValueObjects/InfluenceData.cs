using System.Collections.Generic;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Elicitation.ValueObjects
{
    /// <summary>
    /// Value object for stakeholder influence and interest levels
    /// </summary>
    public class InfluenceData : ValueObject
    {
        public int InfluenceLevel { get; private set; }
        public int InterestLevel { get; private set; }
        public string Notes { get; private set; }

        private InfluenceData() { }

        public InfluenceData(int influenceLevel, int interestLevel, string notes = null)
        {
            SetInfluenceLevel(influenceLevel);
            SetInterestLevel(interestLevel);
            Notes = notes ?? string.Empty;
        }

        private void SetInfluenceLevel(int level)
        {
            if (level < ElicitationConsts.MinInfluenceLevel || level > ElicitationConsts.MaxInfluenceLevel)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidInfluenceLevel")
                    .WithData("min", ElicitationConsts.MinInfluenceLevel)
                    .WithData("max", ElicitationConsts.MaxInfluenceLevel);
            }

            InfluenceLevel = level;
        }

        private void SetInterestLevel(int level)
        {
            if (level < ElicitationConsts.MinInterestLevel || level > ElicitationConsts.MaxInterestLevel)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidInterestLevel")
                    .WithData("min", ElicitationConsts.MinInterestLevel)
                    .WithData("max", ElicitationConsts.MaxInterestLevel);
            }

            InterestLevel = level;
        }

        public string GetInfluenceCategory()
        {
            return InfluenceLevel switch
            {
                >= 8 => "High",
                >= 6 => "Medium",
                >= 4 => "Low",
                _ => "Minimal"
            };
        }

        public string GetInterestCategory()
        {
            return InterestLevel switch
            {
                >= 8 => "High",
                >= 6 => "Medium",
                >= 4 => "Low",
                _ => "Minimal"
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return InfluenceLevel;
            yield return InterestLevel;
            yield return Notes ?? string.Empty;
        }
    }
}
