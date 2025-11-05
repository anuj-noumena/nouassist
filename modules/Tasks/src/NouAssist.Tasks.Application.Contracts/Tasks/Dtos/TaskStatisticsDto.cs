using System;

namespace NouAssist.Tasks.Tasks.Dtos;

public class TaskStatisticsDto
{
    public int TotalInstances { get; set; }
    public int ActiveInstances { get; set; }
    public int CompletedInstances { get; set; }
    public int CancelledInstances { get; set; }
    public double AverageCompletionTimeMinutes { get; set; }
    public DateTime? LastInstanceCreatedAt { get; set; }
}
