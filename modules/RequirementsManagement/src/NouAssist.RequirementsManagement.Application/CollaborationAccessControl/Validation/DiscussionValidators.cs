using FluentValidation;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Constants;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions.Dtos;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Validation
{
    public class CreateDiscussionThreadDtoValidator : AbstractValidator<CreateDiscussionThreadDto>
    {
        public CreateDiscussionThreadDtoValidator()
        {
            RuleFor(x => x.DiscussionBoardId)
                .NotEmpty()
                .WithMessage(L["Validation:Thread:BoardRequired"]);

            RuleFor(x => x.Subject)
                .NotEmpty()
                .MaximumLength(CollaborationConsts.MaxThreadSubjectLength)
                .WithMessage(L["Validation:Thread:SubjectRequired"]);

            RuleFor(x => x.InitialComment)
                .NotEmpty()
                .MaximumLength(CollaborationConsts.MaxCommentContentLength)
                .WithMessage(L["Validation:Comment:ContentRequired"]);
        }
    }

    public class AddCommentDtoValidator : AbstractValidator<AddCommentDto>
    {
        public AddCommentDtoValidator()
        {
            RuleFor(x => x.ThreadId)
                .NotEmpty()
                .WithMessage(L["Validation:Comment:ThreadRequired"]);

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(CollaborationConsts.MaxCommentContentLength)
                .WithMessage(L["Validation:Comment:ContentRequired"]);
        }
    }

    public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
    {
        public UpdateCommentDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(CollaborationConsts.MaxCommentContentLength)
                .WithMessage(L["Validation:Comment:ContentRequired"]);
        }
    }
}
