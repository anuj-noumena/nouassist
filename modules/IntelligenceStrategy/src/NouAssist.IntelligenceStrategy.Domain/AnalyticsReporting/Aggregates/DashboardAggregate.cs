using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Analytics.Constants;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.Events;
using NouAssist.Analytics.Exceptions;
using NouAssist.Analytics.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Analytics.Aggregates
{
    /// <summary>
    /// Represents a user-defined dashboard with layout settings and widgets
    /// </summary>
    public class Dashboard : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        
        public string Name { get; private set; }
        
        public string Description { get; private set; }
        
        public bool IsDefault { get; private set; }
        
        private readonly List<WidgetInstance> _widgets;
        public IReadOnlyCollection<WidgetInstance> Widgets => _widgets.AsReadOnly();
        
        private DashboardLayoutSetting _layoutSetting;
        public DashboardLayoutSetting LayoutSetting => _layoutSetting;
        
        protected Dashboard()
        {
            _widgets = new List<WidgetInstance>();
        }
        
        public Dashboard(
            Guid id,
            string name,
            string description,
            bool isDefault,
            DashboardLayoutSetting layoutSetting,
            Guid? tenantId
        ) : base(id)
        {
            TenantId = tenantId;
            SetName(name);
            SetDescription(description);
            IsDefault = isDefault;
            _layoutSetting = layoutSetting ?? new DashboardLayoutSetting();
            _widgets = new List<WidgetInstance>();
            
            AddDistributedEvent(new DashboardCreatedEto 
            { 
                Id = Id, 
                Name = Name, 
                TenantId = TenantId, 
                CreatorId = GetAuditInfoOrDefault<Guid>(), 
                WidgetCount = 0 
            });
        }
        
        private Guid? GetAuditInfoOrDefault<T>() where T : struct
        {
            if (this.LastModifierId.HasValue)
            {
                return this.LastModifierId.Value;
            }
            else if (this.CreatorId.HasValue)
            {
                return this.CreatorId.Value;
            }
            
            return default;
        }
        
        public void UpdateLayoutSetting(DashboardLayoutSetting layoutSetting)
        {
            Check.NotNull(layoutSetting, nameof(layoutSetting));
            _layoutSetting = layoutSetting;
        }
        
        public void AddWidget(WidgetInstance widget)
        {
            Check.NotNull(widget, nameof(widget));
            
            if (_widgets.Count >= AnalyticsConsts.Dashboard.MaxWidgetsPerDashboard)
            {
                throw new InvalidDashboardConfigurationException(Id, $"Maximum allowed widgets ({AnalyticsConsts.Dashboard.MaxWidgetsPerDashboard}) exceeded");
            }
            
            _widgets.Add(widget);
        }
        
        public void RemoveWidget(Guid widgetId)
        {
            var widget = _widgets.FirstOrDefault(w => w.Id == widgetId);
            if (widget == null)
            {
                return;
            }
            
            _widgets.Remove(widget);
        }
        
        public void UpdateWidget(Guid widgetId, WidgetSettings settings)
        {
            var widget = _widgets.FirstOrDefault(w => w.Id == widgetId);
            if (widget == null)
            {
                throw new InvalidDashboardConfigurationException(Id, $"Widget with ID {widgetId} not found");
            }
            
            widget.UpdateSettings(settings);
        }
        
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: AnalyticsConsts.Dashboard.MaxNameLength
            );
        }
        
        public void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: AnalyticsConsts.Dashboard.MaxDescriptionLength
            );
        }
        
        public void SetDefault(bool isDefault)
        {
            IsDefault = isDefault;
        }
    }
}
