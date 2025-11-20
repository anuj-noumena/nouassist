namespace NouAssist.BusinessOperations.ProjectExecution.Specifications
{
    /// <summary>
    /// Base specification class for Task aggregate
    /// </summary>
    public abstract class TaskSpecification : Specification<Task>
    {
    }
    
    /// <summary>
    /// Specification that identifies tasks eligible for completion
    /// </summary>
    public class CompletableTaskSpecification : TaskSpecification
    {
        public override Expression<Func<Task, bool>> ToExpression()
        {
            return task => task.Status == TaskStatus.InProgress && 
                          task.Assignments.Any(a => a.IsActive);
        }
    }
    
    /// <summary>
    /// Specification that identifies tasks that are blocked and should be unblocked
    /// </summary>
    public class UnblockableTaskSpecification : TaskSpecification
    {
        private readonly List<Guid> _completedTaskIds;
        
        public UnblockableTaskSpecification(List<Guid> completedTaskIds)
        {
            _completedTaskIds = completedTaskIds;
        }
        
        public override Expression<Func<Task, bool>> ToExpression()
        {
            return task => task.Status == TaskStatus.Blocked && 
                           task.Dependencies.All(d => 
                               _completedTaskIds.Contains(d.DependsOnTaskId) ||
                               d.DependencyType != DependencyType.FinishToStart);
        }
    }
    
    /// <summary>
    /// Specification that identifies overdue tasks
    /// </summary>
    public class OverdueTaskSpecification : TaskSpecification
    {
        private readonly DateTime _currentDate;
        
        public OverdueTaskSpecification(DateTime currentDate)
        {
            _currentDate = currentDate;
        }
        
        public override Expression<Func<Task, bool>> ToExpression()
        {
            return task => task.Status != TaskStatus.Done && 
                           task.DueDate.HasValue && 
                           task.DueDate.Value < _currentDate;
        }
    }
    
    /// <summary>
    /// Specification that identifies tasks with specific priority
    /// </summary>
    public class TaskPrioritySpecification : TaskSpecification
    {
        private readonly TaskPriority _priority;
        
        public TaskPrioritySpecification(TaskPriority priority)
        {
            _priority = priority;
        }
        
        public override Expression<Func<Task, bool>> ToExpression()
        {
            return task => task.Details.Priority == _priority;
        }
    }
}
