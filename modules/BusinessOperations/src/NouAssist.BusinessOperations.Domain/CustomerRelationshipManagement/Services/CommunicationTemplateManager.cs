using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services
{
    /// <summary>
    /// Implementation of the communication template domain service
    /// </summary>
    public class CommunicationTemplateManager : DomainService, ICommunicationTemplateManager
    {
        private readonly ICommunicationTemplateRepository _templateRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<CommunicationTemplateManager> _logger;

        public CommunicationTemplateManager(
            ICommunicationTemplateRepository templateRepository,
            IGuidGenerator guidGenerator,
            ILogger<CommunicationTemplateManager> logger)
        {
            _templateRepository = templateRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        public async Task<CommunicationTemplate> CreateAsync(
            string code,
            string name,
            InteractionType type,
            string subject,
            string content,
            string description = null,
            string category = null)
        {
            // Check for duplicate code
            var existingTemplate = await _templateRepository.GetByCodeAsync(code);
            if (existingTemplate != null)
            {
                throw new BusinessException("CRM:ErrorMessage:DuplicateTemplateCode")
                    .WithData("code", code);
            }
            
            var template = new CommunicationTemplate(
                _guidGenerator.Create(),
                code,
                name,
                type,
                subject,
                content,
                description,
                category,
                CurrentTenant.Id);
                
            return await _templateRepository.InsertAsync(template, autoSave: true);
        }

        public async Task<CommunicationTemplate> UpdateAsync(
            Guid id,
            string name = null,
            string subject = null,
            string content = null,
            string description = null,
            string category = null)
        {
            var template = await _templateRepository.GetAsync(id);
            
            template.Update(name, subject, content, description, category);
            
            return await _templateRepository.UpdateAsync(template, autoSave: true);
        }

        public async Task<CommunicationTemplate> ActivateAsync(
            Guid id)
        {
            var template = await _templateRepository.GetAsync(id);
            template.Activate();
            return await _templateRepository.UpdateAsync(template, autoSave: true);
        }

        public async Task<CommunicationTemplate> DeactivateAsync(
            Guid id)
        {
            var template = await _templateRepository.GetAsync(id);
            template.Deactivate();
            return await _templateRepository.UpdateAsync(template, autoSave: true);
        }

        public async Task<CommunicationTemplate> GetByCodeAsync(
            string code)
        {
            var template = await _templateRepository.GetByCodeAsync(code);
            
            if (template == null)
            {
                throw new BusinessException("CRM:ErrorMessage:TemplateNotFound")
                    .WithData("code", code);
            }
            
            return template;
        }

        public async Task<(string Subject, string Content)> RenderTemplateAsync(
            Guid id,
            Dictionary<string, string> variables)
        {
            var template = await _templateRepository.GetAsync(id);
            
            if (!template.IsActive)
            {
                throw new BusinessException("CRM:ErrorMessage:TemplateNotActive");
            }
            
            string renderedSubject = template.RenderSubject(variables);
            string renderedContent = template.RenderContent(variables);
            
            return (renderedSubject, renderedContent);
        }

        public async Task<(string Subject, string Content)> RenderTemplateByCodeAsync(
            string code,
            Dictionary<string, string> variables)
        {
            var template = await _templateRepository.GetByCodeAsync(code);
            
            if (template == null)
            {
                throw new BusinessException("CRM:ErrorMessage:TemplateNotFound")
                    .WithData("code", code);
            }
            
            if (!template.IsActive)
            {
                throw new BusinessException("CRM:ErrorMessage:TemplateNotActive");
            }
            
            string renderedSubject = template.RenderSubject(variables);
            string renderedContent = template.RenderContent(variables);
            
            return (renderedSubject, renderedContent);
        }
    }
}
