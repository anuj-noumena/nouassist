using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment
{
    /// <summary>
    /// Interface for customer segment profile management application service
    /// </summary>
    public interface ICustomerSegmentProfileAppService : IApplicationService
    {
        /// <summary>
        /// Gets a customer segment profile by ID
        /// </summary>
        Task<CustomerSegmentProfileDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a paged list of customer segment profiles with optional filtering
        /// </summary>
        Task<PagedResultDto<CustomerSegmentProfileDto>> GetListAsync(GetCustomerSegmentListDto input);

        /// <summary>
        /// Creates a new customer segment profile
        /// </summary>
        Task<CustomerSegmentProfileDto> CreateAsync(CreateCustomerSegmentProfileDto input);

        /// <summary>
        /// Updates an existing customer segment profile
        /// </summary>
        Task<CustomerSegmentProfileDto> UpdateAsync(Guid id, UpdateCustomerSegmentProfileDto input);

        /// <summary>
        /// Deletes a customer segment profile
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Updates market sizing information for a segment
        /// </summary>
        Task<CustomerSegmentProfileDto> UpdateMarketSizingAsync(Guid id, UpdateMarketSizingDto input);

        /// <summary>
        /// Adds a characteristic to a segment profile
        /// </summary>
        Task<SegmentCharacteristicsDto> AddCharacteristicAsync(Guid id, CreateSegmentCharacteristicDto input);

        /// <summary>
        /// Updates a segment characteristic
        /// </summary>
        Task<SegmentCharacteristicsDto> UpdateCharacteristicAsync(Guid segmentId, Guid characteristicId, CreateSegmentCharacteristicDto input);

        /// <summary>
        /// Removes a characteristic from a segment profile
        /// </summary>
        Task DeleteCharacteristicAsync(Guid segmentId, Guid characteristicId);

        /// <summary>
        /// Changes the segment type
        /// </summary>
        Task<CustomerSegmentProfileDto> ChangeTypeAsync(Guid id, SegmentType newType);

        /// <summary>
        /// Deactivates a segment profile
        /// </summary>
        Task<CustomerSegmentProfileDto> DeactivateAsync(Guid id);

        /// <summary>
        /// Reactivates a segment profile
        /// </summary>
        Task<CustomerSegmentProfileDto> ReactivateAsync(Guid id);

        /// <summary>
        /// Gets active segment profiles
        /// </summary>
        Task<List<CustomerSegmentProfileDto>> GetActiveSegmentsAsync();

        /// <summary>
        /// Gets primary target segments
        /// </summary>
        Task<List<CustomerSegmentProfileDto>> GetPrimarySegmentsAsync();

        /// <summary>
        /// Evaluates segment attractiveness
        /// </summary>
        Task<Dictionary<Guid, decimal>> EvaluateSegmentAttractivenessAsync();
    }
}
