using NouAssist.CollaborationHub.ContactManagement.Enums;

namespace NouAssist.CollaborationHub.ContactManagement.Dtos
{
    public class SocialProfileDto
    {
        public SocialPlatform Platform { get; set; }
        public string Username { get; set; }
        public string ProfileUrl { get; set; }
    }
}
