using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.DebtManagement.Entities;
using NouAssist.FinancialManagement.DebtManagement.Enums;
using NouAssist.FinancialManagement.DebtManagement.ValueObjects;
using NouAssist.SharedKernel.Extension;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.DebtManagement.Aggregates
{
    public class DebtAccount : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string DebtName { get; private set; }
        public DebtType Type { get; private set; }
        public string? AccountNumber { get; private set; }
        public string? CreditorName { get; private set; }
        
        public decimal OriginalAmount { get; private set; }
        public decimal CurrentBalance { get; private set; }
        public decimal InterestRate { get; private set; }
        public decimal MinimumPayment { get; private set; }
        
        public DateTime? StartDate { get; private set; }
        public DateTime? MaturityDate { get; private set; }
        public PaymentFrequency PaymentFrequency { get; private set; }
        public DateTime? NextPaymentDueDate { get; private set; }
        
        public DebtStatus Status { get; private set; }
        public DateTime? PaidOffDate { get; private set; }

        private readonly List<DebtPayment> _payments = new();
        public IReadOnlyCollection<DebtPayment> Payments => _payments.AsReadOnly();

        protected DebtAccount() { /* ORM */ }

        public DebtAccount(
            Guid id,
            Guid userId,
            Guid? tenantId,
            string debtName,
            DebtType type,
            decimal originalAmount,
            decimal currentBalance,
            decimal interestRate,
            decimal minimumPayment,
            PaymentFrequency paymentFrequency)
            : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            DebtName = Check.NotNullOrWhiteSpace(debtName, nameof(debtName));
            Type = type;
            OriginalAmount = Check.Positive(originalAmount, nameof(originalAmount));
            CurrentBalance = Ensure.NotNegative(currentBalance, nameof(currentBalance));
            InterestRate = Ensure.NotNegative(interestRate, nameof(interestRate));
            MinimumPayment = Check.Positive(minimumPayment, nameof(minimumPayment));
            PaymentFrequency = paymentFrequency;
            Status = DebtStatus.Active;
            StartDate = DateTime.UtcNow;
        }

        public void RecordPayment(
            DateTime paymentDate,
            decimal paymentAmount,
            string? transactionReference = null)
        {
            if (Status != DebtStatus.Active)
            {
                throw new BusinessException("DebtManagement:CannotRecordPaymentOnInactiveDebt");
            }

            if (paymentAmount > CurrentBalance)
            {
                throw new BusinessException("DebtManagement:PaymentExceedsBalance");
            }

            var interestCalc = CalculateInterestSplit(paymentAmount);
            
            var payment = new DebtPayment(
                Id,
                paymentDate,
                paymentAmount,
                interestCalc.PrincipalAmount,
                interestCalc.InterestAmount,
                interestCalc.RemainingBalance,
                transactionReference
            );

            _payments.Add(payment);
            CurrentBalance = interestCalc.RemainingBalance;

            if (CurrentBalance <= 0)
            {
                MarkAsPaidOff(paymentDate);
            }
            else
            {
                UpdateNextPaymentDueDate(paymentDate);
            }
        }

        public InterestCalculation CalculateInterestSplit(decimal paymentAmount)
        {
            var dailyRate = InterestRate / 365 / 100;
            var lastPaymentDate = _payments.Any() 
                ? _payments.Max(p => p.PaymentDate) 
                : StartDate ?? DateTime.UtcNow;
            
            var daysSinceLastPayment = (DateTime.UtcNow - lastPaymentDate).Days;
            var interestAmount = CurrentBalance * (decimal)dailyRate * daysSinceLastPayment;
            var principalAmount = paymentAmount - interestAmount;

            if (principalAmount < 0)
            {
                principalAmount = 0;
                interestAmount = paymentAmount;
            }

            var remainingBalance = CurrentBalance - principalAmount;

            return new InterestCalculation(
                principalAmount,
                interestAmount,
                paymentAmount,
                remainingBalance,
                DateTime.UtcNow,
                daysSinceLastPayment
            );
        }

        public PayoffProjection CalculatePayoffProjection(decimal monthlyPayment)
        {
            if (monthlyPayment <= MinimumPayment)
            {
                throw new BusinessException("DebtManagement:PaymentMustExceedMinimum");
            }

            var balance = CurrentBalance;
            var monthlyRate = InterestRate / 12 / 100;
            var months = 0;
            var totalInterest = 0m;

            while (balance > 0 && months < 600) // Max 50 years
            {
                var interestCharge = balance * (decimal)monthlyRate;
                var principalPayment = monthlyPayment - interestCharge;

                if (principalPayment <= 0)
                {
                    throw new BusinessException("DebtManagement:PaymentTooSmallToCoverInterest");
                }

                totalInterest += interestCharge;
                balance -= principalPayment;
                months++;

                if (balance < 0)
                {
                    balance = 0;
                }
            }

            var projectedPayoffDate = DateTime.UtcNow.AddMonths(months);
            var totalToPay = CurrentBalance + totalInterest;

            return new PayoffProjection(
                projectedPayoffDate,
                months,
                totalInterest,
                totalToPay,
                monthlyPayment
            );
        }

        private void MarkAsPaidOff(DateTime paidOffDate)
        {
            Status = DebtStatus.PaidOff;
            PaidOffDate = paidOffDate;
            CurrentBalance = 0;
            NextPaymentDueDate = null;
        }

        private void UpdateNextPaymentDueDate(DateTime lastPaymentDate)
        {
            NextPaymentDueDate = PaymentFrequency switch
            {
                PaymentFrequency.Weekly => lastPaymentDate.AddDays(7),
                PaymentFrequency.BiWeekly => lastPaymentDate.AddDays(14),
                PaymentFrequency.Monthly => lastPaymentDate.AddMonths(1),
                PaymentFrequency.Quarterly => lastPaymentDate.AddMonths(3),
                PaymentFrequency.Annually => lastPaymentDate.AddYears(1),
                _ => lastPaymentDate.AddMonths(1)
            };
        }

        public void UpdateDebtDetails(
            string? debtName = null,
            string? creditorName = null,
            decimal? minimumPayment = null)
        {
            if (!string.IsNullOrWhiteSpace(debtName))
            {
                DebtName = debtName;
            }

            if (!string.IsNullOrWhiteSpace(creditorName))
            {
                CreditorName = creditorName;
            }

            if (minimumPayment.HasValue)
            {
                MinimumPayment = Check.Positive(minimumPayment.Value, nameof(minimumPayment));
            }
        }

        public decimal GetTotalPaid() => _payments.Sum(p => p.PaymentAmount);
        public decimal GetTotalInterestPaid() => _payments.Sum(p => p.InterestAmount);
    }
}
