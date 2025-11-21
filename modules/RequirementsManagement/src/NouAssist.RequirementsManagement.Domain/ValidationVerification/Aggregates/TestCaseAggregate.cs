using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.ValidationVerification.Constants;
using NouAssist.RequirementsManagement.ValidationVerification.Entities;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using NouAssist.RequirementsManagement.ValidationVerification.Events;
using NouAssist.RequirementsManagement.ValidationVerification.Exceptions;
using NouAssist.RequirementsManagement.ValidationVerification.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.ValidationVerification.Aggregates
{
    /// <summary>
    /// Represents a test case that validates one or more requirements.
    /// Contains test steps with instructions, expected results, and execution logs.
    /// </summary>
    public class TestCase : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public TestCaseStatus Status { get; private set; }
        public TestCasePriority Priority { get; private set; }
        public string Preconditions { get; private set; }
        public string Postconditions { get; private set; }
        public TimeSpan? EstimatedDuration { get; private set; }

        // Collections - controlled access
        private readonly List<TestStep> _testSteps;
        private readonly List<Guid> _requirementIds;
        private readonly List<TestExecutionLog> _executionLogs;

        public IReadOnlyCollection<TestStep> TestSteps => _testSteps.AsReadOnly();
        public IReadOnlyCollection<Guid> RequirementIds => _requirementIds.AsReadOnly();
        public IReadOnlyCollection<TestExecutionLog> ExecutionLogs => _executionLogs.AsReadOnly();

        protected TestCase()
        {
            _testSteps = new List<TestStep>();
            _requirementIds = new List<Guid>();
            _executionLogs = new List<TestExecutionLog>();
        }

        /// <summary>
        /// Creates a new test case
        /// </summary>
        public static TestCase Create(
            Guid id,
            string name,
            string description,
            TestCasePriority priority,
            IEnumerable<Guid> requirementIds,
            string preconditions = null,
            string postconditions = null,
            Guid? tenantId = null)
        {
            var testCase = new TestCase(
                id, name, description, priority, 
                requirementIds, preconditions, postconditions, tenantId);

            return testCase;
        }

        private TestCase(
            Guid id,
            string name,
            string description,
            TestCasePriority priority,
            IEnumerable<Guid> requirementIds,
            string preconditions,
            string postconditions,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            Priority = priority;
            SetPreconditions(preconditions);
            SetPostconditions(postconditions);
            TenantId = tenantId;
            Status = TestCaseStatus.Draft;

            _testSteps = new List<TestStep>();
            _requirementIds = new List<Guid>();
            _executionLogs = new List<TestExecutionLog>();

            // Add requirement references
            if (requirementIds != null)
            {
                _requirementIds.AddRange(requirementIds.Distinct());
            }

            AddDistributedEvent(new TestCaseCreatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Name = Name,
                Description = Description,
                Priority = Priority,
                RequirementIds = _requirementIds.ToList(),
                CreatedBy = CreatorId.Value,
                StepCount = 0
            });
        }

        /// <summary>
        /// Adds a test step to the test case
        /// </summary>
        public void AddTestStep(
            string instruction,
            string expectedResult,
            string testData = null,
            string prerequisites = null)
        {
            if (Status == TestCaseStatus.InExecution)
            {
                throw new TestExecutionInvalidStateException(Id, Status);
            }

            var stepNumber = _testSteps.Count + 1;
            var testStep = TestStep.Create(
                Guid.NewGuid(),
                Id,
                stepNumber,
                new TestStepInstruction(instruction, testData, prerequisites),
                new ExpectedResult(expectedResult)
            );

            _testSteps.Add(testStep);
        }

        /// <summary>
        /// Updates an existing test step
        /// </summary>
        public void UpdateTestStep(
            Guid stepId,
            string instruction,
            string expectedResult,
            string testData = null,
            string prerequisites = null)
        {
            if (Status == TestCaseStatus.InExecution)
            {
                throw new TestExecutionInvalidStateException(Id, Status);
            }

            var step = _testSteps.FirstOrDefault(s => s.Id == stepId);
            if (step == null)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:TestStepNotFound");
            }

            step.UpdateInstruction(new TestStepInstruction(instruction, testData, prerequisites));
            step.UpdateExpectedResult(new ExpectedResult(expectedResult));
        }

        /// <summary>
        /// Removes a test step
        /// </summary>
        public void RemoveTestStep(Guid stepId)
        {
            if (Status == TestCaseStatus.InExecution)
            {
                throw new TestExecutionInvalidStateException(Id, Status);
            }

            var step = _testSteps.FirstOrDefault(s => s.Id == stepId);
            if (step != null)
            {
                _testSteps.Remove(step);
                
                // Renumber remaining steps
                for (int i = 0; i < _testSteps.Count; i++)
                {
                    _testSteps[i].UpdateStepNumber(i + 1);
                }
            }
        }

        /// <summary>
        /// Marks the test case as ready for execution
        /// </summary>
        public void MarkAsReady()
        {
            if (!_testSteps.Any())
            {
                throw new BusinessException("ValidationVerification:Validation:NoTestSteps");
            }

            if (Status != TestCaseStatus.Draft && Status != TestCaseStatus.Blocked)
            {
                throw new TestExecutionInvalidStateException(Id, Status);
            }

            Status = TestCaseStatus.Ready;
        }

        /// <summary>
        /// Starts test execution
        /// </summary>
        public TestExecutionLog StartExecution(Guid executorId, DateTime startTime)
        {
            if (Status != TestCaseStatus.Ready)
            {
                throw new TestExecutionInvalidStateException(Id, Status);
            }

            if (!_testSteps.Any())
            {
                throw new BusinessException("ValidationVerification:Validation:NoTestSteps");
            }

            Status = TestCaseStatus.InExecution;

            var executionLog = TestExecutionLog.Create(
                Guid.NewGuid(),
                Id,
                executorId,
                startTime,
                _testSteps.Count
            );

            _executionLogs.Add(executionLog);
            return executionLog;
        }

        /// <summary>
        /// Completes test execution with results
        /// </summary>
        public void CompleteExecution(
            Guid executionLogId,
            TestExecutionStatus overallStatus,
            DateTime completionTime,
            Dictionary<Guid, TestExecutionStatus> stepResults,
            string executionNotes = null)
        {
            var executionLog = _executionLogs.FirstOrDefault(e => e.Id == executionLogId);
            if (executionLog == null)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:ExecutionLogNotFound");
            }

            if (executionLog.Status != TestExecutionStatus.NotExecuted)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:ExecutionAlreadyCompleted");
            }

            // Update execution log
            executionLog.Complete(completionTime, overallStatus, executionNotes);

            // Update test step results
            foreach (var stepResult in stepResults)
            {
                var step = _testSteps.FirstOrDefault(s => s.Id == stepResult.Key);
                if (step != null)
                {
                    var actualResult = new ActualResult(
                        stepResult.Value.ToString(),
                        null, // Evidence can be added separately
                        completionTime,
                        executionLog.ExecutorId
                    );
                    step.RecordExecution(actualResult, stepResult.Value);
                }
            }

            // Update test case status based on execution result
            Status = overallStatus switch
            {
                TestExecutionStatus.Pass => TestCaseStatus.Ready,
                TestExecutionStatus.Fail => TestCaseStatus.Ready,
                TestExecutionStatus.Blocked => TestCaseStatus.Blocked,
                _ => TestCaseStatus.Ready
            };

            // Calculate execution statistics
            var passedSteps = stepResults.Values.Count(s => s == TestExecutionStatus.Pass);
            var failedSteps = stepResults.Values.Count(s => s == TestExecutionStatus.Fail);

            AddDistributedEvent(new TestExecutionCompletedEto
            {
                Id = executionLogId,
                TenantId = TenantId,
                TestCaseId = Id,
                TestCaseName = Name,
                ExecutionStatus = overallStatus,
                ExecutedBy = executionLog.ExecutorId,
                ExecutedAt = completionTime,
                TotalSteps = _testSteps.Count,
                PassedSteps = passedSteps,
                FailedSteps = failedSteps,
                RequirementIds = _requirementIds.ToList(),
                ExecutionNotes = executionNotes
            });
        }

        /// <summary>
        /// Blocks the test case with a reason
        /// </summary>
        public void Block(string reason)
        {
            if (Status == TestCaseStatus.Obsolete)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:CannotBlockObsoleteTest");
            }

            Status = TestCaseStatus.Blocked;
        }

        /// <summary>
        /// Marks the test case as obsolete
        /// </summary>
        public void MarkAsObsolete(string reason)
        {
            Status = TestCaseStatus.Obsolete;
        }

        /// <summary>
        /// Adds a requirement linkage
        /// </summary>
        public void AddRequirementLink(Guid requirementId)
        {
            if (!_requirementIds.Contains(requirementId))
            {
                _requirementIds.Add(requirementId);
            }
        }

        /// <summary>
        /// Removes a requirement linkage
        /// </summary>
        public void RemoveRequirementLink(Guid requirementId)
        {
            _requirementIds.Remove(requirementId);
        }

        /// <summary>
        /// Updates test case properties
        /// </summary>
        public void Update(
            string name,
            string description,
            TestCasePriority priority,
            string preconditions = null,
            string postconditions = null,
            TimeSpan? estimatedDuration = null)
        {
            if (Status == TestCaseStatus.InExecution)
            {
                throw new TestExecutionInvalidStateException(Id, Status);
            }

            SetName(name);
            SetDescription(description);
            Priority = priority;
            SetPreconditions(preconditions);
            SetPostconditions(postconditions);
            EstimatedDuration = estimatedDuration;
        }

        /// <summary>
        /// Gets test execution statistics
        /// </summary>
        public TestCaseStatistics GetExecutionStatistics()
        {
            var completedExecutions = _executionLogs.Where(e => e.Status != TestExecutionStatus.NotExecuted).ToList();
            var latestExecution = completedExecutions.OrderByDescending(e => e.CompletedAt).FirstOrDefault();

            return new TestCaseStatistics
            {
                TotalExecutions = completedExecutions.Count,
                PassedExecutions = completedExecutions.Count(e => e.Status == TestExecutionStatus.Pass),
                FailedExecutions = completedExecutions.Count(e => e.Status == TestExecutionStatus.Fail),
                BlockedExecutions = completedExecutions.Count(e => e.Status == TestExecutionStatus.Blocked),
                LastExecutionStatus = latestExecution?.Status,
                LastExecutionDate = latestExecution?.CompletedAt,
                AverageExecutionTime = completedExecutions.Any() 
                    ? TimeSpan.FromTicks((long)completedExecutions.Average(e => e.ExecutionDuration?.Ticks ?? 0))
                    : (TimeSpan?)null,
                TotalSteps = _testSteps.Count,
                RequirementCoverage = _requirementIds.Count
            };
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ValidationVerificationConsts.TestCase.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: ValidationVerificationConsts.TestCase.MaxDescriptionLength
            );
        }

        private void SetPreconditions(string preconditions)
        {
            Preconditions = Check.Length(
                preconditions,
                nameof(preconditions),
                maxLength: ValidationVerificationConsts.TestCase.MaxPreconditionsLength
            );
        }

        private void SetPostconditions(string postconditions)
        {
            Postconditions = Check.Length(
                postconditions,
                nameof(postconditions),
                maxLength: ValidationVerificationConsts.TestCase.MaxPostconditionsLength
            );
        }
    }

    /// <summary>
    /// Test case execution statistics data structure
    /// </summary>
    public class TestCaseStatistics
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
    }
}
