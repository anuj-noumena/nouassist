using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects
{
    /// <summary>
    /// Value object containing competitor's basic information
    /// </summary>
    public class CompetitorInfo : ValueObject
    {
        public string Website { get; private set; }
        public string Industry { get; private set; }
        public string Location { get; private set; }
        public int? EmployeeCount { get; private set; }
        public decimal? FundingAmount { get; private set; }
        public string FundingSource { get; private set; }

        private CompetitorInfo() { }

        public CompetitorInfo(
            string website = null,
            string industry = null,
            string location = null,
            int? employeeCount = null,
            decimal? fundingAmount = null,
            string fundingSource = null)
        {
            Website = website?.Trim();
            Industry = industry?.Trim();
            Location = location?.Trim();
            EmployeeCount = employeeCount;
            FundingAmount = fundingAmount;
            FundingSource = fundingSource?.Trim();

            ValidateData();
        }

        private void ValidateData()
        {
            if (!string.IsNullOrEmpty(Website) && Website.Length > MarketIntelligenceConsts.Competitor.MaxWebsiteLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Website")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.Competitor.MaxWebsiteLength);
            }

            if (!string.IsNullOrEmpty(Industry) && Industry.Length > MarketIntelligenceConsts.Competitor.MaxIndustryLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Industry")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.Competitor.MaxIndustryLength);
            }

            if (!string.IsNullOrEmpty(Location) && Location.Length > MarketIntelligenceConsts.Competitor.MaxLocationLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Location")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.Competitor.MaxLocationLength);
            }

            if (EmployeeCount.HasValue && EmployeeCount.Value < 0)
            {
                throw new BusinessException("MarketIntelligence:Validation:InvalidValue")
                    .WithData("propertyName", "Employee Count");
            }

            if (FundingAmount.HasValue && FundingAmount.Value < 0)
            {
                throw new BusinessException("MarketIntelligence:Validation:InvalidValue")
                    .WithData("propertyName", "Funding Amount");
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Website;
            yield return Industry;
            yield return Location;
            yield return EmployeeCount;
            yield return FundingAmount;
            yield return FundingSource;
        }
    }
}
