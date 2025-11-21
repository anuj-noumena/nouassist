using System;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities
{
    /// <summary>
    /// Represents content for a knowledge asset
    /// </summary>
    public class AssetContent : Entity
    {
        public Guid KnowledgeAssetId { get; private set; }
        public string Content { get; private set; }
        public string ContentType { get; private set; } // MIME type or format identifier
        public int Version { get; private set; }
        public string ChangeLog { get; private set; }

        protected AssetContent() { }

        internal AssetContent(
            Guid knowledgeAssetId, 
            string content, 
            string contentType, 
            int version, 
            string changeLog = null)
        {
            KnowledgeAssetId = knowledgeAssetId;
            
            Content = Check.NotNullOrWhiteSpace(
                content, 
                nameof(content), 
                maxLength: KnowledgeInnovationConsts.KnowledgeAsset.ContentMaxLength);
            
            ContentType = Check.NotNullOrWhiteSpace(contentType, nameof(contentType));
            Version = version;
            ChangeLog = changeLog;
        }

        internal void UpdateContent(string content, string changeLog = null)
        {
            Content = Check.NotNullOrWhiteSpace(
                content, 
                nameof(content), 
                maxLength: KnowledgeInnovationConsts.KnowledgeAsset.ContentMaxLength);
            
            Version++;
            ChangeLog = changeLog;
        }

        public override object[] GetKeys()
        {
            return new object[] { KnowledgeAssetId, Version };
        }
    }
}
