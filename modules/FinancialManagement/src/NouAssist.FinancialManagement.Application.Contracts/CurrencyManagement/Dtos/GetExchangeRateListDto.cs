using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CurrencyManagement.Dtos
{
    public class GetExchangeRateListDto : PagedAndSortedResultRequestDto
    {
        public string BaseCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? ActiveOnly { get; set; } = true;
    }
}
