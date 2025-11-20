using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using TaskAggregate = NouAssist.WorkManagement.TaskManagement.Aggregates.Task;

namespace NouAssist.WorkManagement.TaskManagement.Services
{
    /// <summary>
    /// Domain service for automating task creation based on financial events
    /// </summary>
    public class FinancialTaskAutomationService : DomainService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskInstanceRepository _taskInstanceRepository;

        public FinancialTaskAutomationService(
            ITaskRepository taskRepository,
            ITaskInstanceRepository taskInstanceRepository)
        {
            _taskRepository = taskRepository;
            _taskInstanceRepository = taskInstanceRepository;
        }

        /// <summary>
        /// Creates a bill approval task
        /// </summary>
        public async Task<TaskInstance> CreateBillApprovalTaskAsync(
            Guid billId,
            string billNumber,
            string vendorName,
            decimal amount,
            string currency,
            DateTime dueDate,
            Guid categoryId,
            Guid? projectId,
            Guid userId,
            Guid taskTypeId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid? tenantId = null)
        {
            var taskTitle = $"Approve Bill #{billNumber} - {vendorName}";
            var taskDescription = $"Review and approve bill from {vendorName} for {amount:N2} {currency}. Due: {dueDate:yyyy-MM-dd}";

            var (importance, urgency) = GetBillApprovalPriority(amount);

            var task = new TaskAggregate(
                GuidGenerator.Create(),
                userId,
                taskTitle,
                taskTypeId,
                tenantId,
                projectId,
                null, // areaId
                null, // contextId
                null, // taskHierarchy
                importance,
                urgency);

            task.Description = taskDescription;
            task.UpdateDefaultSchedule(dueDate.AddDays(-2), 60); // Due 2 days before bill due date, 1 hour estimate

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                workflowId,
                workflowStatusId,
                userId,
                projectId,
                null, // areaId
                null, // contextId
                importance,
                urgency,
                tenantId);

            // Add custom fields for financial integration
            instance.SetCustomFieldValue("FinancialEntityType", "Bill");
            instance.SetCustomFieldValue("BillId", billId.ToString());
            instance.SetCustomFieldValue("BillNumber", billNumber);
            instance.SetCustomFieldValue("VendorName", vendorName);
            instance.SetCustomFieldValue("Amount", amount.ToString());
            instance.SetCustomFieldValue("Currency", currency);
            instance.SetCustomFieldValue("CategoryId", categoryId.ToString());
            if (projectId.HasValue)
            {
                instance.SetCustomFieldValue("ProjectId", projectId.Value.ToString());
            }

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        /// <summary>
        /// Creates a bill payment task
        /// </summary>
        public async Task<TaskInstance> CreateBillPaymentTaskAsync(
            Guid billId,
            string billNumber,
            string vendorName,
            decimal amount,
            string currency,
            DateTime dueDate,
            Guid userId,
            Guid taskTypeId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid? tenantId = null)
        {
            var taskTitle = $"Pay Bill #{billNumber} - {vendorName}";
            var taskDescription = $"Process payment to {vendorName} for {amount:N2} {currency}. Due: {dueDate:yyyy-MM-dd}";

            var daysUntilDue = (dueDate - DateTime.Today).Days;
            var (importance, urgency) = daysUntilDue <= 2
                ? (0.9, 0.9) // Important & Urgent
                : (0.9, 0.3); // Important & Not Urgent

            var task = new TaskAggregate(
                GuidGenerator.Create(),
                userId,
                taskTitle,
                taskTypeId,
                tenantId,
                null, // projectId
                null, // areaId
                null, // contextId
                null, // taskHierarchy
                importance,
                urgency);

            task.Description = taskDescription;
            task.UpdateDefaultSchedule(dueDate.AddDays(-1), 30); // Due 1 day before bill due date, 30 min estimate

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                workflowId,
                workflowStatusId,
                userId,
                null, // projectId
                null, // areaId
                null, // contextId
                importance,
                urgency,
                tenantId);

