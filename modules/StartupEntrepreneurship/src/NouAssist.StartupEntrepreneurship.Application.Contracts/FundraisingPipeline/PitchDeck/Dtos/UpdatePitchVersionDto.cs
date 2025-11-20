using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos
{
    public class UpdatePitchVersionDto
    {
        [Required]
        [StringLength(100)]
        public string VersionName { get; set; }

        [Required]
        public UpdateDeckContentDto Content { get; set; }

        [StringLength(1000)]
        public string ChangeNotes { get; set; }
    }

    public class UpdateDeckContentDto
    {
        [Required]
        [Url]
        [StringLength(1000)]
        public string StorageUrl { get; set; }

        [Url]
        [StringLength(1000)]
        public string ThumbnailUrl { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Slide count must be positive.")]
        public int SlideCount { get; set; }

        [Required]
        [StringLength(100)]
        public string ContentHash { get; set; }

        public List<string> SlideTopics { get; set; }
    }
}
