using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Enums;

namespace NouAssist.WorkManagement.ProjectManagement.Specifications
{
    public class ActiveSprintsSpecification : BaseSpecification<ProjectSprint>
    {
        public override Expression<Func<ProjectSprint, bool>> ToExpression() =>
            s => s.Status == SprintStatus.InProgress &&
                 s.Timeline.StartDate <= DateTime.UtcNow &&
                 s.Timeline.EndDate >= DateTime.UtcNow;
    }

    public class SprintsByProjectSpecification : BaseSpecification<ProjectSprint>
    {
        private readonly Guid _projectId;

        public SprintsByProjectSpecification(Guid projectId)
        {
            _projectId = projectId;
        }

        public override Expression<Func<ProjectSprint, bool>> ToExpression() =>
            s => s.ProjectId == _projectId;
    }

    public class CompletedSprintsSpecification : BaseSpecification<ProjectSprint>
    {
        public override Expression<Func<ProjectSprint, bool>> ToExpression() =>
            s => s.Status == SprintStatus.Completed;
    }

    public class UpcomingSprintsSpecification : BaseSpecification<ProjectSprint>
    {
        public override Expression<Func<ProjectSprint, bool>> ToExpression() =>
            s => s.Status == SprintStatus.Planned &&
                 s.Timeline.StartDate > DateTime.UtcNow;
    }

    public class SprintsEndingInNextDaysSpecification : BaseSpecification<ProjectSprint>
    {
        private readonly int _days;

        public SprintsEndingInNextDaysSpecification(int days)
        {
            _days = days;
        }

        public override Expression<Func<ProjectSprint, bool>> ToExpression()
        {
            var endDate = DateTime.UtcNow.AddDays(_days);
            return s => s.Status == SprintStatus.InProgress &&
                       s.Timeline.EndDate <= endDate &&
                       s.Timeline.EndDate >= DateTime.UtcNow;
        }
    }
}