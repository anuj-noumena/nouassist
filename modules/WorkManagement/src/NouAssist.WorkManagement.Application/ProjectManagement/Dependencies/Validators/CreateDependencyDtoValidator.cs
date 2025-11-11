using NouAssist.WorkManagement.ProjectManagement.Dependencies.Dtos;

namespace NouAssist.WorkManagement.ProjectManagement.Dependencies.Validators
{
    public class CreateDependencyDtoValidator : AbstractValidator<CreateDependencyDto>
    {
        public CreateDependencyDtoValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();

            RuleFor(x => x.DependsOnProjectId)
                .NotEmpty();

            RuleFor(x => x)
                .Must(x => x.ProjectId != x.DependsOnProjectId)
                .WithMessage("Project cannot depend on itself.");

            RuleFor(x => x.DependencyType)
                .IsInEnum();

            RuleFor(x => x.LagDays)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Description)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }
}
