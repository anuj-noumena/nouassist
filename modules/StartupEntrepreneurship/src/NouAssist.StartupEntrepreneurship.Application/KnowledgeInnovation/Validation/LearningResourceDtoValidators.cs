using FluentValidation;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Validation
{
    public class CreateLearningResourceDtoValidator : AbstractValidator<CreateLearningResourceDto>
    {
        public CreateLearningResourceDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters")
                .MaximumLength(128).WithMessage("Title cannot exceed 128 characters");

            RuleFor(x => x.ResourceUrl)
                .NotEmpty().WithMessage("Resource URL is required")
                .MaximumLength(2048).WithMessage("Resource URL cannot exceed 2048 characters");

            RuleFor(x => x.Description).NotNull().WithMessage("Description is required");
            
            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description.Overview)
                    .NotEmpty().WithMessage("Overview is required")
                    .MaximumLength(2000).WithMessage("Overview cannot exceed 2000 characters");
                
                RuleFor(x => x.Description.LearningObjectives)
                    .NotEmpty().WithMessage("Learning objectives are required")
                    .MaximumLength(2000).WithMessage("Learning objectives cannot exceed 2000 characters");
                
                RuleFor(x => x.Description.EstimatedDurationMinutes)
                    .GreaterThanOrEqualTo(1).WithMessage("Duration must be at least 1 minute")
                    .LessThanOrEqualTo(1440).WithMessage("Duration cannot exceed 1440 minutes (24 hours)");
            });
        }
    }

    public class UpdateLearningResourceDtoValidator : AbstractValidator<UpdateLearningResourceDto>
    {
        public UpdateLearningResourceDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters")
                .MaximumLength(128).WithMessage("Title cannot exceed 128 characters");

            RuleFor(x => x.ResourceUrl)
                .NotEmpty().WithMessage("Resource URL is required")
                .MaximumLength(2048).WithMessage("Resource URL cannot exceed 2048 characters");

            RuleFor(x => x.Description).NotNull().WithMessage("Description is required");
            
            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description.Overview)
                    .NotEmpty().WithMessage("Overview is required")
                    .MaximumLength(2000).WithMessage("Overview cannot exceed 2000 characters");
                
                RuleFor(x => x.Description.LearningObjectives)
                    .NotEmpty().WithMessage("Learning objectives are required")
                    .MaximumLength(2000).WithMessage("Learning objectives cannot exceed 2000 characters");
                
                RuleFor(x => x.Description.EstimatedDurationMinutes)
                    .GreaterThanOrEqualTo(1).WithMessage("Duration must be at least 1 minute")
                    .LessThanOrEqualTo(1440).WithMessage("Duration cannot exceed 1440 minutes (24 hours)");
            });
        }
    }

    public class UpdateProgressDtoValidator : AbstractValidator<UpdateProgressDto>
    {
        public UpdateProgressDtoValidator()
        {
            RuleFor(x => x.ProgressPercentage)
                .InclusiveBetween(0, 100).WithMessage("Progress percentage must be between 0 and 100");
        }
    }
}
