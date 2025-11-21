using NouAssist.IdentityAccess.Contacts.Aggregates;
using NouAssist.IdentityAccess.Contacts.Clients.Dtos;
using NouAssist.IdentityAccess.Contacts.Contacts.Dtos;
using NouAssist.IdentityAccess.Contacts.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.IdentityAccess.Contacts
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContactToContactDtoMapper : MapperBase<Contact, ContactDto>
    {
        [MapperIgnoreTarget(nameof(ContactDto.ClientName))]
        public override partial ContactDto Map(Contact source);

        public override partial void Map(Contact source, ContactDto destination);

        public override void AfterMap(Contact source, ContactDto destination)
        {
            destination.HasEmail = source.HasEmail;
            destination.HasPhone = source.HasPhone;
            destination.NeedsFollowup = source.NeedsFollowup;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContactMethodToContactMethodDtoMapper : MapperBase<ContactMethod, ContactMethodDto>
    {
        public override partial ContactMethodDto Map(ContactMethod source);

        public override partial void Map(ContactMethod source, ContactMethodDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SocialProfileToSocialProfileDtoMapper : MapperBase<SocialProfile, SocialProfileDto>
    {
        public override partial SocialProfileDto Map(SocialProfile source);

        public override partial void Map(SocialProfile source, SocialProfileDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ClientToClientDtoMapper : MapperBase<Client, ClientDto>
    {
        [MapperIgnoreTarget(nameof(ClientDto.PrimaryContactName))]
        public override partial ClientDto Map(Client source);

        public override partial void Map(Client source, ClientDto destination);

        public override void AfterMap(Client source, ClientDto destination)
        {
            destination.NeedsReview = source.NeedsReview;
        }
    }
}
