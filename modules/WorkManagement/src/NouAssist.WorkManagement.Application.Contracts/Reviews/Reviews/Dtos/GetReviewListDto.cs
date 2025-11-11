using System;
using NouAssist.WorkManagement.Reviews.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.Reviews.Reviews.Dtos
{
    public class GetReviewListDto : PagedAndSortedResultRequestDto
    {
        public ReviewType? ReviewType { get; set; }
        public bool? IsComplete { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
