using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Elicitation.Constants;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts.Dtos
{
    /// <summary>
    /// DTO for creating use case content
    /// </summary>
    public class CreateUseCaseDto : CreateElicitationArtifactDto
    {
        [Required]
        public CreateUseCaseModelDto UseCaseModel { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating use case model
    /// </summary>
    public class CreateUseCaseModelDto
    {
        [Required]
        [StringLength(ElicitationConsts.MaxUseCaseNameLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public List<string> Actors { get; set; } = new();

        [Required]
        public List<CreateUseCaseStepDto> Steps { get; set; } = new();

        public CreatePrePostConditionDto? PreCondition { get; set; }
        public CreatePrePostConditionDto? PostCondition { get; set; }
    }

    /// <summary>
    /// DTO for creating use case step
    /// </summary>
    public class CreateUseCaseStepDto
    {
        [Required]
        public int StepNumber { get; set; }

        [Required]
        [StringLength(ElicitationConsts.MaxActorNameLength)]
        public string Actor { get; set; } = string.Empty;

        [Required]
        [StringLength(ElicitationConsts.MaxStepDescriptionLength)]
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for creating pre/post condition
    /// </summary>
    public class CreatePrePostConditionDto
    {
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsMandatory { get; set; } = true;
    }
}
