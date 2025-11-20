using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents benchmark data for performance comparison
    /// </summary>
    public class BenchmarkData : Entity
    {
        /// <summary>
        /// Name of the benchmark (usually matches a metric name)
        /// </summary>
        public string BenchmarkName { get; protected set; }
        
        /// <summary>
        /// Type of benchmark (e.g., "TeamAverage", "PersonalBest", "IndustryStandard")
        /// </summary>
        public string BenchmarkType { get; protected set; }
        
        /// <summary>
        /// Value of the benchmark
        /// </summary>
        public double Value { get; protected set; }
        
        /// <summary>
        /// Unit of measurement
        /// </summary>
        public string Unit { get; protected set; }

        protected BenchmarkData()
        {
        }

        /// <summary>
        /// Creates a new benchmark data
        /// </summary>
        public BenchmarkData(
            string benchmarkName,
            string benchmarkType,
            double value,
            string unit = null)
        {
            BenchmarkName = benchmarkName;
            BenchmarkType = benchmarkType;
            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Gets the entity keys
        /// </summary>
        public override object[] GetKeys()
        {
            return new object[] { BenchmarkName, BenchmarkType };
        }
    }
}
