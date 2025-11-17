using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.TaxManagement.Entities;
using NouAssist.FinancialManagement.TaxManagement.ValueObjects;
using NouAssist.TaxManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.TaxManagement.Aggregates
{
    public class TaxReturn : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public int TaxYear { get; private set; }
        public TaxFilingStatus FilingStatus { get; private set; }
        public TaxReturnStatus Status { get; private set; }

        // Income
        public decimal W2Income { get; private set; }
        public decimal SelfEmploymentIncome { get; private set; }
        public decimal InvestmentIncome { get; private set; }
        public decimal OtherIncome { get; private set; }
        public decimal GrossIncome => W2Income + SelfEmploymentIncome + InvestmentIncome + OtherIncome;

        // Deductions
        public decimal StandardDeduction { get; private set; }
        public decimal TotalItemizedDeductions { get; private set; }
        public bool UseStandardDeduction { get; private set; }
        public decimal AdjustedGrossIncome { get; private set; }
        public decimal TaxableIncome { get; private set; }

        // Tax Liability
        public decimal TotalTaxLiability { get; private set; }
        public decimal WithholdingsPaid { get; private set; }
        public decimal EstimatedTaxPayments { get; private set; }
        public decimal RefundAmount { get; private set; }
        public decimal AmountOwed { get; private set; }

        // Filing Info
        public DateTime? FiledDate { get; private set; }
        public string? ConfirmationNumber { get; private set; }

        private readonly List<TaxDeduction> _deductions = new();
        public IReadOnlyCollection<TaxDeduction> Deductions => _deductions.AsReadOnly();

        private readonly List<TaxDocument> _documents = new();
        public IReadOnlyCollection<TaxDocument> Documents => _documents.AsReadOnly();

        private readonly List<EstimatedTaxPayment> _estimatedPayments = new();
        public IReadOnlyCollection<EstimatedTaxPayment> EstimatedPayments => _estimatedPayments.AsReadOnly();

        protected TaxReturn() { /* ORM */ }

        public TaxReturn(
            Guid id,
            Guid userId,
            Guid? tenantId,
            int taxYear,
            TaxFilingStatus filingStatus)
            : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            TaxYear = Check.Range(taxYear, nameof(taxYear), 2000, 2100);
            FilingStatus = filingStatus;
            Status = TaxReturnStatus.Draft;
            UseStandardDeduction = true;
        }

        public void UpdateIncome(
            decimal w2Income,
            decimal selfEmploymentIncome,
            decimal investmentIncome,
            decimal otherIncome)
        {
            if (Status != TaxReturnStatus.Draft)
            {
                throw new BusinessException("TaxManagement:CannotModifyFiledReturn");
            }

            W2Income = Check.NotNegative(w2Income, nameof(w2Income));
            SelfEmploymentIncome = Check.NotNegative(selfEmploymentIncome, nameof(selfEmploymentIncome));
            InvestmentIncome = Check.NotNegative(investmentIncome, nameof(investmentIncome));
            OtherIncome = Check.NotNegative(otherIncome, nameof(otherIncome));
        }

        public void AddDeduction(
            DeductionCategory category,
            string description,
            decimal claimedAmount,
            decimal allowedAmount,
            Guid? documentId = null)
        {
            if (Status != TaxReturnStatus.Draft)
            {
                throw new BusinessException("TaxManagement:CannotModifyFiledReturn");
            }

            var deduction = new TaxDeduction(
                Id,
                category,
                description,
                claimedAmount,
                allowedAmount,
                documentId
            );

            _deductions.Add(deduction);
            TotalItemizedDeductions = _deductions.Sum(d => d.AllowedAmount);
        }

        public void AttachDocument(
            TaxFormType formType,
            string documentName,
            Guid fileId)
        {
            var document = new TaxDocument(
                Guid.NewGuid(),
                Id,
                formType,
                documentName,
                fileId
            );

            _documents.Add(document);
        }

        public void CalculateTax(
            TaxCalculation calculation,
            decimal standardDeduction)
        {
            if (Status == TaxReturnStatus.Filed)
            {
                throw new BusinessException("TaxManagement:CannotRecalculateFiledReturn");
            }

            StandardDeduction = standardDeduction;
            var itemizedTotal = _deductions.Sum(d => d.AllowedAmount);

            UseStandardDeduction = standardDeduction > itemizedTotal;
            var deductionAmount = UseStandardDeduction ? standardDeduction : itemizedTotal;

            AdjustedGrossIncome = calculation.AdjustedGrossIncome;
            TaxableIncome = Math.Max(0, AdjustedGrossIncome - deductionAmount);
            TotalTaxLiability = calculation.TotalTaxLiability;

            CalculateRefundOrOwed();
        }

        public void RecordWithholdings(decimal amount)
        {
            WithholdingsPaid = Check.NotNegative(amount, nameof(amount));
            CalculateRefundOrOwed();
        }

        public void AddEstimatedPayment(
            int quarter,
            DateTime dueDate,
            decimal amount)
        {
            var payment = new EstimatedTaxPayment(
                Guid.NewGuid(),
                Id,
                quarter,
                dueDate,
                amount
            );

            _estimatedPayments.Add(payment);
            EstimatedTaxPayments = _estimatedPayments.Where(p => p.IsPaid).Sum(p => p.Amount);
            CalculateRefundOrOwed();
        }

        private void CalculateRefundOrOwed()
        {
            var totalPayments = WithholdingsPaid + EstimatedTaxPayments;
            var difference = totalPayments - TotalTaxLiability;

            if (difference > 0)
            {
                RefundAmount = difference;
                AmountOwed = 0;
            }
            else
            {
                RefundAmount = 0;
                AmountOwed = Math.Abs(difference);
            }
        }

        public void MarkAsReadyToFile()
        {
            if (Status != TaxReturnStatus.Draft)
            {
                throw new BusinessException("TaxManagement:ReturnNotInDraftStatus");
            }

            if (!ValidateReadyToFile())
            {
                throw new BusinessException("TaxManagement:ReturnNotReadyToFile");
            }

            Status = TaxReturnStatus.ReadyToFile;
        }

        public void File(string confirmationNumber)
        {
            if (Status != TaxReturnStatus.ReadyToFile)
            {
                throw new BusinessException("TaxManagement:ReturnNotReadyToFile");
            }

            Status = TaxReturnStatus.Filed;
            FiledDate = Clock.Now;
            ConfirmationNumber = Check.NotNullOrWhiteSpace(confirmationNumber, nameof(confirmationNumber));
        }

        private bool ValidateReadyToFile()
        {
            // Must have income
            if (GrossIncome <= 0) return false;

            // Must have required documents for income types
            if (W2Income > 0 && !_documents.Any(d => d.FormType == TaxFormType.FormW2 && d.Status == TaxDocumentStatus.Verified))
                return false;

            // All uploaded documents must be verified
            if (_documents.Any(d => d.Status == TaxDocumentStatus.Uploaded || d.Status == TaxDocumentStatus.Rejected))
                return false;

            return true;
        }

        public decimal GetEffectiveTaxRate()
        {
            if (GrossIncome == 0) return 0;
            return (TotalTaxLiability / GrossIncome) * 100;
        }
    }
}
