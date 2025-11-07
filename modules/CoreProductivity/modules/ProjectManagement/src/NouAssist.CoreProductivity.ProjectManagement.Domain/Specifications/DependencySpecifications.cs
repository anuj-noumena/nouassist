using System;
using System.Linq.Expressions;
using NouAssist.Core.Specifications;
using NouAssist.CoreProductivity.ProjectManagement.Entities;
using NouAssist.CoreProductivity.ProjectManagement.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.CoreProductivity.ProjectManagement.Specifications
{
    public class ActiveDependenciesSpecification : BaseSpecification<ProjectDependency>
    {
        public override Expression<Func<ProjectDependency, bool>> ToExpression() =>
            d => d.Status == ProjectDependencyStatus.Active;
    }

    public class BlockingDependenciesSpecification : BaseSpecification<ProjectDependency>
    {
        public override Expression<Func<ProjectDependency, bool>> ToExpression() =>
            d => d.IsBlocking && 
                 d.Status == ProjectDependencyStatus.Active &&
                 !d.IsResolved;
    }

    public class OverdueDependenciesSpecification : BaseSpecification<ProjectDependency>
    {
        public override Expression<Func<ProjectDependency, bool>> ToExpression() =>
            d => d.ExpectedResolutionDate.HasValue &&
                 d.ExpectedResolutionDate.Value < DateTime.UtcNow &&
                 d.Status == ProjectDependencyStatus.Active;
    }

    public class ProjectDependenciesSpecification : BaseSpecification<ProjectDependency>
    {
        private readonly Guid _projectId;

        public ProjectDependenciesSpecification(Guid projectId)
        {
            _projectId = projectId;
        }

        public override Expression<Func<ProjectDependency, bool>> ToExpression() =>
            d => d.ProjectId == _projectId;
    }
}
