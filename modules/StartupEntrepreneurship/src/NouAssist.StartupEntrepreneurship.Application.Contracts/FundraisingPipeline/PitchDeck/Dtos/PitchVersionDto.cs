using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos
{
    public class PitchVersionDto : EntityDto<Guid>
    {
        public Guid PitchDeckId { get; set; }
        public string VersionName { get; set; }
        public int VersionNumber { get; set; }
        public string ChangeNotes { get; set; }
        public DeckContentDto Content { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class DeckContentDto
    {
        public string StorageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public int SlideCount { get; set; }
        public string ContentHash { get; set; }
        public List<string> SlideTopics { get; set; }
    }
}
