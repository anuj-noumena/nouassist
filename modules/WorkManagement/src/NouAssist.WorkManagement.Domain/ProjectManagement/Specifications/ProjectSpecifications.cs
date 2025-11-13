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

    public class OverdueProjectsSpecification : BaseSpecification<Project>
    {
        public override Expression<Func<Project, bool>> ToExpression() =>
            p => p.Timeline.PlannedDueDate.HasValue &&
                 p.Timeline.PlannedDueDate.Value < DateTime.UtcNow &&
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

        public override Expression<Func<Project, bool>> ToExpression()
        {
            // Map quadrant to importance/urgency thresholds
            // Assuming values above 0.5 are "high"
            return _quadrant switch
            {
                EisenhowerQuadrant.Q1DoFirst => 
                    p => p.Priority.Importance > 0.5 && p.Priority.Urgency > 0.5 && !p.IsArchived,
                EisenhowerQuadrant.Q2Schedule => 
                    p => p.Priority.Importance > 0.5 && p.Priority.Urgency <= 0.5 && !p.IsArchived,
                EisenhowerQuadrant.Q3Delegate => 
                    p => p.Priority.Importance <= 0.5 && p.Priority.Urgency > 0.5 && !p.IsArchived,
                EisenhowerQuadrant.Q4Eliminate => 
                    p => p.Priority.Importance <= 0.5 && p.Priority.Urgency <= 0.5 && !p.IsArchived,
                _ => p => !p.IsArchived
            };
        }
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