using System;
using System.Collections.Generic;
using NouAssist.TaxManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.TaxManagement.TaxReturns.Dtos
{
    public class TaxReturnDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public int TaxYear { get; set; }
        public TaxFilingStatus FilingStatus { get; set; }
        public TaxReturnStatus Status { get; set; }

        public decimal W2Income { get; set; }
        public decimal SelfEmploymentIncome { get; set; }
        public decimal InvestmentIncome { get; set; }
        public decimal OtherIncome { get; set; }
        public decimal GrossIncome { get; set; }

        public decimal StandardDeduction { get; set; }
        public decimal TotalItemizedDeductions { get; set; }
        public bool UseStandardDeduction { get; set; }
        public decimal AdjustedGrossIncome { get; set; }
        public decimal TaxableIncome { get; set; }

        public decimal TotalTaxLiability { get; set; }
        public decimal WithholdingsPaid { get; set; }
        public decimal EstimatedTaxPayments { get; set; }
        public decimal RefundAmount { get; set; }
        public decimal AmountOwed { get; set; }
        public decimal EffectiveTaxRate { get; set; }

        public DateTime? FiledDate { get; set; }
        public string ConfirmationNumber { get; set; }

        public List<TaxDeductionDto> Deductions { get; set; }
        public List<TaxDocumentDto> Documents { get; set; }
        public List<EstimatedTaxPaymentDto> EstimatedPayments { get; set; }
    }

    public class TaxDeductionDto
    {
        public DeductionCategory Category { get; set; }
        public string Description { get; set; }
        public decimal ClaimedAmount { get; set; }
        public decimal AllowedAmount { get; set; }
        public bool IsVerified { get; set; }
        public string Notes { get; set; }
    }

    public class TaxDocumentDto
    {
        public Guid Id { get; set; }
        public TaxFormType FormType { get; set; }
        public string DocumentName { get; set; }
        public Guid FileId { get; set; }
        public TaxDocumentStatus Status { get; set; }
        public string RejectionReason { get; set; }
    }

    public class EstimatedTaxPaymentDto
    {
        public Guid Id { get; set; }
        public int Quarter { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsPaid { get; set; }
        public string ConfirmationNumber { get; set; }
    }

    public class CreateTaxReturnDto
    {
        public int TaxYear { get; set; }
        public TaxFilingStatus FilingStatus { get; set; }
    }

    public class UpdateTaxReturnIncomeDto
    {
        public decimal W2Income { get; set; }
        public decimal SelfEmploymentIncome { get; set; }
        public decimal InvestmentIncome { get; set; }
        public decimal OtherIncome { get; set; }
    }

    public class AddTaxDeductionDto
    {
        public DeductionCategory Category { get; set; }
        public string Description { get; set; }
        public decimal ClaimedAmount { get; set; }
        public Guid? DocumentId { get; set; }
    }

    public class AttachTaxDocumentDto
    {
        public TaxFormType FormType { get; set; }
        public string DocumentName { get; set; }
        public Guid FileId { get; set; }
    }

    public class RecordWithholdingsDto
    {
        public decimal Amount { get; set; }
    }

    public class AddEstimatedPaymentDto
    {
        public int Quarter { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
    }

    public class FileTaxReturnDto
    {
        public string ConfirmationNumber { get; set; }
    }

    public class TaxCalculationResultDto
    {
        public decimal GrossIncome { get; set; }
        public decimal AdjustedGrossIncome { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal TotalTaxLiability { get; set; }
        public decimal EffectiveTaxRate { get; set; }
        public decimal MarginalTaxRate { get; set; }
        public decimal RefundAmount { get; set; }
        public decimal AmountOwed { get; set; }
    }

    public class GetTaxReturnListDto : PagedAndSortedResultRequestDto
    {
        public TaxReturnStatus? Status { get; set; }
    }
}
