using System;

namespace NouAssist.Analytics.DTOs.KPI
{
    /// <summary>
    /// DTO for updating a KPI value
    /// </summary>
    public class UpdateKpiValueDto
    {
        public decimal NewValue { get; set; }
        
        public DateTime UpdateTime { get; set; }
    }
}
