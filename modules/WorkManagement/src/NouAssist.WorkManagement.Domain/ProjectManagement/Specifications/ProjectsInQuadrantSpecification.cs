using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Enums;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;

namespace NouAssist.WorkManagement.ProjectManagement.Specifications
{
    public class ProjectsInQuadrantSpecification : BaseSpecification<Project>
    {
        public ProjectsInQuadrantSpecification(EisenhowerQuadrant quadrant)
            : base(p => p.Priority.Quadrant == quadrant)
        {
        }
    }
}