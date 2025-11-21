using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Interfaces.Services
{
    /// <summary>
    /// Interface for the workflow template manager domain service
    /// </summary>
    public interface IWorkflowTemplateManager
    {
        /// <summary>
        /// Creates a new workflow template
        /// </summary>
        /// <param name="name">The name of the template</param>
        /// <param name="description">The description of the template</param>
        /// <param name="category">The category of the template</param>
        /// <param name="bpmnXml">The BPMN XML content</param>
        /// <param name="bpmnVersion">The BPMN version</param>
        /// <returns>The created workflow template</returns>
        Task<WorkflowTemplate> CreateTemplateAsync(
            string name,
            string description,
            string category,
            string bpmnXml,
            string bpmnVersion);

        /// <summary>
        /// Creates a workflow definition from a template
        /// </summary>
        /// <param name="templateId">The ID of the template</param>
        /// <param name="name">The name for the new workflow definition</param>
        /// <returns>The created workflow definition</returns>
        Task<WorkflowDefinition> CreateFromTemplateAsync(
            Guid templateId,
            string name);

        /// <summary>
        /// Makes a template public and available to all tenants
        /// </summary>
        /// <param name="templateId">The ID of the template</param>
        /// <returns>The updated workflow template</returns>
        Task<WorkflowTemplate> MakeTemplatePublicAsync(
            Guid templateId);

        /// <summary>
        /// Makes a template private and available only to its tenant
        /// </summary>
        /// <param name="templateId">The ID of the template</param>
        /// <returns>The updated workflow template</returns>
        Task<WorkflowTemplate> MakeTemplatePrivateAsync(
            Guid templateId);

        /// <summary>
        /// Sets tags for a template
        /// </summary>
        /// <param name="templateId">The ID of the template</param>
        /// <param name="tags">The tags to set</param>
        /// <returns>The updated workflow template</returns>
        Task<WorkflowTemplate> SetTemplateTagsAsync(
            Guid templateId,
            List<string> tags);
    }
}
