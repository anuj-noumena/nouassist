namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants
{
    /// <summary>
    /// Constants for the Knowledge & Innovation module
    /// </summary>
    public static class KnowledgeInnovationConsts
    {
        /// <summary>
        /// Default length constraints for string properties
        /// </summary>
        public static class DefaultLengths
        {
            public const int Title = 128;
            public const int ShortDescription = 256;
            public const int Name = 64;
            public const int Tag = 32;
            public const int Url = 2048;
        }

        /// <summary>
        /// Length constraints for Knowledge Assets
        /// </summary>
        public static class KnowledgeAsset
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = DefaultLengths.Title;
            public const int DescriptionMaxLength = 1000;
            public const int ContentMaxLength = 100000; // 100KB for rich text content
        }

        /// <summary>
        /// Length constraints for Learning Resources
        /// </summary>
        public static class LearningResource
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = DefaultLengths.Title;
            public const int DescriptionMaxLength = 2000;
            public const int DurationMinutes = 1;
            public const int DurationMinutesMax = 1440; // 24 hours max
        }

        /// <summary>
        /// Length constraints for Ideas
        /// </summary>
        public static class Idea
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = DefaultLengths.Title;
            public const int DescriptionMaxLength = 2000;
        }

        /// <summary>
        /// Length constraints for Decision Records
        /// </summary>
        public static class DecisionRecord
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = DefaultLengths.Title;
            public const int ContextMaxLength = 3000;
            public const int RationaleMaxLength = 2000;
            public const int OutcomeMaxLength = 2000;
        }

        /// <summary>
        /// Length constraints for Mentor Profiles
        /// </summary>
        public static class MentorProfile
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = DefaultLengths.Name;
            public const int BioMaxLength = 1000;
            public const int ExperienceYearsMax = 100;
        }
    }
}
