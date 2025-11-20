namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates
{
    /// <summary>
    /// Automation workflow aggregate root for defining automated processes
    /// </summary>
    public class AutomationWorkflow : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the workflow
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of the workflow
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Whether this workflow is active
        /// </summary>
        public bool IsActive { get; private set; }
        
        /// <summary>
        /// Trigger condition for the workflow
        /// </summary>
        public AutomationTriggerCondition TriggerCondition { get; private set; }
        
        /// <summary>
        /// Steps in the workflow
        /// </summary>
        private readonly List<WorkflowStep> _steps;
        
        /// <summary>
        /// Read-only access to workflow steps
        /// </summary>
        public IReadOnlyCollection<WorkflowStep> Steps => _steps.AsReadOnly();
        
        /// <summary>
        /// Execution history of this workflow
        /// </summary>
        private readonly List<WorkflowExecutionRecord> _executionRecords;
        
        /// <summary>
        /// Read-only access to execution records
        /// </summary>
        public IReadOnlyCollection<WorkflowExecutionRecord> ExecutionRecords => _executionRecords.AsReadOnly();

        protected AutomationWorkflow()
        {
            _steps = new List<WorkflowStep>();
            _executionRecords = new List<WorkflowExecutionRecord>();
        }

        public static AutomationWorkflow Create(
            Guid id,
            string name,
            string description,
            AutomationTriggerCondition triggerCondition,
            bool isActive = false,
            Guid? tenantId = null)
        {
            return new AutomationWorkflow(id, name, description, triggerCondition, isActive, tenantId);
        }

        private AutomationWorkflow(
            Guid id,
            string name,
            string description,
            AutomationTriggerCondition triggerCondition,
            bool isActive = false,
            Guid? tenantId = null) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Description = description;
            TriggerCondition = triggerCondition ?? throw new ArgumentNullException(nameof(triggerCondition));
            IsActive = isActive;
            TenantId = tenantId;
            _steps = new List<WorkflowStep>();
            _executionRecords = new List<WorkflowExecutionRecord>();
        }

        public void UpdateName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdateTriggerCondition(AutomationTriggerCondition triggerCondition)
        {
            TriggerCondition = triggerCondition ?? throw new ArgumentNullException(nameof(triggerCondition));
        }

        public void Activate()
        {
            if (_steps.Count == 0)
            {
                throw new BusinessException("Cannot activate a workflow with no steps");
            }
            
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public WorkflowStep AddStep(
            string name,
            string description,
            AutomationActionType actionType,
            string actionConfiguration,
            TimeSpan? delayBefore = null)
        {
            var order = _steps.Count > 0 ? _steps.Max(s => s.Order) + 1 : 0;
            
            var step = new WorkflowStep(
                Id,
                order,
                name,
                description,
                actionType,
                actionConfiguration,
                delayBefore);
                
            _steps.Add(step);
            return step;
        }

        public void UpdateStep(
            WorkflowStep step,
            string name = null,
            string description = null,
            AutomationActionType? actionType = null,
            string actionConfiguration = null,
            TimeSpan? delayBefore = null,
            int? order = null)
        {
            if (!_steps.Contains(step))
            {
                throw new BusinessException("Step is not part of this workflow");
            }
            
            step.Update(name, description, actionType, actionConfiguration, delayBefore, order);
        }

        public void EnableStep(WorkflowStep step)
        {
            if (!_steps.Contains(step))
            {
                throw new BusinessException("Step is not part of this workflow");
            }
            
            step.Enable();
        }

        public void DisableStep(WorkflowStep step)
        {
            if (!_steps.Contains(step))
            {
                throw new BusinessException("Step is not part of this workflow");
            }
            
            step.Disable();
        }

        public void RemoveStep(WorkflowStep step)
        {
            _steps.Remove(step);
            
            // Reorder remaining steps
            int order = 0;
            foreach (var remainingStep in _steps.OrderBy(s => s.Order))
            {
                remainingStep.Update(order: order++);
            }
        }

        public void ReorderSteps(IEnumerable<(WorkflowStep Step, int NewOrder)> newOrdering)
        {
            var orderedSteps = newOrdering.ToList();
            
            // Validate all steps are part of this workflow
            foreach (var (step, _) in orderedSteps)
            {
                if (!_steps.Contains(step))
                {
                    throw new BusinessException("One or more steps are not part of this workflow");
                }
            }
            
            // Apply new ordering
            foreach (var (step, newOrder) in orderedSteps)
            {
                step.Update(order: newOrder);
            }
        }

        public WorkflowExecutionRecord RecordExecution(
            Guid executionId,
            Guid triggeredByEntityId,
            string triggeredByEntityType)
        {
            var record = new WorkflowExecutionRecord(
                Id,
                executionId,
                triggeredByEntityId,
                triggeredByEntityType,
                Clock.Now);
                
            _executionRecords.Add(record);
            
            AddDistributedEvent(new AutomationWorkflowTriggeredEto
            {
                Id = Id,
                WorkflowName = Name,
                TriggerType = TriggerCondition.TriggerType,
                TriggeredByEntityId = triggeredByEntityId,
                TriggeredByEntityType = triggeredByEntityType,
                TriggerTime = Clock.Now,
                TenantId = TenantId
            });
            
            return record;
        }

        public void CompleteExecution(WorkflowExecutionRecord record)
        {
            if (!_executionRecords.Contains(record))
            {
                throw new BusinessException("Execution record is not part of this workflow");
            }
            
            record.Complete(Clock.Now);
        }

        public void FailExecution(WorkflowExecutionRecord record, string errorMessage)
        {
            if (!_executionRecords.Contains(record))
            {
                throw new BusinessException("Execution record is not part of this workflow");
            }
            
            record.Fail(errorMessage);
        }

        public void LogExecutionStep(WorkflowExecutionRecord record, string logEntry)
        {
            if (!_executionRecords.Contains(record))
            {
                throw new BusinessException("Execution record is not part of this workflow");
            }
            
            record.AppendToLog(logEntry);
        }
    }
}
