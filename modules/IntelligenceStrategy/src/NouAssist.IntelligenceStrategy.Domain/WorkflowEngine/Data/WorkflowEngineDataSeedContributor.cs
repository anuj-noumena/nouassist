namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Data
{
    /// <summary>
    /// Data seeder for the Workflow Engine module
    /// </summary>
    public class WorkflowEngineDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IWorkflowTemplateRepository _templateRepository;
        private readonly IWorkflowDefinitionRepository _workflowRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<WorkflowEngineDataSeedContributor> _logger;

        public WorkflowEngineDataSeedContributor(
            IWorkflowTemplateRepository templateRepository,
            IWorkflowDefinitionRepository workflowRepository,
            IGuidGenerator guidGenerator,
            ILogger<WorkflowEngineDataSeedContributor> logger)
        {
            _templateRepository = templateRepository;
            _workflowRepository = workflowRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _templateRepository.GetCountAsync() > 0)
            {
                return;
            }

            _logger.LogInformation("Seeding workflow templates and definitions");

            try
            {
                // Create a basic approval workflow template
                var approvalTemplate = WorkflowTemplate.Create(
                    _guidGenerator.Create(),
                    "Basic Approval Process",
                    "A simple approval workflow with request, review, and approval/rejection steps",
                    "Approval",
                    GenerateBasicApprovalBpmn(),
                    "2.0",
                    context.TenantId
                );
                
                approvalTemplate.SetTags(new List<string> { "approval", "basic", "review" });
                approvalTemplate.MakePublic();
                
                await _templateRepository.InsertAsync(approvalTemplate);
                
                // Create a workflow definition from the template
                var workflowDefinition = approvalTemplate.CreateWorkflowDefinition(
                    _guidGenerator.Create(),
                    "Document Approval Workflow",
                    context.TenantId
                );
                
                await _workflowRepository.InsertAsync(workflowDefinition);
                
                // Publish the workflow
                var version = workflowDefinition.GetLatestVersion();
                
                // Add activities
                var startEvent = version.AddActivity(
                    _guidGenerator.Create(),
                    "Start",
                    ActivityType.StartEvent
                );
                
                var requestActivity = version.AddActivity(
                    _guidGenerator.Create(),
                    "Submit Request",
                    ActivityType.UserTask
                );
                
                var reviewActivity = version.AddActivity(
                    _guidGenerator.Create(),
                    "Review Request",
                    ActivityType.UserTask
                );
                
                var approveGateway = version.AddActivity(
                    _guidGenerator.Create(),
                    "Approval Decision",
                    ActivityType.Gateway
                );
                
                var approveActivity = version.AddActivity(
                    _guidGenerator.Create(),
                    "Approve Request",
                    ActivityType.UserTask
                );
                
                var rejectActivity = version.AddActivity(
                    _guidGenerator.Create(),
                    "Reject Request",
                    ActivityType.UserTask
                );
                
                var endEvent = version.AddActivity(
                    _guidGenerator.Create(),
                    "End",
                    ActivityType.EndEvent
                );
                
                // Add conditions
                version.AddCondition(
                    _guidGenerator.Create(),
                    "To Submit",
                    startEvent.Id,
                    requestActivity.Id,
                    "true",
                    "javascript"
                );
                
                version.AddCondition(
                    _guidGenerator.Create(),
                    "To Review",
                    requestActivity.Id,
                    reviewActivity.Id,
                    "true",
                    "javascript"
                );
                
                version.AddCondition(
                    _guidGenerator.Create(),
                    "To Decision",
                    reviewActivity.Id,
                    approveGateway.Id,
                    "true",
                    "javascript"
                );
                
                version.AddCondition(
                    _guidGenerator.Create(),
                    "Approve Path",
                    approveGateway.Id,
                    approveActivity.Id,
                    "decision == 'approve'",
                    "javascript"
                );
                
                version.AddCondition(
                    _guidGenerator.Create(),
                    "Reject Path",
                    approveGateway.Id,
                    rejectActivity.Id,
                    "decision == 'reject'",
                    "javascript"
                );
                
                version.AddCondition(
                    _guidGenerator.Create(),
                    "Approved To End",
                    approveActivity.Id,
                    endEvent.Id,
                    "true",
                    "javascript"
                );
                
                version.AddCondition(
                    _guidGenerator.Create(),
                    "Rejected To End",
                    rejectActivity.Id,
                    endEvent.Id,
                    "true",
                    "javascript"
                );
                
                workflowDefinition.PublishVersion(version.VersionNumber);
                
                await _workflowRepository.UpdateAsync(workflowDefinition);
                
                _logger.LogInformation("Successfully seeded workflow templates and definitions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding workflow templates and definitions");
            }
        }

        private string GenerateBasicApprovalBpmn()
        {
            // This is a simplified BPMN XML for a basic approval process
            // In a real implementation, this would be a properly structured BPMN 2.0 XML
            return @"<?xml version=""1.0"" encoding=""UTF-8""?>
<bpmn:definitions xmlns:bpmn=""http://www.omg.org/spec/BPMN/20100524/MODEL"" xmlns:bpmndi=""http://www.omg.org/spec/BPMN/20100524/DI"" xmlns:dc=""http://www.omg.org/spec/DD/20100524/DC"" xmlns:di=""http://www.omg.org/spec/DD/20100524/DI"" xmlns:modeler=""http://camunda.org/schema/modeler/1.0"" id=""Definitions_1"" targetNamespace=""http://bpmn.io/schema/bpmn"" exporter=""Camunda Modeler"" exporterVersion=""5.0.0"" modeler:executionPlatform=""Camunda Platform"" modeler:executionPlatformVersion=""7.15.0"">
  <bpmn:process id=""Process_0yai2jw"" isExecutable=""true"">
    <bpmn:startEvent id=""StartEvent_1"" name=""Start"">
      <bpmn:outgoing>Flow_0tof9l9</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id=""Activity_0wgbdrd"" name=""Submit Request"">
      <bpmn:incoming>Flow_0tof9l9</bpmn:incoming>
      <bpmn:outgoing>Flow_17gd9eq</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id=""Flow_0tof9l9"" sourceRef=""StartEvent_1"" targetRef=""Activity_0wgbdrd"" />
    <bpmn:task id=""Activity_0ymqgzy"" name=""Review Request"">
      <bpmn:incoming>Flow_17gd9eq</bpmn:incoming>
      <bpmn:outgoing>Flow_0hqxpjf</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id=""Flow_17gd9eq"" sourceRef=""Activity_0wgbdrd"" targetRef=""Activity_0ymqgzy"" />
    <bpmn:exclusiveGateway id=""Gateway_0u6jnov"" name=""Approval Decision"">
      <bpmn:incoming>Flow_0hqxpjf</bpmn:incoming>
      <bpmn:outgoing>Flow_1biqbeu</bpmn:outgoing>
      <bpmn:outgoing>Flow_1rnwfnj</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id=""Flow_0hqxpjf"" sourceRef=""Activity_0ymqgzy"" targetRef=""Gateway_0u6jnov"" />
    <bpmn:task id=""Activity_1ixe4cw"" name=""Approve Request"">
      <bpmn:incoming>Flow_1biqbeu</bpmn:incoming>
      <bpmn:outgoing>Flow_0bx48nm</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id=""Flow_1biqbeu"" name=""Approve"" sourceRef=""Gateway_0u6jnov"" targetRef=""Activity_1ixe4cw"" />
    <bpmn:task id=""Activity_0eggveu"" name=""Reject Request"">
      <bpmn:incoming>Flow_1rnwfnj</bpmn:incoming>
      <bpmn:outgoing>Flow_19lpw1v</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id=""Flow_1rnwfnj"" name=""Reject"" sourceRef=""Gateway_0u6jnov"" targetRef=""Activity_0eggveu"" />
    <bpmn:endEvent id=""Event_1adfn2j"" name=""End"">
      <bpmn:incoming>Flow_0bx48nm</bpmn:incoming>
      <bpmn:incoming>Flow_19lpw1v</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id=""Flow_0bx48nm"" sourceRef=""Activity_1ixe4cw"" targetRef=""Event_1adfn2j"" />
    <bpmn:sequenceFlow id=""Flow_19lpw1v"" sourceRef=""Activity_0eggveu"" targetRef=""Event_1adfn2j"" />
  </bpmn:process>
</bpmn:definitions>";
        }
    }
}
