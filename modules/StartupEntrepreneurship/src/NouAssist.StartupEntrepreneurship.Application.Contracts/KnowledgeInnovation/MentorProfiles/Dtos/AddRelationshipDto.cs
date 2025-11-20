using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.MentorProfiles.Dtos
{
    public class AddRelationshipDto
    {
        [Required]
        public Guid RelatedEntityId { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.DefaultLengths.Name)]
        public string RelatedEntityName { get; set; }

        [Required]
        public NetworkRelationshipType RelationshipType { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
