namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Represents a condition that triggers an automation workflow
    /// </summary>
    public class AutomationTriggerCondition : ValueObject
    {
        /// <summary>
        /// The type of trigger
        /// </summary>
        public AutomationTriggerType TriggerType { get; private set; }
        
        /// <summary>
        /// The entity type this trigger applies to
        /// </summary>
        public string EntityType { get; private set; }
        
        /// <summary>
        /// Optional field name if trigger is field-specific
        /// </summary>
        public string FieldName { get; private set; }
        
        /// <summary>
        /// Optional value to compare against for triggers like status changes
        /// </summary>
        public string ExpectedValue { get; private set; }
        
        /// <summary>
        /// For scheduled triggers, specifies when to run
        /// </summary>
        public string ScheduleExpression { get; private set; }

        protected AutomationTriggerCondition()
        {
        }

        public AutomationTriggerCondition(
            AutomationTriggerType triggerType,
            string entityType,
            string fieldName = null,
            string expectedValue = null,
            string scheduleExpression = null)
        {
            TriggerType = triggerType;
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            FieldName = fieldName;
            ExpectedValue = expectedValue;
            ScheduleExpression = scheduleExpression;
            
            ValidateTriggerConfiguration();
        }

        private void ValidateTriggerConfiguration()
        {
            switch (TriggerType)
            {
                case AutomationTriggerType.LeadStatusChanged:
                case AutomationTriggerType.CustomField:
                    if (string.IsNullOrWhiteSpace(FieldName))
                    {
                        throw new BusinessException("CRM:ErrorMessage:InvalidWorkflowConfiguration")
                            .WithData("reason", "Field name is required for this trigger type");
                    }
                    break;
                    
                case AutomationTriggerType.ScheduledTime:
                    if (string.IsNullOrWhiteSpace(ScheduleExpression))
                    {
                        throw new BusinessException("CRM:ErrorMessage:InvalidWorkflowConfiguration")
                            .WithData("reason", "Schedule expression is required for scheduled triggers");
                    }
                    break;
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TriggerType;
            yield return EntityType;
            yield return FieldName ?? string.Empty;
            yield return ExpectedValue ?? string.Empty;
            yield return ScheduleExpression ?? string.Empty;
        }
    }
}
