using System;
using System.Collections.Generic;
using NouAssist.Customization.CustomFields.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Customization.CustomFields.Definitions.Dtos
{
    public class CustomFieldDefinitionDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public string FieldKey { get; set; }
        public string Description { get; set; }
        public CustomFieldType FieldType { get; set; }
        public CustomFieldEntityType EntityType { get; set; }
        public bool IsRequired { get; set; }
        public string ValidationRegex { get; set; }
        public string ValidationMessage { get; set; }
        public List<CustomFieldOptionDto> Options { get; set; }
        public string DefaultValue { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public int DisplayOrder { get; set; }
        public string Placeholder { get; set; }
        public string HelpText { get; set; }
        public string Icon { get; set; }
        public Guid? ShowIfFieldId { get; set; }
        public string ShowIfValue { get; set; }
        public bool IsSystem { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomFieldOptionDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateCustomFieldDefinitionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public CustomFieldType FieldType { get; set; }
        public CustomFieldEntityType EntityType { get; set; }
        public bool IsRequired { get; set; }
        public List<CreateCustomFieldOptionDto> Options { get; set; }
    }

    public class CreateCustomFieldOptionDto
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public string Color { get; set; }
    }

    public class UpdateCustomFieldDefinitionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public string ValidationRegex { get; set; }
        public string ValidationMessage { get; set; }
        public string DefaultValue { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public int DisplayOrder { get; set; }
        public string Placeholder { get; set; }
        public string HelpText { get; set; }
        public string Icon { get; set; }
    }

    public class GetCustomFieldDefinitionListDto : PagedAndSortedResultRequestDto
    {
        public CustomFieldEntityType? EntityType { get; set; }
        public bool? IsActive { get; set; }
        public string Filter { get; set; }
    }
}
