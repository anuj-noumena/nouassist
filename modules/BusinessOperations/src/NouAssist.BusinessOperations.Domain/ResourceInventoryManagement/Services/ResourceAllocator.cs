using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Services
{
    /// <summary>
    /// Domain service for resource allocation operations
    /// </summary>
    public class ResourceAllocator : DomainService
    {
        private readonly IResourceInstanceRepository _resourceRepository;
        private readonly ILogger<ResourceAllocator> _logger;

        /// <summary>
        /// Creates a new instance of ResourceAllocator
        /// </summary>
        /// <param name="resourceRepository">Resource instance repository</param>
        /// <param name="logger">Logger</param>
        public ResourceAllocator(
            IResourceInstanceRepository resourceRepository,
            ILogger<ResourceAllocator> logger)
        {
            _resourceRepository = resourceRepository;
            _logger = logger;
        }

        /// <summary>
        /// Allocates a resource to an entity
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="allocatedToEntity">Entity type the resource is allocated to</param>
        /// <param name="allocatedToEntityId">ID of the entity the resource is allocated to</param>
        /// <param name="startTime">Allocation start time</param>
        /// <param name="endTime">Allocation end time (optional)</param>
        /// <param name="allocationPercentage">Allocation percentage (optional)</param>
        /// <param name="notes">Allocation notes (optional)</param>
        /// <returns>The created allocation record</returns>
        public async Task<AllocationRecord> AllocateResourceAsync(
            Guid resourceInstanceId,
            string allocatedToEntity,
            Guid allocatedToEntityId,
            DateTime startTime,
            DateTime? endTime = null,
            decimal? allocationPercentage = null,
            string notes = null)
        {
            var resource = await _resourceRepository.GetAsync(resourceInstanceId);
            if (resource == null)
            {
                throw new ResourceInstanceNotFoundException(resourceInstanceId);
            }

            // Check if resource can be allocated
            if (resource.Status == ResourceStatus.Retired)
            {
                throw new ResourceAllocationException(resourceInstanceId, "Cannot allocate a retired resource");
            }

            if (resource.Status == ResourceStatus.Unavailable)
            {
                throw new ResourceAllocationException(resourceInstanceId, "Cannot allocate an unavailable resource");
            }

            // Generate a new allocation ID
            var allocationId = GuidGenerator.Create();

            // Allocate the resource
            var allocationRecord = resource.Allocate(
                allocationId,
                allocatedToEntity,
                allocatedToEntityId,
                startTime,
                endTime,
                allocationPercentage,
                notes);

            // Save changes
            await _resourceRepository.UpdateAsync(resource);

            _logger.LogInformation(
                "Resource {ResourceId} allocated to {EntityType} {EntityId} from {StartTime} to {EndTime}",
                resourceInstanceId, allocatedToEntity, allocatedToEntityId, startTime, endTime);

            return allocationRecord;
        }

        /// <summary>
        /// Allocates multiple resources to an entity
        /// </summary>
        /// <param name="resourceInstanceIds">List of resource instance IDs</param>
        /// <param name="allocatedToEntity">Entity type the resources are allocated to</param>
        /// <param name="allocatedToEntityId">ID of the entity the resources are allocated to</param>
        /// <param name="startTime">Allocation start time</param>
        /// <param name="endTime">Allocation end time (optional)</param>
        /// <param name="allocationPercentage">Allocation percentage (optional)</param>
        /// <param name="notes">Allocation notes (optional)</param>
        /// <returns>Dictionary of resource IDs to allocation records</returns>
        public async Task<Dictionary<Guid, AllocationRecord>> AllocateResourcesAsync(
            IEnumerable<Guid> resourceInstanceIds,
            string allocatedToEntity,
            Guid allocatedToEntityId,
            DateTime startTime,
            DateTime? endTime = null,
            decimal? allocationPercentage = null,
            string notes = null)
        {
            var resources = await _resourceRepository.GetListAsync(resourceInstanceIds.ToList());
            
            var allocations = new Dictionary<Guid, AllocationRecord>();
            var failedAllocations = new List<(Guid ResourceId, string Reason)>();

            foreach (var resource in resources)
            {
                try
                {
                    // Check if resource can be allocated
                    if (resource.Status == ResourceStatus.Retired)
                    {
                        failedAllocations.Add((resource.Id, "Resource is retired"));
                        continue;
                    }

                    if (resource.Status == ResourceStatus.Unavailable)
                    {
                        failedAllocations.Add((resource.Id, "Resource is unavailable"));
                        continue;
                    }

                    // Generate a new allocation ID
                    var allocationId = GuidGenerator.Create();

                    // Allocate the resource
                    var allocationRecord = resource.Allocate(
                        allocationId,
                        allocatedToEntity,
                        allocatedToEntityId,
                        startTime,
                        endTime,
                        allocationPercentage,
                        notes);

                    allocations.Add(resource.Id, allocationRecord);
                }
                catch (Exception ex)
                {
                    failedAllocations.Add((resource.Id, ex.Message));
                    _logger.LogWarning(ex, "Failed to allocate resource {ResourceId}", resource.Id);
                }
            }

            // Save changes
            if (allocations.Any())
            {
                await _resourceRepository.UpdateManyAsync(resources.Where(r => allocations.ContainsKey(r.Id)));
            }

            // Log the operation
            _logger.LogInformation(
                "Allocated {SuccessCount} resources to {EntityType} {EntityId}, {FailureCount} failed",
                allocations.Count, allocatedToEntity, allocatedToEntityId, failedAllocations.Count);

            // If any allocations failed, but some succeeded, we could throw a custom exception with partial success info
            if (failedAllocations.Any() && allocations.Any())
            {
                _logger.LogWarning(
                    "Partial allocation success: {SuccessCount} succeeded, {FailureCount} failed",
                    allocations.Count, failedAllocations.Count);
            }
            else if (failedAllocations.Any() && !allocations.Any())
            {
                throw new ResourceAllocationException(failedAllocations.First().ResourceId, 
                    "All resource allocations failed: " + failedAllocations.First().Reason);
            }

            return allocations;
        }

        /// <summary>
        /// Ends a resource allocation
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="endTime">End time (defaults to now)</param>
        /// <param name="notes">End allocation notes (optional)</param>
        /// <returns>The updated allocation record</returns>
        public async Task<AllocationRecord> EndAllocationAsync(
            Guid resourceInstanceId,
            DateTime? endTime = null,
            string notes = null)
        {
            var resource = await _resourceRepository.GetAsync(resourceInstanceId);
            if (resource == null)
            {
                throw new ResourceInstanceNotFoundException(resourceInstanceId);
            }

            // Check if resource is allocated
            if (resource.Status != ResourceStatus.Allocated || resource.CurrentAllocation == null)
            {
                throw new ResourceAllocationException(resourceInstanceId, "Resource is not currently allocated");
            }

            // End the allocation
            var allocationRecord = resource.EndAllocation(endTime ?? DateTime.Now, notes);

            // Save changes
            await _resourceRepository.UpdateAsync(resource);

            _logger.LogInformation(
                "Resource {ResourceId} allocation ended at {EndTime}",
                resourceInstanceId, allocationRecord.EndTime);

            return allocationRecord;
        }

        /// <summary>
        /// Finds available resources matching specified criteria
        /// </summary>
        /// <param name="resourceTypeId">Resource type ID (optional)</param>
        /// <param name="startTime">Availability start time</param>
        /// <param name="endTime">Availability end time</param>
        /// <param name="attributes">Required attributes (optional)</param>
        /// <returns>List of available resource instances</returns>
        public async Task<List<ResourceInstanceAggregate>> FindAvailableResourcesAsync(
            Guid? resourceTypeId,
            DateTime startTime,
            DateTime endTime,
            Dictionary<string, string> attributes = null)
        {
            return await _resourceRepository.FindAvailableResourcesAsync(
                resourceTypeId,
                startTime,
                endTime,
                attributes);
        }
    }
}
