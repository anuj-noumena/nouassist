using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCoverage.Dtos
{
    /// <summary>
    /// Input DTO for generating test coverage report
    /// </summary>
    public class GenerateCoverageReportDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one requirement must be specified")]
        public List<Guid> RequirementIds { get; set; } = new();

        public string ReportScope { get; set; }
    }

    /// <summary>
    /// Input DTO for identifying test gaps
    /// </summary>
    public class IdentifyTestGapsDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one requirement must be specified")]
        public List<Guid> RequirementIds { get; set; } = new();

        [Range(0.0, 1.0, ErrorMessage = "Coverage threshold must be between 0 and 1")]
        public double MinimumCoverageThreshold { get; set; } = 0.8;
    }
}
