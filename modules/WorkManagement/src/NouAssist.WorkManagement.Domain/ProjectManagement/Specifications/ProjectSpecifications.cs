using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using NouAssist.WorkManagement.Shared.Enums;

namespace NouAssist.WorkManagement.ProjectManagement.Specifications
{
    public class ActiveProjectsSpecification : BaseSpecification<Project>
    {
        public override Expression<Func<Project, bool>> ToExpression() =>
            p => p.Status == ProjectStatus.Active && !p.IsArchived;
    }

    public class OverdueProjectsSpecification : BaseSpecification<Project>
    {
        public override Expression<Func<Project, bool>> ToExpression() =>
            p => p.DueDate.HasValue &&
                 p.DueDate.Value < DateTime.UtcNow &&
                 p.Status != ProjectStatus.Completed &&
                 p.Status != ProjectStatus.Cancelled;
    }

    public class ProjectsByOwnerSpecification : BaseSpecification<Project>
    {
        private readonly Guid _ownerId;

        public ProjectsByOwnerSpecification(Guid ownerId)
        {
            _ownerId = ownerId;
        }

        public override Expression<Func<Project, bool>> ToExpression() =>
            p => p.OwnerId == _ownerId;
    }

    public class HealthyProjectsSpecification : BaseSpecification<Project>
    {
        public override Expression<Func<Project, bool>> ToExpression() =>
            p => p.Status == ProjectStatus.Active &&
                 !p.IsArchived &&
                 p.Progress >= 50;
    }

    public class ProjectsInQuadrantSpecification : BaseSpecification<Project>
    {
        private readonly EisenhowerQuadrant _quadrant;

        public ProjectsInQuadrantSpecification(EisenhowerQuadrant quadrant)
        {
            _quadrant = quadrant;
        }

        public override Expression<Func<Project, bool>> ToExpression() =>
            p => p.EisenhowerQuadrant == _quadrant && !p.IsArchived;
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
            return p => p.DueDate.HasValue &&
                       p.DueDate.Value <= endDate &&
                       p.DueDate.Value >= DateTime.UtcNow &&
                       p.Status == ProjectStatus.Active;
        }
    }
}
