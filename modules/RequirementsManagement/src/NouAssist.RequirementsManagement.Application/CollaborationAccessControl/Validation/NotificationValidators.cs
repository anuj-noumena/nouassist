using FluentValidation;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Notifications.Dtos;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Validation
{
    public class AddSubscriptionRuleDtoValidator : AbstractValidator<AddSubscriptionRuleDto>
    {
        public AddSubscriptionRuleDtoValidator()
        {
            RuleFor(x => x.EventType)
                .IsInEnum()
                .WithMessage(L["Validation:Notification:InvalidEventType"]);

            RuleFor(x => x.ResourceType)
                .IsInEnum()
                .WithMessage(L["Validation:Resource:InvalidType"]);

            RuleFor(x => x.Channel)
                .IsInEnum()
                .WithMessage(L["Validation:Notification:InvalidChannel"]);
        }
    }

    public class UpdateSubscriptionRuleDtoValidator : AbstractValidator<UpdateSubscriptionRuleDto>
    {
        public UpdateSubscriptionRuleDtoValidator()
        {
            RuleFor(x => x.Channel)
                .IsInEnum()
                .WithMessage(L["Validation:Notification:InvalidChannel"]);
        }
    }
}
