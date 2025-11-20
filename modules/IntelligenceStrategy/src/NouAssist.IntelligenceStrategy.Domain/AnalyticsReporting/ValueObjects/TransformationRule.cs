using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects
{
    /// <summary>
    /// Rule for transforming data
    /// </summary>
    public class TransformationRule : ValueObject
    {
        /// <summary>
        /// Name of the transformation rule
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Source field or expression
        /// </summary>
        public string SourceExpression { get; private set; }
        
        /// <summary>
        /// Target field
        /// </summary>
        public string TargetField { get; private set; }
        
        /// <summary>
        /// Transformation expression or formula
        /// </summary>
        public string TransformationExpression { get; private set; }
        
        /// <summary>
        /// Order in which this transformation should be applied
        /// </summary>
        public int ExecutionOrder { get; private set; }
        
        private TransformationRule()
        {
        }
        
        public TransformationRule(
            string name,
            string sourceExpression,
            string targetField,
            string transformationExpression,
            int executionOrder = 0)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), AnalyticsReportingConsts.MaxNameLength);
            SourceExpression = Check.NotNullOrWhiteSpace(sourceExpression, nameof(sourceExpression));
            TargetField = Check.NotNullOrWhiteSpace(targetField, nameof(targetField));
            TransformationExpression = Check.NotNullOrWhiteSpace(
                transformationExpression,
                nameof(transformationExpression),
                AnalyticsReportingConsts.MaxTransformationRuleLength
            );
            
            if (executionOrder < 0)
            {
                throw new BusinessException("AnalyticsReporting:Validation:InvalidValue")
                    .WithData("propertyName", "executionOrder");
            }
            
            ExecutionOrder = executionOrder;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return SourceExpression;
            yield return TargetField;
            yield return TransformationExpression;
            yield return ExecutionOrder;
        }
    }
}
