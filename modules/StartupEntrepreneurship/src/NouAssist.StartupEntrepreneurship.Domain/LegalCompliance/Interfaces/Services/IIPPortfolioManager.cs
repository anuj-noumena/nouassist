using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Services
{
    /// <summary>
    /// Service for managing intellectual property portfolios
    /// </summary>
    public interface IIPPortfolioManager
    {
        /// <summary>
        /// Creates a new intellectual property asset
        /// </summary>
        Task<IntellectualPropertyAsset> CreateIPAssetAsync(
            string name,
            string description,
            IntellectualPropertyType type,
            string identifier,
            string issuingAuthority,
            string country,
            string state,
            DateTime filingDate,
            FilingStatusType initialStatus,
            DateTime? registrationDate,
            DateTime? expirationDate);
        
        /// <summary>
        /// Updates an intellectual property asset
        /// </summary>
        Task<IntellectualPropertyAsset> UpdateIPAssetAsync(
            Guid id,
            string name,
            string description,
            string country,
            string state);
        
        /// <summary>
        /// Updates the filing status of an intellectual property asset
        /// </summary>
        Task UpdateIPStatusAsync(
            Guid id,
            FilingStatusType newStatus,
            string notes = null);
        
        /// <summary>
        /// Sets the valuation for an intellectual property asset
        /// </summary>
        Task SetIPValuationAsync(
            Guid id,
            decimal estimatedValue,
            string currency,
            string valuationMethod,
            string valuationNotes = null);
        
        /// <summary>
        /// Adds a filing deadline to an intellectual property asset
        /// </summary>
        Task AddIPFilingDeadlineAsync(
            Guid ipId,
            string title,
            DateTime deadline,
            string description = null,
            bool isRecurring = false,
            string recurrencePattern = null);
        
        /// <summary>
        /// Gets intellectual property assets with approaching deadlines
        /// </summary>
        Task<List<IntellectualPropertyAsset>> GetIPAssetsWithApproachingDeadlinesAsync(int daysThreshold);
    }
}
