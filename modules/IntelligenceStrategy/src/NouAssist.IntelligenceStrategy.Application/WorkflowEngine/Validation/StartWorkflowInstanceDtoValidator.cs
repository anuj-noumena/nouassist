namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Validation
{
    /// <summary>
    /// Validator for the StartWorkflowInstanceDto
    /// </summary>
    public class StartWorkflowInstanceDtoValidator : AbstractValidator<StartWorkflowInstanceDto>
    {
        /// <summary>
        /// Creates a new instance of <see cref="StartWorkflowInstanceDtoValidator"/>
        /// </summary>
        public StartWorkflowInstanceDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(WorkflowEngineConsts.MaxNameLength)
                .WithMessage(L("The name must not be empty and should be at most {0} characters.", WorkflowEngineConsts.MaxNameLength));
            
            // Add validation for context data if needed
            RuleForEach(x => x.ContextData)
                .Must((kvp) => kvp.Key.Length <= 100)
                .WithMessage(L("Context data keys should be at most {0} characters.", 100));
            
            RuleForEach(x => x.ContextData)
                .Must((kvp) => kvp.Value == null || kvp.Value.Length <= 1000)
                .WithMessage(L("Context data values should be at most {0} characters.", 1000));
        }

        private static string L(string name, params object[] args)
        {
            return string.Format(name, args);
        }
    }
}
