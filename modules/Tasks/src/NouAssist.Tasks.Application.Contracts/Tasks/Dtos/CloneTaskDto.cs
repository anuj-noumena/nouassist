using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Tasks.Tasks.Dtos;

public class CloneTaskDto
{
    [Required] [StringLength(500)] public string NewTitle { get; set; }

    public Guid? NewProjectId { get; set; }

    public bool IncludeActionSteps { get; set; } = true;
    public bool IncludeAcceptanceCriteria { get; set; } = true;
}