            instance.SetCustomFieldValue("FinancialEntityType", "Bill");
            instance.SetCustomFieldValue("BillId", billId.ToString());
            instance.SetCustomFieldValue("BillNumber", billNumber);
            instance.SetCustomFieldValue("VendorName", vendorName);
            instance.SetCustomFieldValue("Amount", amount.ToString());
            instance.SetCustomFieldValue("Currency", currency);
            instance.SetCustomFieldValue("TaskSubtype", "Payment");

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        /// <summary>
        /// Creates an invoice follow-up task
        /// </summary>
        public async Task<TaskInstance> CreateInvoiceFollowUpTaskAsync(
            Guid invoiceId,
            string invoiceNumber,
            string customerName,
            decimal totalAmount,
            string currency,
            DateTime issueDate,
            Guid userId,
            Guid taskTypeId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid? tenantId = null)
        {
            var taskTitle = $"Follow up on Invoice #{invoiceNumber} - {customerName}";
            var taskDescription = $"Contact {customerName} regarding invoice for {totalAmount:N2} {currency} sent on {issueDate:yyyy-MM-dd}";

            var (importance, urgency) = (0.9, 0.3); // Important & Not Urgent

            var task = new TaskAggregate(
                GuidGenerator.Create(),
                userId,
                taskTitle,
                taskTypeId,
                tenantId,
                null, // projectId
                null, // areaId
                null, // contextId
                null, // taskHierarchy
                importance,
                urgency);

            task.Description = taskDescription;
            task.UpdateDefaultSchedule(issueDate.AddDays(7), 15); // Due 7 days after invoice sent, 15 min estimate

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                workflowId,
                workflowStatusId,
                userId,
                null, // projectId
                null, // areaId
                null, // contextId
                importance,
                urgency,
                tenantId);

            instance.SetCustomFieldValue("FinancialEntityType", "Invoice");
            instance.SetCustomFieldValue("InvoiceId", invoiceId.ToString());
            instance.SetCustomFieldValue("InvoiceNumber", invoiceNumber);
            instance.SetCustomFieldValue("CustomerName", customerName);
            instance.SetCustomFieldValue("Amount", totalAmount.ToString());
            instance.SetCustomFieldValue("Currency", currency);
            instance.SetCustomFieldValue("TaskSubtype", "FollowUp");

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        /// <summary>
        /// Creates an invoice collection task
        /// </summary>
        public async Task<TaskInstance> CreateInvoiceCollectionTaskAsync(
            Guid invoiceId,
            string invoiceNumber,
            string customerName,
            decimal amountDue,
            string currency,
            int daysOverdue,
            Guid userId,
            Guid taskTypeId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid? tenantId = null)
        {
            var taskTitle = $"Collect Overdue Payment - Invoice #{invoiceNumber} ({daysOverdue} days overdue)";
            var taskDescription = $"Collect {amountDue:N2} {currency} from {customerName}. Invoice is {daysOverdue} days overdue.";

            var (importance, urgency) = GetCollectionPriority(daysOverdue);

            var task = new TaskAggregate(
                GuidGenerator.Create(),
                userId,
                taskTitle,
                taskTypeId,
                tenantId,
                null, // projectId
                null, // areaId
                null, // contextId
                null, // taskHierarchy
                importance,
                urgency);

            task.Description = taskDescription;
            task.UpdateDefaultSchedule(DateTime.Today.AddDays(2), 30); // Due in 2 days, 30 min estimate

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                workflowId,
                workflowStatusId,
                userId,
                null, // projectId
                null, // areaId
                null, // contextId
                importance,
                urgency,
                tenantId);

