using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.MentorProfiles.Dtos
{
    public class AddAdviceDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.DefaultLengths.Title)]
        public string Topic { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.KnowledgeAsset.ContentMaxLength)]
        public string Content { get; set; }

        public List<string> Tags { get; set; }
    }
}
