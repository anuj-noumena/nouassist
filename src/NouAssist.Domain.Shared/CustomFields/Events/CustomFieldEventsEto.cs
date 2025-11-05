using System;
using NouAssist.CustomFields.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CustomFields.Events
{
    [EventName("NouAssist.CustomFields.DefinitionCreated")]
    public class CustomFieldDefinitionCreatedEto : IMultiTenant
    {
        public Guid FieldDefinitionId { get; set; }
        public string Name { get; set; }
        public string FieldKey { get; set; }
        public CustomFieldType FieldType { get; set; }
        public CustomFieldEntityType EntityType { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    [EventName("NouAssist.CustomFields.DefinitionUpdated")]
    public class CustomFieldDefinitionUpdatedEto : IMultiTenant
    {
        public Guid FieldDefinitionId { get; set; }
        public string Name { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    [EventName("NouAssist.CustomFields.DefinitionDeleted")]
    public class CustomFieldDefinitionDeletedEto : IMultiTenant
    {
        public Guid FieldDefinitionId { get; set; }
        public string FieldKey { get; set; }
        public CustomFieldEntityType EntityType { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}

