using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Events;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates
{
    /// <summary>
    /// Aggregate root for knowledge assets (playbooks, advice, templates, mental models)
    /// </summary>
    [Table("AppKnowledgeAssets")]
    public class KnowledgeAsset : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public KnowledgeAssetType AssetType { get; private set; }
        public Guid? TenantId { get; private set; }
        public AuthorInfo Author { get; private set; }
        public List<string> Tags { get; private set; }

        private readonly List<AssetContent> _contentVersions;
        public IReadOnlyCollection<AssetContent> ContentVersions => _contentVersions.AsReadOnly();
        public AssetContent CurrentContent => _contentVersions.OrderByDescending(v => v.Version).FirstOrDefault();

        protected KnowledgeAsset()
        {
            _contentVersions = new List<AssetContent>();
            Tags = new List<string>();
        }

        public static KnowledgeAsset Create(
            Guid id,
            string title,
            string description,
            KnowledgeAssetType assetType,
            AuthorInfo author,
            string initialContent,
            string contentType,
            List<string> tags = null,
            Guid? tenantId = null)
        {
            var asset = new KnowledgeAsset(
                id,
                title,
                description,
                assetType,
                author,
                tags,
                tenantId);
            
            asset.AddContent(initialContent, contentType);
            return asset;
        }

        private KnowledgeAsset(
            Guid id,
            string title,
            string description,
            KnowledgeAssetType assetType,
            AuthorInfo author,
            List<string> tags = null,
            Guid? tenantId = null) : base(id)
        {
            SetTitle(title);
            SetDescription(description);
            AssetType = assetType;
            Author = author;
            Tags = tags ?? new List<string>();
            TenantId = tenantId;
            _contentVersions = new List<AssetContent>();
        }

        public void UpdateDetails(
            string title,
            string description,
            KnowledgeAssetType assetType,
            List<string> tags)
        {
            SetTitle(title);
            SetDescription(description);
            AssetType = assetType;
            Tags = tags ?? new List<string>();

            AddDistributedEvent(new KnowledgeAssetUpdatedEto
            {
                Id = Id,
                Title = Title,
                AssetType = AssetType,
                TenantId = TenantId,
                LastModificationTime = LastModificationTime ?? DateTime.UtcNow,
                LastModifierId = LastModifierId
            });
        }

        public void AddContent(string content, string contentType, string changeLog = null)
        {
            var nextVersion = (_contentVersions.Any() ? _contentVersions.Max(c => c.Version) : 0) + 1;
            
            var assetContent = new AssetContent(
                Id,
                content,
                contentType,
                nextVersion,
                changeLog);
            
            _contentVersions.Add(assetContent);
        }

        public void UpdateCurrentContent(string content, string changeLog = null)
        {
            var currentContent = CurrentContent;
            if (currentContent == null)
            {
                throw new BusinessRuleViolationException("No content exists for this asset");
            }

            currentContent.UpdateContent(content, changeLog);
        }

        public AssetContent GetContentVersion(int version)
        {
            return _contentVersions.FirstOrDefault(c => c.Version == version);
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                KnowledgeInnovationConsts.KnowledgeAsset.TitleMinLength,
                KnowledgeInnovationConsts.KnowledgeAsset.TitleMaxLength);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: KnowledgeInnovationConsts.KnowledgeAsset.DescriptionMaxLength);
        }
    }
}
