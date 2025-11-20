using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos
{
    public class UpdateProgressDto
    {
        [Required]
        [Range(0, 100)]
        public int ProgressPercentage { get; set; }
    }
}
