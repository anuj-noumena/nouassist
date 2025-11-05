using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Users.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Users.Data
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> FindByEmailAsync(string email);
        Task<List<User>> GetActiveUsersAsync();
    }

    public interface IUserSettingsRepository : IRepository<UserSettings, Guid>
    {
        Task<UserSettings?> FindByUserIdAsync(Guid userId);
    }

    public interface IUserEisenhowerPreferenceRepository : IRepository<UserEisenhowerPreference, Guid>
    {
        Task<UserEisenhowerPreference?> FindByUserIdAsync(Guid userId);
    }
}
