// src/NouAssist.Projects.Domain/Specifications/RequirementSpecifications.cs
using System;
using System.Linq.Expressions;
using NouAssist.Core.Specifications;
using NouAssist.Projects.Entities;
using NouAssist.Projects.Enums;
using NouAssist.Workflow.Entities;
using Volo.Abp.Specifications;

namespace NouAssist.Projects.Specifications
{
    public class PendingRequirementsSpecification : BaseSpecification<ProjectRequirement>
    {
        public override Expression<Func<ProjectRequirement, bool>> ToExpression() =>
            req => req.Status == ProjectRequirementStatus.Proposed;
    }

    public class ApprovedRequirementsSpecification : BaseSpecification<ProjectRequirement>
    {
        public override Expression<Func<ProjectRequirement, bool>> ToExpression() =>
            req => req.Status == ProjectRequirementStatus.Approved;
    }

    public class ImplementedRequirementsSpecification : BaseSpecification<ProjectRequirement>
    {
        public override Expression<Func<ProjectRequirement, bool>> ToExpression() =>
            req => req.Status == ProjectRequirementStatus.Implemented ||
                   req.Status == ProjectRequirementStatus.Verified;
    }
}