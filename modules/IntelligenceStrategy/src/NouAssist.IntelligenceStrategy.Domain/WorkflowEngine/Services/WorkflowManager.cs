using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Entities;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Exceptions;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Interfaces.Services;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Repositories;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Specifications;
using NouAssist.IntelligenceStrategy.WorkflowEngine.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Services
{
    /// <summary>
    /// Domain service for managing workflows
    /// </summary>
    public class WorkflowManager : DomainService, IWorkflowManager
    {
        private readonly IWorkflowDefinitionRepository _workflowRepository;
        private readonly IWorkflowInstanceRepository _instanceRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<WorkflowManager> _logger;

        public WorkflowManager(
            IWorkflowDefinitionRepository workflowRepository,
            IWorkflowInstanceRepository instanceRepository,
            IGuidGenerator guidGenerator,
            ILogger<WorkflowManager> logger)
        {
            _workflowRepository = workflowRepository;
            _instanceRepository = instanceRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<WorkflowDefinition> CreateWorkflowDefinitionAsync(
            string name,
            string description,
            string category)
        {
            // Check if a workflow with the same name already exists
            var exists = await _workflowRepository.ExistsByNameAsync(name);
            if (exists)
            {
                throw new WorkflowEngineDomainException("WorkflowEngine:ErrorMessage:DuplicateName")
                    .WithData("entityType", "WorkflowDefinition")
                    .WithData("name", name);
            }

            try
            {
                var workflowDefinition = WorkflowDefinition.Create(
                    _guidGenerator.Create(),
                    name,
                    description,
                    category,
                    CurrentTenant.Id
                );

                await _workflowRepository.InsertAsync(workflowDefinition);

                _logger.LogInformation(
                    "Created workflow definition {Name} with ID {Id}",
                    name, workflowDefinition.Id);

                return workflowDefinition;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating workflow definition {Name}",
                    name);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowVersion> CreateNewVersionAsync(
            Guid workflowDefinitionId,
            string comments = null)
        {
            var workflowDefinition = await _workflowRepository.GetAsync(
                workflowDefinitionId,
                includeDetails: true);

            try
            {
                var newVersion = workflowDefinition.CreateNewVersion(comments);

                await _workflowRepository.UpdateAsync(workflowDefinition);

                _logger.LogInformation(
                    "Created new version {VersionNumber} for workflow {Name} ({Id})",
                    newVersion.VersionNumber, workflowDefinition.Name, workflowDefinitionId);

                return newVersion;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating new version for workflow {Name} ({Id})",
                    workflowDefinition.Name, workflowDefinitionId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowDefinition> PublishVersionAsync(
            Guid workflowDefinitionId,
            int versionNumber)
        {
            var workflowDefinition = await _workflowRepository.GetAsync(
                workflowDefinitionId,
                includeDetails: true);

            try
            {
                workflowDefinition.PublishVersion(versionNumber);

                await _workflowRepository.UpdateAsync(workflowDefinition);

                _logger.LogInformation(
                    "Published version {VersionNumber} of workflow {Name} ({Id})",
                    versionNumber, workflowDefinition.Name, workflowDefinitionId);

                return workflowDefinition;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error publishing version {VersionNumber} of workflow {Name} ({Id})",
                    versionNumber, workflowDefinition.Name, workflowDefinitionId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowInstance> StartWorkflowAsync(
            Guid workflowDefinitionId,
            string name,
            Dictionary<string, string> initialContextData = null)
        {
            var workflowDefinition = await _workflowRepository.GetAsync(
                workflowDefinitionId,
                includeDetails: true);

            if (!workflowDefinition.IsPublished)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:WorkflowNotPublished",
                    "Cannot start an instance of an unpublished workflow."
                );
            }

            try
            {
                var publishedVersion = workflowDefinition.GetPublishedVersion();

                var instance = WorkflowInstance.Create(
                    _guidGenerator.Create(),
                    workflowDefinitionId,
                    publishedVersion.VersionNumber,
                    name,
                    CurrentTenant.Id,
                    initialContextData
                );

                // Create activity instances for start events
                foreach (var activity in publishedVersion.Activities.Where(a => a.ActivityType == ActivityType.StartEvent))
                {
                    instance.AddActivityInstance(
                        _guidGenerator.Create(),
                        activity.Id,
                        activity.Name,
                        activity.ActivityType
                    );
                }

                await _instanceRepository.InsertAsync(instance);

                _logger.LogInformation(
                    "Started workflow instance {Name} ({Id}) for workflow definition {WorkflowName} ({WorkflowId})",
                    name, instance.Id, workflowDefinition.Name, workflowDefinitionId);

                return instance;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error starting workflow instance {Name} for workflow definition {WorkflowName} ({WorkflowId})",
                    name, workflowDefinition.Name, workflowDefinitionId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowInstance> AssignTaskAsync(
            Guid workflowInstanceId,
            Guid activityInstanceId,
            AssigneeType assigneeType,
            Guid assigneeId,
            DateTime? dueDate = null)
        {
            var instance = await _instanceRepository.GetAsync(
                workflowInstanceId,
                includeDetails: true);

            try
            {
                var assignee = new AssigneeReference(assigneeType, assigneeId);
                instance.AssignTask(activityInstanceId, assignee, dueDate);

                await _instanceRepository.UpdateAsync(instance);

                _logger.LogInformation(
                    "Assigned task {ActivityId} in workflow instance {InstanceId} to {AssigneeType} {AssigneeId}",
                    activityInstanceId, workflowInstanceId, assigneeType, assigneeId);

                return instance;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error assigning task {ActivityId} in workflow instance {InstanceId}",
                    activityInstanceId, workflowInstanceId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowInstance> CompleteTaskAsync(
            Guid workflowInstanceId,
            Guid activityInstanceId,
            Dictionary<string, string> outputData = null)
        {
            var instance = await _instanceRepository.GetAsync(
                workflowInstanceId,
                includeDetails: true);

            try
            {
                instance.CompleteActivity(activityInstanceId, outputData);

                await _instanceRepository.UpdateAsync(instance);

                _logger.LogInformation(
                    "Completed task {ActivityId} in workflow instance {InstanceId}",
                    activityInstanceId, workflowInstanceId);

                return instance;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error completing task {ActivityId} in workflow instance {InstanceId}",
                    activityInstanceId, workflowInstanceId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowDefinition> AnalyzePerformanceAsync(
            Guid workflowDefinitionId)
        {
            var workflowDefinition = await _workflowRepository.GetAsync(
                workflowDefinitionId,
                includeDetails: true);

            try
            {
                // Get completed instances for this workflow definition
                var instances = await _instanceRepository.GetListAsync(
                    new CompletedWorkflowInstanceSpecification()
                        .And(new WorkflowDefinitionInstanceSpecification(workflowDefinitionId)),
                    includeDetails: true);

                if (!instances.Any())
                {
                    _logger.LogInformation(
                        "No completed instances found for workflow {Name} ({Id}), skipping performance analysis",
                        workflowDefinition.Name, workflowDefinitionId);
                    return workflowDefinition;
                }

                // Calculate average cycle time
                var totalCycleTime = instances
                    .Where(i => i.CompletionTime.HasValue)
                    .Sum(i => (i.CompletionTime.Value - i.StartTime).TotalMinutes);

                var averageCycleTime = totalCycleTime / instances.Count;

                // Find bottleneck activities (simplified logic)
                var bottleneckActivityId = Guid.Empty;
                var bottleneckScore = 0;

                // In a real implementation, this would involve more complex analysis
                // For example, finding activities with the longest average duration

                var metrics = new PerformanceMetrics(
                    averageCycleTime,
                    bottleneckScore,
                    bottleneckActivityId,
                    instances.Count);

                workflowDefinition.UpdatePerformanceMetrics(metrics);

                await _workflowRepository.UpdateAsync(workflowDefinition);

                _logger.LogInformation(
                    "Analyzed performance for workflow {Name} ({Id}), average cycle time: {CycleTime} minutes",
                    workflowDefinition.Name, workflowDefinitionId, averageCycleTime);

                return workflowDefinition;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error analyzing performance for workflow {Name} ({Id})",
                    workflowDefinition.Name, workflowDefinitionId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowDefinition> GenerateDocumentationAsync(
            Guid workflowDefinitionId)
        {
            var workflowDefinition = await _workflowRepository.GetAsync(
                workflowDefinitionId,
                includeDetails: true);

            try
            {
                var version = workflowDefinition.IsPublished 
                    ? workflowDefinition.GetPublishedVersion() 
                    : workflowDefinition.GetLatestVersion();

                // Generate documentation (simplified)
                var documentation = new StringBuilder();
                documentation.AppendLine($"# {workflowDefinition.Name}");
                documentation.AppendLine();
                documentation.AppendLine(workflowDefinition.Description);
                documentation.AppendLine();
                
                documentation.AppendLine("## Activities");
                documentation.AppendLine();
                
                foreach (var activity in version.Activities)
                {
                    documentation.AppendLine($"### {activity.Name}");
                    documentation.AppendLine($"Type: {activity.ActivityType}");
                    if (!string.IsNullOrEmpty(activity.Description))
                    {
                        documentation.AppendLine($"Description: {activity.Description}");
                    }
                    documentation.AppendLine();
                }
                
                documentation.AppendLine("## Conditions");
                documentation.AppendLine();
                
                foreach (var condition in version.Conditions)
                {
                    documentation.AppendLine($"### {condition.Name}");
                    var sourceActivity = version.Activities.FirstOrDefault(a => a.Id == condition.SourceActivityId);
                    var targetActivity = version.Activities.FirstOrDefault(a => a.Id == condition.TargetActivityId);
                    
                    documentation.AppendLine($"From: {sourceActivity?.Name ?? condition.SourceActivityId.ToString()}");
                    documentation.AppendLine($"To: {targetActivity?.Name ?? condition.TargetActivityId.ToString()}");
                    documentation.AppendLine($"Expression: `{condition.Expression.Expression}`");
                    documentation.AppendLine();
                }

                workflowDefinition.UpdateDocumentation(documentation.ToString());

                await _workflowRepository.UpdateAsync(workflowDefinition);

                _logger.LogInformation(
                    "Generated documentation for workflow {Name} ({Id})",
                    workflowDefinition.Name, workflowDefinitionId);

                return workflowDefinition;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error generating documentation for workflow {Name} ({Id})",
                    workflowDefinition.Name, workflowDefinitionId);
                throw;
            }
        }
    }
}
