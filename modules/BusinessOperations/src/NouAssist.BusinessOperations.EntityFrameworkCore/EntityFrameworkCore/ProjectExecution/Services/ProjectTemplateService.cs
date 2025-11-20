using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Services
{
    /// <summary>
    /// Implementation of the project template service
    /// </summary>
    public class ProjectTemplateService : DomainService, IProjectTemplateService, ITransientDependency
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<ProjectTemplateService> _logger;
        
        // In a real implementation, we would have a repository for templates
        private readonly Dictionary<string, string> _templates = new()
        {
            { "Software Development", "Standard software development project template" },
            { "Marketing Campaign", "Template for marketing campaign projects" },
            { "Infrastructure Deployment", "Template for IT infrastructure deployment projects" }
        };
        
        public ProjectTemplateService(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            ILogger<ProjectTemplateService> logger)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _logger = logger;
        }
        
        public async Task<Project> CreateProjectFromTemplateAsync(
            string templateName,
            string projectName,
            string projectDescription,
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                if (!_templates.ContainsKey(templateName))
                {
                    throw new BusinessException("Template not found");
                }
                
                _logger.LogInformation(
                    "Creating project from template {TemplateName}",
                    templateName);
                
                var project = Project.Create(
                    GuidGenerator.Create(),
                    projectName,
                    projectDescription,
                    startDate,
                    endDate,
                    CurrentTenant.Id
                );
                
                // In a real implementation, we would load the template structure
                // and create phases, milestones, and tasks accordingly
                
                if (templateName == "Software Development")
                {
                    // Add phases
                    project.AddPhase("Requirements", startDate, startDate.AddDays(14), 1);
                    project.AddPhase("Design", startDate.AddDays(15), startDate.AddDays(30), 2);
                    project.AddPhase("Implementation", startDate.AddDays(31), startDate.AddDays(75), 3);
                    project.AddPhase("Testing", startDate.AddDays(76), endDate.AddDays(-15), 4);
                    project.AddPhase("Deployment", endDate.AddDays(-14), endDate, 5);
                    
                    // Add milestones
                    project.AddMilestone("Project Kickoff", startDate);
                    project.AddMilestone("Requirements Approved", startDate.AddDays(14));
                    project.AddMilestone("Design Completed", startDate.AddDays(30));
                    project.AddMilestone("Alpha Release", startDate.AddDays(60));
                    project.AddMilestone("Beta Release", startDate.AddDays(75));
                    project.AddMilestone("Go Live", endDate);
                }
                
                await _projectRepository.InsertAsync(project);
                
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create project from template {TemplateName}",
                    templateName);
                throw;
            }
        }
        
        public async Task<bool> CreateTemplateFromProjectAsync(
            Guid projectId,
            string templateName,
            string templateDescription)
        {
            try
            {
                var project = await _projectRepository.GetWithDetailsAsync(
                    projectId,
                    includePhases: true,
                    includeMilestones: true,
                    includeMembers: false);
                    
                if (project == null)
                {
                    throw new BusinessException("Project not found");
                }
                
                _logger.LogInformation(
                    "Creating template {TemplateName} from project {ProjectId}",
                    templateName,
                    projectId);
                
                // In a real implementation, we would save the template structure to a repository
                // For now, we'll just add it to our in-memory dictionary
                
                _templates[templateName] = templateDescription;
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create template from project {ProjectId}",
                    projectId);
                return false;
            }
        }
        
        public async Task<List<string>> GetAvailableTemplatesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching available project templates");
                
                // In a real implementation, we would load templates from a repository
                // For now, we'll just return our in-memory dictionary keys
                
                await Task.Delay(50); // Simulate loading delay
                
                return new List<string>(_templates.Keys);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch available project templates");
                return new List<string>();
            }
        }
    }
}
