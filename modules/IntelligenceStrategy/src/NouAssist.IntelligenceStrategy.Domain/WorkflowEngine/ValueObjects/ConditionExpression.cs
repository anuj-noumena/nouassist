namespace NouAssist.IntelligenceStrategy.WorkflowEngine.ValueObjects
{
    /// <summary>
    /// Value object representing a condition expression in a workflow
    /// </summary>
    public class ConditionExpression : ValueObject
    {
        /// <summary>
        /// The expression text
        /// </summary>
        public string Expression { get; private set; }
        
        /// <summary>
        /// The language or format of the expression (e.g., "javascript", "csharp", "jexl")
        /// </summary>
        public string Language { get; private set; }
        
        /// <summary>
        /// Description of what the condition does
        /// </summary>
        public string Description { get; private set; }

        protected ConditionExpression() { }

        public ConditionExpression(string expression, string language, string description = null)
        {
            SetExpression(expression);
            SetLanguage(language);
            Description = description;
        }
        
        private void SetExpression(string expression)
        {
            Expression = Check.NotNullOrWhiteSpace(
                expression,
                nameof(expression),
                maxLength: WorkflowEngineConsts.MaxExpressionLength
            );
        }
        
        private void SetLanguage(string language)
        {
            Language = Check.NotNullOrWhiteSpace(
                language,
                nameof(language),
                maxLength: 50
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Expression;
            yield return Language;
            yield return Description;
        }
    }
}
