using System;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Services;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.TaskManagement.Managers
{
    public class TaskInstanceManager : DomainService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskInstanceRepository _taskInstanceRepository;
        private readonly GtdOrganizationService _gtdOrganizationService;

        public TaskInstanceManager(
            ITaskRepository taskRepository,
            ITaskInstanceRepository taskInstanceRepository,
            GtdOrganizationService gtdOrganizationService)
        {
            _taskRepository = taskRepository;
            _taskInstanceRepository = taskInstanceRepository;
            _gtdOrganizationService = gtdOrganizationService;
        }

        public async Task<TaskInstance> CreateAsync(
            Guid taskId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid ownerId,
            Guid? projectId = null,
            Guid? areaId = null,
            Guid? contextId = null,
            Guid? reporterId = null,
            Guid? assigneeId = null,
            double? importance = null,
            double? urgency = null,
            Guid? tenantId = null)
        {
            // Load blueprint
            var taskBlueprint = await _taskRepository.GetWithDetailsAsync(taskId);

            if (!taskBlueprint.IsActive)
            {
                throw new BusinessException("Tasks:InactiveTaskBlueprint");
            }

            // Validate organization
            _gtdOrganizationService.ValidateOrganization(projectId, areaId);

            // Use blueprint defaults if not provided
            var effectiveProjectId = projectId ?? taskBlueprint.ProjectId;
            var effectiveAreaId = areaId ?? taskBlueprint.GetEffectiveAreaForInstance();
            var effectiveContextId = contextId ?? taskBlueprint.GetEffectiveContextForInstance();
            var effectiveImportance = importance ?? taskBlueprint.DefaultImportance;
            var effectiveUrgency = urgency ?? taskBlueprint.DefaultUrgency;

            // Final validation
            _gtdOrganizationService.ValidateOrganization(effectiveProjectId, effectiveAreaId);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                taskId,
                workflowId,
                workflowStatusId,
                ownerId,
                effectiveProjectId,
                effectiveAreaId,
                effectiveContextId,
                effectiveImportance,
                effectiveUrgency,
                tenantId,
                reporterId,
                assigneeId);

            // Initialize checklist from blueprint
            var checklistItemIds = taskBlueprint.ChecklistItems.Select(i => i.Id).ToList();
            if (checklistItemIds.Any())
            {
                instance.InitializeChecklist(checklistItemIds);
            }

            return instance;
        }
    }
}