using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services
{
    /// <summary>
    /// Domain service for business model canvas editing operations
    /// </summary>
    public class CanvasEditorService : DomainService, ICanvasEditorService
    {
        private readonly IBusinessModelCanvasRepository _canvasRepository;
        private readonly ILogger<CanvasEditorService> _logger;

        public CanvasEditorService(
            IBusinessModelCanvasRepository canvasRepository,
            ILogger<CanvasEditorService> logger)
        {
            _canvasRepository = canvasRepository;
            _logger = logger;
        }

        public async Task<BusinessModelCanvas> CreateCanvasAsync(
            string title,
            string description,
            Guid ownerId,
            Guid? tenantId,
            DateTime creationTime)
        {
            try
            {
                // Check for duplicate titles within the same tenant
                var existingCanvas = await _canvasRepository.FindByTitleAsync(title, tenantId);
                if (existingCanvas != null)
                {
                    throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:DuplicateTitle")
                        .WithData("title", title);
                }

                var canvas = BusinessModelCanvas.Create(
                    GuidGenerator.Create(),
                    title,
                    description,
                    ownerId,
                    tenantId,
                    creationTime
                );

                await _canvasRepository.InsertAsync(canvas);

                _logger.LogInformation("Business model canvas created: {CanvasId} - {Title}", canvas.Id, canvas.Title);

                return canvas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create business model canvas: {Title}", title);
                throw;
            }
        }

        public async Task<CanvasVersion> CreateVersionAsync(
            BusinessModelCanvas canvas,
            string versionName,
            string notes,
            Guid createdBy,
            DateTime creationTime)
        {
            try
            {
                var version = canvas.CreateNewVersion(versionName, notes, createdBy, creationTime);

                await _canvasRepository.UpdateAsync(canvas);

                _logger.LogInformation("Canvas version created: {VersionId} - {VersionName} for canvas {CanvasId}",
                    version.Id, version.VersionName, canvas.Id);

                return version;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create canvas version: {VersionName} for canvas {CanvasId}",
                    versionName, canvas.Id);
                throw;
            }
        }

        public async Task ValidateComponentContentAsync(CanvasComponentType componentType, ComponentContent content)
        {
            var validationRules = GetValidationRulesForComponent(componentType);

            foreach (var rule in validationRules)
            {
                await rule.ValidateAsync(content);
            }
        }

        public async Task<ValidationResult> AnalyzeCanvasCompletenessAsync(BusinessModelCanvas canvas)
        {
            var result = new ValidationResult();

            foreach (CanvasComponentType componentType in Enum.GetValues<CanvasComponentType>())
            {
                var component = canvas.GetComponent(componentType);
                if (component == null)
                {
                    result.AddError($"Component {componentType} is missing");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(component.Content.Text) && !component.Content.BulletPoints.Any())
                {
                    result.AddError($"Component {componentType} has no content");
                }

                if (component.ValidationState.Status == ValidationStatus.NotTested)
                {
                    result.AddWarning($"Component {componentType} has not been validated");
                }
            }

            return result;
        }

        public async Task<List<string>> GenerateContentSuggestionsAsync(
            CanvasComponentType componentType,
            BusinessModelCanvas canvas)
        {
            // This would typically integrate with AI/ML services for content suggestions
            // For now, providing basic template suggestions
            
            return componentType switch
            {
                CanvasComponentType.ValuePropositions => new List<string>
                {
                    "What unique value do you deliver to your customers?",
                    "What problems are you solving?",
                    "What customer needs are you addressing?",
                    "What products/services do you offer?"
                },
                CanvasComponentType.CustomerSegments => new List<string>
                {
                    "Who are your most important customers?",
                    "What are their characteristics?",
                    "How would you group them into segments?",
                    "What are their specific needs?"
                },
                CanvasComponentType.Channels => new List<string>
                {
                    "How do you reach your customers?",
                    "What channels work best?",
                    "Which channels are most cost-effective?",
                    "How do customers prefer to be reached?"
                },
                CanvasComponentType.RevenueStreams => new List<string>
                {
                    "How does your business make money?",
                    "What are customers willing to pay for?",
                    "What pricing models work best?",
                    "What are your revenue sources?"
                },
                _ => new List<string> { "Consider the key elements for this component", "Think about relationships with other components" }
            };
        }

        private List<IComponentValidationRule> GetValidationRulesForComponent(CanvasComponentType componentType)
        {
            // Factory pattern for component validation rules
            return componentType switch
            {
                CanvasComponentType.ValuePropositions => new List<IComponentValidationRule>
                {
                    new ContentLengthValidationRule(10, 1000),
                    new ValuePropositionSpecificValidationRule()
                },
                CanvasComponentType.CustomerSegments => new List<IComponentValidationRule>
                {
                    new ContentLengthValidationRule(10, 500),
                    new CustomerSegmentSpecificValidationRule()
                },
                _ => new List<IComponentValidationRule>
                {
                    new ContentLengthValidationRule(5, 2000)
                }
            };
        }
    }

    // Supporting interfaces and classes for validation
    public interface IComponentValidationRule
    {
        Task ValidateAsync(ComponentContent content);
    }

    public class ValidationResult
    {
        public List<string> Errors { get; } = new List<string>();
        public List<string> Warnings { get; } = new List<string>();
        public bool IsValid => !Errors.Any();

        public void AddError(string error) => Errors.Add(error);
        public void AddWarning(string warning) => Warnings.Add(warning);
    }

    public class ContentLengthValidationRule : IComponentValidationRule
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public ContentLengthValidationRule(int minLength, int maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        public Task ValidateAsync(ComponentContent content)
        {
            var totalLength = content.Text.Length + string.Join("", content.BulletPoints).Length;
            
            if (totalLength < _minLength)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:Validation:ContentTooShort")
                    .WithData("minLength", _minLength)
                    .WithData("actualLength", totalLength);
            }

            if (totalLength > _maxLength)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:Validation:ContentTooLong")
                    .WithData("maxLength", _maxLength)
                    .WithData("actualLength", totalLength);
            }

            return Task.CompletedTask;
        }
    }

    public class ValuePropositionSpecificValidationRule : IComponentValidationRule
    {
        public Task ValidateAsync(ComponentContent content)
        {
            // Specific validation for value propositions
            var text = content.Text.ToLower();
            
            if (!text.Contains("value") && !text.Contains("benefit") && !text.Contains("problem"))
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:Validation:ValuePropositionIncomplete");
            }

            return Task.CompletedTask;
        }
    }

    public class CustomerSegmentSpecificValidationRule : IComponentValidationRule
    {
        public Task ValidateAsync(ComponentContent content)
        {
            // Specific validation for customer segments
            var hasSegmentIdentifiers = content.BulletPoints.Any() || 
                                       content.Text.Contains("segment") || 
                                       content.Text.Contains("customer");

            if (!hasSegmentIdentifiers)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:Validation:CustomerSegmentIncomplete");
            }

            return Task.CompletedTask;
        }
    }
}
