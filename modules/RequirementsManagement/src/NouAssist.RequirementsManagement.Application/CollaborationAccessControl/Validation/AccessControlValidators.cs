using FluentValidation;
using NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl.Dtos;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Constants;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Validation
{
    public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(CollaborationConsts.MaxRoleNameLength)
                .WithMessage(L["Validation:Role:NameRequired"]);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(CollaborationConsts.MaxRoleDescriptionLength)
                .WithMessage(L["Validation:Role:DescriptionRequired"]);
        }
    }

    public class UpdateRoleDtoValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(CollaborationConsts.MaxRoleNameLength)
                .WithMessage(L["Validation:Role:NameRequired"]);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(CollaborationConsts.MaxRoleDescriptionLength)
                .WithMessage(L["Validation:Role:DescriptionRequired"]);
        }
    }

    public class AddPermissionToRoleDtoValidator : AbstractValidator<AddPermissionToRoleDto>
    {
        public AddPermissionToRoleDtoValidator()
        {
            RuleFor(x => x.PermissionType)
                .IsInEnum()
                .WithMessage(L["Validation:Permission:InvalidType"]);

            RuleFor(x => x.ResourceType)
                .IsInEnum()
                .WithMessage(L["Validation:Resource:InvalidType"]);
        }
    }
}
