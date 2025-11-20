using System;
using NouAssist.RequirementsManagement.Specification.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class DataModelReferenceDto : EntityDto<Guid>
    {
        public string FieldName { get; set; }
        public DataFieldType DataType { get; set; }
        public bool IsRequired { get; set; }
        public string Description { get; set; }
        public int? MaxLength { get; set; }
    }
}
