using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Ideas.Dtos
{
    public class CreateIdeaDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.Idea.TitleMaxLength, 
            MinimumLength = KnowledgeInnovationConsts.Idea.TitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.Idea.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public CreateIdeaSourceDto Source { get; set; }

        public List<string> Tags { get; set; }

        [StringLength(KnowledgeInnovationConsts.DefaultLengths.ShortDescription)]
        public string ImpactStatement { get; set; }

        [Range(1, 10)]
        public int Priority { get; set; } = 5;
    }

    public class CreateIdeaSourceDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.DefaultLengths.Name)]
        public string SourceName { get; set; }

        [StringLength(KnowledgeInnovationConsts.DefaultLengths.ShortDescription)]
        public string SourceDescription { get; set; }

        public DateTime SourcedDate { get; set; } = DateTime.UtcNow;
    }
}
