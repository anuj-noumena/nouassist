using System;
using FluentValidation;
using NouAssist.RequirementsManagement.ValidationVerification.Constants;
using NouAssist.RequirementsManagement.ValidationVerification.TestCases.Dtos;

namespace NouAssist.RequirementsManagement.ValidationVerification.Validation
{
    /// <summary>
    /// Validator for CreateTestCaseDto
    /// </summary>
    public class CreateTestCaseDtoValidator : AbstractValidator<CreateTestCaseDto>
    {
        public CreateTestCaseDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationVerificationConsts.TestCase.MaxNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(ValidationVerificationConsts.TestCase.MaxDescriptionLength);

            RuleFor(x => x.Priority)
                .IsInEnum();

            RuleFor(x => x.RequirementIds)
                .NotEmpty()
                .WithMessage("At least one requirement must be linked");

            RuleFor(x => x.Preconditions)
                .MaximumLength(ValidationVerificationConsts.TestCase.MaxPreconditionsLength);

            RuleFor(x => x.Postconditions)
                .MaximumLength(ValidationVerificationConsts.TestCase.MaxPostconditionsLength);

            RuleFor(x => x.EstimatedDuration)
                .GreaterThan(TimeSpan.Zero)
                .When(x => x.EstimatedDuration.HasValue)
                .WithMessage("Estimated duration must be positive");

            RuleForEach(x => x.TestSteps)
                .SetValidator(new CreateTestStepDtoValidator());
        }
    }

    /// <summary>
    /// Validator for CreateTestStepDto
    /// </summary>
    public class CreateTestStepDtoValidator : AbstractValidator<CreateTestStepDto>
    {
        public CreateTestStepDtoValidator()
        {
            RuleFor(x => x.Instruction)
                .NotEmpty()
                .MaximumLength(ValidationVerificationConsts.TestStep.MaxInstructionLength);

            RuleFor(x => x.ExpectedResult)
                .NotEmpty()
                .MaximumLength(ValidationVerificationConsts.TestStep.MaxExpectedResultLength);
        }
    }

    /// <summary>
    /// Validator for UpdateTestCaseDto
    /// </summary>
    public class UpdateTestCaseDtoValidator : AbstractValidator<UpdateTestCaseDto>
    {
        public UpdateTestCaseDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationVerificationConsts.TestCase.MaxNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(ValidationVerificationConsts.TestCase.MaxDescriptionLength);

            RuleFor(x => x.Priority)
                .IsInEnum();

            RuleFor(x => x.Preconditions)
                .MaximumLength(ValidationVerificationConsts.TestCase.MaxPreconditionsLength);

            RuleFor(x => x.Postconditions)
                .MaximumLength(ValidationVerificationConsts.TestCase.MaxPostconditionsLength);

            RuleFor(x => x.EstimatedDuration)
                .GreaterThan(TimeSpan.Zero)
                .When(x => x.EstimatedDuration.HasValue)
                .WithMessage("Estimated duration must be positive");
        }
    }

    /// <summary>
    /// Validator for UpdateTestStepDto
    /// </summary>
    public class UpdateTestStepDtoValidator : AbstractValidator<UpdateTestStepDto>
    {
        public UpdateTestStepDtoValidator()
        {
            RuleFor(x => x.Instruction)
                .NotEmpty()
                .MaximumLength(ValidationVerificationConsts.TestStep.MaxInstructionLength);

            RuleFor(x => x.ExpectedResult)
                .NotEmpty()
                .MaximumLength(ValidationVerificationConsts.TestStep.MaxExpectedResultLength);
        }
    }

    /// <summary>
    /// Validator for CompleteTestExecutionDto
    /// </summary>
    public class CompleteTestExecutionDtoValidator : AbstractValidator<CompleteTestExecutionDto>
    {
        public CompleteTestExecutionDtoValidator()
        {
            RuleFor(x => x.OverallStatus)
                .IsInEnum();

            RuleFor(x => x.StepResults)
                .NotEmpty()
                .WithMessage("At least one step result must be provided");

            RuleFor(x => x.ExecutionNotes)
                .MaximumLength(2000);

            RuleForEach(x => x.StepResults.Values)
                .SetValidator(new TestStepExecutionResultDtoValidator());
        }
    }

    /// <summary>
    /// Validator for TestStepExecutionResultDto
    /// </summary>
    public class TestStepExecutionResultDtoValidator : AbstractValidator<TestStepExecutionResultDto>
    {
        public TestStepExecutionResultDtoValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum();

            RuleFor(x => x.ActualResult)
                .NotEmpty()
                .MaximumLength(ValidationVerificationConsts.TestStep.MaxActualResultLength);

            RuleFor(x => x.Comments)
                .MaximumLength(ValidationVerificationConsts.TestStep.MaxCommentsLength);
        }
    }

    /// <summary>
    /// Validator for UpdateRequirementLinksDto
    /// </summary>
    public class UpdateRequirementLinksDtoValidator : AbstractValidator<UpdateRequirementLinksDto>
    {
        public UpdateRequirementLinksDtoValidator()
        {
            RuleFor(x => x.RequirementIds)
                .NotEmpty()
                .WithMessage("At least one requirement must be linked");
        }
    }
}
