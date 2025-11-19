using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IdentityAccess.Users.ValueObjects
{
    public class UserProfile : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Bio { get; private set; }
        public string Avatar { get; private set; }

        public const int MaxBioLength = 500;
        public const int MaxNameLength = 100;

        private UserProfile() { }

        public UserProfile(string? firstName, string? lastName, string? bio = null, string? avatar = null)
        {
            if (firstName != null && firstName.Length > MaxNameLength)
            {
                throw new BusinessException("Users:FirstNameTooLong",
                    $"First name cannot exceed {MaxNameLength} characters.");
            }

            if (lastName != null && lastName.Length > MaxNameLength)
            {
                throw new BusinessException("Users:LastNameTooLong",
                    $"Last name cannot exceed {MaxNameLength} characters.");
            }

            if (bio != null && bio.Length > MaxBioLength)
            {
                throw new BusinessException("Users:BioTooLong",
                    $"Bio cannot exceed {MaxBioLength} characters.");
            }

            FirstName = firstName?.Trim();
            LastName = lastName?.Trim();
            Bio = bio?.Trim();
            Avatar = avatar;
        }

        public static UserProfile Empty() => new UserProfile(null, null);

        public string GetFullName()
        {
            if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
            {
                return string.Empty;
            }
            return $"{FirstName} {LastName}".Trim();
        }

        public string GetInitials()
        {
            var first = !string.IsNullOrWhiteSpace(FirstName) ? FirstName[0].ToString() : "";
            var last = !string.IsNullOrWhiteSpace(LastName) ? LastName[0].ToString() : "";
            return $"{first}{last}".ToUpper();
        }

        public bool HasAvatar => !string.IsNullOrWhiteSpace(Avatar);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FirstName ?? string.Empty;
            yield return LastName ?? string.Empty;
            yield return Bio ?? string.Empty;
            yield return Avatar ?? string.Empty;
        }
    }
}
