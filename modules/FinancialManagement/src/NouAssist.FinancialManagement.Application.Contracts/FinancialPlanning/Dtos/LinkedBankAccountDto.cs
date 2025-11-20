using System;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.FinancialPlanning.Dtos
{
    public class LinkedBankAccountDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public BankAccountType AccountType { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Currency { get; set; }
        public DateTime LastSyncDate { get; set; }
        public bool IsActive { get; set; }
        public ConnectionStatus ConnectionStatus { get; set; }
    }

    public class CreateLinkedBankAccountDto
    {
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public BankAccountType AccountType { get; set; }
        public string Currency { get; set; }
    }

    public class UpdateBankAccountBalanceDto
    {
        public Guid AccountId { get; set; }
        public decimal NewBalance { get; set; }
    }

    public class GetLinkedBankAccountListDto : PagedAndSortedResultRequestDto
    {
        public BankAccountType? TypeFilter { get; set; }
        public ConnectionStatus? StatusFilter { get; set; }
        public bool? ActiveOnly { get; set; } = true;
    }
}
