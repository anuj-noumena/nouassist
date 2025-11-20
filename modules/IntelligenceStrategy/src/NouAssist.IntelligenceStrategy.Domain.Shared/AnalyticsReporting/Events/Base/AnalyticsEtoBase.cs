using System;
using Volo.Abp.EventBus;

namespace NouAssist.Analytics.Events.Base
{
    /// <summary>
    /// Base class for all Analytics module event transfer objects
    /// </summary>
    [Serializable]
    public abstract class AnalyticsEtoBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
    }
}
