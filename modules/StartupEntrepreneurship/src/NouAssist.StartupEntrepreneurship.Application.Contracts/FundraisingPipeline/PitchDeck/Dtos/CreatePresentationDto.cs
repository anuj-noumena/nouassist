using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos
{
    public class CreatePresentationDto
    {
        [Required]
        public Guid VersionId { get; set; }

        [Required]
        public Guid ProspectId { get; set; }

        [Required]
        public CreatePresentationDetailsDto Details { get; set; }
    }

    public class CreatePresentationDetailsDto
    {
        [Required]
        public DateTime PresentationDate { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [StringLength(500)]
        public string Location { get; set; }

        [Required]
        [StringLength(50)]
        public string MeetingFormat { get; set; }

        public List<string> Attendees { get; set; }
    }
}
