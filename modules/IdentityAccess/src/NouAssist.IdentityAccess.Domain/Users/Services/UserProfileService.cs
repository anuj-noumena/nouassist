using System;
using NouAssist.IdentityAccess.Users.Aggregates;
using Volo.Abp.Domain.Services;

namespace NouAssist.IdentityAccess.Users.Services
{
    public class UserProfileService : DomainService
    {
        public bool IsProfileComplete(User user)
        {
            return !string.IsNullOrWhiteSpace(user.FirstName) &&
                   !string.IsNullOrWhiteSpace(user.LastName) &&
                   !string.IsNullOrWhiteSpace(user.Email);
        }

        public int CalculateProfileCompleteness(User user)
        {
            var score = 0;
            var totalFields = 6;

            if (!string.IsNullOrWhiteSpace(user.FirstName)) score++;
            if (!string.IsNullOrWhiteSpace(user.LastName)) score++;
            if (!string.IsNullOrWhiteSpace(user.Email)) score++;
            if (!string.IsNullOrWhiteSpace(user.Bio)) score++;
            if (!string.IsNullOrWhiteSpace(user.Avatar)) score++;
            if (!string.IsNullOrWhiteSpace(user.Timezone)) score++;

            return (int)((double)score / totalFields * 100);
        }

        public string GenerateDefaultAvatar(User user)
        {
            // Generate a simple avatar URL based on initials
            var initials = user.GetInitials();
            return $"https://ui-avatars.com/api/?name={initials}&background=random";
        }

        public bool HasRecentActivity(User user, int daysThreshold = 30)
        {
            if (!user.LastLoginAt.HasValue)
            {
                return false;
            }

            return (Clock.Now - user.LastLoginAt.Value).Days <= daysThreshold;
        }
    }
}
