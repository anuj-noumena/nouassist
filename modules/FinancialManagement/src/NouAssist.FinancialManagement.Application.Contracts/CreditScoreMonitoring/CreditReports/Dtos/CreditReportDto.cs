using System;
using System.Collections.Generic;
using NouAssist.CreditScoreMonitoring.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CreditScoreMonitoring.CreditReports.Dtos
{
    public class CreditReportDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public CreditBureau Bureau { get; set; }
        public DateTime ReportDate { get; set; }
        
        public int CreditScore { get; set; }
        public CreditScoreRange ScoreRange { get; set; }
        public int? PreviousScore { get; set; }
        public int? ScoreChange { get; set; }

        public decimal TotalCreditLimit { get; set; }
        public decimal TotalBalances { get; set; }
        public decimal CreditUtilizationPercentage { get; set; }
        
        public int TotalAccounts { get; set; }
        public int OpenAccounts { get; set; }
        public int ClosedAccounts { get; set; }
        public int DelinquentAccounts { get; set; }
        public int CollectionAccounts { get; set; }
        public int PublicRecords { get; set; }

        public DateTime? OldestAccountDate { get; set; }
        public int AverageAccountAgeMonths { get; set; }
        public int TotalInquiriesLast12Months { get; set; }
        public int HardInquiriesLast12Months { get; set; }

        public List<CreditAccountDto> Accounts { get; set; }
        public List<CreditInquiryDto> Inquiries { get; set; }
        public List<CreditAlertDto> Alerts { get; set; }
    }

    public class CreditAccountDto
    {
        public string AccountNumber { get; set; }
        public string CreditorName { get; set; }
        public string AccountType { get; set; }
        public AccountStatus Status { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal UtilizationPercentage { get; set; }
        public DateTime OpenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int PaymentHistory30Days { get; set; }
        public int PaymentHistory60Days { get; set; }
        public int PaymentHistory90Days { get; set; }
        public bool HasDelinquentPayments { get; set; }
    }

    public class CreditInquiryDto
    {
        public Guid Id { get; set; }
        public InquiryType Type { get; set; }
        public string InquirerName { get; set; }
        public DateTime InquiryDate { get; set; }
        public string Purpose { get; set; }
        public bool IsRecent { get; set; }
    }

    public class CreditAlertDto
    {
        public Guid Id { get; set; }
        public AlertType Type { get; set; }
        public string Message { get; set; }
        public string Severity { get; set; }
        public bool IsRead { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class ImportCreditReportDto
    {
        public CreditBureau Bureau { get; set; }
        public DateTime ReportDate { get; set; }
        public int CreditScore { get; set; }
        public int? PreviousScore { get; set; }
        public List<ImportCreditAccountDto> Accounts { get; set; }
        public List<ImportCreditInquiryDto> Inquiries { get; set; }
    }

    public class ImportCreditAccountDto
    {
        public string AccountNumber { get; set; }
        public string CreditorName { get; set; }
        public string AccountType { get; set; }
        public AccountStatus Status { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime OpenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int PaymentHistory30Days { get; set; }
        public int PaymentHistory60Days { get; set; }
        public int PaymentHistory90Days { get; set; }
    }

    public class ImportCreditInquiryDto
    {
        public InquiryType Type { get; set; }
        public string InquirerName { get; set; }
        public DateTime InquiryDate { get; set; }
        public string Purpose { get; set; }
    }

    public class GetCreditReportListDto : PagedAndSortedResultRequestDto
    {
        public CreditBureau? Bureau { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class CreditScoreHistoryDto
    {
        public CreditBureau Bureau { get; set; }
        public List<CreditScorePointDto> ScoreHistory { get; set; }
    }

    public class CreditScorePointDto
    {
        public DateTime Date { get; set; }
        public int Score { get; set; }
        public int? Change { get; set; }
    }

    public class CreditHealthReportDto
    {
        public int CreditScore { get; set; }
        public CreditScoreRange ScoreRange { get; set; }
        public int OverallHealthScore { get; set; }
        public string HealthLevel { get; set; }
        public List<CreditHealthFactorDto> Factors { get; set; }
        public List<string> Recommendations { get; set; }
    }

    public class CreditHealthFactorDto
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public string Impact { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
    }

    public class CreditImprovementPlanDto
    {
        public decimal? ReduceUtilizationTo { get; set; }
        public bool PayOffDelinquentAccounts { get; set; }
        public int MonthsToWait { get; set; }
    }

    public class ScoreProjectionDto
    {
        public int CurrentScore { get; set; }
        public int ProjectedScore { get; set; }
        public int ProjectedGain { get; set; }
        public int TimelineMonths { get; set; }
        public DateTime ProjectedAchievementDate { get; set; }
    }
}
