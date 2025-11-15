using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.CreditScoreMonitoring.Entities;
using NouAssist.CreditScoreMonitoring.Enums;
using NouAssist.CreditScoreMonitoring.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CreditScoreMonitoring.Aggregates
{
    public class CreditReport : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public CreditBureau Bureau { get; private set; }
        public DateTime ReportDate { get; private set; }
        public int CreditScore { get; private set; }
        public CreditScoreRange ScoreRange { get; private set; }
        public int? PreviousScore { get; private set; }
        public int? ScoreChange { get; private set; }

        // Credit Profile Metrics
        public decimal TotalCreditLimit { get; private set; }
        public decimal TotalBalances { get; private set; }
        public decimal CreditUtilizationPercentage { get; private set; }
        public int TotalAccounts { get; private set; }
        public int OpenAccounts { get; private set; }
        public int ClosedAccounts { get; private set; }
        public int DelinquentAccounts { get; private set; }
        public int CollectionAccounts { get; private set; }
        public int PublicRecords { get; private set; }

        // Credit History
        public DateTime? OldestAccountDate { get; private set; }
        public int AverageAccountAgeMonths { get; private set; }
        public int TotalInquiriesLast12Months { get; private set; }
        public int HardInquiriesLast12Months { get; private set; }

        private readonly List<CreditAccount> _accounts = new();
        public IReadOnlyCollection<CreditAccount> Accounts => _accounts.AsReadOnly();

        private readonly List<CreditInquiry> _inquiries = new();
        public IReadOnlyCollection<CreditInquiry> Inquiries => _inquiries.AsReadOnly();

        private readonly List<CreditAlert> _alerts = new();
        public IReadOnlyCollection<CreditAlert> Alerts => _alerts.AsReadOnly();

        protected CreditReport() { /* ORM */ }

        public CreditReport(
            Guid id,
            Guid userId,
            Guid? tenantId,
            CreditBureau bureau,
            DateTime reportDate,
            int creditScore,
            int? previousScore = null)
            : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            Bureau = bureau;
            ReportDate = reportDate;
            UpdateScore(creditScore, previousScore);
        }

        public void UpdateScore(int newScore, int? previousScore = null)
        {
            CreditScore = Check.Range(newScore, nameof(newScore), 300, 850);
            PreviousScore = previousScore;
            
            if (previousScore.HasValue)
            {
                ScoreChange = newScore - previousScore.Value;
            }

            ScoreRange = DetermineScoreRange(newScore);
        }

        public void AddAccount(
            string accountNumber,
            string creditorName,
            string accountType,
            AccountStatus status,
            decimal creditLimit,
            decimal currentBalance,
            DateTime openedDate)
        {
            if (_accounts.Any(a => a.AccountNumber == accountNumber))
            {
                throw new BusinessException("CreditScoreMonitoring:DuplicateAccount");
            }

            var account = new CreditAccount(
                Id,
                accountNumber,
                creditorName,
                accountType,
                status,
                creditLimit,
                currentBalance,
                openedDate
            );

            _accounts.Add(account);
            RecalculateMetrics();
        }

        public void AddInquiry(
            InquiryType type,
            string inquirerName,
            DateTime inquiryDate,
            string? purpose = null)
        {
            var inquiry = new CreditInquiry(
                Guid.NewGuid(),
                Id,
                type,
                inquirerName,
                inquiryDate,
                purpose
            );

            _inquiries.Add(inquiry);
            RecalculateInquiryMetrics();

            // Create alert for hard inquiries
            if (type == InquiryType.Hard)
            {
                CreateAlert(
                    AlertType.NewInquiry,
                    $"New hard inquiry from {inquirerName}",
                    "Medium"
                );
            }
        }

        public void CreateAlert(
            AlertType type,
            string message,
            string severity)
        {
            var alert = new CreditAlert(
                Guid.NewGuid(),
                Id,
                type,
                message,
                severity
            );

            _alerts.Add(alert);
        }

        public void RecalculateMetrics()
        {
            TotalAccounts = _accounts.Count;
            OpenAccounts = _accounts.Count(a => a.Status == AccountStatus.Open);
            ClosedAccounts = _accounts.Count(a => a.Status == AccountStatus.Closed);
            DelinquentAccounts = _accounts.Count(a => a.HasDelinquentPayments());
            CollectionAccounts = _accounts.Count(a => a.Status == AccountStatus.InCollections);

            TotalCreditLimit = _accounts.Sum(a => a.CreditLimit);
            TotalBalances = _accounts.Sum(a => a.CurrentBalance);
            CreditUtilizationPercentage = TotalCreditLimit > 0 
                ? (TotalBalances / TotalCreditLimit) * 100 
                : 0;

            if (_accounts.Any())
            {
                OldestAccountDate = _accounts.Min(a => a.OpenedDate);
                var totalMonths = _accounts.Sum(a => 
                    (int)((DateTime.UtcNow - a.OpenedDate).TotalDays / 30));
                AverageAccountAgeMonths = totalMonths / _accounts.Count;
            }
        }

        private void RecalculateInquiryMetrics()
        {
            var last12Months = DateTime.UtcNow.AddMonths(-12);
            TotalInquiriesLast12Months = _inquiries.Count(i => i.InquiryDate >= last12Months);
            HardInquiriesLast12Months = _inquiries.Count(i => 
                i.Type == InquiryType.Hard && i.InquiryDate >= last12Months);
        }

        private CreditScoreRange DetermineScoreRange(int score)
        {
            return score switch
            {
                >= 800 => CreditScoreRange.Excellent,
                >= 740 => CreditScoreRange.VeryGood,
                >= 670 => CreditScoreRange.Good,
                >= 580 => CreditScoreRange.Fair,
                _ => CreditScoreRange.Poor
            };
        }

        public CreditUtilization GetCreditUtilization()
        {
            return new CreditUtilization(TotalCreditLimit, TotalBalances);
        }

        public PaymentHistory GetPaymentHistory()
        {
            var accountsWithLatePayments = _accounts.Count(a => a.HasDelinquentPayments());
            var maxDaysPastDue = _accounts.Any(a => a.HasDelinquentPayments())
                ? _accounts.Max(a => Math.Max(a.PaymentHistory30Days, Math.Max(a.PaymentHistory60Days, a.PaymentHistory90Days)))
                : 0;

            DateTime? mostRecentLate = null;
            // Would need additional data to determine this accurately

            return new PaymentHistory(
                TotalAccounts,
                accountsWithLatePayments,
                maxDaysPastDue,
                mostRecentLate
            );
        }

        public List<string> GetCreditImprovementRecommendations()
        {
            var recommendations = new List<string>();

            // High utilization
            if (CreditUtilizationPercentage > 30)
            {
                recommendations.Add($"Reduce credit utilization from {CreditUtilizationPercentage:F1}% to below 30%");
            }

            // Delinquent accounts
            if (DelinquentAccounts > 0)
            {
                recommendations.Add($"Bring {DelinquentAccounts} delinquent account(s) current");
            }

            // Too many inquiries
            if (HardInquiriesLast12Months > 3)
            {
                recommendations.Add($"Limit hard inquiries (currently {HardInquiriesLast12Months} in last 12 months)");
            }

            // Collections
            if (CollectionAccounts > 0)
            {
                recommendations.Add($"Address {CollectionAccounts} collection account(s)");
            }

            // Short credit history
            if (AverageAccountAgeMonths < 24)
            {
                recommendations.Add("Build credit history by keeping accounts open longer");
            }

            return recommendations;
        }
    }
}
