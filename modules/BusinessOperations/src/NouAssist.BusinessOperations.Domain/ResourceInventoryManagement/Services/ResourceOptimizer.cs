using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Services
{
    /// <summary>
    /// Domain service for resource optimization operations
    /// </summary>
    public class ResourceOptimizer : DomainService
    {
        private readonly IResourceInstanceRepository _resourceRepository;
        private readonly IResourceBookingRepository _bookingRepository;
        private readonly ILogger<ResourceOptimizer> _logger;

        /// <summary>
        /// Creates a new instance of ResourceOptimizer
        /// </summary>
        /// <param name="resourceRepository">Resource instance repository</param>
        /// <param name="bookingRepository">Resource booking repository</param>
        /// <param name="logger">Logger</param>
        public ResourceOptimizer(
            IResourceInstanceRepository resourceRepository,
            IResourceBookingRepository bookingRepository,
            ILogger<ResourceOptimizer> logger)
        {
            _resourceRepository = resourceRepository;
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        /// <summary>
        /// Calculates resource utilization for a specific time period
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Utilization percentage (0-100)</returns>
        public async Task<decimal> CalculateResourceUtilizationAsync(Guid resourceInstanceId, DateTime startDate, DateTime endDate)
        {
            // Get resource bookings for the time period
            var bookings = await _bookingRepository.GetActiveBookingsForResourceAsync(
                resourceInstanceId, 
                startDate, 
                endDate);

            if (bookings.Count == 0)
            {
                return 0;
            }

            // Calculate total time period duration in minutes
            var totalMinutes = (endDate - startDate).TotalMinutes;
            
            if (totalMinutes <= 0)
            {
                return 0;
            }

            // Calculate total booked time in minutes
            decimal bookedMinutes = 0;
            
            foreach (var booking in bookings)
            {
                var bookingStart = booking.TimeSlot.StartTime < startDate ? startDate : booking.TimeSlot.StartTime;
                var bookingEnd = booking.TimeSlot.EndTime > endDate ? endDate : booking.TimeSlot.EndTime;
                
                var duration = (bookingEnd - bookingStart).TotalMinutes;
                
                if (duration > 0)
                {
                    bookedMinutes += (decimal)duration;
                }
            }

            // Calculate utilization percentage
            var utilization = (bookedMinutes / (decimal)totalMinutes) * 100;
            
            // Cap at 100%
            return Math.Min(utilization, 100);
        }

        /// <summary>
        /// Calculates utilization for multiple resources
        /// </summary>
        /// <param name="resourceTypeId">Resource type ID (optional)</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Dictionary of resource ID to utilization percentage</returns>
        public async Task<Dictionary<Guid, decimal>> CalculateResourceTypeUtilizationAsync(
            Guid? resourceTypeId,
            DateTime startDate,
            DateTime endDate)
        {
            // Get resources
            List<ResourceInstanceAggregate> resources;
            if (resourceTypeId.HasValue)
            {
                resources = await _resourceRepository.GetListByResourceTypeAsync(resourceTypeId.Value);
            }
            else
            {
                resources = await _resourceRepository.GetListAsync();
            }

            var result = new Dictionary<Guid, decimal>();

            foreach (var resource in resources)
            {
                var utilization = await CalculateResourceUtilizationAsync(resource.Id, startDate, endDate);
                result.Add(resource.Id, utilization);
            }

            return result;
        }

        /// <summary>
        /// Identifies resource bottlenecks (resources with high utilization)
        /// </summary>
        /// <param name="resourceTypeId">Resource type ID (optional)</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <param name="threshold">Utilization threshold percentage (default 80%)</param>
        /// <returns>List of high-utilization resources</returns>
        public async Task<List<ResourceUtilizationInfo>> IdentifyResourceBottlenecksAsync(
            Guid? resourceTypeId,
            DateTime startDate,
            DateTime endDate,
            decimal threshold = 80)
        {
            var utilizationData = await CalculateResourceTypeUtilizationAsync(
                resourceTypeId,
                startDate,
                endDate);

            if (utilizationData.Count == 0)
            {
                return new List<ResourceUtilizationInfo>();
            }

            // Get resources
            var resourceIds = utilizationData.Keys.ToList();
            var resources = await _resourceRepository.GetListAsync(resourceIds);
            
            // Create result list
            var result = new List<ResourceUtilizationInfo>();
            
            foreach (var resource in resources)
            {
                if (utilizationData.TryGetValue(resource.Id, out var utilization) && utilization >= threshold)
                {
                    result.Add(new ResourceUtilizationInfo
                    {
                        ResourceId = resource.Id,
                        ResourceName = resource.Name,
                        ResourceTypeId = resource.ResourceTypeId,
                        Utilization = utilization
                    });
                }
            }

            return result.OrderByDescending(r => r.Utilization).ToList();
        }

        /// <summary>
        /// Suggests alternative resources for a booking
        /// </summary>
        /// <param name="resourceTypeId">Resource type ID</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="attributes">Required attributes (optional)</param>
        /// <param name="maxResults">Maximum number of results (default 5)</param>
        /// <returns>List of available alternative resources</returns>
        public async Task<List<ResourceInstanceAggregate>> SuggestAlternativeResourcesAsync(
            Guid resourceTypeId,
            DateTime startTime,
            DateTime endTime,
            Dictionary<string, string> attributes = null,
            int maxResults = 5)
        {
            // Find available resources matching criteria
            var availableResources = await _resourceRepository.FindAvailableResourcesAsync(
                resourceTypeId,
                startTime,
                endTime,
                attributes);

            return availableResources.Take(maxResults).ToList();
        }
    }

    /// <summary>
    /// Resource utilization information
    /// </summary>
    public class ResourceUtilizationInfo
    {
        /// <summary>
        /// Resource instance ID
        /// </summary>
        public Guid ResourceId { get; set; }

        /// <summary>
        /// Resource name
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Resource type ID
        /// </summary>
        public Guid ResourceTypeId { get; set; }

        /// <summary>
        /// Utilization percentage (0-100)
        /// </summary>
        public decimal Utilization { get; set; }
    }
}
