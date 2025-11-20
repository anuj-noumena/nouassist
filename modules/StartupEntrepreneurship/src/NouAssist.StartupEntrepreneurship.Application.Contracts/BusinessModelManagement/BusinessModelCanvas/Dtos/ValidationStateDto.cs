using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class ValidationStateDto
    {
        public ValidationStatus Status { get; set; }
        public double ConfidenceLevel { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Notes { get; set; }
    }
}
