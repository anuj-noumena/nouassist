using FluentValidation;

namespace NouAssist.WorkManagement.Approval.Requests.Dtos
{
    public class SetCustomFieldDtoValidator : AbstractValidator<SetCustomFieldDto>
    {
        public SetCustomFieldDtoValidator()
        {
            RuleFor(x => x.Key)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Value)
                .NotNull();
        }
    }
}