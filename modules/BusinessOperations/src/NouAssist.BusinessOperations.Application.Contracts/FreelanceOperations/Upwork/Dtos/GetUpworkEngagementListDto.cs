using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Upwork.Dtos
{
    public class GetUpworkEngagementListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string Status { get; set; }
        public bool? IsLinkedToSystem { get; set; }
        public DateTime? StartDateMin { get; set; }
        public DateTime? StartDateMax { get; set; }
    }
}
