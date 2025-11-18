using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;

namespace NouAssist.WorkManagement.TaskManagement.Specifications
{
    /// <summary>
    /// Specification for active task instances in area and context
    /// </summary>
    public class TaskInstancesByAreaAndContextSpecification : BaseSpecification<TaskInstance>
    {
        private readonly Guid _areaId;
        private readonly Guid _contextId;

        public TaskInstancesByAreaAndContextSpecification(Guid areaId, Guid contextId)
        {
            _areaId = areaId;
            _contextId = contextId;
        }

        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.AreaId == _areaId && 
                  ti.ContextId == _contextId && 
                  !ti.WorkTracking.IsCompleted &&
                  !ti.IsDeleted;
    }

    /// <summary>
    /// Specification for unorganized tasks (no area, no project, no context)
    /// </summary>
    public class UnorganizedTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => !ti.AreaId.HasValue && 
                  !ti.ProjectId.HasValue && 
                  !ti.ContextId.HasValue &&
                  !ti.WorkTracking.IsCompleted &&
                  !ti.IsDeleted;
    }

    /// <summary>
    /// Specification for task instances in areas (not projects)
    /// </summary>
    public class AreaTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.AreaId.HasValue && 
                  !ti.ProjectId.HasValue &&
                  !ti.IsDeleted;
    }

    /// <summary>
    /// Specification for active tasks in area by context
    /// Useful for GTD-style "Next Actions by Context" views
    /// </summary>
    public class NextActionsByAreaAndContextSpecification : BaseSpecification<TaskInstance>
    {
        private readonly Guid _areaId;
        private readonly Guid? _contextId;

        public NextActionsByAreaAndContextSpecification(Guid areaId, Guid? contextId = null)
        {
            _areaId = areaId;
            _contextId = contextId;
        }

        public override Expression<Func<TaskInstance, bool>> ToExpression()
        {
            if (_contextId.HasValue)
            {
                return ti => ti.AreaId == _areaId &&
                             ti.ContextId == _contextId &&
                             ti.WorkTracking.IsNextAction &&
                             !ti.WorkTracking.IsCompleted &&
                             !ti.IsBlocked &&
                             !ti.IsDeleted;
            }

            return ti => ti.AreaId == _areaId &&
                         ti.WorkTracking.IsNextAction &&
                         !ti.WorkTracking.IsCompleted &&
                         !ti.IsBlocked &&
                         !ti.IsDeleted;
        }
        
    }
}