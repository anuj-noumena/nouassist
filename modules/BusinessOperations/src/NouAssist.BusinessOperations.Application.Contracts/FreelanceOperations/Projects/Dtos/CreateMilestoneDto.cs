using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos
{
    public class CreateMilestoneDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string Currency { get; set; }

        public string AcceptanceCriteria { get; set; }

        public DateTime? Deadline { get; set; }
    }
}
