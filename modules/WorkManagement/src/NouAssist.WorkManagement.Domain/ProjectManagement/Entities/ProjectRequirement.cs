using System;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.ProjectManagement.Entities
{
    public class ProjectRequirement : FullAuditedEntity<Guid>, IHasConcurrencyStamp, IHasExtraProperties
    {
        public Guid ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public ProjectRequirementType RequirementType { get; private set; }
        public ProjectRequirementStatus Status { get; private set; }
        public PriorityLevel Priority { get; set; }
        public string AcceptanceCriteria { get; set; }
        public int? StoryPoints { get; set; }
        public int? BusinessValue { get; set; }
        public int OrderIndex { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid? ParentRequirementId { get; set; }

        protected ProjectRequirement() { }

        public ProjectRequirement(
            Guid id,
            Guid projectId,
            string title,
            ProjectRequirementType requirementType,
            string description = null) : base(id)
        {
            ProjectId = projectId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            RequirementType = requirementType;
            Description = description;
            Status = ProjectRequirementStatus.Proposed;
            Priority = PriorityLevel.Medium;
        }
        
        public void Update(string title, string description)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = description;
        }

        public void Approve()
        {
            if (Status != ProjectRequirementStatus.Proposed)
            {
                throw new BusinessException("Projects:CannotApproveRequirement");
            }
            Status = ProjectRequirementStatus.Approved;
        }

        public void StartImplementation()
        {
            if (Status != ProjectRequirementStatus.Approved)
            {
                throw new BusinessException("Projects:RequirementNotApproved");
            }
            Status = ProjectRequirementStatus.InProgress;
        }

        public void MarkAsImplemented()
        {
            if (Status != ProjectRequirementStatus.InProgress)
            {
                throw new BusinessException("Projects:RequirementNotInProgress");
            }
            Status = ProjectRequirementStatus.Implemented;
        }

        public void Verify()
        {
            if (Status != ProjectRequirementStatus.Implemented)
            {
                throw new BusinessException("Projects:RequirementNotImplemented");
            }
            Status = ProjectRequirementStatus.Verified;
        }

        public void Reject(string reason)
        {
            Status = ProjectRequirementStatus.Rejected;
        }

        public string ConcurrencyStamp { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; }
    }
}