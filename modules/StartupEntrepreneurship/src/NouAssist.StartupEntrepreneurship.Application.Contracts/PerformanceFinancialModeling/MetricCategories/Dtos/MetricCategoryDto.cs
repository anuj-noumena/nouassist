using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricCategories.Dtos
{
    /// <summary>
    /// Output DTO for MetricCategory
    /// </summary>
    public class MetricCategoryDto : PerformanceFinancialModelingFullAuditedDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public int MetricCount { get; set; }
        public int DefinitionCount { get; set; }
    }

    /// <summary>
    /// Input DTO for creating MetricCategory
    /// </summary>
    public class CreateMetricCategoryDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxCategoryNameLength)]
        public string Name { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxCategoryDescriptionLength)]
        public string Description { get; set; }

        [StringLength(7)]
        public string Color { get; set; }

        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// Input DTO for updating MetricCategory
    /// </summary>
    public class UpdateMetricCategoryDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxCategoryNameLength)]
        public string Name { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxCategoryDescriptionLength)]
        public string Description { get; set; }

        [StringLength(7)]
        public string Color { get; set; }

        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// Input DTO for filtering MetricCategory list
    /// </summary>
    public class GetMetricCategoryListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// Input DTO for reordering categories
    /// </summary>
    public class ReorderCategoriesDto
    {
        public List<CategoryOrderItem> Categories { get; set; } = new List<CategoryOrderItem>();
    }

    public class CategoryOrderItem
    {
        public Guid Id { get; set; }
        public int DisplayOrder { get; set; }
    }
}
