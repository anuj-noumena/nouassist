using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.Workflow.Enums;

namespace NouAssist.WorkManagement.Workflow.Workflows.Dtos

{

    /// <summary>

    /// DTO for creating a new transition permission policy.

    /// </summary>

    public class CreateTransitionPermissionPolicyDto

    {

        [Required]

        public PermissionPolicyScope Scope { get; set; }

 

        /// <summary>

        /// Required if Scope is Project, otherwise should be null.

        /// </summary>

        public Guid? ProjectId { get; set; }

 

        [Required]

        public Guid WorkflowId { get; set; }

 

        [Required]

        public Guid TransitionId { get; set; }

 

        [Required]

        [StringLength(256)]

        public string Name { get; set; }

 

        [StringLength(2000)]

        public string Description { get; set; }

 

        public int Priority { get; set; } = 0;

 

        public List<string> AllowedRoles { get; set; } = new();

 

        public List<Guid> AllowedUserIds { get; set; } = new();

    }

}