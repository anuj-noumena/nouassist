using System;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos
{
    public class GetFreelanceProjectListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public Guid? ClientId { get; set; }
        public ProjectStatus? Status { get; set; }
        public BillingType? BillingType { get; set; }
        public DateTime? StartDateMin { get; set; }
        public DateTime? StartDateMax { get; set; }
        public DateTime? EndDateMin { get; set; }
        public DateTime? EndDateMax { get; set; }
    }
}
