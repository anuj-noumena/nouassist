namespace NouAssist.IdentityAccess.Team.Teams.Dtos
{
    public class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
    {
        public CreateTeamDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.TeamType)
                .IsInEnum();
        }
    }

    public class UpdateTeamDtoValidator : AbstractValidator<UpdateTeamDto>
    {
        public UpdateTeamDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.MaxMembers)
                .GreaterThan(0)
                .When(x => x.MaxMembers.HasValue);
        }
    }

    public class AddTeamMemberDtoValidator : AbstractValidator<AddTeamMemberDto>
    {
        public AddTeamMemberDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Role)
                .IsInEnum();

            RuleFor(x => x.Title)
                .MaximumLength(200);
        }
    }
}
