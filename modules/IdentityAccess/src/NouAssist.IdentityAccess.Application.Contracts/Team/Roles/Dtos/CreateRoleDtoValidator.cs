namespace NouAssist.IdentityAccess.Team.Roles.Dtos
{
    public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.Permissions)
                .NotEmpty()
                .WithMessage("At least one permission must be assigned");
        }
    }

    public class UpdateRoleDtoValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }

    public class AssignRoleToUserDtoValidator : AbstractValidator<AssignRoleToUserDto>
    {
        public AssignRoleToUserDtoValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ExpiresAt)
                .GreaterThan(System.DateTime.UtcNow)
                .When(x => x.ExpiresAt.HasValue)
                .WithMessage("Expiry date must be in the future");
        }
    }
}
