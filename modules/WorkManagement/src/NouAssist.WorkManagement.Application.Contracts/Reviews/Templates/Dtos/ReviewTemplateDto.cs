using System;
using System.Collections.Generic;
using NouAssist.Reviews.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Reviews.Templates.Dtos
{
    public class ReviewTemplateDto : FullAuditedEntityDto<Guid>
    {
        public ReviewType ReviewType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsSystem { get; set; }
        public List<ReviewChecklistStepDto> ChecklistItems { get; set; }
        public List<string> ReflectionPrompts { get; set; }
        public string RecommendedFrequency { get; set; }
        public int? EstimatedMinutes { get; set; }
        public bool IsCustomTemplate { get; set; }
        public bool HasChecklist { get; set; }
        public bool HasReflectionPrompts { get; set; }
        public int TotalChecklistItems { get; set; }
    }

    public class ReviewChecklistStepDto
    {
        public int Step { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Items { get; set; }
        public int ItemCount { get; set; }
    }

    public class CreateReviewTemplateDto
    {
        [Required]
        public ReviewType ReviewType { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(100)]
        public string RecommendedFrequency { get; set; }

        [Range(0, 1440)]
        public int? EstimatedMinutes { get; set; }
    }

    public class UpdateReviewTemplateDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(100)]
        public string RecommendedFrequency { get; set; }

        [Range(0, 1440)]
        public int? EstimatedMinutes { get; set; }
    }

    public class AddChecklistStepDto
    {
        [Required]
        public int Step { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public List<string> Items { get; set; }
    }

    public class GetTemplateListDto : PagedAndSortedResultRequestDto
    {
        public ReviewType? ReviewType { get; set; }
        public bool? IsSystem { get; set; }
    }
}
