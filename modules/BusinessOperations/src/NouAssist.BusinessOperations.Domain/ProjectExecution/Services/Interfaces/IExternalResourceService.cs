using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ProjectExecution.ValueObjects;

namespace NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces
{
    /// <summary>
    /// Service for integrating with external resource systems
    /// </summary>
    public interface IExternalResourceService
    {
        /// <summary>
        /// Allocates resources to a project based on requirements
        /// </summary>
        Task<bool> AllocateResourcesAsync(
            Guid projectId,
            List<ResourceRequirement> requirements);
            
        /// <summary>
        /// Requests a specific resource allocation for a task
        /// </summary>
        Task<bool> RequestResourceForTaskAsync(
            Guid taskId,
            ResourceRequirement requirement);
            
        /// <summary>
        /// Gets resource availability for a date range
        /// </summary>
        Task<Dictionary<string, int>> GetResourceAvailabilityAsync(
            DateTime startDate,
            DateTime endDate,
            string skillFilter = null);
    }
}
