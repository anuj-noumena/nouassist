namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects
{
    /// <summary>
    /// Details of a hypothesis including assumption, test, and result
    /// </summary>
    public class HypothesisDetails : ValueObject
    {
        public string Assumption { get; private set; }
        public string TestDescription { get; private set; }
        public string TestResult { get; private set; }
        public ValidationStatus Status { get; private set; }
        public DateTime? TestStartDate { get; private set; }
        public DateTime? TestEndDate { get; private set; }

        protected HypothesisDetails() { }

        public HypothesisDetails(string assumption, ValidationStatus status = ValidationStatus.NotTested)
        {
            SetAssumption(assumption);
            Status = status;
        }

        private void SetAssumption(string assumption)
        {
            Assumption = Check.NotNullOrWhiteSpace(
                assumption,
                nameof(assumption),
                maxLength: BusinessModelManagementConsts.Hypothesis.MaxAssumptionLength
            );
        }

        public HypothesisDetails StartTest(string testDescription, DateTime testStartDate)
        {
            if (Status != ValidationStatus.NotTested && Status != ValidationStatus.NeedsMoreTesting)
            {
                throw new HypothesisAlreadyTestedException(Guid.Empty, Status);
            }

            Check.NotNullOrWhiteSpace(
                testDescription,
                nameof(testDescription),
                maxLength: BusinessModelManagementConsts.Hypothesis.MaxTestDescriptionLength
            );

            return new HypothesisDetails(Assumption, ValidationStatus.InProgress)
            {
                TestDescription = testDescription,
                TestStartDate = testStartDate
            };
        }

        public HypothesisDetails CompleteTest(string testResult, ValidationStatus finalStatus, DateTime testEndDate)
        {
            if (Status != ValidationStatus.InProgress)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:InvalidOperation");
            }

            if (finalStatus == ValidationStatus.NotTested || finalStatus == ValidationStatus.InProgress)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:InvalidValue")
                    .WithData("propertyName", "finalStatus");
            }

            Check.NotNullOrWhiteSpace(
                testResult,
                nameof(testResult),
                maxLength: BusinessModelManagementConsts.Hypothesis.MaxResultDescriptionLength
            );

            return new HypothesisDetails(Assumption, finalStatus)
            {
                TestDescription = TestDescription,
                TestResult = testResult,
                TestStartDate = TestStartDate,
                TestEndDate = testEndDate
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Assumption;
            yield return TestDescription;
            yield return TestResult;
            yield return Status;
            yield return TestStartDate;
            yield return TestEndDate;
        }
    }
}
