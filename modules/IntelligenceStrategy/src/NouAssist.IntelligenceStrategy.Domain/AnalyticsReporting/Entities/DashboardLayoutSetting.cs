using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities
{
    /// <summary>
    /// Settings for a dashboard layout
    /// </summary>
    public class DashboardLayoutSetting : Entity
    {
        /// <summary>
        /// The dashboard this layout setting belongs to
        /// </summary>
        public Guid DashboardId { get; private set; }
        
        /// <summary>
        /// Number of columns in the layout
        /// </summary>
        public int ColumnCount { get; private set; }
        
        /// <summary>
        /// Layout type (e.g., grid, masonry, fixed)
        /// </summary>
        public string LayoutType { get; private set; }
        
        /// <summary>
        /// Whether automatic resizing is enabled
        /// </summary>
        public bool AutoResize { get; private set; }
        
        /// <summary>
        /// CSS styles for the layout
        /// </summary>
        public string CustomCss { get; private set; }
        
        /// <summary>
        /// Serialized layout configuration
        /// </summary>
        public string LayoutConfig { get; private set; }
        
        protected DashboardLayoutSetting()
        {
        }
        
        internal DashboardLayoutSetting(
            Guid dashboardId,
            int columnCount,
            string layoutType,
            bool autoResize = true,
            string customCss = null,
            string layoutConfig = null)
        {
            DashboardId = dashboardId;
            SetColumnCount(columnCount);
            SetLayoutType(layoutType);
            AutoResize = autoResize;
            CustomCss = customCss;
            LayoutConfig = layoutConfig;
        }
        
        internal void SetColumnCount(int columnCount)
        {
            if (columnCount < 1)
            {
                throw new BusinessException("AnalyticsReporting:Validation:InvalidValue")
                    .WithData("propertyName", "columnCount");
            }
            
            ColumnCount = columnCount;
        }
        
        internal void SetLayoutType(string layoutType)
        {
            LayoutType = Check.NotNullOrWhiteSpace(layoutType, nameof(layoutType));
        }
        
        internal void SetAutoResize(bool autoResize)
        {
            AutoResize = autoResize;
        }
        
        internal void SetCustomCss(string customCss)
        {
            CustomCss = customCss;
        }
        
        internal void SetLayoutConfig(string layoutConfig)
        {
            LayoutConfig = layoutConfig;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { DashboardId };
        }
    }
}
