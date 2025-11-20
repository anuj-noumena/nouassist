namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for reminders
    /// </summary>
    public interface IRemindersRepository
    {
        /// <summary>
        /// Creates a new reminder
        /// </summary>
        Task CreateReminderAsync(
            Guid id,
            string title,
            string message,
            DateTime dueDate,
            Guid entityId,
            string entityType);
            
        /// <summary>
        /// Gets due reminders for a user
        /// </summary>
        Task<List<Dictionary<string, object>>> GetDueRemindersAsync(Guid userId);
        
        /// <summary>
        /// Marks a reminder as completed
        /// </summary>
        Task MarkReminderCompletedAsync(Guid reminderId);
    }
}
