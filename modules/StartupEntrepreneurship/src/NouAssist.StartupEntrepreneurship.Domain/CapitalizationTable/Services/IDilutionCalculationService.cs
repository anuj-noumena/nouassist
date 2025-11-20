using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Services
{
    /// <summary>
    /// Domain service for calculating dilution impact
    /// </summary>
    public interface IDilutionCalculationService
    {
        /// <summary>
        /// Calculates dilution impact for a security holder given a new issuance
        /// </summary>
        Task<DilutionImpactResult> CalculateDilutionImpactAsync(
            Guid capTableId,
            Guid securityHolderId,
            ShareQuantity newSharesIssued,
            DateTime effectiveDate);

        /// <summary>
        /// Calculates dilution impact for all holders given a new issuance
        /// </summary>
        Task<Dictionary<Guid, DilutionImpactResult>> CalculateDilutionForAllHoldersAsync(
            Guid capTableId,
            ShareQuantity newSharesIssued,
            DateTime effectiveDate);

        /// <summary>
        /// Calculates dilution impact from a funding round
        /// </summary>
        Task<Dictionary<Guid, DilutionImpactResult>> CalculateFundingRoundDilutionAsync(
            Guid capTableId,
            Guid fundingRoundId);

        /// <summary>
        /// Calculates what-if dilution scenarios
        /// </summary>
        Task<Dictionary<Guid, DilutionImpactResult>> CalculateWhatIfDilutionAsync(
            Guid capTableId,
            ShareQuantity hypotheticalIssuance,
            DateTime effectiveDate);
    }
}
