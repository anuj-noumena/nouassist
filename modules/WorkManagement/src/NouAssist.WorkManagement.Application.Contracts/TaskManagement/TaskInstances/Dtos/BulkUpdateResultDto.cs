using System.Collections.Generic;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos;

public class BulkUpdateResultDto
{
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public List<string> Errors { get; set; } = new();
}