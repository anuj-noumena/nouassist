using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Services;

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
            Guid? tenantId = null)
        {
            var taskTitle = $"Approve Bill #{billNumber} - {vendorName}";
            var taskDescription = $"Review and approve bill from {vendorName} for {amount:N2} {currency}. Due: {dueDate:yyyy-MM-dd}";

            var task = new Task(
                GuidGenerator.Create(),
                tenantId,
                userId,
                taskTitle,
                taskDescription,
                GetBillApprovalPriority(amount),
                dueDate.AddDays(-2), // Due 2 days before bill due date
                null, // No project reference
                TaskType.Approval,
                EstimationUnit.Hours,
                1); // 1 hour estimated

            task.SetCategory("Financial");
            task.AddTag("Bill");
            task.AddTag("Approval");
            task.AddTag("AccountsPayable");

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                userId,
                tenantId);

            // Add custom fields for financial integration
            instance.AddCustomField("FinancialEntityType", "Bill");
            instance.AddCustomField("BillId", billId.ToString());
            instance.AddCustomField("BillNumber", billNumber);
            instance.AddCustomField("VendorName", vendorName);
            instance.AddCustomField("Amount", amount.ToString());
            instance.AddCustomField("Currency", currency);
            instance.AddCustomField("CategoryId", categoryId.ToString());
            if (projectId.HasValue)
            {
                instance.AddCustomField("ProjectId", projectId.Value.ToString());
            }

            instance.SetCognitiveLoad(CognitiveLoad.Medium);

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
            Guid? tenantId = null)
        {
            var taskTitle = $"Pay Bill #{billNumber} - {vendorName}";
            var taskDescription = $"Process payment to {vendorName} for {amount:N2} {currency}. Due: {dueDate:yyyy-MM-dd}";

            var daysUntilDue = (dueDate - DateTime.Today).Days;
            var priority = daysUntilDue <= 2
                ? ImportanceUrgencyMatrix.ImportantUrgent
                : ImportanceUrgencyMatrix.ImportantNotUrgent;

            var task = new Task(
                GuidGenerator.Create(),
                tenantId,
                userId,
                taskTitle,
                taskDescription,
                priority,
                dueDate.AddDays(-1), // Due 1 day before bill due date
                null,
                TaskType.Action,
                EstimationUnit.Minutes,
                30);

            task.SetCategory("Financial");
            task.AddTag("Bill");
            task.AddTag("Payment");
            task.AddTag("AccountsPayable");

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                userId,
                tenantId);

            instance.AddCustomField("FinancialEntityType", "Bill");
            instance.AddCustomField("BillId", billId.ToString());
            instance.AddCustomField("BillNumber", billNumber);
            instance.AddCustomField("VendorName", vendorName);
            instance.AddCustomField("Amount", amount.ToString());
            instance.AddCustomField("Currency", currency);
            instance.AddCustomField("TaskSubtype", "Payment");

            instance.SetCognitiveLoad(CognitiveLoad.Low);

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
            Guid? tenantId = null)
        {
            var taskTitle = $"Follow up on Invoice #{invoiceNumber} - {customerName}";
            var taskDescription = $"Contact {customerName} regarding invoice for {totalAmount:N2} {currency} sent on {issueDate:yyyy-MM-dd}";

            var task = new Task(
                GuidGenerator.Create(),
                tenantId,
                userId,
                taskTitle,
                taskDescription,
                ImportanceUrgencyMatrix.ImportantNotUrgent,
                issueDate.AddDays(7), // Due 7 days after invoice sent
                null,
                TaskType.Communication,
                EstimationUnit.Minutes,
                15);

            task.SetCategory("Financial");
            task.AddTag("Invoice");
            task.AddTag("FollowUp");
            task.AddTag("AccountsReceivable");

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                userId,
                tenantId);

            instance.AddCustomField("FinancialEntityType", "Invoice");
            instance.AddCustomField("InvoiceId", invoiceId.ToString());
            instance.AddCustomField("InvoiceNumber", invoiceNumber);
            instance.AddCustomField("CustomerName", customerName);
            instance.AddCustomField("Amount", totalAmount.ToString());
            instance.AddCustomField("Currency", currency);
            instance.AddCustomField("TaskSubtype", "FollowUp");

            instance.SetCognitiveLoad(CognitiveLoad.Low);

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
            Guid? tenantId = null)
        {
            var taskTitle = $"Collect Overdue Payment - Invoice #{invoiceNumber} ({daysOverdue} days overdue)";
            var taskDescription = $"Collect {amountDue:N2} {currency} from {customerName}. Invoice is {daysOverdue} days overdue.";

            var priority = GetCollectionPriority(daysOverdue);

            var task = new Task(
                GuidGenerator.Create(),
                tenantId,
                userId,
                taskTitle,
                taskDescription,
                priority,
                DateTime.Today.AddDays(2), // Due in 2 days
                null,
                TaskType.Communication,
                EstimationUnit.Minutes,
                30);

            task.SetCategory("Financial");
            task.AddTag("Invoice");
            task.AddTag("Collection");
            task.AddTag("Overdue");
            task.AddTag("AccountsReceivable");

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                userId,
                tenantId);

            instance.AddCustomField("FinancialEntityType", "Invoice");
            instance.AddCustomField("InvoiceId", invoiceId.ToString());
            instance.AddCustomField("InvoiceNumber", invoiceNumber);
            instance.AddCustomField("CustomerName", customerName);
            instance.AddCustomField("AmountDue", amountDue.ToString());
            instance.AddCustomField("Currency", currency);
            instance.AddCustomField("DaysOverdue", daysOverdue.ToString());
            instance.AddCustomField("TaskSubtype", "Collection");

            instance.SetCognitiveLoad(CognitiveLoad.Medium);

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
            Guid? tenantId = null)
        {
            var taskTitle = $"Approve Budget: {budgetName}";
            var taskDescription = $"Review and approve budget for {totalBudget:N2} {currency}. Period: {periodStart:yyyy-MM-dd} to {periodEnd:yyyy-MM-dd}";

            var task = new Task(
                GuidGenerator.Create(),
                tenantId,
                userId,
                taskTitle,
                taskDescription,
                ImportanceUrgencyMatrix.ImportantNotUrgent,
                DateTime.Today.AddDays(3), // Due in 3 days
                null,
                TaskType.Approval,
                EstimationUnit.Hours,
                2);

            task.SetCategory("Financial");
            task.AddTag("Budget");
            task.AddTag("Approval");
            task.AddTag("Planning");

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                userId,
                tenantId);

            instance.AddCustomField("FinancialEntityType", "Budget");
            instance.AddCustomField("BudgetId", budgetId.ToString());
            instance.AddCustomField("BudgetName", budgetName);
            instance.AddCustomField("TotalBudget", totalBudget.ToString());
            instance.AddCustomField("Currency", currency);
            instance.AddCustomField("PeriodStart", periodStart.ToString("O"));
            instance.AddCustomField("PeriodEnd", periodEnd.ToString("O"));

            instance.SetCognitiveLoad(CognitiveLoad.High);

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
            Guid? tenantId = null)
        {
            var variance = actualAmount - plannedAmount;
            var taskTitle = $"Investigate Budget Variance - {budgetName}: {categoryName}";
            var taskDescription = isOverBudget
                ? $"{categoryName} is over budget by {Math.Abs(variance):N2} ({Math.Abs(variancePercent):N1}%). Planned: {plannedAmount:N2}, Actual: {actualAmount:N2}"
                : $"{categoryName} has variance of {Math.Abs(variancePercent):N1}%. Planned: {plannedAmount:N2}, Actual: {actualAmount:N2}";

            var priority = isOverBudget
                ? ImportanceUrgencyMatrix.ImportantUrgent
                : ImportanceUrgencyMatrix.ImportantNotUrgent;

            var task = new Task(
                GuidGenerator.Create(),
                tenantId,
                userId,
                taskTitle,
                taskDescription,
                priority,
                DateTime.Today.AddDays(1), // Due tomorrow
                null,
                TaskType.Analysis,
                EstimationUnit.Hours,
                1);

            task.SetCategory("Financial");
            task.AddTag("Budget");
            task.AddTag("Variance");
            task.AddTag("Investigation");

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                userId,
                tenantId);

            instance.AddCustomField("FinancialEntityType", "Budget");
            instance.AddCustomField("BudgetId", budgetId.ToString());
            instance.AddCustomField("CategoryId", categoryId.ToString());
            instance.AddCustomField("CategoryName", categoryName);
            instance.AddCustomField("PlannedAmount", plannedAmount.ToString());
            instance.AddCustomField("ActualAmount", actualAmount.ToString());
            instance.AddCustomField("VariancePercent", variancePercent.ToString());
            instance.AddCustomField("IsOverBudget", isOverBudget.ToString());
            instance.AddCustomField("TaskSubtype", "VarianceInvestigation");

            instance.SetCognitiveLoad(CognitiveLoad.High);

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
            Guid? tenantId = null)
        {
            var tasks = new List<TaskInstance>();

            // Task 1: Create action plan
            var actionPlanTask = await CreateGoalActionPlanTaskAsync(
                goalId, goalName, targetAmount, currency, targetDate, userId, tenantId);
            tasks.Add(actionPlanTask);

            // Task 2: Set up automation (if savings goal)
            if (goalType == "Savings")
            {
                var automationTask = await CreateGoalAutomationTaskAsync(
                    goalId, goalName, targetAmount, currency, targetDate, userId, tenantId);
                tasks.Add(automationTask);
            }

            // Task 3: Budget review
            var budgetReviewTask = await CreateGoalBudgetReviewTaskAsync(
                goalId, goalName, userId, tenantId);
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
            Guid? tenantId = null)
        {
            var taskTitle = $"Review Progress: {goalName} ({progressPercent:N1}% complete)";
            var remaining = targetAmount - currentAmount;
            var taskDescription = $"Review progress on {goalName}. Target: {targetAmount:N2} {currency}, Current: {currentAmount:N2} {currency}, Remaining: {remaining:N2} {currency}";

            var task = new Task(
                GuidGenerator.Create(),
                tenantId,
                userId,
                taskTitle,
                taskDescription,
                ImportanceUrgencyMatrix.ImportantNotUrgent,
                DateTime.Today.AddDays(7), // Due in 7 days
                null,
                TaskType.Review,
                EstimationUnit.Minutes,
                30);

            task.SetCategory("Financial");
            task.AddTag("Goal");
            task.AddTag("CheckIn");
            task.AddTag("Progress");

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                userId,
                tenantId);

            instance.AddCustomField("FinancialEntityType", "Goal");
            instance.AddCustomField("GoalId", goalId.ToString());
            instance.AddCustomField("GoalName", goalName);
            instance.AddCustomField("TargetAmount", targetAmount.ToString());
            instance.AddCustomField("CurrentAmount", currentAmount.ToString());
            instance.AddCustomField("ProgressPercent", progressPercent.ToString());
            instance.AddCustomField("Currency", currency);
            instance.AddCustomField("CheckInFrequency", checkInFrequency);
            instance.AddCustomField("TaskSubtype", "CheckIn");

            instance.SetCognitiveLoad(CognitiveLoad.Medium);

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
            Guid? tenantId)
        {
            var taskTitle = $"Create Action Plan for {goalName}";
            var taskDescription = $"Develop detailed action plan to achieve {targetAmount:N2} {currency} by {targetDate:yyyy-MM-dd}";

            var task = new Task(
                GuidGenerator.Create(),
                tenantId,
                userId,
                taskTitle,
                taskDescription,
                ImportanceUrgencyMatrix.ImportantNotUrgent,
                DateTime.Today.AddDays(7),
                null,
                TaskType.Planning,
                EstimationUnit.Hours,
                2);

            task.SetCategory("Financial");
            task.AddTag("Goal");
            task.AddTag("Planning");

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                userId,
                tenantId);

            instance.AddCustomField("FinancialEntityType", "Goal");
            instance.AddCustomField("GoalId", goalId.ToString());
            instance.AddCustomField("TaskSubtype", "ActionPlan");

            instance.SetCognitiveLoad(CognitiveLoad.High);

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
            Guid? tenantId)
        {
            var taskTitle = $"Set Up Automatic Transfer for {goalName}";
            var taskDescription = $"Configure automatic monthly transfer to reach {targetAmount:N2} {currency} by {targetDate:yyyy-MM-dd}";

            var task = new Task(
                GuidGenerator.Create(),
                tenantId,
                userId,
                taskTitle,
                taskDescription,
                ImportanceUrgencyMatrix.ImportantNotUrgent,
                DateTime.Today.AddDays(3),
                null,
                TaskType.Action,
                EstimationUnit.Minutes,
                30);

            task.SetCategory("Financial");
            task.AddTag("Goal");
            task.AddTag("Automation");

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                userId,
                tenantId);

            instance.AddCustomField("FinancialEntityType", "Goal");
            instance.AddCustomField("GoalId", goalId.ToString());
            instance.AddCustomField("TaskSubtype", "Automation");

            instance.SetCognitiveLoad(CognitiveLoad.Medium);

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        private async Task<TaskInstance> CreateGoalBudgetReviewTaskAsync(
            Guid goalId,
            string goalName,
            Guid userId,
            Guid? tenantId)
        {
            var taskTitle = $"Review Budget for {goalName}";
            var taskDescription = $"Analyze current budget and identify opportunities to allocate more toward {goalName}";

            var task = new Task(
                GuidGenerator.Create(),
                tenantId,
                userId,
                taskTitle,
                taskDescription,
                ImportanceUrgencyMatrix.ImportantNotUrgent,
                DateTime.Today.AddDays(5),
                null,
                TaskType.Analysis,
                EstimationUnit.Hours,
                1);

            task.SetCategory("Financial");
            task.AddTag("Goal");
            task.AddTag("Budget");
            task.AddTag("Analysis");

            await _taskRepository.InsertAsync(task, autoSave: true);

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                task.Id,
                userId,
                tenantId);

            instance.AddCustomField("FinancialEntityType", "Goal");
            instance.AddCustomField("GoalId", goalId.ToString());
            instance.AddCustomField("TaskSubtype", "BudgetReview");

            instance.SetCognitiveLoad(CognitiveLoad.High);

            await _taskInstanceRepository.InsertAsync(instance, autoSave: true);

            return instance;
        }

        private ImportanceUrgencyMatrix GetBillApprovalPriority(decimal amount)
        {
            if (amount > 1000)
                return ImportanceUrgencyMatrix.ImportantUrgent;
            if (amount > 500)
                return ImportanceUrgencyMatrix.ImportantNotUrgent;
            return ImportanceUrgencyMatrix.NotImportantNotUrgent;
        }

        private ImportanceUrgencyMatrix GetCollectionPriority(int daysOverdue)
        {
            if (daysOverdue > 14)
                return ImportanceUrgencyMatrix.ImportantUrgent;
            if (daysOverdue > 7)
                return ImportanceUrgencyMatrix.ImportantNotUrgent;
            return ImportanceUrgencyMatrix.NotImportantUrgent;
        }

        #endregion
    }
}
