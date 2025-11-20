using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.LearningDevelopment.Integration.LearningPaths.Dtos
{
    public class CreateLearningPathDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public Guid TopicId { get; set; }

        [Required]
        public Guid SkillId { get; set; }
    }
}
