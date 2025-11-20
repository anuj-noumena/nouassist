namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Specifications
{
    /// <summary>
    /// Base specification class for MentorProfile aggregate
    /// </summary>
    public abstract class MentorProfileSpecification : Specification<MentorProfile>
    {
    }

    /// <summary>
    /// Specification for mentor profiles with specific expertise
    /// </summary>
    public class MentorExpertiseSpecification : MentorProfileSpecification
    {
        private readonly string _expertiseKeyword;

        public MentorExpertiseSpecification(string expertiseKeyword)
        {
            _expertiseKeyword = expertiseKeyword.ToLower();
        }

        public override Expression<Func<MentorProfile, bool>> ToExpression()
        {
            return mentor => mentor.Expertise.ToLower().Contains(_expertiseKeyword);
        }
    }

    /// <summary>
    /// Specification for experienced mentors with a minimum years of experience
    /// </summary>
    public class ExperiencedMentorSpecification : MentorProfileSpecification
    {
        private readonly int _minimumYears;

        public ExperiencedMentorSpecification(int minimumYears)
        {
            _minimumYears = minimumYears;
        }

        public override Expression<Func<MentorProfile, bool>> ToExpression()
        {
            return mentor => mentor.ExperienceYears >= _minimumYears;
        }
    }

    /// <summary>
    /// Specification for mentors with specific tags
    /// </summary>
    public class MentorWithTagSpecification : MentorProfileSpecification
    {
        private readonly string _tag;

        public MentorWithTagSpecification(string tag)
        {
            _tag = tag;
        }

        public override Expression<Func<MentorProfile, bool>> ToExpression()
        {
            return mentor => mentor.Tags.Contains(_tag);
        }
    }
}
