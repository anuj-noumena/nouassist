namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.Common
{
    /// <summary>
    /// DTO for condition expression data
    /// </summary>
    public class ConditionExpressionDto
    {
        /// <summary>
        /// The expression text
        /// </summary>
        public string Expression { get; set; }
        
        /// <summary>
        /// The language or format of the expression
        /// </summary>
        public string Language { get; set; }
        
        /// <summary>
        /// Description of what the condition does
        /// </summary>
        public string Description { get; set; }
    }
}
