using System;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Events
{
    /// <summary>
    /// Event raised when a knowledge asset is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssetCreated")]
    public class KnowledgeAssetCreatedEto : KnowledgeAssetEtoBase
    {
        public DateTime CreationTime { get; set; }
        public Guid CreatorId { get; set; }
    }
}
