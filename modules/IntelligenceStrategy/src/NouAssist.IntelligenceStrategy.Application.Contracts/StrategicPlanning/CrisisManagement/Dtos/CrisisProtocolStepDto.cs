using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement.Dtos
{
    /// <summary>
    /// DTO for Crisis Protocol Step output
    /// </summary>
    public class CrisisProtocolStepDto : EntityDto<Guid>
    {
        public string StepName { get; set; }
        public string Description { get; set; }
        public int OrderSequence { get; set; }
        public TimeSpan MaxExecutionTime { get; set; }
        public bool IsAutomated { get; set; }
        public List<string> RequiredRoles { get; set; }
        
        // Emergency Procedure properties
        public string ProcedureName { get; set; }
        public string ProcedureDescription { get; set; }
        public CrisisLevel RequiredCrisisLevel { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public List<string> ProcedureSteps { get; set; }

        public CrisisProtocolStepDto()
        {
            RequiredRoles = new List<string>();
            ProcedureSteps = new List<string>();
        }
    }
}
