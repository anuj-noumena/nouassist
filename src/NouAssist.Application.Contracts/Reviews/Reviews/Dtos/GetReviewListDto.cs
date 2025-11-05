using System;
using NouAssist.Reviews.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Reviews.Dtos
{
    public class GetReviewListDto : PagedAndSortedResultRequestDto
    {
        public ReviewType? ReviewType { get; set; }
        public bool? IsComplete { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
