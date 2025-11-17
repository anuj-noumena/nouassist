using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.Learning.Enums;

namespace NouAssist.LearningDevelopment.Learning.Topics.Dtos
{
    public class CreateTopicDto
    {
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public Guid? ParentId { get; set; }

        [Required]
        public bool IsFolder { get; set; }

        [StringLength(128)]
        public string Category { get; set; }

        [Required]
        public DifficultyLevel Difficulty { get; set; }

        [Required]
        [Range(1, 1000)]
        public int EstimatedHours { get; set; }

        [StringLength(2000)]
        public string LearningGoals { get; set; }

        public List<string> Tags { get; set; } = new();
    }
}
