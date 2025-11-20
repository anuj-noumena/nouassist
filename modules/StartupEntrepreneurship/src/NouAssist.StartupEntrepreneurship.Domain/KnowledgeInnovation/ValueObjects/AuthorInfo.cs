namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects
{
    /// <summary>
    /// Value object representing author information for knowledge assets
    /// </summary>
    public class AuthorInfo : ValueObject
    {
        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string AvatarUrl { get; private set; }
        public string Title { get; private set; }

        private AuthorInfo() { }

        public AuthorInfo(Guid userId, string name, string title, string avatarUrl = null)
        {
            UserId = userId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), KnowledgeInnovationConsts.DefaultLengths.Name);
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), KnowledgeInnovationConsts.DefaultLengths.Title);
            AvatarUrl = avatarUrl;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserId;
            yield return Name;
            yield return AvatarUrl;
            yield return Title;
        }
    }
}
