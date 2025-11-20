using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.Enums;

namespace NouAssist.RequirementsManagement.Elicitation.Surveys.Dtos
{
    /// <summary>
    /// DTO for creating a new survey
    /// </summary>
    public class CreateSurveyDto
    {
        [Required]
        [StringLength(ElicitationConsts.MaxSessionTitleLength)]
        public string Title { get; set; } = string.Empty;

        [StringLength(ElicitationConsts.MaxSessionDescriptionLength)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public Guid OrganizerId { get; set; }

        [Required]
        public CreateQuestionnaireStructureDto Questionnaire { get; set; } = new();

        [Required]
        public List<Guid> TargetStakeholderIds { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating questionnaire structure
    /// </summary>
    public class CreateQuestionnaireStructureDto
    {
        [Required]
        public List<CreateSurveyPageDto> Pages { get; set; } = new();

        public Dictionary<string, string> Logic { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating survey page
    /// </summary>
    public class CreateSurveyPageDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public List<CreateSurveyQuestionDto> Questions { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating survey question
    /// </summary>
    public class CreateSurveyQuestionDto
    {
        [Required]
        [StringLength(50)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(ElicitationConsts.MaxQuestionTextLength)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public QuestionType Type { get; set; }

        public bool IsRequired { get; set; }

        public List<string> Options { get; set; } = new();
    }
}
