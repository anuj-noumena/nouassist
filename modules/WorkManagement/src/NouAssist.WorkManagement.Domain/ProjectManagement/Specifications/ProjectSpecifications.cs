using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Enums;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Enums;

namespace NouAssist.WorkManagement.ProjectManagement.Specifications
{
    public class ActiveProjectsSpecification : BaseSpecification<Project>
    {
        public override Expression<Func<Project, bool>> ToExpression() =>
            p => p.Status == ProjectStatus.Active && !p.IsArchived;
    }

    public class HealthyProjectsSpecification : BaseSpecification<Project>
    {
        public override Expression<Func<Project, bool>> ToExpression() =>
            p => p.Status == ProjectStatus.Active &&
                 !p.IsArchived &&
                 p.Progress >= 50;
    }

    public class ProjectsDueInNextDaysSpecification : BaseSpecification<Project>
    {
        private readonly int _days;

        public ProjectsDueInNextDaysSpecification(int days)
        {
            _days = days;
        }

        public override Expression<Func<Project, bool>> ToExpression()
        {
            var endDate = DateTime.UtcNow.AddDays(_days);
            return p => p.Timeline.PlannedDueDate.HasValue &&
                       p.Timeline.PlannedDueDate.Value <= endDate &&
                       p.Timeline.PlannedDueDate.Value >= DateTime.UtcNow &&
                       p.Status == ProjectStatus.Active;
        }
    }
}