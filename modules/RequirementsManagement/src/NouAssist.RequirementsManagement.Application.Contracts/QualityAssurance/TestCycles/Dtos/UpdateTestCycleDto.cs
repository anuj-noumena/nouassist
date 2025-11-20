using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestCycles.Dtos
{
    /// <summary>
    /// Input for updating a test cycle
    /// </summary>
    public class UpdateTestCycleDto
    {
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
