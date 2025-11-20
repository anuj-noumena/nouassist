using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos
{
    public class UpdateLearningResourceDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.LearningResource.TitleMaxLength, 
            MinimumLength = KnowledgeInnovationConsts.LearningResource.TitleMinLength)]
        public string Title { get; set; }

        [Required]
        public ResourceType ResourceType { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.DefaultLengths.Url)]
        public string ResourceUrl { get; set; }

        [Required]
        public UpdateLearningModuleDescriptionDto Description { get; set; }

        public List<string> Tags { get; set; }
    }

    public class UpdateLearningModuleDescriptionDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.LearningResource.DescriptionMaxLength)]
        public string Overview { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.LearningResource.DescriptionMaxLength)]
        public string LearningObjectives { get; set; }

        [StringLength(KnowledgeInnovationConsts.LearningResource.DescriptionMaxLength)]
        public string PrerequisiteKnowledge { get; set; }

        [Required]
        [Range(KnowledgeInnovationConsts.LearningResource.DurationMinutes, 
            KnowledgeInnovationConsts.LearningResource.DurationMinutesMax)]
        public int EstimatedDurationMinutes { get; set; }
    }
}
