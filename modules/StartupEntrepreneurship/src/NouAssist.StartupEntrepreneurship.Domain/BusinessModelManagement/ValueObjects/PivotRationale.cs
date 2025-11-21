using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects
{
    /// <summary>
    /// Rationale and reasoning behind a strategic pivot
    /// </summary>
    public class PivotRationale : ValueObject
    {
        public string Reason { get; private set; }
        public List<string> KeyInsights { get; private set; }
        public List<string> DataPoints { get; private set; }
        public string RiskAssessment { get; private set; }
        public string ExpectedOutcome { get; private set; }
        public Dictionary<string, string> SuccessMetrics { get; private set; }

        protected PivotRationale()
        {
            KeyInsights = new List<string>();
            DataPoints = new List<string>();
            SuccessMetrics = new Dictionary<string, string>();
        }

        public PivotRationale(
            string reason,
            string expectedOutcome,
            List<string> keyInsights = null,
            List<string> dataPoints = null,
            string riskAssessment = null,
            Dictionary<string, string> successMetrics = null)
        {
            SetReason(reason);
            SetExpectedOutcome(expectedOutcome);
            KeyInsights = keyInsights ?? new List<string>();
            DataPoints = dataPoints ?? new List<string>();
            RiskAssessment = riskAssessment;
            SuccessMetrics = successMetrics ?? new Dictionary<string, string>();
        }

        private void SetReason(string reason)
        {
            Reason = Check.NotNullOrWhiteSpace(
                reason,
                nameof(reason),
                maxLength: BusinessModelManagementConsts.StrategicPivot.MaxReasonLength
            );
        }

        private void SetExpectedOutcome(string expectedOutcome)
        {
            ExpectedOutcome = Check.NotNullOrWhiteSpace(
                expectedOutcome,
                nameof(expectedOutcome),
                maxLength: BusinessModelManagementConsts.StrategicPivot.MaxExpectedOutcomeLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Reason;
            yield return ExpectedOutcome;
            yield return RiskAssessment;
            foreach (var insight in KeyInsights.OrderBy(i => i))
            {
                yield return insight;
            }
            foreach (var dataPoint in DataPoints.OrderBy(d => d))
            {
                yield return dataPoint;
            }
            foreach (var metric in SuccessMetrics.OrderBy(kvp => kvp.Key))
            {
                yield return metric.Key;
                yield return metric.Value;
            }
        }
    }
}
