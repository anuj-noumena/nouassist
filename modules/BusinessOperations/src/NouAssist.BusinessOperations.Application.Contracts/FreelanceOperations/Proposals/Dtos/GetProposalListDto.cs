using System;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Proposals.Dtos
{
    public class GetProposalListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? ProjectId { get; set; }
        public ProposalStatus? Status { get; set; }
        public DateTime? SentDateMin { get; set; }
        public DateTime? SentDateMax { get; set; }
        public bool? IsExpired { get; set; }
    }
}
