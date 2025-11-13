using System;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AccountsReceivable.Dtos
{
    public class CustomerDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public CustomerType Type { get; set; }
        public MonetaryValue CurrentBalance { get; set; }
        public bool IsActive { get; set; }
        public bool IsCreditSuspended { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal AvailableCredit { get; set; }
    }

    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TaxId { get; set; }
        public CustomerType Type { get; set; }
        public int CreditTermsDueDays { get; set; }
        public decimal CreditLimit { get; set; }
        public string Currency { get; set; }
    }

    public class UpdateCustomerDto
    {
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class CustomerPaymentDto : FullAuditedEntityDto<Guid>
    {
        public Guid CustomerId { get; set; }
        public MonetaryValue Amount { get; set; }
        public MonetaryValue UnappliedAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Reference { get; set; }
        public bool IsFullyApplied { get; set; }
    }

    public class RecordPaymentDto
    {
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Reference { get; set; }
        public List<PaymentApplicationDto> Applications { get; set; }
    }

    public class PaymentApplicationDto
    {
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}
