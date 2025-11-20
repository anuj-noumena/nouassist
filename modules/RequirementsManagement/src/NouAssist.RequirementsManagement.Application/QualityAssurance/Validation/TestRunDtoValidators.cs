using FluentValidation;
using NouAssist.RequirementsManagement.QualityAssurance.TestRuns.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.Validation
{
    public class AddTestRunDtoValidator : AbstractValidator<AddTestRunDto>
    {
        public AddTestRunDtoValidator()
        {
            RuleFor(x => x.TestCycleId).NotEmpty().WithMessage("Test cycle ID is required");
            RuleFor(x => x.TestCaseId).NotEmpty().WithMessage("Test case ID is required");
            RuleFor(x => x.TestCaseTitle).NotEmpty().WithMessage("Test case title is required");
            RuleFor(x => x.ExpectedOutcome).NotEmpty().WithMessage("Expected outcome is required");
        }
    }

    public class RecordTestResultDtoValidator : AbstractValidator<RecordTestResultDto>
    {
        public RecordTestResultDtoValidator()
        {
            RuleFor(x => x.TestCycleId).NotEmpty().WithMessage("Test cycle ID is required");
            RuleFor(x => x.TestCaseId).NotEmpty().WithMessage("Test case ID is required");
            RuleFor(x => x.ActualOutcomeDescription).NotEmpty().WithMessage("Actual outcome description is required");
        }
    }
}
