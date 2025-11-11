using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Team.Roles.Dtos
{
    public class RoleDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? OrganizationId { get; set; }
        public List<string> Permissions { get; set; }
        public bool IsSystem { get; set; }
        public bool IsActive { get; set; }
        public int? MaxUsers { get; set; }
        public int CurrentUserCount { get; set; }
    }

    public class CreateRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Permissions { get; set; }
    }

    public class UpdateRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Permissions { get; set; }
    }

    public class AssignRoleToUserDto
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string Scope { get; set; }
        public Guid? ScopeId { get; set; }
    }

    public class UserRoleDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid? AssignedBy { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string Scope { get; set; }
        public Guid? ScopeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
    }

    public class GetRoleListDto : PagedAndSortedResultRequestDto
    {
        public bool? IsActive { get; set; }
        public bool? IsSystem { get; set; }
        public string Filter { get; set; }
    }

    public class GetUserPermissionsDto
    {
        public Guid UserId { get; set; }
        public string Scope { get; set; }
        public Guid? ScopeId { get; set; }
    }

    public class UserPermissionsDto
    {
        public Guid UserId { get; set; }
        public List<string> Permissions { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}
