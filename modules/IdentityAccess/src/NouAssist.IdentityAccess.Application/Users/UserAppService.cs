using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.IdentityAccess.Users.Aggregates;
using NouAssist.IdentityAccess.Users.Data;
using NouAssist.IdentityAccess.Users.Permissions;
using NouAssist.IdentityAccess.Users.Users;
using NouAssist.IdentityAccess.Users.Users.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IdentityAccess.Users
{
    [Authorize(UsersPermissions.Users.Default)]
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IUserRepository _userRepository;

        public UserAppService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(UsersPermissions.Users.View)]
        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            return ObjectMapper.Map<User, UserDto>(user);
        }

        public async Task<UserDto> GetMyProfileAsync()
        {
            var userId = CurrentUser.GetId();
            var user = await _userRepository.GetAsync(userId);
            return ObjectMapper.Map<User, UserDto>(user);
        }

        [Authorize(UsersPermissions.Users.UpdateProfile)]
        public async Task<UserDto> UpdateProfileAsync(UpdateUserProfileDto input)
        {
            var userId = CurrentUser.GetId();
            var user = await _userRepository.GetAsync(userId);

            user.UpdateProfile(
                input.FirstName,
                input.LastName,
                input.Bio,
                input.Avatar
            );

            await _userRepository.UpdateAsync(user);
            return ObjectMapper.Map<User, UserDto>(user);
        }

        public async Task UpdateLastLoginAsync()
        {
            var userId = CurrentUser.GetId();
            var user = await _userRepository.GetAsync(userId);
            
            user.UpdateLastLogin();
            await _userRepository.UpdateAsync(user);
        }

        [Authorize(UsersPermissions.Users.Update)]
        public async Task ActivateAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            user.Activate();
            await _userRepository.UpdateAsync(user);
        }

        [Authorize(UsersPermissions.Users.Update)]
        public async Task DeactivateAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            user.Deactivate();
            await _userRepository.UpdateAsync(user);
        }
    }
}
