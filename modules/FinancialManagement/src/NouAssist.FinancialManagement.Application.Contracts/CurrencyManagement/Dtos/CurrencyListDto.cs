using NouAssist.FinancialManagement.CurrencyManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.CurrencyManagement.Dtos
{
    public class GetCurrencyListDto : PagedAndSortedResultRequestDto
    {
        public CurrencyStatus? Status { get; set; }
        public string Filter { get; set; }
    }
}
