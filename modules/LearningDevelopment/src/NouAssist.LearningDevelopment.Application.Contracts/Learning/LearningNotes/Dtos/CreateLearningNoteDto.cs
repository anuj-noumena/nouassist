using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.Learning.Enums;

namespace NouAssist.LearningDevelopment.Learning.LearningNotes.Dtos
{
    public class CreateLearningNoteDto
    {
        [Required]
        public Guid TopicId { get; set; }

        [Required]
        public LearningNoteType Type { get; set; }

        [StringLength(2000)]
        public string Questions { get; set; }

        [StringLength(8000)]
        public string Notes { get; set; }

        [StringLength(4000)]
        public string Summary { get; set; }

        public string Content { get; set; }

        public List<string> KeyPoints { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
