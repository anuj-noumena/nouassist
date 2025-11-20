using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance
{
    /// <summary>
    /// Application service interface for managing resource instances
    /// </summary>
    public interface IResourceInstanceAppService : IApplicationService
    {
        /// <summary>
        /// Gets a resource instance by ID
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <returns>Resource instance DTO</returns>
        Task<ResourceInstanceDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a resource instance by identifier
        /// </summary>
        /// <param name="identifier">Resource instance identifier</param>
        /// <returns>Resource instance DTO</returns>
        Task<ResourceInstanceDto> GetByIdentifierAsync(string identifier);

        /// <summary>
        /// Gets a paged and filtered list of resource instances
        /// </summary>
        /// <param name="input">Filter and paging parameters</param>
        /// <returns>Paged result of resource instance DTOs</returns>
        Task<PagedResultDto<ResourceInstanceDto>> GetListAsync(GetResourceInstanceListDto input);

        /// <summary>
        /// Creates a new resource instance
        /// </summary>
        /// <param name="input">Resource instance creation data</param>
        /// <returns>Created resource instance DTO</returns>
        Task<ResourceInstanceDto> CreateAsync(CreateResourceInstanceDto input);

        /// <summary>
        /// Updates an existing resource instance
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <param name="input">Resource instance update data</param>
        /// <returns>Updated resource instance DTO</returns>
        Task<ResourceInstanceDto> UpdateAsync(Guid id, UpdateResourceInstanceDto input);

        /// <summary>
        /// Deletes a resource instance
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Marks a resource instance as available
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <returns>Updated resource instance DTO</returns>
        Task<ResourceInstanceDto> MarkAsAvailableAsync(Guid id);

        /// <summary>
        /// Marks a resource instance as unavailable
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <param name="reason">Reason for unavailability</param>
        /// <returns>Updated resource instance DTO</returns>
        Task<ResourceInstanceDto> MarkAsUnavailableAsync(Guid id, string reason);

        /// <summary>
        /// Retires a resource instance
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <param name="retirementDate">Retirement date (defaults to now)</param>
        /// <param name="reason">Reason for retirement</param>
        /// <returns>Updated resource instance DTO</returns>
        Task<ResourceInstanceDto> RetireAsync(Guid id, DateTime? retirementDate = null, string? reason = null);

        /// <summary>
        /// Allocates a resource instance
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <param name="input">Allocation data</param>
        /// <returns>Updated resource instance DTO with allocation information</returns>
        Task<ResourceInstanceDto> AllocateAsync(Guid id, AllocateResourceDto input);

        /// <summary>
        /// Ends the current allocation for a resource instance
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <param name="input">End allocation data</param>
        /// <returns>Updated resource instance DTO</returns>
        Task<ResourceInstanceDto> EndAllocationAsync(Guid id, EndAllocationDto input);

        /// <summary>
        /// Gets all allocation records for a resource instance
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <returns>List of allocation records</returns>
        Task<ListResultDto<AllocationRecordDto>> GetAllocationHistoryAsync(Guid id);

        /// <summary>
        /// Logs usage of a resource instance
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <param name="input">Usage data</param>
        /// <returns>Created usage record DTO</returns>
        Task<UsageRecordDto> LogUsageAsync(Guid id, LogResourceUsageDto input);

        /// <summary>
        /// Gets all usage records for a resource instance
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <returns>List of usage records</returns>
        Task<ListResultDto<UsageRecordDto>> GetUsageHistoryAsync(Guid id);

        /// <summary>
        /// Sets an attribute value for a resource instance
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="value">Attribute value</param>
        /// <returns>Updated resource instance DTO</returns>
        Task<ResourceInstanceDto> SetAttributeValueAsync(Guid id, string attributeName, string? value);
        
        /// <summary>
        /// Finds available resources matching specified criteria
        /// </summary>
        /// <param name="resourceTypeId">Resource type ID (optional)</param>
        /// <param name="startTime">Availability start time</param>
        /// <param name="endTime">Availability end time</param>
        /// <param name="attributes">Required attributes (optional)</param>
        /// <returns>List of available resource instances</returns>
        Task<ListResultDto<ResourceInstanceDto>> FindAvailableResourcesAsync(
            Guid? resourceTypeId, 
            DateTime startTime, 
            DateTime endTime, 
            Dictionary<string, string>? attributes = null);
    }
}
