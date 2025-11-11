using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IdentityAccess.Team.Roles.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IdentityAccess.Team.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task<RoleDto> GetAsync(Guid id);
        Task<PagedResultDto<RoleDto>> GetListAsync(GetRoleListDto input);
        Task<List<RoleDto>> GetSystemRolesAsync();
        Task<List<RoleDto>> GetUserRolesAsync(Guid userId);
        Task<RoleDto> CreateAsync(CreateRoleDto input);
        Task<RoleDto> UpdateAsync(Guid id, UpdateRoleDto input);
        Task DeleteAsync(Guid id);
        Task<RoleDto> ActivateAsync(Guid id);
        Task<RoleDto> DeactivateAsync(Guid id);
        Task<RoleDto> AddPermissionAsync(Guid id, string permission);
        Task<RoleDto> RemovePermissionAsync(Guid id, string permission);
        Task<UserRoleDto> AssignRoleToUserAsync(AssignRoleToUserDto input);
        Task RevokeRoleFromUserAsync(Guid userId, Guid roleId);
        Task<UserPermissionsDto> GetUserPermissionsAsync(GetUserPermissionsDto input);
        Task<List<UserRoleDto>> GetUserRoleAssignmentsAsync(Guid userId);
        Task<bool> UserHasPermissionAsync(Guid userId, string permission);
    }
}
