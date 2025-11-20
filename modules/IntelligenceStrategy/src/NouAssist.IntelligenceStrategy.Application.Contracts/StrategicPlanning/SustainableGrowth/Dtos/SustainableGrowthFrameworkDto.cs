using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.SustainableGrowth.Dtos
{
    /// <summary>
    /// DTO for Sustainable Growth Framework output
    /// </summary>
    public class SustainableGrowthFrameworkDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FrameworkStartDate { get; set; }
        public DateTime FrameworkEndDate { get; set; }
        public bool IsActive { get; set; }
        public decimal OverallEsgScore { get; set; }
        public List<EsgGoalRecordDto> EsgGoals { get; set; }

        public SustainableGrowthFrameworkDto()
        {
            EsgGoals = new List<EsgGoalRecordDto>();
        }
    }
}
