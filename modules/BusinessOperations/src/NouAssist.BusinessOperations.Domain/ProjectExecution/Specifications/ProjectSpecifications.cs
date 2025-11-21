using System;
using System.Linq.Expressions;
using Volo.Abp.Timing;

namespace NouAssist.BusinessOperations.ProjectExecution.Specifications
{
    /// <summary>
    /// Base specification class for Project aggregate
    /// </summary>
    public abstract class ProjectSpecification : Specification<Project>
    {
    }
    
    /// <summary>
    /// Specification that identifies active projects
    /// </summary>
    public class ActiveProjectSpecification : ProjectSpecification
    {
        public override Expression<Func<Project, bool>> ToExpression()
        {
            return project => project.Status == ProjectStatus.Active;
        }
    }
    
    /// <summary>
    /// Specification that identifies projects eligible for completion
    /// </summary>
    public class CompletableProjectSpecification : ProjectSpecification
    {
        public override Expression<Func<Project, bool>> ToExpression()
        {
            return project => project.Status == ProjectStatus.Active &&
                              project.Details.PlannedEndDate <= Clock.Now;
        }
    }
    
    /// <summary>
    /// Specification that identifies projects with upcoming milestones
    /// </summary>
    public class ProjectsWithUpcomingMilestonesSpecification : ProjectSpecification
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        
        public ProjectsWithUpcomingMilestonesSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }
        
        public override Expression<Func<Project, bool>> ToExpression()
        {
            return project => project.Status != ProjectStatus.Cancelled &&
                              project.Milestones.Any(m => 
                                 !m.IsCompleted && 
                                 m.TargetDate >= _startDate && 
                                 m.TargetDate <= _endDate);
        }
    }
}
