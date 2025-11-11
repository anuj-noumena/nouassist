using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.TaskManagement.TaskTypes.Dtos
{
    public class TaskTypeDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; }
        
        public List<TaskTypeCustomFieldDto> CustomFields { get; set; }
    }

    public class TaskTypeCustomFieldDto : EntityDto<Guid>
    {
        public Guid CustomFieldDefinitionId { get; set; }
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; }
        public int DisplayOrder { get; set; }
        public string TabName { get; set; }
        public string SectionName { get; set; }
        public bool IsActive { get; set; }
    }
}
