using NouAssist.IdentityAccess.Contacts.Enums;

namespace NouAssist.IdentityAccess.Contacts.Contacts.Dtos
{
    public class SocialProfileDto
    {
        public SocialPlatform Platform { get; set; }
        public string Username { get; set; }
        public string ProfileUrl { get; set; }
    }
}
