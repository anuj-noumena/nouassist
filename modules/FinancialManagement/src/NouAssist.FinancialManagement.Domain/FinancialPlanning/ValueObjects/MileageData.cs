using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.FinancialPlanning.ValueObjects
{
    public class MileageData : ValueObject
    {
        public decimal Miles { get; }
        public decimal RatePerMile { get; }
        public string Purpose { get; }
        public string StartLocation { get; }
        public string EndLocation { get; }

        protected MileageData() { }

        public MileageData(
            decimal miles,
            decimal ratePerMile,
            string purpose,
            string startLocation = null,
            string endLocation = null)
        {
            if (miles <= 0)
                throw new System.ArgumentException("Miles must be positive", nameof(miles));

            if (ratePerMile <= 0)
                throw new System.ArgumentException("Rate per mile must be positive", nameof(ratePerMile));

            Miles = miles;
            RatePerMile = ratePerMile;
            Purpose = purpose;
            StartLocation = startLocation;
            EndLocation = endLocation;
        }

        public decimal CalculateTotal() => Miles * RatePerMile;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Miles;
            yield return RatePerMile;
            yield return Purpose;
        }
    }
}
