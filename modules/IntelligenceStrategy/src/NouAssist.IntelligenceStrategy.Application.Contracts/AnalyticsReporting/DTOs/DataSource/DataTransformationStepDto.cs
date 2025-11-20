using System;

namespace NouAssist.Analytics.DTOs.DataSource
{
    /// <summary>
    /// DTO for data transformation step information
    /// </summary>
    public class DataTransformationStepDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string TransformationType { get; set; }
        
        public string Configuration { get; set; }
        
        public int Order { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}
