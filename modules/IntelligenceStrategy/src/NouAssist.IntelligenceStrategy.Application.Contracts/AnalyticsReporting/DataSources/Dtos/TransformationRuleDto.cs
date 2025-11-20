namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos
{
    /// <summary>
    /// DTO for transformation rule
    /// </summary>
    public class TransformationRuleDto
    {
        /// <summary>
        /// Name of the transformation rule
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Source field or expression
        /// </summary>
        public string SourceExpression { get; set; }
        
        /// <summary>
        /// Target field
        /// </summary>
        public string TargetField { get; set; }
        
        /// <summary>
        /// Transformation expression or formula
        /// </summary>
        public string TransformationExpression { get; set; }
        
        /// <summary>
        /// Order in which this transformation should be applied
        /// </summary>
        public int ExecutionOrder { get; set; }
    }
}
