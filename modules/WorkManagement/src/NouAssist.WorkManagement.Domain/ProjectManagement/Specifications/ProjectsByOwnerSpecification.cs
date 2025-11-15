using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;

namespace NouAssist.WorkManagement.ProjectManagement.Specifications
{
    public class ProjectsByOwnerSpecification : BaseSpecification<Project>
    {
        public ProjectsByOwnerSpecification(Guid ownerId)
            : base(p => p.OwnerId == ownerId)
        {
        }
    }
}