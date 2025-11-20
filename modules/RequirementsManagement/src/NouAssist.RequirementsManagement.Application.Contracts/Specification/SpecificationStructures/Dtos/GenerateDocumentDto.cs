using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.Specification.SpecificationStructures.Dtos
{
    public class GenerateDocumentDto
    {
        [Required]
        public Guid StructureId { get; set; }

        public List<Guid> RequirementIds { get; set; } = new();

        [Required]
        [StringLength(10)]
        public string Format { get; set; } = "pdf";

        public bool IncludeAcceptanceCriteria { get; set; } = true;
        public bool IncludeBusinessRules { get; set; } = true;
        public bool IncludeConstraints { get; set; } = true;
        public bool IncludeDataRequirements { get; set; } = false;
        public bool IncludeInterfaceSpecs { get; set; } = false;
    }
}
