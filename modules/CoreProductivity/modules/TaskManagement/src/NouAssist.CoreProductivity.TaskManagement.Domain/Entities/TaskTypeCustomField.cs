using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Tasks.Entities
{
    public class TaskTypeCustomField : Entity<Guid>
    {
        public Guid TaskTypeId { get; private set; }
        public Guid CustomFieldDefinitionId { get; private set; }
        public bool IsRequired { get; private set; }
        public string DefaultValue { get; private set; }
        public int DisplayOrder { get; private set; }
        public string TabName { get; private set; }
        public string SectionName { get; private set; }
        public string ShowIfFieldKey { get; set; }
        public string ShowIfValue { get; set; }
        public bool IsActive { get; private set; }

        protected TaskTypeCustomField() { }

        public TaskTypeCustomField(
            Guid taskTypeId,
            Guid customFieldDefinitionId,
            bool isRequired = false,
            object defaultValue = null,
            int displayOrder = 0,
            string tabName = null,
            string sectionName = null)
        {
            Id = Guid.NewGuid();
            TaskTypeId = taskTypeId;
            CustomFieldDefinitionId = customFieldDefinitionId;
            IsRequired = isRequired;
            DefaultValue = defaultValue?.ToString();
            DisplayOrder = displayOrder;
            TabName = tabName;
            SectionName = sectionName;
            IsActive = true;
        }

        public void UpdateDisplayOrder(int newOrder)
        {
            DisplayOrder = newOrder;
        }

        public void AssignToTab(string tabName, string sectionName = null)
        {
            TabName = tabName;
            SectionName = sectionName;
        }

        public void SetRequired(bool isRequired)
        {
            IsRequired = isRequired;
        }

        public void SetDefaultValue(object value)
        {
            DefaultValue = value?.ToString();
        }
    }
}
