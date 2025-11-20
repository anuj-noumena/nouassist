using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts.Dtos
{
    /// <summary>
    /// DTO representing an elicitation artifact
    /// </summary>
    public class ElicitationArtifactDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ArtifactType Type { get; set; }
        public ArtifactStatus Status { get; set; }
        public Guid SessionId { get; set; }
        public string? SessionTitle { get; set; }
        public Guid CreatedByStakeholderId { get; set; }
        public string? CreatedByStakeholderName { get; set; }
        public StoryTextDto? StoryText { get; set; }
        public UseCaseModelDto? UseCaseModel { get; set; }
        public EventDefinitionDto? EventDefinition { get; set; }
        public List<IdeaTagDto> IdeaTags { get; set; } = new();
    }

    /// <summary>
    /// DTO for user story text
    /// </summary>
    public class StoryTextDto
    {
        public string Persona { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;
        public string Benefit { get; set; } = string.Empty;
        public string FullText { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for use case model
    /// </summary>
    public class UseCaseModelDto
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Actors { get; set; } = new();
        public List<UseCaseStepDto> Steps { get; set; } = new();
        public PrePostConditionDto? PreCondition { get; set; }
        public PrePostConditionDto? PostCondition { get; set; }
    }

    /// <summary>
    /// DTO for use case step
    /// </summary>
    public class UseCaseStepDto
    {
        public int StepNumber { get; set; }
        public string Actor { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for pre/post condition
    /// </summary>
    public class PrePostConditionDto
    {
        public string Description { get; set; } = string.Empty;
        public bool IsMandatory { get; set; }
    }

    /// <summary>
    /// DTO for event definition
    /// </summary>
    public class EventDefinitionDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Trigger { get; set; } = string.Empty;
        public List<string> Actors { get; set; } = new();
        public Dictionary<string, string> Properties { get; set; } = new();
    }

    /// <summary>
    /// DTO for idea tag
    /// </summary>
    public class IdeaTagDto
    {
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = "#007ACC";
        public string? Description { get; set; }
    }
}
