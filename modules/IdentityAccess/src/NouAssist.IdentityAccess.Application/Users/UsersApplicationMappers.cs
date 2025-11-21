using NouAssist.IdentityAccess.Users.Aggregates;
using NouAssist.IdentityAccess.Users.Users.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.IdentityAccess.Users
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class UserToUserDtoMapper : MapperBase<User, UserDto>
    {
        [MapperIgnoreTarget(nameof(UserDto.FullName))]
        [MapperIgnoreTarget(nameof(UserDto.DisplayName))]
        [MapperIgnoreTarget(nameof(UserDto.Initials))]
        public override partial UserDto Map(User source);

        public override partial void Map(User source, UserDto destination);

        public override void AfterMap(User source, UserDto destination)
        {
            destination.FullName = source.GetFullName();
            destination.DisplayName = source.GetDisplayName();
            destination.Initials = source.GetInitials();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class UserSettingsToUserSettingsDtoMapper : MapperBase<UserSettings, UserSettingsDto>
    {
        public override partial UserSettingsDto Map(UserSettings source);

        public override partial void Map(UserSettings source, UserSettingsDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class UserEisenhowerPreferenceToUserEisenhowerPreferenceDtoMapper : MapperBase<UserEisenhowerPreference, UserEisenhowerPreferenceDto>
    {
        public override partial UserEisenhowerPreferenceDto Map(UserEisenhowerPreference source);

        public override partial void Map(UserEisenhowerPreference source, UserEisenhowerPreferenceDto destination);
    }
}
