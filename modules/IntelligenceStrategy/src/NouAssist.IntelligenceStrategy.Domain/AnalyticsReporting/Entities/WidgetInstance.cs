using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities
{
    /// <summary>
    /// An instance of a widget placed on a dashboard
    /// </summary>
    public class WidgetInstance : Entity<Guid>
    {
        /// <summary>
        /// The dashboard this widget is placed on
        /// </summary>
        public Guid DashboardId { get; private set; }
        
        /// <summary>
        /// Type of widget
        /// </summary>
        public WidgetType Type { get; private set; }
        
        /// <summary>
        /// Widget settings
        /// </summary>
        public WidgetSettings Settings { get; private set; }
        
        /// <summary>
        /// Grid position - column
        /// </summary>
        public int PositionX { get; private set; }
        
        /// <summary>
        /// Grid position - row
        /// </summary>
        public int PositionY { get; private set; }
        
        /// <summary>
        /// Width in grid units
        /// </summary>
        public int Width { get; private set; }
        
        /// <summary>
        /// Height in grid units
        /// </summary>
        public int Height { get; private set; }
        
        /// <summary>
        /// Reference to data source ID if applicable
        /// </summary>
        public Guid? DataSourceId { get; private set; }
        
        /// <summary>
        /// Visualization configuration for chart widgets
        /// </summary>
        public VisualizationConfiguration VisualizationConfig { get; private set; }
        
        protected WidgetInstance()
        {
        }
        
        internal WidgetInstance(
            Guid id,
            Guid dashboardId,
            WidgetType type,
            WidgetSettings settings,
            int positionX,
            int positionY,
            int width,
            int height,
            Guid? dataSourceId = null,
            VisualizationConfiguration visualizationConfig = null)
            : base(id)
        {
            DashboardId = dashboardId;
            Type = type;
            Settings = settings;
            
            SetPosition(positionX, positionY);
            SetSize(width, height);
            
            DataSourceId = dataSourceId;
            VisualizationConfig = visualizationConfig;
        }
        
        internal void SetPosition(int positionX, int positionY)
        {
            if (positionX < 0 || positionY < 0)
            {
                throw new BusinessException("AnalyticsReporting:Validation:InvalidValue")
                    .WithData("propertyName", "position");
            }
            
            PositionX = positionX;
            PositionY = positionY;
        }
        
        internal void SetSize(int width, int height)
        {
            if (width < 1 || height < 1)
            {
                throw new BusinessException("AnalyticsReporting:Validation:InvalidValue")
                    .WithData("propertyName", "size");
            }
            
            Width = width;
            Height = height;
        }
        
        internal void UpdateSettings(WidgetSettings settings)
        {
            Settings = settings;
        }
        
        internal void SetDataSource(Guid dataSourceId)
        {
            DataSourceId = dataSourceId;
        }
        
        internal void SetVisualizationConfig(VisualizationConfiguration config)
        {
            if (Type != WidgetType.Chart && 
                Type != WidgetType.Heatmap &&
                Type != WidgetType.Timeline)
            {
                throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidWidgetConfiguration")
                    .WithData("reason", "Visualization config only applicable for chart-based widgets");
            }
            
            VisualizationConfig = config;
        }
    }
}
