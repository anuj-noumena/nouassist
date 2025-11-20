using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos
{
    public class PitchPresentationDto : EntityDto<Guid>
    {
        public Guid PitchDeckId { get; set; }
        public Guid VersionId { get; set; }
        public string VersionName { get; set; }
        public Guid ProspectId { get; set; }
        public string ProspectName { get; set; }
        public string ProspectCompany { get; set; }
        public PresentationDetailsDto Details { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class PresentationDetailsDto
    {
        public DateTime PresentationDate { get; set; }
        public TimeSpan Duration { get; set; }
        public string Location { get; set; }
        public string MeetingFormat { get; set; }
        public List<string> Attendees { get; set; }
    }
}
