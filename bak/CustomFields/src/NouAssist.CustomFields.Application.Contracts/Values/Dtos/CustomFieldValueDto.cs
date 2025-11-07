using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CustomFields.Values.Dtos
{
    public class CustomFieldValueDto : FullAuditedEntityDto<Guid>
    {
        public Guid FieldDefinitionId { get; set; }
        public string FieldName { get; set; }
        public string FieldKey { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public object Value { get; set; }
    }

    public class SetCustomFieldValueDto
    {
        public Guid FieldDefinitionId { get; set; }
        public object Value { get; set; }
    }

    public class EntityCustomFieldValuesDto
    {
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public System.Collections.Generic.List<CustomFieldValueDto> Values { get; set; }
    }

    public class BulkSetCustomFieldValuesDto
    {
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public System.Collections.Generic.Dictionary<Guid, object> FieldValues { get; set; }
    }
}
