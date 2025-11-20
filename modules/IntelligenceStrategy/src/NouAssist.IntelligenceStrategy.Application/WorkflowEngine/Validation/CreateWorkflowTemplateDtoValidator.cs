namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Validation
{
    /// <summary>
    /// Validator for the CreateWorkflowTemplateDto
    /// </summary>
    public class CreateWorkflowTemplateDtoValidator : AbstractValidator<CreateWorkflowTemplateDto>
    {
        /// <summary>
        /// Creates a new instance of <see cref="CreateWorkflowTemplateDtoValidator"/>
        /// </summary>
        public CreateWorkflowTemplateDtoValidator()
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
            
            RuleFor(x => x.BpmnXmlContent)
                .NotEmpty()
                .MaximumLength(WorkflowEngineConsts.MaxBpmnDiagramLength)
                .WithMessage(L("The BPMN XML content must not be empty and should be at most {0} characters.", WorkflowEngineConsts.MaxBpmnDiagramLength));
            
            RuleFor(x => x.BpmnVersion)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage(L("The BPMN version must not be empty and should be at most {0} characters.", 20));
        }

        private static string L(string name, params object[] args)
        {
            return string.Format(name, args);
        }
    }
}
