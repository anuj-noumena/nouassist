using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Exceptions;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Interfaces.Services;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Services
{
    /// <summary>
    /// Domain service for managing workflow templates
    /// </summary>
    public class WorkflowTemplateManager : DomainService, IWorkflowTemplateManager
    {
        private readonly IWorkflowTemplateRepository _templateRepository;
        private readonly IWorkflowDefinitionRepository _workflowRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<WorkflowTemplateManager> _logger;

        public WorkflowTemplateManager(
            IWorkflowTemplateRepository templateRepository,
            IWorkflowDefinitionRepository workflowRepository,
            IGuidGenerator guidGenerator,
            ILogger<WorkflowTemplateManager> logger)
        {
            _templateRepository = templateRepository;
            _workflowRepository = workflowRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<WorkflowTemplate> CreateTemplateAsync(
            string name,
            string description,
            string category,
            string bpmnXml,
            string bpmnVersion)
        {
            // Check if a template with the same name already exists
            var exists = await _templateRepository.ExistsByNameAsync(name);
            if (exists)
            {
                throw new WorkflowEngineDomainException("WorkflowEngine:ErrorMessage:DuplicateName")
                    .WithData("entityType", "WorkflowTemplate")
                    .WithData("name", name);
            }

            try
            {
                var template = WorkflowTemplate.Create(
                    _guidGenerator.Create(),
                    name,
                    description,
                    category,
                    bpmnXml,
                    bpmnVersion,
                    CurrentTenant.Id
                );

                await _templateRepository.InsertAsync(template);

                _logger.LogInformation(
                    "Created workflow template {Name} with ID {Id}",
                    name, template.Id);

                return template;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating workflow template {Name}",
                    name);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowDefinition> CreateFromTemplateAsync(
            Guid templateId,
            string name)
        {
            // Check if a workflow with the same name already exists
            var exists = await _workflowRepository.ExistsByNameAsync(name);
            if (exists)
            {
                throw new WorkflowEngineDomainException("WorkflowEngine:ErrorMessage:DuplicateName")
                    .WithData("entityType", "WorkflowDefinition")
                    .WithData("name", name);
            }

            var template = await _templateRepository.GetAsync(templateId);

            try
            {
                var workflowDefinition = template.CreateWorkflowDefinition(
                    _guidGenerator.Create(),
                    name,
                    CurrentTenant.Id
                );

                await _workflowRepository.InsertAsync(workflowDefinition);

                _logger.LogInformation(
                    "Created workflow definition {Name} with ID {Id} from template {TemplateName} ({TemplateId})",
                    name, workflowDefinition.Id, template.Name, templateId);

                return workflowDefinition;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating workflow definition {Name} from template {TemplateName} ({TemplateId})",
                    name, template.Name, templateId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowTemplate> MakeTemplatePublicAsync(
            Guid templateId)
        {
            var template = await _templateRepository.GetAsync(templateId);

            try
            {
                template.MakePublic();

                await _templateRepository.UpdateAsync(template);

                _logger.LogInformation(
                    "Made workflow template {Name} ({Id}) public",
                    template.Name, templateId);

                return template;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error making workflow template {Name} ({Id}) public",
                    template.Name, templateId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowTemplate> MakeTemplatePrivateAsync(
            Guid templateId)
        {
            var template = await _templateRepository.GetAsync(templateId);

            try
            {
                template.MakePrivate();

                await _templateRepository.UpdateAsync(template);

                _logger.LogInformation(
                    "Made workflow template {Name} ({Id}) private",
                    template.Name, templateId);

                return template;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error making workflow template {Name} ({Id}) private",
                    template.Name, templateId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowTemplate> SetTemplateTagsAsync(
            Guid templateId,
            List<string> tags)
        {
            var template = await _templateRepository.GetAsync(templateId);

            try
            {
                template.SetTags(tags);

                await _templateRepository.UpdateAsync(template);

                _logger.LogInformation(
                    "Set tags for workflow template {Name} ({Id}): {Tags}",
                    template.Name, templateId, string.Join(", ", tags));

                return template;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error setting tags for workflow template {Name} ({Id})",
                    template.Name, templateId);
                throw;
            }
        }
    }
}
