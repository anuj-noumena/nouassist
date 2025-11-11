using System;
using NouAssist.IdentityAccess.Contacts.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IdentityAccess.Contacts.Contacts.Dtos
{
    public class GetContactListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public ContactType? ContactType { get; set; }
        public ContactRelationship? Relationship { get; set; }
        public Guid? ClientId { get; set; }
        public bool? IsActive { get; set; }
        public bool? NeedsFollowup { get; set; }
        public string Tag { get; set; }
    }
}
