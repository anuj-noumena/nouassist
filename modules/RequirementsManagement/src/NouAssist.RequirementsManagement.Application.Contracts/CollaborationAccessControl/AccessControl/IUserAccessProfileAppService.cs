using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl.Dtos;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl
{
    public interface IUserAccessProfileAppService : IApplicationService
    {
        Task<UserAccessProfileDto> GetAsync(Guid id);
        Task<UserAccessProfileDto> GetByUserIdAsync(Guid userId);
        Task<PagedResultDto<UserAccessProfileDto>> GetListAsync(GetUserAccessProfileListDto input);
        
        Task<RoleDto> CreateRoleAsync(Guid userProfileId, CreateRoleDto input);
        Task<RoleDto> UpdateRoleAsync(Guid userProfileId, Guid roleId, UpdateRoleDto input);
        Task DeleteRoleAsync(Guid userProfileId, Guid roleId);
        
        Task AddPermissionToRoleAsync(Guid userProfileId, Guid roleId, AddPermissionToRoleDto input);
        Task RemovePermissionFromRoleAsync(Guid userProfileId, Guid roleId, PermissionType permissionType, TargetResourceType resourceType, Guid? resourceId);
        
        Task<bool> HasPermissionAsync(Guid userId, PermissionType permissionType, TargetResourceType resourceType, Guid resourceId);
    }
}
