using System;
using Volo.Abp;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Exceptions
{
    public class SkillDevelopmentDomainException : BusinessException
    {
        public SkillDevelopmentDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static SkillDevelopmentDomainException InvalidProficiencyLevel()
        {
            return new SkillDevelopmentDomainException(
                "SkillDevelopment:InvalidProficiencyLevel",
                "Proficiency level must be between 0 and 100.");
        }

        public static SkillDevelopmentDomainException SkillNotFound(Guid skillId)
        {
            return new SkillDevelopmentDomainException(
                "SkillDevelopment:SkillNotFound",
                $"Skill with ID {skillId} not found.");
        }

        public static SkillDevelopmentDomainException MilestoneAlreadyCompleted()
        {
            return new SkillDevelopmentDomainException(
                "SkillDevelopment:MilestoneAlreadyCompleted",
                "Skill milestone has already been completed.");
        }

        public static SkillDevelopmentDomainException InvalidSkillTreeHierarchy()
        {
            return new SkillDomainException(
                "SkillDevelopment:InvalidSkillTreeHierarchy",
                "Invalid skill tree hierarchy. Circular dependencies detected.");
        }

        public static SkillDevelopmentDomainException SessionNotActive()
        {
            return new SkillDevelopmentDomainException(
                "SkillDevelopment:SessionNotActive",
                "Skill development session is not currently active.");
        }
    }
}
