using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;

namespace NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos
{
    public class CreateTimeEntryDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DurationMinutes { get; set; }

        [Required]
        [StringLength(FreelanceOperationsConsts.TimeEntry.MaxDescriptionLength)]
        public string Description { get; set; }

        public bool IsBillable { get; set; } = true;
    }
}
