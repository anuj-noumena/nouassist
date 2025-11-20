namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects
{
    /// <summary>
    /// Represents a rule for calculating metric values
    /// </summary>
    public class CalculationRule : ValueObject
    {
        public string Formula { get; private set; }
        public string Description { get; private set; }
        public List<string> RequiredMetrics { get; private set; }
        public bool IsAutomated { get; private set; }

        private CalculationRule() 
        {
            RequiredMetrics = new List<string>();
        }

        public CalculationRule(string formula, string description, bool isAutomated = true, List<string> requiredMetrics = null)
        {
            SetFormula(formula);
            SetDescription(description);
            IsAutomated = isAutomated;
            RequiredMetrics = requiredMetrics ?? new List<string>();
        }

        public bool CanCalculate(Dictionary<string, decimal> availableMetrics)
        {
            if (!IsAutomated || RequiredMetrics == null || RequiredMetrics.Count == 0)
                return false;

            return RequiredMetrics.All(metric => availableMetrics.ContainsKey(metric));
        }

        public decimal Calculate(Dictionary<string, decimal> metricValues)
        {
            if (!CanCalculate(metricValues))
            {
                throw new InvalidOperationException("Cannot calculate metric - required metrics not available");
            }

            // Simple expression evaluation - in production, consider using a proper expression engine
            var expression = Formula;
            foreach (var metric in RequiredMetrics)
            {
                expression = expression.Replace($"{{{metric}}}", metricValues[metric].ToString());
            }

            // This is a simplified implementation - use proper expression evaluation library in production
            return EvaluateExpression(expression);
        }

        private void SetFormula(string formula)
        {
            Formula = Check.NotNullOrWhiteSpace(
                formula,
                nameof(formula),
                maxLength: PerformanceFinancialModelingConsts.MaxCalculationRuleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: PerformanceFinancialModelingConsts.MaxDefinitionDescriptionLength
            );
        }

        private decimal EvaluateExpression(string expression)
        {
            // Simplified expression evaluation
            // In production, use a proper expression evaluation library like NCalc or similar
            try
            {
                var result = new System.Data.DataTable().Compute(expression, null);
                return Convert.ToDecimal(result);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to evaluate expression: {expression}", ex);
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Formula;
            yield return Description;
            yield return IsAutomated;
            yield return string.Join(",", RequiredMetrics.OrderBy(x => x));
        }
    }
}
