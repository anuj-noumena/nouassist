using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl.Dtos;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Exceptions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Integration;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Permissions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl
{
    [Authorize(CollaborationPermissions.AccessControl.Default)]
    public class UserAccessProfileAppService : ApplicationService, IUserAccessProfileAppService
    {
        private readonly IUserAccessProfileRepository _userAccessProfileRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IIdentityUserLookupService _userLookupService;
        private readonly IResourceDisplayNameProvider _resourceDisplayNameProvider;

        public UserAccessProfileAppService(
            IUserAccessProfileRepository userAccessProfileRepository,
            IAuthorizationService authorizationService,
            IIdentityUserLookupService userLookupService,
            IResourceDisplayNameProvider resourceDisplayNameProvider)
        {
            _userAccessProfileRepository = userAccessProfileRepository;
            _authorizationService = authorizationService;
            _userLookupService = userLookupService;
            _resourceDisplayNameProvider = resourceDisplayNameProvider;
        }

        [Authorize(CollaborationPermissions.AccessControl.ViewPermissions)]
        public virtual async Task<UserAccessProfileDto> GetAsync(Guid id)
        {
            try
            {
                var userProfile = await _userAccessProfileRepository.GetAsync(id);
                var dto = ObjectMapper.Map<UserAccessProfile, UserAccessProfileDto>(userProfile);

                // Enrich with user name
                var user = await _userLookupService.FindByIdAsync(userProfile.UserId.Value);
                if (user != null)
                {
                    dto.UserName = user.UserName;
                }

                // Enrich permissions with resource display names
                foreach (var role in dto.Roles)
                {
                    foreach (var permission in role.Permissions)
                    {
                        permission.ResourceDisplayName = await _resourceDisplayNameProvider
                            .GetResourceDisplayNameAsync(permission.ResourceType, permission.ResourceId);
                    }
                }

                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotFindUserAccessProfile"]);
            }
        }

        [Authorize(CollaborationPermissions.AccessControl.ViewPermissions)]
        public virtual async Task<UserAccessProfileDto> GetByUserIdAsync(Guid userId)
        {
            try
            {
                var userProfile = await _userAccessProfileRepository.GetByUserIdAsync(userId);
                if (userProfile == null)
                {
                    throw new EntityNotFoundException(typeof(UserAccessProfile), userId);
                }

                return await GetAsync(userProfile.Id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotFindUserAccessProfile"]);
            }
        }

        [Authorize(CollaborationPermissions.AccessControl.ViewPermissions)]
        public virtual async Task<PagedResultDto<UserAccessProfileDto>> GetListAsync(GetUserAccessProfileListDto input)
        {
            try
            {
                var query = await _userAccessProfileRepository.GetQueryableAsync();
                query = query.OrderBy(x => x.Id);

                var totalCount = await AsyncExecuter.CountAsync(query);
                
                query = query.PageBy(input);
                
                var userProfiles = await AsyncExecuter.ToListAsync(query);
                var userProfileDtos = ObjectMapper.Map<List<UserAccessProfile>, List<UserAccessProfileDto>>(userProfiles);

                // Enrich with user names
                foreach (var dto in userProfileDtos)
                {
                    var user = await _userLookupService.FindByIdAsync(dto.UserId);
                    if (user != null)
                    {
                        dto.UserName = user.UserName;
                    }
                }

                return new PagedResultDto<UserAccessProfileDto>(totalCount, userProfileDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotListUserAccessProfiles"]);
            }
        }

        [Authorize(CollaborationPermissions.AccessControl.ManageRoles)]
        public virtual async Task<RoleDto> CreateRoleAsync(Guid userProfileId, CreateRoleDto input)
        {
            try
            {
                var userProfile = await _userAccessProfileRepository.GetAsync(userProfileId);
                var role = userProfile.CreateRole(
                    GuidGenerator.Create(),
                    new RoleName(input.Name),
                    input.Description
                );

                await _userAccessProfileRepository.UpdateAsync(userProfile);
                
                return ObjectMapper.Map<Role, RoleDto>(role);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is CollaborationDomainException domainException && domainException.Code == "Collaboration:ErrorMessage:DuplicateRole")
                {
                    throw new UserFriendlyException(L["Error:DuplicateRole", input.Name]);
                }
                throw new UserFriendlyException(L["Error:CouldNotCreateRole"]);
            }
        }

        [Authorize(CollaborationPermissions.AccessControl.ManageRoles)]
        public virtual async Task<RoleDto> UpdateRoleAsync(Guid userProfileId, Guid roleId, UpdateRoleDto input)
        {
            try
            {
                var userProfile = await _userAccessProfileRepository.GetAsync(userProfileId);
                userProfile.UpdateRole(roleId, new RoleName(input.Name), input.Description);
                
                await _userAccessProfileRepository.UpdateAsync(userProfile);
                
                var updatedRole = userProfile.Roles.FirstOrDefault(r => r.Id == roleId);
                return ObjectMapper.Map<Role, RoleDto>(updatedRole);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is CollaborationDomainException domainException && domainException.Code == "Collaboration:ErrorMessage:InvalidOperation")
                {
                    throw new UserFriendlyException(L["Error:CannotModifySystemRole"]);
                }
                throw new UserFriendlyException(L["Error:CouldNotUpdateRole"]);
            }
        }

        [Authorize(CollaborationPermissions.AccessControl.ManageRoles)]
        public virtual async Task DeleteRoleAsync(Guid userProfileId, Guid roleId)
        {
            try
            {
                var userProfile = await _userAccessProfileRepository.GetAsync(userProfileId);
                userProfile.DeleteRole(roleId);
                
                await _userAccessProfileRepository.UpdateAsync(userProfile);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is CollaborationDomainException domainException && domainException.Code == "Collaboration:ErrorMessage:InvalidOperation")
                {
                    throw new UserFriendlyException(L["Error:CannotDeleteSystemRole"]);
                }
                throw new UserFriendlyException(L["Error:CouldNotDeleteRole"]);
            }
        }

        [Authorize(CollaborationPermissions.AccessControl.ManagePermissions)]
        public virtual async Task AddPermissionToRoleAsync(Guid userProfileId, Guid roleId, AddPermissionToRoleDto input)
        {
            try
            {
                var userProfile = await _userAccessProfileRepository.GetAsync(userProfileId);
                
                var targetResource = input.ResourceId.HasValue
                    ? new TargetResource(input.ResourceType, input.ResourceId.Value)
                    : new TargetResource(input.ResourceType);
                
                userProfile.AddPermissionToRole(roleId, input.PermissionType, targetResource);
                
                await _userAccessProfileRepository.UpdateAsync(userProfile);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotAddPermission"]);
            }
        }

        [Authorize(CollaborationPermissions.AccessControl.ManagePermissions)]
        public virtual async Task RemovePermissionFromRoleAsync(
            Guid userProfileId, 
            Guid roleId, 
            PermissionType permissionType, 
            TargetResourceType resourceType, 
            Guid? resourceId)
        {
            try
            {
                var userProfile = await _userAccessProfileRepository.GetAsync(userProfileId);
                
                var targetResource = resourceId.HasValue
                    ? new TargetResource(resourceType, resourceId.Value)
                    : new TargetResource(resourceType);
                
                userProfile.RemovePermissionFromRole(roleId, permissionType, targetResource);
                
                await _userAccessProfileRepository.UpdateAsync(userProfile);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotRemovePermission"]);
            }
        }

        [AllowAnonymous]
        public virtual async Task<bool> HasPermissionAsync(
            Guid userId, 
            PermissionType permissionType, 
            TargetResourceType resourceType, 
            Guid resourceId)
        {
            try
            {
                return await _authorizationService.HasPermissionAsync(userId, permissionType, resourceType, resourceId);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                return false;
            }
        }

        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, 
                "Error in UserAccessProfileAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
        }
    }
}
