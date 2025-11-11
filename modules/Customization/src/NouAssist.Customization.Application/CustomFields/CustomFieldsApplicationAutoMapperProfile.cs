using AutoMapper;
using NouAssist.Customization.CustomFields.Aggregates;
using NouAssist.Customization.CustomFields.Definitions.Dtos;
using NouAssist.Customization.CustomFields.Entities;
using NouAssist.Customization.CustomFields.ValueObjects;
using NouAssist.Customization.CustomFields.Values.Dtos;

namespace NouAssist.Customization.CustomFields
{
    public class CustomFieldsApplicationAutoMapperProfile : Profile
    {
        public CustomFieldsApplicationAutoMapperProfile()
        {
            CreateMap<CustomFieldDefinition, CustomFieldDefinitionDto>();
            CreateMap<CustomFieldOption, CustomFieldOptionDto>();

            CreateMap<CustomFieldValue, CustomFieldValueDto>()
                .ForMember(dest => dest.Value, 
                    opt => opt.MapFrom(src => src.GetValue<object>()))
                .ForMember(dest => dest.FieldName, opt => opt.Ignore())
                .ForMember(dest => dest.FieldKey, opt => opt.Ignore());
        }
    }
}
