using System;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AccountsReceivable.Dtos
{
    public class CustomerDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BillingAddress { get; set; }
        public CustomerType Type { get; set; }
        public int CreditTermsDays { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
        public bool CreditOnHold { get; set; }
    }

    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string Email { get; set; }
        public CustomerType Type { get; set; }
        public int CreditTermsDays { get; set; } = 30;
    }

    public class UpdateCustomerDto
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BillingAddress { get; set; }
        public int CreditTermsDays { get; set; }
    }

    public class GetCustomerListDto : PagedAndSortedResultRequestDto
    {
        public string SearchTerm { get; set; }
        public CustomerType? TypeFilter { get; set; }
        public bool? ActiveOnly { get; set; } = true;
    }
}
