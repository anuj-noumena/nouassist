using AutoMapper;
using NouAssist.ReflectionAndSupport.CustomFields.Aggregates;
using NouAssist.ReflectionAndSupport.CustomFields.Definitions.Dtos;
using NouAssist.ReflectionAndSupport.CustomFields.Entities;
using NouAssist.ReflectionAndSupport.CustomFields.ValueObjects;
using NouAssist.ReflectionAndSupport.CustomFields.Values.Dtos;

namespace NouAssist.ReflectionAndSupport.CustomFields
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
