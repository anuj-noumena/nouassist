using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ValidationVerification.ValueObjects
{
    /// <summary>
    /// Represents an instruction for executing a test step
    /// </summary>
    public class TestStepInstruction : ValueObject
    {
        public string Instruction { get; private set; }
        public string TestData { get; private set; }
        public string Prerequisites { get; private set; }

        private TestStepInstruction() { }

        public TestStepInstruction(string instruction, string testData = null, string prerequisites = null)
        {
            SetInstruction(instruction);
            TestData = testData;
            Prerequisites = prerequisites;
        }

        private void SetInstruction(string instruction)
        {
            Instruction = Check.NotNullOrWhiteSpace(
                instruction,
                nameof(instruction),
                maxLength: ValidationVerificationConsts.TestStep.MaxInstructionLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Instruction;
            yield return TestData;
            yield return Prerequisites;
        }
    }
}
