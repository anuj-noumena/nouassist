using System;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.GeneralLedger.Dtos
{
    public class AccountingPeriodDto : FullAuditedEntityDto<Guid>
    {
        public int FiscalYear { get; set; }
        public int PeriodNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PeriodStatus Status { get; set; }
        public DateTime? ClosedDate { get; set; }
        public Guid? ClosedBy { get; set; }
        public string ClosedByName { get; set; }
    }

    public class CreateAccountingPeriodDto
    {
        public int FiscalYear { get; set; }
        public int PeriodNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CloseAccountingPeriodDto
    {
        public Guid PeriodId { get; set; }
    }

    public class GetAccountingPeriodListDto : PagedAndSortedResultRequestDto
    {
        public int? FiscalYearFilter { get; set; }
        public PeriodStatus? StatusFilter { get; set; }
    }
}
