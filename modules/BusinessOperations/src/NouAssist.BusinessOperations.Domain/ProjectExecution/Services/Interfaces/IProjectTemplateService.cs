using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces
{
    /// <summary>
    /// Service for managing project templates
    /// </summary>
    public interface IProjectTemplateService
    {
        /// <summary>
        /// Creates a new project from a template
        /// </summary>
        Task<Project> CreateProjectFromTemplateAsync(
            string templateName,
            string projectName,
            string projectDescription,
            DateTime startDate,
            DateTime endDate);
            
        /// <summary>
        /// Creates a template from an existing project
        /// </summary>
        Task<bool> CreateTemplateFromProjectAsync(
            Guid projectId,
            string templateName,
            string templateDescription);
            
        /// <summary>
        /// Gets a list of available templates
        /// </summary>
        Task<List<string>> GetAvailableTemplatesAsync();
    }
}
