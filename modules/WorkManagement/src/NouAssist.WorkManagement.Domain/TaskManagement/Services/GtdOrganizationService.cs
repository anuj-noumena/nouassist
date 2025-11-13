using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.TaskManagement.Services
{
    /// <summary>
    /// Domain service for GTD organization logic
    /// </summary>
    public class GtdOrganizationService : DomainService
    {
        /// <summary>
        /// Validates area/project exclusivity rule
        /// </summary>
        public void ValidateOrganization(Guid? projectId, Guid? areaId)
        {
            if (projectId.HasValue && areaId.HasValue)
            {
                throw new BusinessException("Tasks:CannotBeInBothProjectAndArea",
                    "A task cannot belong to both a Project and an Area. " +
                    "In GTD: Projects are specific outcomes with an end date. " +
                    "Areas are ongoing responsibilities with no end date.");
            }
        }

        /// <summary>
        /// Determines if a task should be in an area vs project
        /// Based on GTD principles
        /// </summary>
        public OrganizationType DetermineOrganizationType(
            bool hasSpecificOutcome,
            bool hasEndDate,
            bool isOngoingResponsibility)
        {
            if (hasSpecificOutcome && hasEndDate)
            {
                return OrganizationType.Project;
            }

            if (isOngoingResponsibility && !hasEndDate)
            {
                return OrganizationType.Area;
            }

            return OrganizationType.Unorganized;
        }

        /// <summary>
        /// Suggests organization for a task based on its properties
        /// </summary>
        public OrganizationSuggestion SuggestOrganization(
            Task task,
            List<Guid> userAreas,
            List<Guid> userProjects)
        {
            var suggestion = new OrganizationSuggestion
            {
                TaskId = task.Id
            };

            // If task has a due date and expected result, suggest project
            if (task.DefaultDueDate.HasValue && !string.IsNullOrEmpty(task.ExpectedResult))
            {
                suggestion.SuggestedType = OrganizationType.Project;
                suggestion.Reason = "Task has a due date and expected outcome";
            }
            // If task is recurring with no due date, suggest area
            else if (task.IsRecurring && !task.DefaultDueDate.HasValue)
            {
                suggestion.SuggestedType = OrganizationType.Area;
                suggestion.Reason = "Recurring task with no end date suggests ongoing responsibility";
            }
            // If task already in project, keep it there
            else if (task.ProjectId.HasValue)
            {
                suggestion.SuggestedType = OrganizationType.Project;
                suggestion.SuggestedId = task.ProjectId.Value;
                suggestion.Reason = "Already organized in a project";
            }
            // If task already in area, keep it there
            else if (task.AreaId.HasValue)
            {
                suggestion.SuggestedType = OrganizationType.Area;
                suggestion.SuggestedId = task.AreaId.Value;
                suggestion.Reason = "Already organized in an area";
            }
            else
            {
                suggestion.SuggestedType = OrganizationType.Unorganized;
                suggestion.Reason = "Needs manual organization";
            }

            return suggestion;
        }
    }

    public enum OrganizationType
    {
        Unorganized,
        Area,
        Project
    }

    public class OrganizationSuggestion
    {
        public Guid TaskId { get; set; }
        public OrganizationType SuggestedType { get; set; }
        public Guid? SuggestedId { get; set; }
        public string Reason { get; set; }
        public double Confidence { get; set; }
    }
}