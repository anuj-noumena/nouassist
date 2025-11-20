using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.ValidationVerification.Entities
{
    /// <summary>
    /// Represents a single step within a test case with instruction and expected result
    /// </summary>
    public class TestStep : Entity<Guid>
    {
        public Guid TestCaseId { get; private set; }
        public int StepNumber { get; private set; }
        public TestStepInstruction Instruction { get; private set; }
        public ExpectedResult ExpectedResult { get; private set; }
        public ActualResult ActualResult { get; private set; }
        public TestExecutionStatus ExecutionStatus { get; private set; }
        public string Comments { get; private set; }

        protected TestStep() { }

        internal static TestStep Create(
            Guid id,
            Guid testCaseId,
            int stepNumber,
            TestStepInstruction instruction,
            ExpectedResult expectedResult)
        {
            return new TestStep(id, testCaseId, stepNumber, instruction, expectedResult);
        }

        private TestStep(
            Guid id,
            Guid testCaseId,
            int stepNumber,
            TestStepInstruction instruction,
            ExpectedResult expectedResult) : base(id)
        {
            TestCaseId = testCaseId;
            StepNumber = stepNumber;
            Instruction = Check.NotNull(instruction, nameof(instruction));
            ExpectedResult = Check.NotNull(expectedResult, nameof(expectedResult));
            ExecutionStatus = TestExecutionStatus.NotExecuted;
        }

        internal void UpdateStepNumber(int newStepNumber)
        {
            if (newStepNumber <= 0)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:InvalidStepNumber");
            }

            StepNumber = newStepNumber;
        }

        internal void UpdateInstruction(TestStepInstruction newInstruction)
        {
            Instruction = Check.NotNull(newInstruction, nameof(newInstruction));
        }

        internal void UpdateExpectedResult(ExpectedResult newExpectedResult)
        {
            ExpectedResult = Check.NotNull(newExpectedResult, nameof(newExpectedResult));
        }

        internal void RecordExecution(ActualResult actualResult, TestExecutionStatus status)
        {
            ActualResult = Check.NotNull(actualResult, nameof(actualResult));
            ExecutionStatus = status;
        }

        public void AddComments(string comments)
        {
            Comments = Check.Length(
                comments,
                nameof(comments),
                maxLength: ValidationVerificationConsts.TestStep.MaxCommentsLength
            );
        }

        public bool IsExecuted()
        {
            return ExecutionStatus != TestExecutionStatus.NotExecuted;
        }

        public bool HasPassed()
        {
            return ExecutionStatus == TestExecutionStatus.Pass;
        }

        public bool HasFailed()
        {
            return ExecutionStatus == TestExecutionStatus.Fail;
        }
    }
}
