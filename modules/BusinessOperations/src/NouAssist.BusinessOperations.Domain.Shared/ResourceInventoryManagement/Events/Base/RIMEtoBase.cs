using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Events.Base
{
    /// <summary>
    /// Base class for all RIM module event transfer objects
    /// </summary>
    [Serializable]
    public abstract class RIMEtoBase
    {
        /// <summary>
        /// The tenant ID associated with the event
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
