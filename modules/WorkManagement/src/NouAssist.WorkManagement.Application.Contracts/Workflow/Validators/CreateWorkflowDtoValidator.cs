using FluentValidation;
using NouAssist.WorkManagement.Workflow.Dtos;

namespace NouAssist.WorkManagement.Workflow.Validators
{
    public class CreateWorkflowDtoValidator : AbstractValidator<CreateWorkflowDto>
    {
        public CreateWorkflowDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(256);
                
            RuleFor(x => x.Description)
                .MaximumLength(2000);
                
            RuleFor(x => x.InitialStatusId)
                .NotEmpty();
        }
    }
}