using System;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;
using NouAssist.IntelligenceStrategy.WorkflowEngine.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Entities
{
    /// <summary>
    /// Represents a condition between activities in a workflow
    /// </summary>
    public class ConditionDefinition : Entity<Guid>
    {
        /// <summary>
        /// The ID of the workflow definition this condition belongs to
        /// </summary>
        public Guid WorkflowDefinitionId { get; private set; }
        
        /// <summary>
        /// The name of the condition
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The ID of the source activity
        /// </summary>
        public Guid SourceActivityId { get; private set; }
        
        /// <summary>
        /// The ID of the target activity
        /// </summary>
        public Guid TargetActivityId { get; private set; }
        
        /// <summary>
        /// The condition expression
        /// </summary>
        public ConditionExpression Expression { get; private set; }
        
        /// <summary>
        /// Optional description of the condition
        /// </summary>
        public string Description { get; private set; }

        protected ConditionDefinition() { }

        internal ConditionDefinition(
            Guid id,
            Guid workflowDefinitionId,
            string name,
            Guid sourceActivityId,
            Guid targetActivityId,
            ConditionExpression expression)
            : base(id)
        {
            WorkflowDefinitionId = workflowDefinitionId;
            SetName(name);
            SourceActivityId = sourceActivityId;
            TargetActivityId = targetActivityId;
            Expression = expression;
        }
        
        /// <summary>
        /// Updates the name of the condition
        /// </summary>
        /// <param name="name">The new name</param>
        public void UpdateName(string name)
        {
            SetName(name);
        }
        
        /// <summary>
        /// Updates the description of the condition
        /// </summary>
        /// <param name="description">The new description</param>
        public void UpdateDescription(string description)
        {
            Description = description;
        }
        
        /// <summary>
        /// Updates the condition expression
        /// </summary>
        /// <param name="expression">The new expression text</param>
        /// <param name="language">The expression language</param>
        public void UpdateExpression(string expression, string language)
        {
            Expression = new ConditionExpression(expression, language, Expression?.Description);
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: WorkflowEngineConsts.MaxNameLength
            );
        }
    }
}
