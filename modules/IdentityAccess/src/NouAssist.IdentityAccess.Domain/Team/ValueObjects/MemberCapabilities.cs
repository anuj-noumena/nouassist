using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Values;

namespace NouAssist.IdentityAccess.Team.ValueObjects
{
    public class MemberCapabilities : ValueObject
    {
        private readonly HashSet<string> _capabilities;
        public IReadOnlySet<string> Capabilities => _capabilities;

        private MemberCapabilities()
        {
            _capabilities = new HashSet<string>();
        }

        public MemberCapabilities(IEnumerable<string> capabilities)
        {
            _capabilities = new HashSet<string>(capabilities ?? Enumerable.Empty<string>());
        }

        public static MemberCapabilities Empty() => new MemberCapabilities();

        public static MemberCapabilities Create(params string[] capabilities)
        {
            return new MemberCapabilities(capabilities);
        }

        public static MemberCapabilities FullAccess()
        {
            return new MemberCapabilities(new[]
            {
                Capability.ManageTeam,
                Capability.ManageMembers,
                Capability.ManageRoles,
                Capability.ManageSettings,
                Capability.InviteMembers,
                Capability.RemoveMembers,
                Capability.ViewMembers,
                Capability.PostContent,
                Capability.DeleteContent,
                Capability.EditContent
            });
        }

        public static MemberCapabilities ReadOnly()
        {
            return new MemberCapabilities(new[]
            {
                Capability.ViewMembers
            });
        }

        public bool Has(string capability) => _capabilities.Contains(capability);

        public bool HasAll(params string[] capabilities)
        {
            return capabilities.All(c => _capabilities.Contains(c));
        }

        public bool HasAny(params string[] capabilities)
        {
            return capabilities.Any(c => _capabilities.Contains(c));
        }

        public MemberCapabilities Add(string capability)
        {
            var newCapabilities = new HashSet<string>(_capabilities) { capability };
            return new MemberCapabilities(newCapabilities);
        }

        public MemberCapabilities Remove(string capability)
        {
            var newCapabilities = new HashSet<string>(_capabilities);
            newCapabilities.Remove(capability);
            return new MemberCapabilities(newCapabilities);
        }

        public int Count => _capabilities.Count;

        protected override IEnumerable<object> GetAtomicValues()
        {
            foreach (var capability in _capabilities.OrderBy(c => c))
            {
                yield return capability;
            }
        }
    }

    public static class Capability
    {
        public const string ManageTeam = "team.manage";
        public const string ManageMembers = "members.manage";
        public const string ManageRoles = "roles.manage";
        public const string ManageSettings = "settings.manage";
        public const string InviteMembers = "members.invite";
        public const string RemoveMembers = "members.remove";
        public const string ViewMembers = "members.view";
        public const string PostContent = "content.post";
        public const string EditContent = "content.edit";
        public const string DeleteContent = "content.delete";
    }
}
