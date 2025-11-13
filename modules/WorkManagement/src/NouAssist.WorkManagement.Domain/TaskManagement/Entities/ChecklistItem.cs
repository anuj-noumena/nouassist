using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.WorkManagement.TaskManagement.Entities;

/// <summary>
/// Simplified checklist item definition (part of Task blueprint)
/// </summary>
public class ChecklistItem : Entity<Guid>
{
    public Guid TaskId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; set; }
    public int OrderIndex { get; private set; }
    public bool IsRequired { get; private set; }

    protected ChecklistItem() { }

    public ChecklistItem(
        Guid id,
        Guid taskId,
        string title,
        int orderIndex,
        bool isRequired = false) : base(id)
    {
        TaskId = taskId;
        Title = Check.NotNullOrWhiteSpace(title, nameof(title));
        OrderIndex = orderIndex;
        IsRequired = isRequired;
    }

    public void UpdateOrder(int newOrder)
    {
        OrderIndex = newOrder;
    }

    public void MakeRequired()
    {
        IsRequired = true;
    }
}