using System;
using Volo.Abp;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.Exceptions
{
    public class CreditScoreMonitoringDomainException : BusinessException
    {
        public CreditScoreMonitoringDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static CreditScoreMonitoringDomainException InvalidCreditScore(int score)
        {
            return new CreditScoreMonitoringDomainException(
                "CreditScoreMonitoring:InvalidScore",
                $"Credit score {score} is out of valid range (300-850).");
        }

        public static CreditScoreMonitoringDomainException DisputeAlreadyExists()
        {
            return new CreditScoreMonitoringDomainException(
                "CreditScoreMonitoring:DisputeExists",
                "A dispute for this item already exists.");
        }

        public static CreditScoreMonitoringDomainException InvalidDisputeStatus()
        {
            return new CreditScoreMonitoringDomainException(
                "CreditScoreMonitoring:InvalidDisputeStatus",
                "Cannot perform this operation in the current dispute status.");
        }
    }
}
