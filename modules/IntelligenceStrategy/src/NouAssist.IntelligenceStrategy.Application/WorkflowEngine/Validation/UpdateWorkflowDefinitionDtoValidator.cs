namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Validation
{
    /// <summary>
    /// Validator for the UpdateWorkflowDefinitionDto
    /// </summary>
    public class UpdateWorkflowDefinitionDtoValidator : AbstractValidator<UpdateWorkflowDefinitionDto>
    {
        /// <summary>
        /// Creates a new instance of <see cref="UpdateWorkflowDefinitionDtoValidator"/>
        /// </summary>
        public UpdateWorkflowDefinitionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(WorkflowEngineConsts.MaxNameLength)
                .WithMessage(L("The name must not be empty and should be at most {0} characters.", WorkflowEngineConsts.MaxNameLength));
            
            RuleFor(x => x.Description)
                .MaximumLength(WorkflowEngineConsts.MaxDescriptionLength)
                .When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage(L("The description should be at most {0} characters.", WorkflowEngineConsts.MaxDescriptionLength));
            
            RuleFor(x => x.Category)
                .MaximumLength(128)
                .When(x => !string.IsNullOrEmpty(x.Category))
                .WithMessage(L("The category should be at most {0} characters.", 128));
        }

        private static string L(string name, params object[] args)
        {
            return string.Format(name, args);
        }
    }
}
