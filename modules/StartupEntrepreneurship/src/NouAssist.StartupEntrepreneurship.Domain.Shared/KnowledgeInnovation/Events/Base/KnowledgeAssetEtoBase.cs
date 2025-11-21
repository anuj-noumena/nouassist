using System;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Events.Base
{
    /// <summary>
    /// Base class for KnowledgeAsset event transfer objects
    /// </summary>
    [Serializable]
    public abstract class KnowledgeAssetEtoBase
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public KnowledgeAssetType AssetType { get; set; }
        public Guid? TenantId { get; set; }
    }
}
