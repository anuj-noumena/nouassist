using System;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AccountsReceivable.Dtos
{
    public class CustomerPaymentDto : FullAuditedEntityDto<Guid>
    {
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Reference { get; set; }
        public PaymentSource PaymentSource { get; set; }
    }

    public class CreateCustomerPaymentDto
    {
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Reference { get; set; }
        public PaymentSource PaymentSource { get; set; }
    }

    public class GetCustomerPaymentListDto : PagedAndSortedResultRequestDto
    {
        public Guid? CustomerFilter { get; set; }
        public PaymentSource? SourceFilter { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
