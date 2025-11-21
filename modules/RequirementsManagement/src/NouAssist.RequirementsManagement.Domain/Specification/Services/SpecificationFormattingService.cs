using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Specification.Interfaces.Services;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.Specification.Services
{
    /// <summary>
    /// Domain service for formatting requirements specifications
    /// </summary>
    public class SpecificationFormattingService : DomainService, ISpecificationFormattingService
    {
        private readonly ILogger<SpecificationFormattingService> _logger;

        public SpecificationFormattingService(ILogger<SpecificationFormattingService> logger)
        {
            _logger = logger;
        }

        public async Task<string> FormatRequirementAsync(Requirement requirement, string format = "html")
        {
            Check.NotNull(requirement, nameof(requirement));

            try
            {
                return format.ToLowerInvariant() switch
                {
                    "html" => await FormatRequirementAsHtmlAsync(requirement),
                    "markdown" => await FormatRequirementAsMarkdownAsync(requirement),
                    "json" => await FormatRequirementAsJsonAsync(requirement),
                    _ => throw new ArgumentException($"Unsupported format: {format}", nameof(format))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to format requirement {RequirementId} as {Format}",
                    requirement.Id,
                    format);
                throw;
            }
        }

        public async Task<string> FormatSpecificationDocumentAsync(
            SpecificationStructure structure,
            List<Requirement> requirements,
            string format = "html")
        {
            Check.NotNull(structure, nameof(structure));
            Check.NotNull(requirements, nameof(requirements));

            try
            {
                return format.ToLowerInvariant() switch
                {
                    "html" => await FormatDocumentAsHtmlAsync(structure, requirements),
                    "markdown" => await FormatDocumentAsMarkdownAsync(structure, requirements),
                    _ => throw new ArgumentException($"Unsupported format: {format}", nameof(format))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to format specification document {StructureId} as {Format}",
                    structure.Id,
                    format);
                throw;
            }
        }

        public async Task<byte[]> GenerateDocumentAsync(
            SpecificationStructure structure,
            List<Requirement> requirements,
            string format = "pdf")
        {
            Check.NotNull(structure, nameof(structure));
            Check.NotNull(requirements, nameof(requirements));

            try
            {
                return format.ToLowerInvariant() switch
                {
                    "pdf" => await GeneratePdfDocumentAsync(structure, requirements),
                    "docx" => await GenerateWordDocumentAsync(structure, requirements),
                    _ => throw new ArgumentException($"Unsupported format: {format}", nameof(format))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to generate document {StructureId} as {Format}",
                    structure.Id,
                    format);
                throw;
            }
        }

        private async Task<string> FormatRequirementAsHtmlAsync(Requirement requirement)
        {
            var html = new StringBuilder();
            html.AppendLine($"<div class='requirement' id='req-{requirement.Identifier.Value}'>");
            html.AppendLine($"  <h3>{requirement.Identifier.Value}: {requirement.Text.Title}</h3>");
            html.AppendLine($"  <p><strong>Type:</strong> {requirement.Type}</p>");
            html.AppendLine($"  <p><strong>Priority:</strong> {requirement.Priority}</p>");
            html.AppendLine($"  <p><strong>Status:</strong> {requirement.Status}</p>");
            html.AppendLine($"  <div class='description'>{requirement.Text.Description}</div>");

            if (requirement.AcceptanceCriteria.Any())
            {
                html.AppendLine("  <h4>Acceptance Criteria</h4>");
                html.AppendLine("  <ul>");
                foreach (var criterion in requirement.AcceptanceCriteria.OrderBy(c => c.Order))
                {
                    html.AppendLine($"    <li>{criterion.Statement.Statement}</li>");
                }
                html.AppendLine("  </ul>");
            }

            html.AppendLine("</div>");
            return await Task.FromResult(html.ToString());
        }

        private async Task<string> FormatRequirementAsMarkdownAsync(Requirement requirement)
        {
            var markdown = new StringBuilder();
            markdown.AppendLine($"## {requirement.Identifier.Value}: {requirement.Text.Title}");
            markdown.AppendLine();
            markdown.AppendLine($"**Type:** {requirement.Type}");
            markdown.AppendLine($"**Priority:** {requirement.Priority}");
            markdown.AppendLine($"**Status:** {requirement.Status}");
            markdown.AppendLine();
            markdown.AppendLine(requirement.Text.Description);

            if (requirement.AcceptanceCriteria.Any())
            {
                markdown.AppendLine();
                markdown.AppendLine("### Acceptance Criteria");
                foreach (var criterion in requirement.AcceptanceCriteria.OrderBy(c => c.Order))
                {
                    markdown.AppendLine($"- {criterion.Statement.Statement}");
                }
            }

            return await Task.FromResult(markdown.ToString());
        }

        private async Task<string> FormatRequirementAsJsonAsync(Requirement requirement)
        {
            var data = new
            {
                requirement.Id,
                Identifier = requirement.Identifier.Value,
                Title = requirement.Text.Title,
                Description = requirement.Text.Description,
                requirement.Type,
                requirement.Priority,
                requirement.Status,
                requirement.Version,
                AcceptanceCriteria = requirement.AcceptanceCriteria.Select(c => new
                {
                    c.Id,
                    Statement = c.Statement.Statement,
                    Description = c.Statement.Description,
                    Type = c.Statement.Type,
                    c.Order
                }),
                BusinessRules = requirement.AssociatedRules.Select(r => new
                {
                    r.Id,
                    Name = r.Rule.Name,
                    Description = r.Rule.Description
                }),
                Constraints = requirement.AssociatedConstraints.Select(c => new
                {
                    c.Id,
                    Name = c.Constraint.Name,
                    Statement = c.Constraint.Statement,
                    Type = c.Constraint.Type
                })
            };

            return await Task.FromResult(System.Text.Json.JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
        }

        private async Task<string> FormatDocumentAsHtmlAsync(SpecificationStructure structure, List<Requirement> requirements)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine($"  <title>{structure.Name}</title>");
            html.AppendLine("  <style>");
            html.AppendLine("    body { font-family: Arial, sans-serif; margin: 40px; }");
            html.AppendLine("    .requirement { margin-bottom: 30px; padding: 20px; border: 1px solid #ddd; }");
            html.AppendLine("    h1, h2, h3 { color: #333; }");
            html.AppendLine("  </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine($"  <h1>{structure.Name}</h1>");
            html.AppendLine($"  <p>{structure.Description}</p>");

            foreach (var section in structure.GetRootSections())
            {
                html.AppendLine($"  <section>");
                html.AppendLine($"    <h2>{section.SectionName}</h2>");
                html.AppendLine($"    <div>{section.Content}</div>");

                // Add requirements for this section if applicable
                var sectionRequirements = requirements.Where(r => /* logic to associate requirements with sections */ true);
                foreach (var requirement in sectionRequirements)
                {
                    var formattedReq = await FormatRequirementAsHtmlAsync(requirement);
                    html.AppendLine(formattedReq);
                }

                html.AppendLine($"  </section>");
            }

            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }

        private async Task<string> FormatDocumentAsMarkdownAsync(SpecificationStructure structure, List<Requirement> requirements)
        {
            var markdown = new StringBuilder();
            markdown.AppendLine($"# {structure.Name}");
            markdown.AppendLine();
            markdown.AppendLine(structure.Description);
            markdown.AppendLine();

            foreach (var section in structure.GetRootSections())
            {
                markdown.AppendLine($"## {section.SectionName}");
                markdown.AppendLine();
                markdown.AppendLine(section.Content);
                markdown.AppendLine();

                // Add requirements for this section if applicable
                var sectionRequirements = requirements.Where(r => /* logic to associate requirements with sections */ true);
                foreach (var requirement in sectionRequirements)
                {
                    var formattedReq = await FormatRequirementAsMarkdownAsync(requirement);
                    markdown.AppendLine(formattedReq);
                    markdown.AppendLine();
                }
            }

            return markdown.ToString();
        }

        private async Task<byte[]> GeneratePdfDocumentAsync(SpecificationStructure structure, List<Requirement> requirements)
        {
            // This would typically use a PDF generation library like iTextSharp or similar
            // For now, returning a placeholder
            var htmlContent = await FormatDocumentAsHtmlAsync(structure, requirements);
            return Encoding.UTF8.GetBytes(htmlContent);
        }

        private async Task<byte[]> GenerateWordDocumentAsync(SpecificationStructure structure, List<Requirement> requirements)
        {
            // This would typically use a library like DocumentFormat.OpenXml
            // For now, returning a placeholder
            var htmlContent = await FormatDocumentAsHtmlAsync(structure, requirements);
            return Encoding.UTF8.GetBytes(htmlContent);
        }
    }
}
