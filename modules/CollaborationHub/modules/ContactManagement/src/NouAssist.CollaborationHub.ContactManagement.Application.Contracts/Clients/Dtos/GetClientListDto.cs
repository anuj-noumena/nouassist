using NouAssist.CollaborationHub.ContactManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CollaborationHub.ContactManagement.Dtos
{
    public class GetClientListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public ClientStatus? Status { get; set; }
        public bool? IsActive { get; set; }
        public bool? NeedsReview { get; set; }
        public string Industry { get; set; }
        public string Tag { get; set; }
    }
}
