using System;
using System.Collections.Generic;
using NouAssist.DebtManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.DebtManagement.DebtAccounts.Dtos
{
    public class DebtAccountDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string DebtName { get; set; }
        public DebtType Type { get; set; }
        public string AccountNumber { get; set; }
        public string CreditorName { get; set; }
        
        public decimal OriginalAmount { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MinimumPayment { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public PaymentFrequency PaymentFrequency { get; set; }
        public DateTime? NextPaymentDueDate { get; set; }
        
        public DebtStatus Status { get; set; }
        public DateTime? PaidOffDate { get; set; }
        
        public decimal TotalPaid { get; set; }
        public decimal TotalInterestPaid { get; set; }
        
        public List<DebtPaymentDto> Payments { get; set; }
    }

    public class DebtPaymentDto
    {
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal RemainingBalance { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionReference { get; set; }
        public string Notes { get; set; }
    }

    public class CreateDebtAccountDto
    {
        public string DebtName { get; set; }
        public DebtType Type { get; set; }
        public string AccountNumber { get; set; }
        public string CreditorName { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MinimumPayment { get; set; }
        public PaymentFrequency PaymentFrequency { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? MaturityDate { get; set; }
    }

    public class UpdateDebtAccountDto
    {
        public string DebtName { get; set; }
        public string CreditorName { get; set; }
        public decimal? MinimumPayment { get; set; }
    }

    public class RecordDebtPaymentDto
    {
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string TransactionReference { get; set; }
    }

    public class GetDebtAccountListDto : PagedAndSortedResultRequestDto
    {
        public DebtType? Type { get; set; }
        public DebtStatus? Status { get; set; }
    }

    public class PayoffProjectionDto
    {
        public DateTime ProjectedPayoffDate { get; set; }
        public int MonthsToPayoff { get; set; }
        public decimal TotalInterestToPay { get; set; }
        public decimal TotalAmountToPay { get; set; }
        public decimal MonthlyPayment { get; set; }
    }
}
