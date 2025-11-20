using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCases.Dtos
{
    /// <summary>
    /// Data transfer object for test case
    /// </summary>
    public class TestCaseDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TestCaseStatus Status { get; set; }
        public TestCasePriority Priority { get; set; }
        public string Preconditions { get; set; }
        public string Postconditions { get; set; }
        public TimeSpan? EstimatedDuration { get; set; }
        public List<Guid> RequirementIds { get; set; } = new();
        public List<TestStepDto> TestSteps { get; set; } = new();
        public List<TestExecutionLogDto> ExecutionLogs { get; set; } = new();
        public TestCaseStatisticsDto Statistics { get; set; }
    }

    /// <summary>
    /// Lightweight DTO for test case lists
    /// </summary>
    public class TestCaseSummaryDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TestCaseStatus Status { get; set; }
        public TestCasePriority Priority { get; set; }
        public int StepCount { get; set; }
        public int RequirementCount { get; set; }
        public TestExecutionStatus? LastExecutionStatus { get; set; }
        public DateTime? LastExecutionDate { get; set; }
        public DateTime CreationTime { get; set; }
    }

    /// <summary>
    /// DTO for test step
    /// </summary>
    public class TestStepDto
    {
        public Guid Id { get; set; }
        public int StepNumber { get; set; }
        public TestStepInstructionDto Instruction { get; set; }
        public ExpectedResultDto ExpectedResult { get; set; }
        public ActualResultDto ActualResult { get; set; }
        public TestExecutionStatus ExecutionStatus { get; set; }
        public string Comments { get; set; }
    }

    /// <summary>
    /// DTO for test step instruction
    /// </summary>
    public class TestStepInstructionDto
    {
        public string Instruction { get; set; }
        public string TestData { get; set; }
        public string Prerequisites { get; set; }
    }

    /// <summary>
    /// DTO for expected result
    /// </summary>
    public class ExpectedResultDto
    {
        public string Description { get; set; }
        public string AcceptanceCriteria { get; set; }
    }

    /// <summary>
    /// DTO for actual result
    /// </summary>
    public class ActualResultDto
    {
        public string Description { get; set; }
        public string Evidence { get; set; }
        public DateTime ExecutedAt { get; set; }
        public Guid ExecutedBy { get; set; }
    }

    /// <summary>
    /// DTO for test execution log
    /// </summary>
    public class TestExecutionLogDto
    {
        public Guid Id { get; set; }
        public Guid ExecutorId { get; set; }
        public string ExecutorName { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TestExecutionStatus Status { get; set; }
        public string ExecutionNotes { get; set; }
        public int TotalSteps { get; set; }
        public string Environment { get; set; }
        public string TestDataUsed { get; set; }
        public TimeSpan? ExecutionDuration { get; set; }
    }

    /// <summary>
    /// DTO for test case statistics
    /// </summary>
    public class TestCaseStatisticsDto
    {
        public int TotalExecutions { get; set; }
        public int PassedExecutions { get; set; }
        public int FailedExecutions { get; set; }
        public int BlockedExecutions { get; set; }
        public TestExecutionStatus? LastExecutionStatus { get; set; }
        public DateTime? LastExecutionDate { get; set; }
        public TimeSpan? AverageExecutionTime { get; set; }
        public int TotalSteps { get; set; }
        public int RequirementCoverage { get; set; }
        public double SuccessRate { get; set; }
    }
}
