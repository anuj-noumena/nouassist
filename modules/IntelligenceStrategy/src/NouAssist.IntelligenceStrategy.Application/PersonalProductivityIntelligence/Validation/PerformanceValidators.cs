using System;
using FluentValidation;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Validation
{
    /// <summary>
    /// Validator for CreatePerformanceSnapshotDto
    /// </summary>
    public class CreatePerformanceSnapshotDtoValidator : AbstractValidator<CreatePerformanceSnapshotDto>
    {
        public CreatePerformanceSnapshotDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required");
                
            RuleFor(x => x.SnapshotType)
                .NotEmpty()
                .WithMessage("Snapshot type is required")
                .MaximumLength(50)
                .WithMessage("Snapshot type must not exceed 50 characters");
                
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required");
                
            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("End date is required")
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be greater than or equal to start date");
        }
    }
    
    /// <summary>
    /// Validator for GeneratePerformanceInsightsDto
    /// </summary>
    public class GeneratePerformanceInsightsDtoValidator : AbstractValidator<GeneratePerformanceInsightsDto>
    {
        public GeneratePerformanceInsightsDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required");
                
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required");
                
            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("End date is required")
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be greater than or equal to start date");
                
            RuleFor(x => x.MaxInsights)
                .InclusiveBetween(1, 10)
                .WithMessage("Max insights must be between 1 and 10");
        }
    }
}
