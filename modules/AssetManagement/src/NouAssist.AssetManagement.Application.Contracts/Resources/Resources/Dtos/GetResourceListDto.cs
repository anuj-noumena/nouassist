using System;
using NouAssist.AssetManagement.Resources.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.AssetManagement.Resources.Resources.Dtos
{
    public class GetResourceListDto : PagedAndSortedResultRequestDto
    {
        public ResourceKind? Kind { get; set; }
        public string Visibility { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SearchTerm { get; set; }
    }
}
