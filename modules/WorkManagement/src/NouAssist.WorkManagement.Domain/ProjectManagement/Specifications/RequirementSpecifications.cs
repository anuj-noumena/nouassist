// src/NouAssist.CoreProductivity.ProjectManagement.Domain/Specifications/RequirementSpecifications.cs

using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.ProjectManagement.Enums;

namespace NouAssist.WorkManagement.ProjectManagement.Specifications
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