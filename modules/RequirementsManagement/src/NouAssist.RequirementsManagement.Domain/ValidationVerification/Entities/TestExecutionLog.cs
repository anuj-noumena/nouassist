using System;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.ValidationVerification.Entities
{
    /// <summary>
    /// Represents a log entry for test case execution
    /// </summary>
    public class TestExecutionLog : Entity<Guid>
    {
        public Guid TestCaseId { get; private set; }
        public Guid ExecutorId { get; private set; }
        public string ExecutorName { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public TestExecutionStatus Status { get; private set; }
        public string ExecutionNotes { get; private set; }
        public int TotalSteps { get; private set; }
        public string Environment { get; private set; }
        public string TestDataUsed { get; private set; }

        protected TestExecutionLog() { }

        internal static TestExecutionLog Create(
            Guid id,
            Guid testCaseId,
            Guid executorId,
            DateTime startTime,
            int totalSteps)
        {
            return new TestExecutionLog(id, testCaseId, executorId, startTime, totalSteps);
        }

        private TestExecutionLog(
            Guid id,
            Guid testCaseId,
            Guid executorId,
            DateTime startTime,
            int totalSteps) : base(id)
        {
            TestCaseId = testCaseId;
            ExecutorId = executorId;
            StartedAt = startTime;
            TotalSteps = totalSteps;
            Status = TestExecutionStatus.NotExecuted;
        }

        internal void Complete(
            DateTime completionTime,
            TestExecutionStatus finalStatus,
            string executionNotes = null)
        {
            if (CompletedAt.HasValue)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:ExecutionAlreadyCompleted");
            }

            CompletedAt = completionTime;
            Status = finalStatus;
            ExecutionNotes = executionNotes;
        }

        public void SetEnvironment(string environment)
        {
            Environment = environment;
        }

        public void SetTestData(string testData)
        {
            TestDataUsed = testData;
        }

        public TimeSpan? ExecutionDuration
        {
            get
            {
                if (CompletedAt.HasValue)
                {
                    return CompletedAt.Value - StartedAt;
                }
                return null;
            }
        }

        public bool IsCompleted()
        {
            return CompletedAt.HasValue;
        }

        public bool IsInProgress()
        {
            return !CompletedAt.HasValue;
        }
    }
}
