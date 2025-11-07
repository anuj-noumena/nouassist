using System;
using System.Linq.Expressions;
using NouAssist.CustomFields.Aggregates;
using NouAssist.CustomFields.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.CustomFields.Specifications
{
    public class ActiveCustomFieldsSpecification : Specification<CustomFieldDefinition>
    {
        public override Expression<Func<CustomFieldDefinition, bool>> ToExpression() =>
            f => f.IsActive;
    }

    public class CustomFieldsByEntityTypeSpecification : Specification<CustomFieldDefinition>
    {
        private readonly CustomFieldEntityType _entityType;

        public CustomFieldsByEntityTypeSpecification(CustomFieldEntityType entityType)
        {
            _entityType = entityType;
        }

        public override Expression<Func<CustomFieldDefinition, bool>> ToExpression() =>
            f => f.EntityType == _entityType && f.IsActive;
    }

    public class RequiredCustomFieldsSpecification : Specification<CustomFieldDefinition>
    {
        public override Expression<Func<CustomFieldDefinition, bool>> ToExpression() =>
            f => f.IsRequired && f.IsActive;
    }

    public class CustomFieldsByOwnerSpecification : Specification<CustomFieldDefinition>
    {
        private readonly Guid _ownerId;

        public CustomFieldsByOwnerSpecification(Guid ownerId)
        {
            _ownerId = ownerId;
        }

        public override Expression<Func<CustomFieldDefinition, bool>> ToExpression() =>
            f => f.OwnerId == _ownerId;
    }

    public class SystemCustomFieldsSpecification : Specification<CustomFieldDefinition>
    {
        public override Expression<Func<CustomFieldDefinition, bool>> ToExpression() =>
            f => f.IsSystem;
    }
}