            instance.SetCustomFieldValue("FinancialEntityType", "Invoice");
            instance.SetCustomFieldValue("InvoiceId", invoiceId.ToString());
            instance.SetCustomFieldValue("InvoiceNumber", invoiceNumber);
            instance.SetCustomFieldValue("CustomerName", customerName);
            instance.SetCustomFieldValue("AmountDue", amountDue.ToString());
            instance.SetCustomFieldValue("Currency", currency);
            instance.SetCustomFieldValue("DaysOverdue", daysOverdue.ToString());
            instance.SetCustomFieldValue("TaskSubtype", "Collection");

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        /// <summary>
        /// Creates a budget approval task
        /// </summary>
        public async Task<TaskInstance> CreateBudgetApprovalTaskAsync(
            Guid budgetId,
            string budgetName,
            decimal totalBudget,
            string currency,
            DateTime periodStart,
            DateTime periodEnd,
            Guid userId,
            Guid taskTypeId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid? tenantId = null)
        {
            var taskTitle = $"Approve Budget: {budgetName}";
            var taskDescription = $"Review and approve budget for {totalBudget:N2} {currency}. Period: {periodStart:yyyy-MM-dd} to {periodEnd:yyyy-MM-dd}";

            var (importance, urgency) = (0.9, 0.3); // Important & Not Urgent

            var task = new TaskAggregate(
                GuidGenerator.Create(),
                userId,
                taskTitle,
                taskTypeId,
                tenantId,
                null, // projectId
                null, // areaId
                null, // contextId
                null, // taskHierarchy
                importance,
                urgency);

            task.Description = taskDescription;
            task.UpdateDefaultSchedule(DateTime.Today.AddDays(3), 120); // Due in 3 days, 2 hours estimate

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                workflowId,
                workflowStatusId,
                userId,
                null, // projectId
                null, // areaId
                null, // contextId
                importance,
                urgency,
                tenantId);

            instance.SetCustomFieldValue("FinancialEntityType", "Budget");
            instance.SetCustomFieldValue("BudgetId", budgetId.ToString());
            instance.SetCustomFieldValue("BudgetName", budgetName);
            instance.SetCustomFieldValue("TotalBudget", totalBudget.ToString());
            instance.SetCustomFieldValue("Currency", currency);
            instance.SetCustomFieldValue("PeriodStart", periodStart.ToString("O"));
            instance.SetCustomFieldValue("PeriodEnd", periodEnd.ToString("O"));

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        /// <summary>
        /// Creates a budget variance investigation task
        /// </summary>
        public async Task<TaskInstance> CreateBudgetVarianceTaskAsync(
            Guid budgetId,
            string budgetName,
            Guid categoryId,
            string categoryName,
            decimal plannedAmount,
            decimal actualAmount,
            decimal variancePercent,
            bool isOverBudget,
            Guid userId,
            Guid taskTypeId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid? tenantId = null)
        {
            var variance = actualAmount - plannedAmount;
            var taskTitle = $"Investigate Budget Variance - {budgetName}: {categoryName}";
            var taskDescription = isOverBudget
                ? $"{categoryName} is over budget by {Math.Abs(variance):N2} ({Math.Abs(variancePercent):N1}%). Planned: {plannedAmount:N2}, Actual: {actualAmount:N2}"
                : $"{categoryName} has variance of {Math.Abs(variancePercent):N1}%. Planned: {plannedAmount:N2}, Actual: {actualAmount:N2}";

            var (importance, urgency) = isOverBudget
                ? (0.9, 0.9) // Important & Urgent
                : (0.9, 0.3); // Important & Not Urgent

            var task = new TaskAggregate(
                GuidGenerator.Create(),
                userId,
                taskTitle,
                taskTypeId,
                tenantId,
                null, // projectId
                null, // areaId
                null, // contextId
                null, // taskHierarchy
                importance,
                urgency);

            task.Description = taskDescription;
            task.UpdateDefaultSchedule(DateTime.Today.AddDays(1), 60); // Due tomorrow, 1 hour estimate

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                workflowId,
                workflowStatusId,
                userId,
                null, // projectId
                null, // areaId
                null, // contextId
                importance,
                urgency,
                tenantId);

