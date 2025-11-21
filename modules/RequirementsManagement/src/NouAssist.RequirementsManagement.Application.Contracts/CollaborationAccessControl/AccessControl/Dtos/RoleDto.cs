using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl.Dtos
{
    public class RoleDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSystemRole { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}
