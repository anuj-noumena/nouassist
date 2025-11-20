namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Events
{
    /// <summary>
    /// Event raised when a knowledge asset is updated
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssetUpdated")]
    public class KnowledgeAssetUpdatedEto : KnowledgeAssetEtoBase
    {
        public DateTime LastModificationTime { get; set; }
        public Guid? LastModifierId { get; set; }
    }
}
