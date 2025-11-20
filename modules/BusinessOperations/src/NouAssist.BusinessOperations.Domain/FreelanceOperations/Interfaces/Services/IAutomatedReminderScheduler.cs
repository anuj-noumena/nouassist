namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services
{
    /// <summary>
    /// Domain service interface for scheduling automated reminders
    /// </summary>
    public interface IAutomatedReminderScheduler
    {
        /// <summary>
        /// Schedules project-related reminders
        /// </summary>
        Task ScheduleProjectRemindersAsync(Guid projectId);
        
        /// <summary>
        /// Schedules client follow-up reminders
        /// </summary>
        Task ScheduleClientFollowUpRemindersAsync(Guid clientId);
        
        /// <summary>
        /// Schedules proposal-related reminders
        /// </summary>
        Task ScheduleProposalRemindersAsync(Guid proposalId);
        
        /// <summary>
        /// Gets all due reminders for a user
        /// </summary>
        Task<List<Dictionary<string, object>>> GetDueRemindersAsync(Guid userId);
        
        /// <summary>
        /// Marks a reminder as completed
        /// </summary>
        Task MarkReminderCompletedAsync(Guid reminderId);
    }
}
