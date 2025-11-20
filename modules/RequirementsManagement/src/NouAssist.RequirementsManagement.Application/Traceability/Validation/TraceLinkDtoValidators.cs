using FluentValidation;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.TraceLink.Dtos;

namespace NouAssist.RequirementsManagement.Traceability.Validation
{
    /// <summary>
    /// FluentValidation validator for CreateTraceLinkDto
    /// </summary>
    public class CreateTraceLinkDtoValidator : AbstractValidator<CreateTraceLinkDto>
    {
        public CreateTraceLinkDtoValidator()
        {
            RuleFor(x => x.SourceArtifactId).NotEmpty().WithMessage("Source artifact ID is required");
            RuleFor(x => x.SourceArtifactType).IsInEnum().WithMessage("Invalid source artifact type");
            RuleFor(x => x.SourceArtifactName).NotEmpty().WithMessage("Source artifact name is required");
            
            RuleFor(x => x.TargetArtifactId).NotEmpty().WithMessage("Target artifact ID is required");
            RuleFor(x => x.TargetArtifactType).IsInEnum().WithMessage("Invalid target artifact type");
            RuleFor(x => x.TargetArtifactName).NotEmpty().WithMessage("Target artifact name is required");
            
            RuleFor(x => x.LinkType).IsInEnum().WithMessage("Invalid link type");
            RuleFor(x => x.LinkDirection).IsInEnum().WithMessage("Invalid link direction");
            
            RuleFor(x => x.CustomLinkType)
                .NotEmpty()
                .When(x => x.LinkType == LinkType.Custom)
                .WithMessage("Custom link type is required when Link Type is Custom");
                
            RuleFor(x => x.SourceArtifactId)
                .NotEqual(x => x.TargetArtifactId)
                .When(x => x.SourceArtifactType == x.TargetArtifactType)
                .WithMessage("Source and target artifacts cannot be the same");
        }
    }
    
    /// <summary>
    /// FluentValidation validator for UpdateTraceLinkDto
    /// </summary>
    public class UpdateTraceLinkDtoValidator : AbstractValidator<UpdateTraceLinkDto>
    {
        public UpdateTraceLinkDtoValidator()
        {
            RuleFor(x => x.LinkType).IsInEnum().WithMessage("Invalid link type");
            RuleFor(x => x.LinkDirection).IsInEnum().WithMessage("Invalid link direction");
            
            RuleFor(x => x.CustomLinkType)
                .NotEmpty()
                .When(x => x.LinkType == LinkType.Custom)
                .WithMessage("Custom link type is required when Link Type is Custom");
        }
    }
}
