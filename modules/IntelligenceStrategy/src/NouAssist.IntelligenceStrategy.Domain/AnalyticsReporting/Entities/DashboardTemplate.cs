using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities
{
    /// <summary>
    /// A template for creating dashboards
    /// </summary>
    public class DashboardTemplate : Entity<Guid>, IMultiTenant
    {
        /// <summary>
        /// Optional tenant ID for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the template
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of the template
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Thumbnail image URL
        /// </summary>
        public string ThumbnailUrl { get; private set; }
        
        /// <summary>
        /// Template configuration as JSON
        /// </summary>
        public string TemplateJson { get; private set; }
        
        /// <summary>
        /// Whether this is a system-provided template
        /// </summary>
        public bool IsSystem { get; private set; }
        
        /// <summary>
        /// Categories this template belongs to
        /// </summary>
        public string[] Categories { get; private set; }
        
        protected DashboardTemplate()
        {
            Categories = Array.Empty<string>();
        }
        
        public DashboardTemplate(
            Guid id,
            string name,
            string description,
            string templateJson,
            Guid? tenantId = null,
            bool isSystem = false,
            string thumbnailUrl = null,
            string[] categories = null)
            : base(id)
        {
            SetName(name);
            SetDescription(description);
            TemplateJson = templateJson;
            TenantId = tenantId;
            IsSystem = isSystem;
            ThumbnailUrl = thumbnailUrl;
            Categories = categories ?? Array.Empty<string>();
        }
        
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), AnalyticsReportingConsts.MaxNameLength);
        }
        
        public void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                AnalyticsReportingConsts.MaxDescriptionLength
            );
        }
        
        public void SetThumbnail(string thumbnailUrl)
        {
            ThumbnailUrl = thumbnailUrl;
        }
        
        public void UpdateTemplateJson(string templateJson)
        {
            TemplateJson = Check.NotNullOrWhiteSpace(templateJson, nameof(templateJson));
        }
        
        public void SetCategories(string[] categories)
        {
            Categories = categories ?? Array.Empty<string>();
        }
    }
}
