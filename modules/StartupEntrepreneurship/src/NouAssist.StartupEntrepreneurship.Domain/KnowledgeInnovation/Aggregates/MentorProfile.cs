namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates
{
    /// <summary>
    /// Aggregate root for mentor profiles
    /// </summary>
    [Table("AppMentorProfiles")]
    public class MentorProfile : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Name { get; private set; }
        public string Bio { get; private set; }
        public string ProfilePictureUrl { get; private set; }
        public string Expertise { get; private set; }
        public int ExperienceYears { get; private set; }
        public List<string> Tags { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<NetworkRelationship> _relationships;
        public IReadOnlyCollection<NetworkRelationship> Relationships => _relationships.AsReadOnly();

        private readonly List<AdviceRecord> _adviceRecords;
        public IReadOnlyCollection<AdviceRecord> AdviceRecords => _adviceRecords.AsReadOnly();

        protected MentorProfile()
        {
            _relationships = new List<NetworkRelationship>();
            _adviceRecords = new List<AdviceRecord>();
            Tags = new List<string>();
        }

        public static MentorProfile Create(
            Guid id,
            string name,
            string bio,
            string expertise,
            int experienceYears,
            string profilePictureUrl = null,
            List<string> tags = null,
            Guid? tenantId = null)
        {
            return new MentorProfile(
                id,
                name,
                bio,
                expertise,
                experienceYears,
                profilePictureUrl,
                tags,
                tenantId);
        }

        private MentorProfile(
            Guid id,
            string name,
            string bio,
            string expertise,
            int experienceYears,
            string profilePictureUrl = null,
            List<string> tags = null,
            Guid? tenantId = null) : base(id)
        {
            SetName(name);
            SetBio(bio);
            Expertise = expertise;
            SetExperienceYears(experienceYears);
            ProfilePictureUrl = profilePictureUrl;
            Tags = tags ?? new List<string>();
            TenantId = tenantId;
            _relationships = new List<NetworkRelationship>();
            _adviceRecords = new List<AdviceRecord>();
        }

        public void UpdateProfile(
            string name,
            string bio,
            string expertise,
            int experienceYears,
            string profilePictureUrl,
            List<string> tags)
        {
            SetName(name);
            SetBio(bio);
            Expertise = expertise;
            SetExperienceYears(experienceYears);
            ProfilePictureUrl = profilePictureUrl;
            Tags = tags ?? new List<string>();
        }

        public void AddRelationship(NetworkRelationship relationship)
        {
            _relationships.Add(relationship);
        }

        public void RemoveRelationship(Guid relatedEntityId)
        {
            var relationshipToRemove = _relationships.FirstOrDefault(r => r.RelatedEntityId == relatedEntityId);
            if (relationshipToRemove != null)
            {
                _relationships.Remove(relationshipToRemove);
            }
        }

        public AdviceRecord AddAdvice(string topic, string content, DateTime givenOn, List<string> tags = null)
        {
            var advice = new AdviceRecord(Id, topic, content, givenOn, tags);
            _adviceRecords.Add(advice);
            return advice;
        }

        public void UpdateAdvice(string topic, string content, DateTime givenOn, List<string> tags)
        {
            var advice = _adviceRecords.FirstOrDefault(a => a.Topic == topic && a.GivenOn == givenOn);
            if (advice == null)
            {
                throw new BusinessRuleViolationException("Advice record not found");
            }

            advice.Update(topic, content, tags);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                KnowledgeInnovationConsts.MentorProfile.NameMinLength,
                KnowledgeInnovationConsts.MentorProfile.NameMaxLength);
        }

        private void SetBio(string bio)
        {
            Bio = Check.NotNullOrWhiteSpace(
                bio,
                nameof(bio),
                maxLength: KnowledgeInnovationConsts.MentorProfile.BioMaxLength);
        }

        private void SetExperienceYears(int experienceYears)
        {
            ExperienceYears = Check.Range(
                experienceYears,
                nameof(experienceYears),
                0,
                KnowledgeInnovationConsts.MentorProfile.ExperienceYearsMax);
        }
    }
}
