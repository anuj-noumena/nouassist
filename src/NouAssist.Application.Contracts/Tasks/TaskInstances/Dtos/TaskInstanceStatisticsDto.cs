using System;

namespace NouAssist.Tasks.TaskInstances.Dtos;

public class TaskInstanceStatisticsDto
{
    public int TotalMinutesLogged { get; set; }
    public int StepsCompleted { get; set; }
    public int TotalSteps { get; set; }
    public int CriteriaMetCount { get; set; }
    public int TotalCriteria { get; set; }
    public double ProgressPercent { get; set; }
    public int SubtasksCount { get; set; }
    public int SubtasksDoneCount { get; set; }
    public int DependenciesCount { get; set; }
    public int BlockingDependenciesCount { get; set; }
    public bool CanStart { get; set; }
    public TimeSpan? TimeRemaining { get; set; }
}