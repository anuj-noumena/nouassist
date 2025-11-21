using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Specifications
{
    /// <summary>
    /// Base specification class for KnowledgeAsset aggregate
    /// </summary>
    public abstract class KnowledgeAssetSpecification : Specification<KnowledgeAsset>
    {
    }

    /// <summary>
    /// Specification for knowledge assets of a specific type
    /// </summary>
    public class KnowledgeAssetTypeSpecification : KnowledgeAssetSpecification
    {
        private readonly KnowledgeAssetType _assetType;

        public KnowledgeAssetTypeSpecification(KnowledgeAssetType assetType)
        {
            _assetType = assetType;
        }

        public override Expression<Func<KnowledgeAsset, bool>> ToExpression()
        {
            return asset => asset.AssetType == _assetType;
        }
    }

    /// <summary>
    /// Specification for knowledge assets with specific tags
    /// </summary>
    public class KnowledgeAssetWithTagSpecification : KnowledgeAssetSpecification
    {
        private readonly string _tag;

        public KnowledgeAssetWithTagSpecification(string tag)
        {
            _tag = tag;
        }

        public override Expression<Func<KnowledgeAsset, bool>> ToExpression()
        {
            return asset => asset.Tags.Contains(_tag);
        }
    }

    /// <summary>
    /// Specification for knowledge assets by author
    /// </summary>
    public class KnowledgeAssetByAuthorSpecification : KnowledgeAssetSpecification
    {
        private readonly Guid _authorUserId;

        public KnowledgeAssetByAuthorSpecification(Guid authorUserId)
        {
            _authorUserId = authorUserId;
        }

        public override Expression<Func<KnowledgeAsset, bool>> ToExpression()
        {
            return asset => asset.Author.UserId == _authorUserId;
        }
    }
}
