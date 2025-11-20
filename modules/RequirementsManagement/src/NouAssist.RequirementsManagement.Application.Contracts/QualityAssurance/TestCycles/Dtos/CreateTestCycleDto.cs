using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestCycles.Dtos
{
    /// <summary>
    /// Input for creating a new test cycle
    /// </summary>
    public class CreateTestCycleDto
    {
        /// <summary>
        /// The ID of the test plan this cycle belongs to
        /// </summary>
        [Required]
        public Guid TestPlanId { get; set; }
        
        /// <summary>
        /// The name of the test cycle
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxTestCycleNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the test cycle
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxTestCycleDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Information about the build being tested
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxBuildInformationLength)]
        public string BuildInformation { get; set; }
        
        /// <summary>
        /// Scheduled start date for the test cycle
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Scheduled end date for the test cycle
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// The user responsible for this test cycle
        /// </summary>
        [Required]
        public Guid AssigneeId { get; set; }
    }
}
