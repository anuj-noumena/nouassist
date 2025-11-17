using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.TaskManagement.Enums;

namespace NouAssist.WorkManagement.TaskManagement.Tasks.Dtos;

public class CreateComplexityEstimationDto
{
    [Required]
    public int Value { get; set; }

    [Required]
    public ComplexityScale Scale { get; set; }
}