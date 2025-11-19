using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IdentityAccess.Team.ValueObjects
{
    public class TeamRole : ValueObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Level { get; private set; }

        public const int MinLevel = 1;
        public const int MaxLevel = 10;

        private TeamRole() { }

        public TeamRole(string name, string? description = null, int level = 1)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessException("Team:RoleNameRequired",
                    "Team role name is required.");
            }

            if (level < MinLevel || level > MaxLevel)
            {
                throw new BusinessException("Team:InvalidRoleLevel",
                    $"Team role level must be between {MinLevel} and {MaxLevel}.")
                    .WithData("minLevel", MinLevel)
                    .WithData("maxLevel", MaxLevel);
            }

            Name = name.Trim();
            Description = description?.Trim();
            Level = level;
        }

        public static TeamRole Owner() => new TeamRole("Owner", "Team owner with full control", 10);
        public static TeamRole Admin() => new TeamRole("Admin", "Team administrator", 8);
        public static TeamRole Moderator() => new TeamRole("Moderator", "Team moderator", 5);
        public static TeamRole Member() => new TeamRole("Member", "Regular team member", 1);
        public static TeamRole Guest() => new TeamRole("Guest", "Guest with limited access", 1);

        public bool IsOwner => Level >= 10;
        public bool IsAdmin => Level >= 8;
        public bool IsModerator => Level >= 5;

        public bool HasHigherLevelThan(TeamRole other) => Level > other.Level;
        public bool HasSameLevelAs(TeamRole other) => Level == other.Level;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description ?? string.Empty;
            yield return Level;
        }

        public override string ToString() => Name;
    }
}
