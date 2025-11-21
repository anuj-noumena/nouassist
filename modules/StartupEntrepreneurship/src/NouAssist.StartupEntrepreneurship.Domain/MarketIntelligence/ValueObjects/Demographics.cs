using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects
{
    /// <summary>
    /// Value object representing demographic information for customer segments
    /// </summary>
    public class Demographics : ValueObject
    {
        public string AgeRange { get; private set; }
        public string Gender { get; private set; }
        public string Income { get; private set; }
        public string Education { get; private set; }
        public string Location { get; private set; }
        public string Occupation { get; private set; }

        private Demographics() { }

        public Demographics(
            string ageRange = null,
            string gender = null,
            string income = null,
            string education = null,
            string location = null,
            string occupation = null)
        {
            AgeRange = ageRange?.Trim();
            Gender = gender?.Trim();
            Income = income?.Trim();
            Education = education?.Trim();
            Location = location?.Trim();
            Occupation = occupation?.Trim();

            ValidateData();
        }

        private void ValidateData()
        {
            var maxLength = MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength;

            if (!string.IsNullOrEmpty(AgeRange) && AgeRange.Length > maxLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Age Range")
                    .WithData("min", 0)
                    .WithData("max", maxLength);
            }

            if (!string.IsNullOrEmpty(Gender) && Gender.Length > maxLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Gender")
                    .WithData("min", 0)
                    .WithData("max", maxLength);
            }

            if (!string.IsNullOrEmpty(Income) && Income.Length > maxLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Income")
                    .WithData("min", 0)
                    .WithData("max", maxLength);
            }

            if (!string.IsNullOrEmpty(Education) && Education.Length > maxLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Education")
                    .WithData("min", 0)
                    .WithData("max", maxLength);
            }

            if (!string.IsNullOrEmpty(Location) && Location.Length > maxLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Location")
                    .WithData("min", 0)
                    .WithData("max", maxLength);
            }

            if (!string.IsNullOrEmpty(Occupation) && Occupation.Length > maxLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Occupation")
                    .WithData("min", 0)
                    .WithData("max", maxLength);
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return AgeRange;
            yield return Gender;
            yield return Income;
            yield return Education;
            yield return Location;
            yield return Occupation;
        }
    }
}
