using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.Elicitation.Enums;

namespace NouAssist.RequirementsManagement.Elicitation.Surveys.Dtos
{
    /// <summary>
    /// DTO for survey details
    /// </summary>
    public class SurveyDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsDeployed { get; set; }
        public DateTime? DeploymentDate { get; set; }
        public QuestionnaireStructureDto Questionnaire { get; set; } = new();
        public int ResponseCount { get; set; }
        public double CompletionRate { get; set; }
        public List<Guid> TargetStakeholderIds { get; set; } = new();
    }

    /// <summary>
    /// DTO for questionnaire structure
    /// </summary>
    public class QuestionnaireStructureDto
    {
        public List<SurveyPageDto> Pages { get; set; } = new();
        public Dictionary<string, string> Logic { get; set; } = new();
        public int TotalQuestions { get; set; }
    }

    /// <summary>
    /// DTO for survey page
    /// </summary>
    public class SurveyPageDto
    {
        public string Title { get; set; } = string.Empty;
        public List<SurveyQuestionDto> Questions { get; set; } = new();
    }

    /// <summary>
    /// DTO for survey question
    /// </summary>
    public class SurveyQuestionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public bool IsRequired { get; set; }
        public List<string> Options { get; set; } = new();
    }
}
