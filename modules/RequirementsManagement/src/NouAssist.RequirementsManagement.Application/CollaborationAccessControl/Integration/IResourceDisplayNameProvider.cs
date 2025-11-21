using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Integration
{
    /// <summary>
    /// Interface for retrieving display names for resources
    /// </summary>
    public interface IResourceDisplayNameProvider
    {
        /// <summary>
        /// Gets a display name for a resource
        /// </summary>
        Task<string> GetResourceDisplayNameAsync(TargetResourceType resourceType, Guid? resourceId);
    }
}
