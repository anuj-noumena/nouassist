using System;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.GeneralLedger.Dtos
{
    public class LedgerAccountDto : FullAuditedEntityDto<Guid>
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public AccountType Type { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Currency { get; set; }
        public Guid? ParentAccountId { get; set; }
        public string ParentAccountName { get; set; }
        public bool IsActive { get; set; }
        public bool AllowsPosting { get; set; }
        public string Description { get; set; }
    }

    public class CreateLedgerAccountDto
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public AccountType Type { get; set; }
        public string Currency { get; set; } = "USD";
        public Guid? ParentAccountId { get; set; }
        public string Description { get; set; }
    }

    public class UpdateLedgerAccountDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetLedgerAccountListDto : PagedAndSortedResultRequestDto
    {
        public AccountType? TypeFilter { get; set; }
        public bool? ActiveOnly { get; set; } = true;
        public string SearchTerm { get; set; }
    }
}
