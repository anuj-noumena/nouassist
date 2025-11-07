using AutoMapper;
using NouAssist.CollaborationHub.ContactManagement.Aggregates;
using NouAssist.CollaborationHub.ContactManagement.Dtos;
using NouAssist.CollaborationHub.ContactManagement.ValueObjects;

namespace NouAssist.CollaborationHub.ContactManagement
{
    public class ContactsApplicationAutoMapperProfile : Profile
    {
        public ContactsApplicationAutoMapperProfile()
        {
            // Contact mappings
            CreateMap<Contact, ContactDto>()
                .ForMember(dest => dest.HasEmail, opt => opt.MapFrom(src => src.HasEmail))
                .ForMember(dest => dest.HasPhone, opt => opt.MapFrom(src => src.HasPhone))
                .ForMember(dest => dest.NeedsFollowup, opt => opt.MapFrom(src => src.NeedsFollowup))
                .ForMember(dest => dest.ClientName, opt => opt.Ignore());

            CreateMap<ContactMethod, ContactMethodDto>();

            CreateMap<SocialProfile, SocialProfileDto>();

            // Client mappings
            CreateMap<Client, ClientDto>()
                .ForMember(dest => dest.NeedsReview, opt => opt.MapFrom(src => src.NeedsReview))
                .ForMember(dest => dest.PrimaryContactName, opt => opt.Ignore());
        }
    }
}
