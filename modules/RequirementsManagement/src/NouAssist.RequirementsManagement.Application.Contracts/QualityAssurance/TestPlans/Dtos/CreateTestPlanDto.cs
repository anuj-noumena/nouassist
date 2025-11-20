using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestPlans.Dtos
{
    /// <summary>
    /// Input for creating a new test plan
    /// </summary>
    public class CreateTestPlanDto
    {
        /// <summary>
        /// The name of the test plan
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxTestPlanNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the test plan
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxTestPlanDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// The version or release this test plan is associated with
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ReleaseVersion { get; set; }
        
        /// <summary>
        /// The testing approach (e.g., Exploratory, Regression, Smoke)
        /// </summary>
        [Required]
        [StringLength(200)]
        public string StrategyApproach { get; set; }
        
        /// <summary>
        /// Description of what will be covered in testing
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string StrategyCoverageDescription { get; set; }
        
        /// <summary>
        /// Areas or features that are explicitly out of scope
        /// </summary>
        [StringLength(2000)]
        public string StrategyOutOfScope { get; set; }
        
        /// <summary>
        /// The name of the testing environment
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxEnvironmentConfigurationLength)]
        public string EnvironmentName { get; set; }
        
        /// <summary>
        /// Description of the environment configuration
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxEnvironmentConfigurationLength)]
        public string EnvironmentDescription { get; set; }
        
        /// <summary>
        /// URL of the environment
        /// </summary>
        [StringLength(500)]
        public string EnvironmentUrl { get; set; }
        
        /// <summary>
        /// Scheduled start date for the test plan
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Scheduled end date for the test plan
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
    }
}
