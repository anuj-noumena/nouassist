using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.WorkManagement.TaskManagement.Entities;

/// <summary>
/// Simplified checklist item (replaces ActionStepCompletion)
/// </summary>
public class TaskChecklistItem : Entity
{
    public Guid TaskInstanceId { get; private set; }
    public Guid ChecklistItemId { get; private set; } // References Task.ChecklistItems
    public bool IsChecked { get; private set; }
    public DateTime? CheckedAt { get; private set; }
    public Guid? CheckedBy { get; private set; }

    protected TaskChecklistItem() { }

    public TaskChecklistItem(Guid taskInstanceId, Guid checklistItemId)
    {
        TaskInstanceId = taskInstanceId;
        ChecklistItemId = checklistItemId;
        IsChecked = false;
    }

    public void Check(Guid? userId = null)
    {
        IsChecked = true;
        CheckedAt = DateTime.UtcNow;
        CheckedBy = userId;
    }

    public void Uncheck()
    {
        IsChecked = false;
        CheckedAt = null;
        CheckedBy = null;
    }

    public override object[] GetKeys()
    {
        return new object[] { TaskInstanceId, ChecklistItemId };
    }
}
