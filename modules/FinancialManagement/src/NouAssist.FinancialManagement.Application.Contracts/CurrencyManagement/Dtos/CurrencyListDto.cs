using Volo.Abp.Application.Dtos;

namespace NouAssist.CurrencyManagement.Dtos
{
    public class GetCurrencyListDto : PagedAndSortedResultRequestDto
    {
        public CurrencyStatus? Status { get; set; }
        public string Filter { get; set; }
    }
}
