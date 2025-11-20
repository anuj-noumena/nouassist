using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos
{
    public class PivotRationaleDto
    {
        public string Reason { get; set; }
        public List<string> KeyInsights { get; set; }
        public List<string> DataPoints { get; set; }
        public string RiskAssessment { get; set; }
        public string ExpectedOutcome { get; set; }
        public Dictionary<string, string> SuccessMetrics { get; set; }

        public PivotRationaleDto()
        {
            KeyInsights = new List<string>();
            DataPoints = new List<string>();
            SuccessMetrics = new Dictionary<string, string>();
        }
    }
}
