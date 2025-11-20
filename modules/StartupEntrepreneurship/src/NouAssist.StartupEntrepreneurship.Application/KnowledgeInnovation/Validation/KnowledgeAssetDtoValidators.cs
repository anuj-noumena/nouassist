using FluentValidation;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Validation
{
    public class CreateKnowledgeAssetDtoValidator : AbstractValidator<CreateKnowledgeAssetDto>
    {
        public CreateKnowledgeAssetDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters")
                .MaximumLength(128).WithMessage("Title cannot exceed 128 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.Author).NotNull().WithMessage("Author information is required");
            
            When(x => x.Author != null, () =>
            {
                RuleFor(x => x.Author.Name)
                    .NotEmpty().WithMessage("Author name is required")
                    .MaximumLength(64).WithMessage("Author name cannot exceed 64 characters");
                
                RuleFor(x => x.Author.Title)
                    .NotEmpty().WithMessage("Author title is required")
                    .MaximumLength(128).WithMessage("Author title cannot exceed 128 characters");
            });

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(100000).WithMessage("Content cannot exceed 100,000 characters");

            RuleFor(x => x.ContentType)
                .NotEmpty().WithMessage("Content type is required")
                .MaximumLength(100).WithMessage("Content type cannot exceed 100 characters");
        }
    }

    public class UpdateKnowledgeAssetDtoValidator : AbstractValidator<UpdateKnowledgeAssetDto>
    {
        public UpdateKnowledgeAssetDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters")
                .MaximumLength(128).WithMessage("Title cannot exceed 128 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
        }
    }

    public class UpdateContentDtoValidator : AbstractValidator<UpdateContentDto>
    {
        public UpdateContentDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(100000).WithMessage("Content cannot exceed 100,000 characters");
        }
    }
}
