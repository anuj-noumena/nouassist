using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Team.Aggregates;
using NouAssist.Team.Data;
using NouAssist.Team.Entities;
using NouAssist.Team.Permissions;
using NouAssist.Team.Roles.Dtos;
using NouAssist.Team.Services;
using NouAssist.Team.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Specifications;

namespace NouAssist.Team.Roles
{
    [Authorize(TeamPermissions.Roles.Default)]
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly RolePermissionService _permissionService;

        public RoleAppService(
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            RolePermissionService permissionService)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _permissionService = permissionService;
        }

        public async Task<RoleDto> GetAsync(Guid id)
        {
            var role = await _roleRepository.GetAsync(id);
            return await MapToDto(role);
        }

        public async Task<PagedResultDto<RoleDto>> GetListAsync(GetRoleListDto input)
        {
            var spec = CreateListSpecification(input);

            var roles = await _roleRepository.GetListAsync(spec);
            var totalCount = await _roleRepository.CountAsync(spec);

            var dtos = new List<RoleDto>();
            foreach (var role in roles)
            {
                dtos.Add(await MapToDto(role));
            }

            return new PagedResultDto<RoleDto>(totalCount, dtos);
        }

        public async Task<List<RoleDto>> GetSystemRolesAsync()
        {
            var roles = await _roleRepository.GetSystemRolesAsync();
            
            var dtos = new List<RoleDto>();
            foreach (var role in roles)
            {
                dtos.Add(await MapToDto(role));
            }

            return dtos;
        }

        public async Task<List<RoleDto>> GetUserRolesAsync(Guid userId)
        {
            var roles = await _roleRepository.GetUserRolesAsync(userId);
            
            var dtos = new List<RoleDto>();
            foreach (var role in roles)
            {
                dtos.Add(await MapToDto(role));
            }

            return dtos;
        }

        [Authorize(TeamPermissions.Roles.Create)]
        public async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            var role = new Role(
                GuidGenerator.Create(),
                input.Name,
                CurrentTenant.Id,
                input.Description,
                false
            );

            if (input.Permissions != null && input.Permissions.Any())
            {
                foreach (var permission in input.Permissions)
                {
                    role.AddPermission(permission);
                }
            }

            await _roleRepository.InsertAsync(role);

            return await MapToDto(role);
        }

        [Authorize(TeamPermissions.Roles.Update)]
        public async Task<RoleDto> UpdateAsync(Guid id, UpdateRoleDto input)
        {
            var role = await _roleRepository.GetAsync(id);

            role.SetName(input.Name);
            role.SetDescription(input.Description);

            // Update permissions
            if (input.Permissions != null)
            {
                // Remove permissions not in the new list
                var permissionsToRemove = role.Permissions
                    .Where(p => !input.Permissions.Contains(p))
                    .ToList();

                foreach (var permission in permissionsToRemove)
                {
                    role.RemovePermission(permission);
                }

                // Add new permissions
                foreach (var permission in input.Permissions)
                {
                    if (!role.HasPermission(permission))
                    {
                        role.AddPermission(permission);
                    }
                }
            }

            await _roleRepository.UpdateAsync(role);

            return await MapToDto(role);
        }

        [Authorize(TeamPermissions.Roles.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleRepository.GetAsync(id);

            if (role.IsSystem)
            {
                throw new UserFriendlyException("Cannot delete system roles");
            }

            await _roleRepository.DeleteAsync(id);
        }

        [Authorize(TeamPermissions.Roles.Update)]
        public async Task<RoleDto> ActivateAsync(Guid id)
        {
            var role = await _roleRepository.GetAsync(id);
            role.Activate();
            await _roleRepository.UpdateAsync(role);

            return await MapToDto(role);
        }

        [Authorize(TeamPermissions.Roles.Update)]
        public async Task<RoleDto> DeactivateAsync(Guid id)
        {
            var role = await _roleRepository.GetAsync(id);
            role.Deactivate();
            await _roleRepository.UpdateAsync(role);

            return await MapToDto(role);
        }

        [Authorize(TeamPermissions.Roles.Update)]
        public async Task<RoleDto> AddPermissionAsync(Guid id, string permission)
        {
            var role = await _roleRepository.GetAsync(id);
            role.AddPermission(permission);
            await _roleRepository.UpdateAsync(role);

            return await MapToDto(role);
        }

        [Authorize(TeamPermissions.Roles.Update)]
        public async Task<RoleDto> RemovePermissionAsync(Guid id, string permission)
        {
            var role = await _roleRepository.GetAsync(id);
            role.RemovePermission(permission);
            await _roleRepository.UpdateAsync(role);

            return await MapToDto(role);
        }

        [Authorize(TeamPermissions.Roles.AssignToUsers)]
        public async Task<UserRoleDto> AssignRoleToUserAsync(AssignRoleToUserDto input)
        {
            var role = await _roleRepository.GetAsync(input.RoleId);

            var userRole = _permissionService.AssignRoleToUser(
                input.UserId,
                role,
                CurrentUser.Id.Value,
                CurrentTenant.Id,
                input.ExpiresAt,
                input.Scope,
                input.ScopeId
            );

            await _userRoleRepository.InsertAsync(userRole);

            var dto = ObjectMapper.Map<UserRole, UserRoleDto>(userRole);
            dto.UserName = $"User-{input.UserId}";
            dto.RoleName = role.Name;
            return dto;
        }

        [Authorize(TeamPermissions.Roles.AssignToUsers)]
        public async Task RevokeRoleFromUserAsync(Guid userId, Guid roleId)
        {
            var userRoles = await _userRoleRepository.GetUserRolesAsync(userId);
            var userRole = userRoles.FirstOrDefault(ur => ur.RoleId == roleId && ur.IsActive);

            if (userRole != null)
            {
                userRole.Deactivate();
                await _userRoleRepository.UpdateAsync(userRole);
            }
        }

        public async Task<UserPermissionsDto> GetUserPermissionsAsync(GetUserPermissionsDto input)
        {
            var userRoles = await _userRoleRepository.GetActiveUserRolesAsync(input.UserId);
            var roles = await _roleRepository.GetListAsync(
                x => userRoles.Select(ur => ur.RoleId).Contains(x.Id));

            var permissions = _permissionService.GetUserPermissions(
                input.UserId,
                userRoles,
                roles
            );

            return new UserPermissionsDto
            {
                UserId = input.UserId,
                Permissions = permissions,
                Roles = ObjectMapper.Map<List<Role>, List<RoleDto>>(roles)
            };
        }

        public async Task<List<UserRoleDto>> GetUserRoleAssignmentsAsync(Guid userId)
        {
            var userRoles = await _userRoleRepository.GetUserRolesAsync(userId);
            var roleIds = userRoles.Select(ur => ur.RoleId).Distinct().ToList();
            var roles = await _roleRepository.GetListAsync(x => roleIds.Contains(x.Id));

            var dtos = new List<UserRoleDto>();
            foreach (var userRole in userRoles)
            {
                var dto = ObjectMapper.Map<UserRole, UserRoleDto>(userRole);
                dto.UserName = $"User-{userId}";
                
                var role = roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                if (role != null)
                {
                    dto.RoleName = role.Name;
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<bool> UserHasPermissionAsync(Guid userId, string permission)
        {
            var userRoles = await _userRoleRepository.GetActiveUserRolesAsync(userId);
            var roles = await _roleRepository.GetListAsync(
                x => userRoles.Select(ur => ur.RoleId).Contains(x.Id));

            return _permissionService.UserHasPermission(userId, permission, userRoles, roles);
        }

        private ISpecification<Role> CreateListSpecification(GetRoleListDto input)
        {
            var spec = Specification.None<Role>();

            if (input.IsActive.HasValue)
            {
                if (input.IsActive.Value)
                {
                    spec = spec.And(new ActiveRolesSpecification());
                }
                else
                {
                    spec = spec.And(new Specification<Role>(r => !r.IsActive));
                }
            }

            if (input.IsSystem.HasValue)
            {
                if (input.IsSystem.Value)
                {
                    spec = spec.And(new SystemRolesSpecification());
                }
                else
                {
                    spec = spec.And(new CustomRolesSpecification());
                }
            }

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                spec = spec.And(new Specification<Role>(
                    r => r.Name.Contains(input.Filter) || 
                         (r.Description != null && r.Description.Contains(input.Filter))));
            }

            return spec;
        }

        private async Task<RoleDto> MapToDto(Role role)
        {
            var dto = ObjectMapper.Map<Role, RoleDto>(role);

            // Get current user count
            var assignments = await _userRoleRepository.GetRoleAssignmentsAsync(role.Id);
            dto.CurrentUserCount = assignments.Count(ur => ur.IsActive && !ur.IsExpired);

            return dto;
        }
    }
}
