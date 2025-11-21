using System;
using System.Linq.Expressions;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;

namespace NouAssist.BusinessOperations.FreelanceOperations.Specifications
{
    /// <summary>
    /// Base specification for FreelanceProject aggregate
    /// </summary>
    public abstract class FreelanceProjectSpecification : Specification<FreelanceProject>
    {
    }

    /// <summary>
    /// Specification for active projects
    /// </summary>
    public class ActiveProjectsSpecification : FreelanceProjectSpecification
    {
        public override Expression<Func<FreelanceProject, bool>> ToExpression()
        {
            return project => project.Status == ProjectStatus.Active;
        }
    }

    /// <summary>
    /// Specification for projects nearing deadline
    /// </summary>
    public class ProjectsNearingDeadlineSpecification : FreelanceProjectSpecification
    {
        private readonly int _daysUntilDeadline;

        public ProjectsNearingDeadlineSpecification(int daysUntilDeadline = 7)
        {
            _daysUntilDeadline = daysUntilDeadline;
        }

        public override Expression<Func<FreelanceProject, bool>> ToExpression()
        {
            var thresholdDate = DateTime.UtcNow.AddDays(_daysUntilDeadline);
            return project => project.Status == ProjectStatus.Active &&
                             project.EndDate.HasValue &&
                             project.EndDate <= thresholdDate;
        }
    }

    /// <summary>
    /// Specification for projects with overdue milestones
    /// </summary>
    public class ProjectsWithOverdueMilestonesSpecification : FreelanceProjectSpecification
    {
        public override Expression<Func<FreelanceProject, bool>> ToExpression()
        {
            var today = DateTime.UtcNow.Date;
            return project => project.Status == ProjectStatus.Active &&
                             project.Milestones.Any(m => !m.IsCompleted &&
                                                        m.Deadline.HasValue &&
                                                        m.Deadline.Value.Date < today);
        }
    }

    /// <summary>
    /// Specification for projects by client
    /// </summary>
    public class ProjectsByClientSpecification : FreelanceProjectSpecification
    {
        private readonly Guid _clientId;

        public ProjectsByClientSpecification(Guid clientId)
        {
            _clientId = clientId;
        }

        public override Expression<Func<FreelanceProject, bool>> ToExpression()
        {
            return project => project.ClientId == _clientId;
        }
    }
}
