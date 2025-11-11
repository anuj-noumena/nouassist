using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.TaskManagement.ValueObjects;

public class BlockingInfo : ValueObject
{
    public bool IsBlocked { get; private set; }
    public string Reason { get; private set; }
    public DateTime? BlockedAt { get; private set; }

    private BlockingInfo()
    {
    }

    public static BlockingInfo NotBlocked => new() { IsBlocked = false };

    public static BlockingInfo Create(string reason)
    {
        Check.NotNullOrWhiteSpace(reason, nameof(reason));
        return new BlockingInfo
        {
            IsBlocked = true,
            Reason = reason,
            BlockedAt = DateTime.UtcNow
        };
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return IsBlocked;
        yield return Reason;
        yield return BlockedAt;
    }
}