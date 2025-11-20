using System;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Clients.Dtos
{
    public class GetFreelanceClientListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public ClientStatus? Status { get; set; }
        public string Source { get; set; }
        public bool? HasProjects { get; set; }
        public bool? HasPendingFollowUps { get; set; }
    }
}
