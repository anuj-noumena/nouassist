using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services
{
    /// <summary>
    /// Interface for customer segment profiling domain service
    /// </summary>
    public interface ICustomerSegmentProfileService
    {
        /// <summary>
        /// Creates a new customer segment profile
        /// </summary>
        Task<CustomerSegmentProfile> CreateSegmentProfileAsync(
            string name,
            string description,
            SegmentType type,
            Demographics demographics,
            Psychographics psychographics,
            string primaryNeeds);

        /// <summary>
        /// Updates a customer segment profile
        /// </summary>
        Task<CustomerSegmentProfile> UpdateSegmentProfileAsync(
            Guid segmentId,
            string name,
            string description,
            Demographics demographics,
            Psychographics psychographics,
            string primaryNeeds);

        /// <summary>
        /// Updates market sizing information
        /// </summary>
        Task UpdateMarketSizingAsync(
            Guid segmentId,
            decimal estimatedSize,
            decimal estimatedValue);

        /// <summary>
        /// Adds a characteristic to a segment profile
        /// </summary>
        Task AddSegmentCharacteristicAsync(
            Guid segmentId,
            string characteristicName,
            string characteristicValue,
            string category,
            decimal importance = 0.5m,
            string notes = null);

        /// <summary>
        /// Gets active segment profiles
        /// </summary>
        Task<List<CustomerSegmentProfile>> GetActiveSegmentProfilesAsync();

        /// <summary>
        /// Gets primary target segments
        /// </summary>
        Task<List<CustomerSegmentProfile>> GetPrimarySegmentsAsync();

        /// <summary>
        /// Evaluates segment attractiveness
        /// </summary>
        Task<Dictionary<Guid, decimal>> EvaluateSegmentAttractivenessAsync();

        /// <summary>
        /// Checks segments for data quality and completeness
        /// </summary>
        Task<Dictionary<Guid, bool>> EvaluateSegmentDataCompletenessAsync();
    }
}
