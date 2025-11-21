using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Exceptions;
using NouAssist.IntelligenceStrategy.WorkflowEngine.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Entities
{
    /// <summary>
    /// Represents a specific version of a workflow definition
    /// </summary>
    public class WorkflowVersion : Entity
    {
        /// <summary>
        /// The ID of the workflow definition this version belongs to
        /// </summary>
        public Guid WorkflowDefinitionId { get; private set; }
        
        /// <summary>
        /// Version number
        /// </summary>
        public int VersionNumber { get; private set; }
        
        /// <summary>
        /// Indicates if this version is published and available for use
        /// </summary>
        public bool IsPublished { get; private set; }
        
        /// <summary>
        /// The timestamp when this version was created
        /// </summary>
        public DateTime CreationTime { get; private set; }
        
        /// <summary>
        /// Optional comments about this version
        /// </summary>
        public string Comments { get; private set; }
        
        /// <summary>
        /// The BPMN diagram representation of this workflow version
        /// </summary>
        public BpmnDiagram BpmnDiagram { get; private set; }
        
        private readonly List<ActivityDefinition> _activities;
        
        /// <summary>
        /// The activities defined in this workflow version
        /// </summary>
        public IReadOnlyCollection<ActivityDefinition> Activities => _activities.AsReadOnly();
        
        private readonly List<ConditionDefinition> _conditions;
        
        /// <summary>
        /// The conditions defined in this workflow version
        /// </summary>
        public IReadOnlyCollection<ConditionDefinition> Conditions => _conditions.AsReadOnly();

        protected WorkflowVersion()
        {
            _activities = new List<ActivityDefinition>();
            _conditions = new List<ConditionDefinition>();
        }

        internal WorkflowVersion(
            Guid workflowDefinitionId,
            int versionNumber,
            string comments = null)
        {
            WorkflowDefinitionId = workflowDefinitionId;
            VersionNumber = versionNumber;
            IsPublished = false;
            CreationTime = DateTime.UtcNow;
            Comments = comments;
            _activities = new List<ActivityDefinition>();
            _conditions = new List<ConditionDefinition>();
        }
        
        /// <summary>
        /// Publishes this version, making it available for use
        /// </summary>
        public void Publish()
        {
            IsPublished = true;
        }
        
        /// <summary>
        /// Updates the BPMN diagram for this version
        /// </summary>
        /// <param name="xmlContent">The XML content of the BPMN diagram</param>
        /// <param name="bpmnVersion">The BPMN version</param>
        public void UpdateBpmnDiagram(string xmlContent, string bpmnVersion)
        {
            if (IsPublished)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:CannotModifyPublishedVersion",
                    "Cannot modify a published workflow version."
                );
            }
            
            BpmnDiagram = new BpmnDiagram(xmlContent, bpmnVersion);
        }
        
        /// <summary>
        /// Adds an activity to this workflow version
        /// </summary>
        /// <param name="id">The ID of the activity</param>
        /// <param name="name">The name of the activity</param>
        /// <param name="activityType">The type of the activity</param>
        /// <param name="properties">Optional additional properties</param>
        /// <returns>The created activity</returns>
        public ActivityDefinition AddActivity(
            Guid id,
            string name,
            ActivityType activityType,
            Dictionary<string, string> properties = null)
        {
            if (IsPublished)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:CannotModifyPublishedVersion",
                    "Cannot modify a published workflow version."
                );
            }
            
            if (_activities.Any(a => a.Name == name))
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:DuplicateName"
                ).WithData("entityType", "Activity").WithData("name", name);
            }
            
            var activity = new ActivityDefinition(
                id,
                WorkflowDefinitionId,
                name,
                activityType,
                properties
            );
            
            _activities.Add(activity);
            
            return activity;
        }
        
        /// <summary>
        /// Adds a condition to this workflow version
        /// </summary>
        /// <param name="id">The ID of the condition</param>
        /// <param name="name">The name of the condition</param>
        /// <param name="sourceActivityId">The ID of the source activity</param>
        /// <param name="targetActivityId">The ID of the target activity</param>
        /// <param name="expression">The condition expression</param>
        /// <param name="language">The expression language</param>
        /// <returns>The created condition</returns>
        public ConditionDefinition AddCondition(
            Guid id,
            string name,
            Guid sourceActivityId,
            Guid targetActivityId,
            string expression,
            string language)
        {
            if (IsPublished)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:CannotModifyPublishedVersion",
                    "Cannot modify a published workflow version."
                );
            }
            
            if (_conditions.Any(c => c.Name == name))
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:DuplicateName"
                ).WithData("entityType", "Condition").WithData("name", name);
            }
            
            if (!_activities.Any(a => a.Id == sourceActivityId))
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:ActivityNotFound"
                ).WithData("activityId", sourceActivityId);
            }
            
            if (!_activities.Any(a => a.Id == targetActivityId))
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:ActivityNotFound"
                ).WithData("activityId", targetActivityId);
            }
            
            var conditionExpression = new ConditionExpression(expression, language);
            
            var condition = new ConditionDefinition(
                id,
                WorkflowDefinitionId,
                name,
                sourceActivityId,
                targetActivityId,
                conditionExpression
            );
            
            _conditions.Add(condition);
            
            return condition;
        }
        
        /// <summary>
        /// Removes an activity from this workflow version
        /// </summary>
        /// <param name="activityId">The ID of the activity to remove</param>
        public void RemoveActivity(Guid activityId)
        {
            if (IsPublished)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:CannotModifyPublishedVersion",
                    "Cannot modify a published workflow version."
                );
            }
            
            var activity = _activities.FirstOrDefault(a => a.Id == activityId);
            if (activity == null)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:ActivityNotFound"
                ).WithData("activityId", activityId);
            }
            
            // Remove any conditions associated with this activity
            _conditions.RemoveAll(c => 
                c.SourceActivityId == activityId || c.TargetActivityId == activityId);
            
            _activities.Remove(activity);
        }
        
        /// <summary>
        /// Validates that this workflow version is valid and can be published
        /// </summary>
        /// <returns>True if the workflow is valid, false otherwise</returns>
        public bool Validate()
        {
            // A workflow must have at least one activity
            if (!_activities.Any())
            {
                return false;
            }
            
            // A workflow must have a start event
            if (!_activities.Any(a => a.ActivityType == ActivityType.StartEvent))
            {
                return false;
            }
            
            // A workflow must have an end event
            if (!_activities.Any(a => a.ActivityType == ActivityType.EndEvent))
            {
                return false;
            }
            
            // Check for cycles in the workflow graph
            // This is a simplified check, a more complex cycle detection algorithm 
            // would be needed for a production system
            foreach (var condition in _conditions)
            {
                if (condition.SourceActivityId == condition.TargetActivityId)
                {
                    return false;
                }
            }
            
            return true;
        }

        public override object[] GetKeys()
        {
            return new object[] { WorkflowDefinitionId, VersionNumber };
        }
    }
}
