using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates
{
    /// <summary>
    /// TaskType - Categorizes tasks (e.g., Bug, Feature, Task, Story)
    /// </summary>
    public class TaskType : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Name { get; private set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<TaskTypeCustomField> _customFields = new();
        public IReadOnlyCollection<TaskTypeCustomField> CustomFields => _customFields.AsReadOnly();

        protected TaskType() { }

        public TaskType(
            Guid id,
            string name,
            string description = null,
            string color = null,
            Guid? tenantId = null) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 200);
            Description = description;
            Color = color;
            TenantId = tenantId;
            IsActive = true;
        }

        public void UpdateName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 200);
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdateColor(string color)
        {
            if (!string.IsNullOrWhiteSpace(color) && !IsValidColor(color))
            {
                throw new BusinessException("Tasks:InvalidColorFormat");
            }
            Color = color;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void AddCustomField(
            Guid customFieldDefinitionId,
            bool isRequired = false,
            object defaultValue = null,
            int displayOrder = 0,
            string tabName = null,
            string sectionName = null)
        {
            if (_customFields.Any(cf => cf.CustomFieldDefinitionId == customFieldDefinitionId))
            {
                throw new BusinessException("Tasks:CustomFieldAlreadyAdded");
            }

            var customField = new TaskTypeCustomField(
                Id,
                customFieldDefinitionId,
                isRequired,
                defaultValue,
                displayOrder,
                tabName,
                sectionName);

            _customFields.Add(customField);
        }

        public void RemoveCustomField(Guid customFieldDefinitionId)
        {
            var customField = _customFields.FirstOrDefault(cf => 
                cf.CustomFieldDefinitionId == customFieldDefinitionId);
            
            if (customField == null)
            {
                throw new BusinessException("Tasks:CustomFieldNotFound");
            }

            _customFields.Remove(customField);
        }

        public void ReorderCustomFields(List<Guid> orderedFieldIds)
        {
            if (orderedFieldIds.Count != _customFields.Count)
            {
                throw new BusinessException("Tasks:InvalidFieldReorder");
            }

            for (int i = 0; i < orderedFieldIds.Count; i++)
            {
                var field = _customFields.FirstOrDefault(cf => 
                    cf.CustomFieldDefinitionId == orderedFieldIds[i]);
                if (field != null)
                {
                    field.UpdateDisplayOrder(i);
                }
            }
        }

        private bool IsValidColor(string color)
        {
            // Validate hex color format (#RRGGBB or #RGB)
            if (color.Length == 7 || color.Length == 4)
            {
                return color.StartsWith("#") && 
                       color.Substring(1).All(c => 
                           (c >= '0' && c <= '9') || 
                           (c >= 'A' && c <= 'F') || 
                           (c >= 'a' && c <= 'f'));
            }
            return false;
        }
    }
}
