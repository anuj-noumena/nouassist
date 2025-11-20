using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;

namespace NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos
{
    /// <summary>
    /// Input for creating a new defect report
    /// </summary>
    public class CreateDefectDto
    {
        /// <summary>
        /// The title of the defect
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxDefectTitleLength)]
        public string Title { get; set; }
        
        /// <summary>
        /// The detailed description of the defect
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxDefectDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Steps to reproduce the defect
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxStepsToReproduceLength)]
        public string StepsToReproduce { get; set; }
        
        /// <summary>
        /// The expected behavior or outcome
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxExpectedOutcomeLength)]
        public string ExpectedOutcome { get; set; }
        
        /// <summary>
        /// The actual behavior or outcome
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxActualOutcomeLength)]
        public string ActualOutcome { get; set; }
        
        /// <summary>
        /// The severity of the defect
        /// </summary>
        [Required]
        public DefectSeverity Severity { get; set; }
        
        /// <summary>
        /// The priority of the defect
        /// </summary>
        [Required]
        public DefectPriority Priority { get; set; }
        
        /// <summary>
        /// The test run that discovered this defect
        /// </summary>
        public Guid? TestRunId { get; set; }
        
        /// <summary>
        /// The test cycle associated with this defect
        /// </summary>
        public Guid? TestCycleId { get; set; }
        
        /// <summary>
        /// The test plan associated with this defect
        /// </summary>
        public Guid? TestPlanId { get; set; }
        
        /// <summary>
        /// Environment where the defect was found
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Environment { get; set; }
        
        /// <summary>
        /// Version or build where the defect was found
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Version { get; set; }
    }
}