            instance.SetCustomFieldValue("FinancialEntityType", "Budget");
            instance.SetCustomFieldValue("BudgetId", budgetId.ToString());
            instance.SetCustomFieldValue("CategoryId", categoryId.ToString());
            instance.SetCustomFieldValue("CategoryName", categoryName);
            instance.SetCustomFieldValue("PlannedAmount", plannedAmount.ToString());
            instance.SetCustomFieldValue("ActualAmount", actualAmount.ToString());
            instance.SetCustomFieldValue("VariancePercent", variancePercent.ToString());
            instance.SetCustomFieldValue("IsOverBudget", isOverBudget.ToString());
            instance.SetCustomFieldValue("TaskSubtype", "VarianceInvestigation");

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        /// <summary>
        /// Creates financial goal action planning tasks
        /// </summary>
        public async Task<List<TaskInstance>> CreateFinancialGoalTasksAsync(
            Guid goalId,
            string goalName,
            string goalType,
            decimal targetAmount,
            string currency,
            DateTime targetDate,
            Guid userId,
            Guid taskTypeId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid? tenantId = null)
        {
            var tasks = new List<TaskInstance>();

            // Task 1: Create action plan
            var actionPlanTask = await CreateGoalActionPlanTaskAsync(
                goalId, goalName, targetAmount, currency, targetDate, userId, taskTypeId, workflowId, workflowStatusId, tenantId);
            tasks.Add(actionPlanTask);

            // Task 2: Set up automation (if savings goal)
            if (goalType == "Savings")
            {
                var automationTask = await CreateGoalAutomationTaskAsync(
                    goalId, goalName, targetAmount, currency, targetDate, userId, taskTypeId, workflowId, workflowStatusId, tenantId);
                tasks.Add(automationTask);
            }

            // Task 3: Budget review
            var budgetReviewTask = await CreateGoalBudgetReviewTaskAsync(
                goalId, goalName, userId, taskTypeId, workflowId, workflowStatusId, tenantId);
            tasks.Add(budgetReviewTask);

            return tasks;
        }

        /// <summary>
        /// Creates a goal check-in task
        /// </summary>
        public async Task<TaskInstance> CreateGoalCheckInTaskAsync(
            Guid goalId,
            string goalName,
            decimal targetAmount,
            decimal currentAmount,
            decimal progressPercent,
            string currency,
            DateTime targetDate,
            string checkInFrequency,
            Guid userId,
            Guid taskTypeId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid? tenantId = null)
        {
            var taskTitle = $"Review Progress: {goalName} ({progressPercent:N1}% complete)";
            var remaining = targetAmount - currentAmount;
            var taskDescription = $"Review progress on {goalName}. Target: {targetAmount:N2} {currency}, Current: {currentAmount:N2} {currency}, Remaining: {remaining:N2} {currency}";

            var (importance, urgency) = (0.9, 0.3); // Important & Not Urgent

            var task = new TaskAggregate(
                GuidGenerator.Create(),
                userId,
                taskTitle,
                taskTypeId,
                tenantId,
                null, // projectId
                null, // areaId
                null, // contextId
                null, // taskHierarchy
                importance,
                urgency);

            task.Description = taskDescription;
            task.UpdateDefaultSchedule(DateTime.Today.AddDays(7), 30); // Due in 7 days, 30 min estimate

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                workflowId,
                workflowStatusId,
                userId,
                null, // projectId
                null, // areaId
                null, // contextId
                importance,
                urgency,
                tenantId);

            instance.SetCustomFieldValue("FinancialEntityType", "Goal");
            instance.SetCustomFieldValue("GoalId", goalId.ToString());
            instance.SetCustomFieldValue("GoalName", goalName);
            instance.SetCustomFieldValue("TargetAmount", targetAmount.ToString());
            instance.SetCustomFieldValue("CurrentAmount", currentAmount.ToString());
            instance.SetCustomFieldValue("ProgressPercent", progressPercent.ToString());
            instance.SetCustomFieldValue("Currency", currency);
            instance.SetCustomFieldValue("CheckInFrequency", checkInFrequency);
            instance.SetCustomFieldValue("TaskSubtype", "CheckIn");

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        #region Private Helpers

