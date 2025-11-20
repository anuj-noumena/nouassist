using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.SpecificationStructures.Dtos
{
    /// <summary>
    /// DTO representing a specification structure for client consumption
    /// </summary>
    public class SpecificationStructureDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public string Description { get; set; }
        public List<SpecificationSectionDto> Sections { get; set; } = new();
        public int SectionsCount { get; set; }
    }
}
