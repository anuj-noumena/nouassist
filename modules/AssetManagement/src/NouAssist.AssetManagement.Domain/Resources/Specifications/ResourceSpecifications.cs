using System;
using System.Linq.Expressions;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Resources.Specifications
{
    public class ResourcesByKindSpecification : Specification<Resource>
    {
        private readonly ResourceKind _kind;

        public ResourcesByKindSpecification(ResourceKind kind)
        {
            _kind = kind;
        }

        public override Expression<Func<Resource, bool>> ToExpression() =>
            r => r.Kind == _kind && !r.IsDeleted;
    }

    public class ResourcesByOwnerSpecification : Specification<Resource>
    {
        private readonly Guid _ownerId;

        public ResourcesByOwnerSpecification(Guid ownerId)
        {
            _ownerId = ownerId;
        }

        public override Expression<Func<Resource, bool>> ToExpression() =>
            r => r.OwnerId == _ownerId && !r.IsDeleted;
    }

    public class PublicResourcesSpecification : Specification<Resource>
    {
        public override Expression<Func<Resource, bool>> ToExpression() =>
            r => r.Visibility == "public" && !r.IsDeleted;
    }

    public class ResourcesWithCurrentVersionSpecification : Specification<Resource>
    {
        public override Expression<Func<Resource, bool>> ToExpression() =>
            r => r.CurrentVersionId.HasValue && !r.IsDeleted;
    }

    public class LargeResourcesSpecification : Specification<Resource>
    {
        private readonly long _minSizeBytes;

        public LargeResourcesSpecification(long minSizeBytes)
        {
            _minSizeBytes = minSizeBytes;
        }

        public override Expression<Func<Resource, bool>> ToExpression() =>
            r => r.SizeBytes.HasValue && r.SizeBytes.Value >= _minSizeBytes && !r.IsDeleted;
    }
}
