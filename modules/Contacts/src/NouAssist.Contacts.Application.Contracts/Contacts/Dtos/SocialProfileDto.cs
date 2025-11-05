using NouAssist.Contacts.Enums;

namespace NouAssist.Contacts.Dtos
{
    public class SocialProfileDto
    {
        public SocialPlatform Platform { get; set; }
        public string Username { get; set; }
        public string ProfileUrl { get; set; }
    }
}
