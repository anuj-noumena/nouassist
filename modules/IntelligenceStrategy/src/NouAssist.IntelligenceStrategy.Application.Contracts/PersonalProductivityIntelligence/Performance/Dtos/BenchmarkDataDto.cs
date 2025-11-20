using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance.Dtos
{
    /// <summary>
    /// DTO for benchmark data
    /// </summary>
    public class BenchmarkDataDto
    {
        /// <summary>
        /// Name of the benchmark
        /// </summary>
        public string BenchmarkName { get; set; }
        
        /// <summary>
        /// Type of benchmark
        /// </summary>
        public string BenchmarkType { get; set; }
        
        /// <summary>
        /// Value of the benchmark
        /// </summary>
        public double Value { get; set; }
        
        /// <summary>
        /// Unit of measurement
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// Formatted value with unit
        /// </summary>
        public string FormattedValue { get; set; }
    }
}
