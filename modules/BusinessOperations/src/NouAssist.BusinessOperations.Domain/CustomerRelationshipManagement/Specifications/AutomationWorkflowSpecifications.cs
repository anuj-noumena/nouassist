namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Specifications
{
    /// <summary>
    /// Base specification class for AutomationWorkflow aggregate
    /// </summary>
    public abstract class AutomationWorkflowSpecification : Specification<AutomationWorkflow>
    {
    }

    /// <summary>
    /// Specification for active workflows
    /// </summary>
    public class ActiveWorkflowSpecification : AutomationWorkflowSpecification
    {
        public override Expression<Func<AutomationWorkflow, bool>> ToExpression()
        {
            return workflow => workflow.IsActive;
        }
    }

    /// <summary>
    /// Specification for workflows with a specific trigger type
    /// </summary>
    public class WorkflowWithTriggerSpecification : AutomationWorkflowSpecification
    {
        private readonly AutomationTriggerType _triggerType;
        private readonly string _entityType;

        public WorkflowWithTriggerSpecification(AutomationTriggerType triggerType, string entityType)
        {
            _triggerType = triggerType;
            _entityType = entityType;
        }

        public override Expression<Func<AutomationWorkflow, bool>> ToExpression()
        {
            return workflow => 
                workflow.TriggerCondition.TriggerType == _triggerType && 
                workflow.TriggerCondition.EntityType == _entityType;
        }
    }

    /// <summary>
    /// Specification for workflows triggered by field changes
    /// </summary>
    public class FieldTriggerWorkflowSpecification : AutomationWorkflowSpecification
    {
        private readonly string _entityType;
        private readonly string _fieldName;
        private readonly string _fieldValue;

        public FieldTriggerWorkflowSpecification(string entityType, string fieldName, string fieldValue = null)
        {
            _entityType = entityType;
            _fieldName = fieldName;
            _fieldValue = fieldValue;
        }

        public override Expression<Func<AutomationWorkflow, bool>> ToExpression()
        {
            if (_fieldValue == null)
            {
                return workflow => 
                    (workflow.TriggerCondition.TriggerType == AutomationTriggerType.CustomField || 
                     workflow.TriggerCondition.TriggerType == AutomationTriggerType.LeadStatusChanged) && 
                    workflow.TriggerCondition.EntityType == _entityType && 
                    workflow.TriggerCondition.FieldName == _fieldName;
            }
            
            return workflow => 
                (workflow.TriggerCondition.TriggerType == AutomationTriggerType.CustomField || 
                 workflow.TriggerCondition.TriggerType == AutomationTriggerType.LeadStatusChanged) && 
                workflow.TriggerCondition.EntityType == _entityType && 
                workflow.TriggerCondition.FieldName == _fieldName && 
                workflow.TriggerCondition.ExpectedValue == _fieldValue;
        }
    }

    /// <summary>
    /// Specification for scheduled workflows
    /// </summary>
    public class ScheduledWorkflowSpecification : AutomationWorkflowSpecification
    {
        public override Expression<Func<AutomationWorkflow, bool>> ToExpression()
        {
            return workflow => 
                workflow.TriggerCondition.TriggerType == AutomationTriggerType.ScheduledTime && 
                !string.IsNullOrEmpty(workflow.TriggerCondition.ScheduleExpression);
        }
    }
}
