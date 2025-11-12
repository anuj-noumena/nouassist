using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.Workflow.Enums;

namespace NouAssist.WorkManagement.Workflow.Dtos

{

    /// <summary>

    /// DTO for updating an existing transition permission policy.

    /// </summary>

    public class UpdateTransitionPermissionPolicyDto

    {

        [Required]

        [StringLength(256)]

        public string Name { get; set; }

 

        [StringLength(2000)]

        public string Description { get; set; }

 

        public int Priority { get; set; }

 

        public bool IsActive { get; set; }

 

        public PermissionPolicyScope Scope { get; set; }

 

        public Guid? ProjectId { get; set; }

 

        public List<string> AllowedRoles { get; set; } = new();

 

        public List<Guid> AllowedUserIds { get; set; } = new();

    }

}