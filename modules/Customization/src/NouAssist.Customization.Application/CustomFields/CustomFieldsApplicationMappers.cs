using NouAssist.Customization.CustomFields.Aggregates;
using NouAssist.Customization.CustomFields.Definitions.Dtos;
using NouAssist.Customization.CustomFields.Entities;
using NouAssist.Customization.CustomFields.ValueObjects;
using NouAssist.Customization.CustomFields.Values.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.Customization.CustomFields
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CustomFieldDefinitionToCustomFieldDefinitionDtoMapper : MapperBase<CustomFieldDefinition, CustomFieldDefinitionDto>
    {
        public override partial CustomFieldDefinitionDto Map(CustomFieldDefinition source);

        public override partial void Map(CustomFieldDefinition source, CustomFieldDefinitionDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CustomFieldOptionToCustomFieldOptionDtoMapper : MapperBase<CustomFieldOption, CustomFieldOptionDto>
    {
        public override partial CustomFieldOptionDto Map(CustomFieldOption source);

        public override partial void Map(CustomFieldOption source, CustomFieldOptionDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CustomFieldValueToCustomFieldValueDtoMapper : MapperBase<CustomFieldValue, CustomFieldValueDto>
    {
        [MapperIgnoreTarget(nameof(CustomFieldValueDto.Value))]
        [MapperIgnoreTarget(nameof(CustomFieldValueDto.FieldName))]
        [MapperIgnoreTarget(nameof(CustomFieldValueDto.FieldKey))]
        public override partial CustomFieldValueDto Map(CustomFieldValue source);

        public override partial void Map(CustomFieldValue source, CustomFieldValueDto destination);

        public override void AfterMap(CustomFieldValue source, CustomFieldValueDto destination)
        {
            destination.Value = source.GetValue<object>();
        }
    }
}
