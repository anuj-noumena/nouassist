using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects
{
    /// <summary>
    /// Represents the result of a dilution impact calculation
    /// </summary>
    public class DilutionImpactResult : ValueObject
    {
        public OwnershipPercentage OwnershipBefore { get; private set; }
        public OwnershipPercentage OwnershipAfter { get; private set; }
        public decimal DilutionPercentage { get; private set; }
        public ShareQuantity SharesBefore { get; private set; }
        public ShareQuantity SharesAfter { get; private set; }

        private DilutionImpactResult() { }

        public DilutionImpactResult(
            OwnershipPercentage ownershipBefore,
            OwnershipPercentage ownershipAfter,
            ShareQuantity sharesBefore,
            ShareQuantity sharesAfter)
        {
            OwnershipBefore = ownershipBefore;
            OwnershipAfter = ownershipAfter;
            SharesBefore = sharesBefore;
            SharesAfter = sharesAfter;
            DilutionPercentage = CalculateDilutionPercentage();
        }

        private decimal CalculateDilutionPercentage()
        {
            if (OwnershipBefore.Value == 0)
                return 0;

            return ((OwnershipBefore.Value - OwnershipAfter.Value) / OwnershipBefore.Value) * 100;
        }

        public bool HasDilution => DilutionPercentage > 0;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return OwnershipBefore;
            yield return OwnershipAfter;
            yield return DilutionPercentage;
            yield return SharesBefore;
            yield return SharesAfter;
        }

        public override string ToString()
        {
            return $"Dilution: {DilutionPercentage:0.##}% ({OwnershipBefore} → {OwnershipAfter})";
        }
    }
}
