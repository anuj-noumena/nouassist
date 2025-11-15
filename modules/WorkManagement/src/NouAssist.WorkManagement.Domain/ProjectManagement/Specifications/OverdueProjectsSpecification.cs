using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Enums;

namespace NouAssist.WorkManagement.ProjectManagement.Specifications
{
    public class OverdueProjectsSpecification : BaseSpecification<Project>
    {
        public OverdueProjectsSpecification()
            : base(p => p.Timeline.PlannedDueDate != null &&
                        p.Timeline.PlannedDueDate.Value < DateTime.UtcNow &&
                        p.Status != ProjectStatus.Completed &&
                        p.Status != ProjectStatus.Cancelled &&
                        !p.IsArchived)
        {
        }
    }
}