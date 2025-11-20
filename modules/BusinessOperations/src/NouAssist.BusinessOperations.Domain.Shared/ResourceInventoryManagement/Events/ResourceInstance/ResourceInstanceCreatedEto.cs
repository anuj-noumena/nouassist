using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Events.ResourceInstance
{
    /// <summary>
    /// Event raised when a new resource instance is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstanceCreated")]
    public class ResourceInstanceCreatedEto : RIMEtoBase
    {
        /// <summary>
        /// ID of the resource instance
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID of the resource type
        /// </summary>
        public Guid ResourceTypeId { get; set; }

        /// <summary>
        /// Name of the resource instance
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identifier of the resource instance
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Current status of the resource instance
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Associated user ID (only for human resources)
        /// </summary>
        public Guid? UserId { get; set; }
    }
}
