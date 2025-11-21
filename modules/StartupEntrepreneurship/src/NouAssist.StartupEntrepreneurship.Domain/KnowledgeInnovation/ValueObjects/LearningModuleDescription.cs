using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects
{
    /// <summary>
    /// Value object representing the description of a learning module
    /// </summary>
    public class LearningModuleDescription : ValueObject
    {
        public string Overview { get; private set; }
        public string LearningObjectives { get; private set; }
        public string PrerequisiteKnowledge { get; private set; }
        public int EstimatedDurationMinutes { get; private set; }

        private LearningModuleDescription() { }

        public LearningModuleDescription(
            string overview, 
            string learningObjectives, 
            string prerequisiteKnowledge, 
            int estimatedDurationMinutes)
        {
            Overview = Check.NotNullOrWhiteSpace(
                overview, 
                nameof(overview), 
                maxLength: KnowledgeInnovationConsts.LearningResource.DescriptionMaxLength);
            
            LearningObjectives = Check.NotNullOrWhiteSpace(
                learningObjectives, 
                nameof(learningObjectives), 
                maxLength: KnowledgeInnovationConsts.LearningResource.DescriptionMaxLength);
            
            PrerequisiteKnowledge = prerequisiteKnowledge ?? string.Empty;
            
            EstimatedDurationMinutes = Check.Range(
                estimatedDurationMinutes, 
                nameof(estimatedDurationMinutes),
                KnowledgeInnovationConsts.LearningResource.DurationMinutes,
                KnowledgeInnovationConsts.LearningResource.DurationMinutesMax);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Overview;
            yield return LearningObjectives;
            yield return PrerequisiteKnowledge;
            yield return EstimatedDurationMinutes;
        }
    }
}
