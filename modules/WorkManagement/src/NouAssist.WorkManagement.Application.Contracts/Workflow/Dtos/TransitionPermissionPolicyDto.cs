using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.Workflow.Enums;

namespace NouAssist.WorkManagement.Workflow.Workflows.Dtos

{

    /// <summary>

    /// DTO for transition permission policy.

    /// </summary>

    public class TransitionPermissionPolicyDto

    {

        public Guid Id { get; set; }

        public PermissionPolicyScope Scope { get; set; }

        public Guid? ProjectId { get; set; }

        public Guid WorkflowId { get; set; }

        public Guid TransitionId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int Priority { get; set; }

        public List<string> AllowedRoles { get; set; } = new();

        public List<Guid> AllowedUserIds { get; set; } = new();

        public bool HasRestrictions { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }

}