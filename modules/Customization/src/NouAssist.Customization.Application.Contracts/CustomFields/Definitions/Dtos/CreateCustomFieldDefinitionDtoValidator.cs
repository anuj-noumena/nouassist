using FluentValidation;

namespace NouAssist.Customization.CustomFields.Definitions.Dtos
{
    public class CreateCustomFieldDefinitionDtoValidator : AbstractValidator<CreateCustomFieldDefinitionDto>
    {
        public CreateCustomFieldDefinitionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.FieldType)
                .IsInEnum();

            RuleFor(x => x.EntityType)
                .IsInEnum();

            RuleFor(x => x.Options)
                .NotEmpty()
                .When(x => x.FieldType == Enums.CustomFieldType.Select || 
                          x.FieldType == Enums.CustomFieldType.MultiSelect)
                .WithMessage("Select fields must have at least one option");

            RuleForEach(x => x.Options)
                .SetValidator(new CreateCustomFieldOptionDtoValidator())
                .When(x => x.Options != null);
        }
    }

    public class CreateCustomFieldOptionDtoValidator : AbstractValidator<CreateCustomFieldOptionDto>
    {
        public CreateCustomFieldOptionDtoValidator()
        {
            RuleFor(x => x.Label)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Color)
                .MaximumLength(50);
        }
    }
}
