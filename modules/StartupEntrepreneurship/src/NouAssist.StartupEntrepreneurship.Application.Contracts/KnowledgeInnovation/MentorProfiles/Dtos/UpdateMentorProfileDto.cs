using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.MentorProfiles.Dtos
{
    public class UpdateMentorProfileDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.MentorProfile.NameMaxLength, 
            MinimumLength = KnowledgeInnovationConsts.MentorProfile.NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.MentorProfile.BioMaxLength)]
        public string Bio { get; set; }

        [StringLength(KnowledgeInnovationConsts.DefaultLengths.Url)]
        public string ProfilePictureUrl { get; set; }

        [Required]
        [StringLength(500)]
        public string Expertise { get; set; }

        [Required]
        [Range(0, KnowledgeInnovationConsts.MentorProfile.ExperienceYearsMax)]
        public int ExperienceYears { get; set; }

        public List<string> Tags { get; set; }
    }
}
