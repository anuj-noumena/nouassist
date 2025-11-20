using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.TestCycles.Dtos;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestPlans.Dtos
{
    /// <summary>
    /// Data transfer object for a test plan
    /// </summary>
    public class TestPlanDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// The name of the test plan
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the test plan
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The version or release this test plan is associated with
        /// </summary>
        public string ReleaseVersion { get; set; }
        
        /// <summary>
        /// The current status of the test plan
        /// </summary>
        public TestPlanStatus Status { get; set; }
        
        /// <summary>
        /// The testing approach (e.g., Exploratory, Regression, Smoke)
        /// </summary>
        public string StrategyApproach { get; set; }
        
        /// <summary>
        /// Description of what will be covered in testing
        /// </summary>
        public string StrategyCoverageDescription { get; set; }
        
        /// <summary>
        /// Areas or features that are explicitly out of scope
        /// </summary>
        public string StrategyOutOfScope { get; set; }
        
        /// <summary>
        /// The name of the testing environment
        /// </summary>
        public string EnvironmentName { get; set; }
        
        /// <summary>
        /// Description of the environment configuration
        /// </summary>
        public string EnvironmentDescription { get; set; }
        
        /// <summary>
        /// URL of the environment
        /// </summary>
        public string EnvironmentUrl { get; set; }
        
        /// <summary>
        /// Scheduled start date for the test plan
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Scheduled end date for the test plan
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// IDs of requirements that are in scope for this test plan
        /// </summary>
        public List<Guid> InScopeRequirementIds { get; set; }
        
        /// <summary>
        /// Test cycles associated with this test plan (if included)
        /// </summary>
        public List<TestCycleDto> TestCycles { get; set; }
        
        /// <summary>
        /// Test execution progress summary
        /// </summary>
        public TestProgressDto Progress { get; set; }
    }
}
