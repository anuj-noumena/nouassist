using AutoMapper;
using NouAssist.FinancialManagement.AuditTrail.Aggregates;
using NouAssist.FinancialManagement.AuditTrail.Dtos;

namespace NouAssist.FinancialManagement.AuditTrail
{
    public class AuditTrailApplicationAutoMapperProfile : Profile
    {
        public AuditTrailApplicationAutoMapperProfile()
        {
            CreateMap<AuditEntry, AuditEntryDto>()
                .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => src.EntityReference.EntityType))
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.EntityReference.EntityId))
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.EntityReference.EntityName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserContext.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserContext.UserName))
                .ForMember(dest => dest.IPAddress, opt => opt.MapFrom(src => src.UserContext.IPAddress))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata.Properties));
        }
    }
}
