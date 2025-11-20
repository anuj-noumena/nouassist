using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.TestRuns.Dtos;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestCycles.Dtos
{
    /// <summary>
    /// Data transfer object for a test cycle
    /// </summary>
    public class TestCycleDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// The ID of the test plan this cycle belongs to
        /// </summary>
        public Guid TestPlanId { get; set; }
        
        /// <summary>
        /// The name of the test cycle
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the test cycle
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Information about the build being tested
        /// </summary>
        public string BuildInformation { get; set; }
        
        /// <summary>
        /// The current status of the test cycle
        /// </summary>
        public TestCycleStatus Status { get; set; }
        
        /// <summary>
        /// Scheduled start date for the test cycle
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Scheduled end date for the test cycle
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// The user responsible for this test cycle
        /// </summary>
        public Guid AssigneeId { get; set; }
        
        /// <summary>
        /// The actual start time when the cycle was started
        /// </summary>
        public DateTime? ActualStartTime { get; set; }
        
        /// <summary>
        /// The actual completion time when the cycle was completed
        /// </summary>
        public DateTime? ActualEndTime { get; set; }
        
        /// <summary>
        /// Test runs associated with this test cycle (if included)
        /// </summary>
        public List<TestRunDto> TestRuns { get; set; }
    }
}