        private async Task<TaskInstance> CreateGoalActionPlanTaskAsync(
            Guid goalId,
            string goalName,
            decimal targetAmount,
            string currency,
            DateTime targetDate,
            Guid userId,
            Guid taskTypeId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid? tenantId)
        {
            var taskTitle = $"Create Action Plan for {goalName}";
            var taskDescription = $"Develop detailed action plan to achieve {targetAmount:N2} {currency} by {targetDate:yyyy-MM-dd}";

            var (importance, urgency) = (0.9, 0.3); // Important & Not Urgent

            var task = new TaskAggregate(
                GuidGenerator.Create(),
                userId,
                taskTitle,
                taskTypeId,
                tenantId,
                null, // projectId
                null, // areaId
                null, // contextId
                null, // taskHierarchy
                importance,
                urgency);

            task.Description = taskDescription;
            task.UpdateDefaultSchedule(DateTime.Today.AddDays(7), 120); // Due in 7 days, 2 hours estimate

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                workflowId,
                workflowStatusId,
                userId,
                null, // projectId
                null, // areaId
                null, // contextId
                importance,
                urgency,
                tenantId);

            instance.SetCustomFieldValue("FinancialEntityType", "Goal");
            instance.SetCustomFieldValue("GoalId", goalId.ToString());
            instance.SetCustomFieldValue("TaskSubtype", "ActionPlan");

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        private async Task<TaskInstance> CreateGoalAutomationTaskAsync(
            Guid goalId,
            string goalName,
            decimal targetAmount,
            string currency,
            DateTime targetDate,
            Guid userId,
            Guid taskTypeId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid? tenantId)
        {
            var taskTitle = $"Set Up Automatic Transfer for {goalName}";
            var taskDescription = $"Configure automatic monthly transfer to reach {targetAmount:N2} {currency} by {targetDate:yyyy-MM-dd}";

            var (importance, urgency) = (0.9, 0.3); // Important & Not Urgent

            var task = new TaskAggregate(
                GuidGenerator.Create(),
                userId,
                taskTitle,
                taskTypeId,
                tenantId,
                null, // projectId
                null, // areaId
                null, // contextId
                null, // taskHierarchy
                importance,
                urgency);

            task.Description = taskDescription;
            task.UpdateDefaultSchedule(DateTime.Today.AddDays(3), 30); // Due in 3 days, 30 min estimate

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                workflowId,
                workflowStatusId,
                userId,
                null, // projectId
                null, // areaId
                null, // contextId
                importance,
                urgency,
                tenantId);

            instance.SetCustomFieldValue("FinancialEntityType", "Goal");
            instance.SetCustomFieldValue("GoalId", goalId.ToString());
            instance.SetCustomFieldValue("TaskSubtype", "Automation");

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        private async Task<TaskInstance> CreateGoalBudgetReviewTaskAsync(
            Guid goalId,
            string goalName,
            Guid userId,
            Guid taskTypeId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid? tenantId)
        {
            var taskTitle = $"Review Budget for {goalName}";
            var taskDescription = $"Analyze current budget and identify opportunities to allocate more toward {goalName}";

            var (importance, urgency) = (0.9, 0.3); // Important & Not Urgent

            var task = new TaskAggregate(
                GuidGenerator.Create(),
                userId,
                taskTitle,
                taskTypeId,
                tenantId,
                null, // projectId
                null, // areaId
                null, // contextId
                null, // taskHierarchy
                importance,
                urgency);

            task.Description = taskDescription;
            task.UpdateDefaultSchedule(DateTime.Today.AddDays(5), 60); // Due in 5 days, 1 hour estimate

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                workflowId,
                workflowStatusId,
                userId,
                null, // projectId
                null, // areaId
                null, // contextId
                importance,
                urgency,
                tenantId);

            instance.SetCustomFieldValue("FinancialEntityType", "Goal");
            instance.SetCustomFieldValue("GoalId", goalId.ToString());
            instance.SetCustomFieldValue("TaskSubtype", "BudgetReview");

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        private (double importance, double urgency) GetBillApprovalPriority(decimal amount)
        {
            if (amount > 1000)
                return (0.9, 0.9); // Important & Urgent
            if (amount > 500)
                return (0.9, 0.3); // Important & Not Urgent
            return (0.3, 0.3); // Not Important & Not Urgent
        }

        private (double importance, double urgency) GetCollectionPriority(int daysOverdue)
        {
            if (daysOverdue > 14)
                return (0.9, 0.9); // Important & Urgent
            if (daysOverdue > 7)
                return (0.9, 0.3); // Important & Not Urgent
            return (0.3, 0.9); // Not Important & Urgent
        }

        #endregion
    }
}
