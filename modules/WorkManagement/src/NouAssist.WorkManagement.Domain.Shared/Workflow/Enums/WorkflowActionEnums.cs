namespace NouAssist.WorkManagement.Workflow.Enums
{
    /// <summary>
    /// Types of actions that can be executed after a transition
    /// </summary>
    public enum WorkflowActionType : byte
    {
        /// <summary>
        /// Assign the task to a specific user or role
        /// </summary>
        AssignToUser = 1,

        /// <summary>
        /// Assign to the reporter/creator of the task
        /// </summary>
        AssignToReporter = 2,

        /// <summary>
        /// Assign to the current user performing the transition
        /// </summary>
        AssignToCurrentUser = 3,

        /// <summary>
        /// Send notification to users or roles
        /// </summary>
        SendNotification = 4,

        /// <summary>
        /// Send email notification
        /// </summary>
        SendEmail = 5,

        /// <summary>
        /// Update a field on the task
        /// </summary>
        UpdateField = 6,

        /// <summary>
        /// Set a custom field value
        /// </summary>
        SetCustomField = 7,

        /// <summary>
        /// Create a subtask automatically
        /// </summary>
        CreateSubtask = 8,

        /// <summary>
        /// Link to another task
        /// </summary>
        LinkTask = 9,

        /// <summary>
        /// Trigger a webhook
        /// </summary>
        TriggerWebhook = 10,

        /// <summary>
        /// Execute a custom script or function
        /// </summary>
        ExecuteScript = 11,

        /// <summary>
        /// Add a comment
        /// </summary>
        AddComment = 12,

        /// <summary>
        /// Update task priority
        /// </summary>
        UpdatePriority = 13,

        /// <summary>
        /// Set due date
        /// </summary>
        SetDueDate = 14,

        /// <summary>
        /// Clear assignee
        /// </summary>
        ClearAssignee = 15,

        /// <summary>
        /// Fire a domain event
        /// </summary>
        FireDomainEvent = 16,

        /// <summary>
        /// Validate required fields
        /// </summary>
        ValidateFields = 17,
        InitiateApproval = 18,  // Triggers ApprovalRequest
        CompleteApproval = 19,   // Processes pending approvals
        DelegateTask = 20
    }

    /// <summary>
    /// Extension methods for WorkflowActionType enum
    /// </summary>
    public static class WorkflowActionTypeExtensions
    {
        /// <summary>
        /// Gets the display name for the action type
        /// </summary>
        public static string GetDisplayName(this WorkflowActionType actionType)
        {
            return actionType switch
            {
                WorkflowActionType.AssignToUser => "Assign to User",
                WorkflowActionType.AssignToReporter => "Assign to Reporter",
                WorkflowActionType.AssignToCurrentUser => "Assign to Current User",
                WorkflowActionType.SendNotification => "Send Notification",
                WorkflowActionType.SendEmail => "Send Email",
                WorkflowActionType.UpdateField => "Update Field",
                WorkflowActionType.SetCustomField => "Set Custom Field",
                WorkflowActionType.CreateSubtask => "Create Subtask",
                WorkflowActionType.LinkTask => "Link Task",
                WorkflowActionType.TriggerWebhook => "Trigger Webhook",
                WorkflowActionType.ExecuteScript => "Execute Script",
                WorkflowActionType.AddComment => "Add Comment",
                WorkflowActionType.UpdatePriority => "Update Priority",
                WorkflowActionType.SetDueDate => "Set Due Date",
                WorkflowActionType.ClearAssignee => "Clear Assignee",
                WorkflowActionType.FireDomainEvent => "Fire Domain Event",
                WorkflowActionType.ValidateFields => "Validate Fields",
                _ => actionType.ToString()
            };
        }

        /// <summary>
        /// Determines if the action type requires configuration
        /// </summary>
        public static bool RequiresConfiguration(this WorkflowActionType actionType)
        {
            return actionType switch
            {
                // These actions don't need configuration
                WorkflowActionType.AssignToReporter => false,
                WorkflowActionType.AssignToCurrentUser => false,
                WorkflowActionType.ClearAssignee => false,

                // All other actions require some configuration
                WorkflowActionType.AssignToUser => true, // Requires: userId or roleId
                WorkflowActionType.SendNotification => true, // Requires: recipients, message
                WorkflowActionType.SendEmail => true, // Requires: recipients, template, subject
                WorkflowActionType.UpdateField => true, // Requires: fieldName, fieldValue
                WorkflowActionType.SetCustomField => true, // Requires: customFieldId, value
                WorkflowActionType.CreateSubtask => true, // Requires: taskTypeId, title, etc.
                WorkflowActionType.LinkTask => true, // Requires: taskId, linkType
                WorkflowActionType.TriggerWebhook => true, // Requires: webhookUrl, payload
                WorkflowActionType.ExecuteScript => true, // Requires: scriptId or scriptContent
                WorkflowActionType.AddComment => true, // Requires: comment text
                WorkflowActionType.UpdatePriority => true, // Requires: priority value
                WorkflowActionType.SetDueDate => true, // Requires: date value or offset
                WorkflowActionType.FireDomainEvent => true, // Requires: eventType, eventData
                WorkflowActionType.ValidateFields => true, // Requires: field list to validate

                _ => true
            };
        }

        /// <summary>
        /// Gets the required configuration keys for the action type
        /// </summary>
        public static string[] GetRequiredConfigurationKeys(this WorkflowActionType actionType)
        {
            return actionType switch
            {
                WorkflowActionType.AssignToUser => ["userId"],
                WorkflowActionType.SendNotification => ["userIds", "message"],
                WorkflowActionType.SendEmail => ["recipients", "subject", "body"],
                WorkflowActionType.UpdateField => ["fieldName", "fieldValue"],
                WorkflowActionType.SetCustomField => ["customFieldId", "value"],
                WorkflowActionType.CreateSubtask => ["taskTypeId", "title"],
                WorkflowActionType.LinkTask => ["taskId", "linkType"],
                WorkflowActionType.TriggerWebhook => ["webhookUrl"],
                WorkflowActionType.ExecuteScript => ["scriptId"],
                WorkflowActionType.AddComment => ["comment"],
                WorkflowActionType.UpdatePriority => ["priority"],
                WorkflowActionType.SetDueDate => ["dueDate"],
                WorkflowActionType.FireDomainEvent => ["eventType"],
                WorkflowActionType.ValidateFields => ["fieldNames"],
                _ => []
            };
        }

        /// <summary>
        /// Determines if the action should be executed asynchronously by default
        /// </summary>
        public static bool ShouldExecuteAsync(this WorkflowActionType actionType)
        {
            return actionType switch
            {
                WorkflowActionType.SendEmail => true,
                WorkflowActionType.TriggerWebhook => true,
                WorkflowActionType.ExecuteScript => true,
                WorkflowActionType.CreateSubtask => true,
                _ => false
            };
        }

        /// <summary>
        /// Gets the category of the action
        /// </summary>
        public static WorkflowActionCategory GetCategory(this WorkflowActionType actionType)
        {
            return actionType switch
            {
                WorkflowActionType.AssignToUser => WorkflowActionCategory.Assignment,
                WorkflowActionType.AssignToReporter => WorkflowActionCategory.Assignment,
                WorkflowActionType.AssignToCurrentUser => WorkflowActionCategory.Assignment,
                WorkflowActionType.ClearAssignee => WorkflowActionCategory.Assignment,

                WorkflowActionType.SendNotification => WorkflowActionCategory.Notification,
                WorkflowActionType.SendEmail => WorkflowActionCategory.Notification,

                WorkflowActionType.UpdateField => WorkflowActionCategory.FieldUpdate,
                WorkflowActionType.SetCustomField => WorkflowActionCategory.FieldUpdate,
                WorkflowActionType.UpdatePriority => WorkflowActionCategory.FieldUpdate,
                WorkflowActionType.SetDueDate => WorkflowActionCategory.FieldUpdate,

                WorkflowActionType.CreateSubtask => WorkflowActionCategory.TaskManagement,
                WorkflowActionType.LinkTask => WorkflowActionCategory.TaskManagement,

                WorkflowActionType.TriggerWebhook => WorkflowActionCategory.Integration,
                WorkflowActionType.ExecuteScript => WorkflowActionCategory.Integration,
                WorkflowActionType.FireDomainEvent => WorkflowActionCategory.Integration,

                WorkflowActionType.AddComment => WorkflowActionCategory.Documentation,

                WorkflowActionType.ValidateFields => WorkflowActionCategory.Validation,

                _ => WorkflowActionCategory.Other
            };
        }
    }

    /// <summary>
    /// Categories for workflow actions
    /// </summary>
    public enum WorkflowActionCategory
    {
        Assignment,
        Notification,
        FieldUpdate,
        TaskManagement,
        Integration,
        Documentation,
        Validation,
        Other
    }
}