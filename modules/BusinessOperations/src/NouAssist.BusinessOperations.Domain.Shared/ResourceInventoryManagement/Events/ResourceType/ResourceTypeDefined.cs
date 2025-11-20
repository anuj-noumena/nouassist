using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Events.ResourceType
{
    /// <summary>
    /// Event raised when a new resource type is defined
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceTypeDefined")]
    public class ResourceTypeDefinedEto : RIMEtoBase
    {
        /// <summary>
        /// ID of the resource type
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the resource type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Code/identifier of the resource type
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Category of the resource type
        /// </summary>
        public string Category { get; set; }
    }
}
