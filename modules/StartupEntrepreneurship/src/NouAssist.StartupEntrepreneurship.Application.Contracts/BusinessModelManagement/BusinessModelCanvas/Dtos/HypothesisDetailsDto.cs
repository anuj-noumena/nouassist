using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class HypothesisDetailsDto
    {
        public string Assumption { get; set; }
        public string TestDescription { get; set; }
        public string TestResult { get; set; }
        public ValidationStatus Status { get; set; }
        public DateTime? TestStartDate { get; set; }
        public DateTime? TestEndDate { get; set; }
    }
}
