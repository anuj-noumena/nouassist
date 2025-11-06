using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.Tasks.Entities;

public class TaskHistoryEntry : FullAuditedEntity<Guid>
{
    public Guid Id { get; private set; }
    public Guid TaskId { get; private set; }
    public Guid Status { get; private set; }
    public string Note { get; private set; }
    public DateTime Timestamp { get; private set; }

    private TaskHistoryEntry() { }

    public TaskHistoryEntry(Guid id, Guid taskId, TaskStatus status, string note, DateTime timestamp)
    {
        Id = id;
        TaskId = taskId;
        Status = status ?? throw new ArgumentNullException(nameof(status));
        Note = note ?? throw new ArgumentNullException(nameof(note));
        Timestamp = timestamp;
    }
}