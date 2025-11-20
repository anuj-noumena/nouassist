using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.SpecificationStructures.Dtos
{
    public class SpecificationSectionDto : EntityDto<Guid>
    {
        public string SectionName { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public Guid? ParentSectionId { get; set; }
        public List<SpecificationSectionDto> Subsections { get; set; } = new();
    }
}
