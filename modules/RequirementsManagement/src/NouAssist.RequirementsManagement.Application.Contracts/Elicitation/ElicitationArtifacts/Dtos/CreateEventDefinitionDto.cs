using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Elicitation.Constants;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts.Dtos
{
    /// <summary>
    /// DTO for creating event definition content
    /// </summary>
    public class CreateEventDefinitionDto : CreateElicitationArtifactDto
    {
        [Required]
        public CreateEventDefinitionDataDto EventDefinition { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating event definition data
    /// </summary>
    public class CreateEventDefinitionDataDto
    {
        [Required]
        [StringLength(ElicitationConsts.MaxEventNameLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(ElicitationConsts.MaxEventDescriptionLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Trigger { get; set; } = string.Empty;

        public List<string> Actors { get; set; } = new();
        public Dictionary<string, string> Properties { get; set; } = new();
    }
}
