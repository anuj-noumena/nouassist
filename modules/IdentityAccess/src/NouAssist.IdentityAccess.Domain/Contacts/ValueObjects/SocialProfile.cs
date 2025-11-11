using System.Collections.Generic;
using NouAssist.IdentityAccess.Contacts.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IdentityAccess.Contacts.ValueObjects
{
    public class SocialProfile : ValueObject
    {
        public SocialPlatform Platform { get; private set; }
        public string Username { get; private set; }
        public string ProfileUrl { get; private set; }

        protected SocialProfile() { }

        public SocialProfile(SocialPlatform platform, string username, string profileUrl = null)
        {
            Platform = platform;
            Username = Check.NotNullOrWhiteSpace(username, nameof(username));
            ProfileUrl = profileUrl;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Platform;
            yield return Username;
            yield return ProfileUrl;
        }
    }
}
