using System;

namespace NouAssist.FinancialManagement.Integration.FinancialTasks.Dtos
{
    public class GoalActionItemDto
    {
        public Guid GoalId { get; set; }
        public Guid? TaskInstanceId { get; set; }
        public string GoalName { get; set; }
        public string GoalType { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal ProgressPercent { get; set; }
        public string Currency { get; set; }
        public DateTime TargetDate { get; set; }
        public int DaysRemaining { get; set; }
        public string ActionType { get; set; } // "ActionPlan", "CheckIn", "Automation", etc.
        public DateTime? TaskDueDate { get; set; }
        public string TaskStatus { get; set; }
    }
}
