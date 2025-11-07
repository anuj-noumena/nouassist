using System;
using System.Linq.Expressions;
using NouAssist.CollaborationHub.Resources.Aggregates;
using NouAssist.CollaborationHub.Resources.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.CollaborationHub.Resources.Specifications
{
    public class AvailableInventorySpecification : Specification<InventoryItem>
    {
        public override Expression<Func<InventoryItem, bool>> ToExpression() =>
            i => i.Status == InventoryStatus.Available && 
                 i.QuantityAvailable > 0 && 
                 !i.IsDeleted;
    }

    public class CheckedOutInventorySpecification : Specification<InventoryItem>
    {
        public override Expression<Func<InventoryItem, bool>> ToExpression() =>
            i => i.Status == InventoryStatus.CheckedOut && !i.IsDeleted;
    }

    public class OverdueInventorySpecification : Specification<InventoryItem>
    {
        private readonly DateTime _now;

        public OverdueInventorySpecification(DateTime now)
        {
            _now = now;
        }

        public override Expression<Func<InventoryItem, bool>> ToExpression() =>
            i => i.DueBackAt.HasValue && 
                 i.DueBackAt.Value < _now && 
                 i.AssignedToUserId.HasValue && 
                 !i.IsDeleted;
    }

    public class InventoryByKindSpecification : Specification<InventoryItem>
    {
        private readonly InventoryItemKind _kind;

        public InventoryByKindSpecification(InventoryItemKind kind)
        {
            _kind = kind;
        }

        public override Expression<Func<InventoryItem, bool>> ToExpression() =>
            i => i.Kind == _kind && !i.IsDeleted;
    }

    public class InventoryDueForMaintenanceSpecification : Specification<InventoryItem>
    {
        private readonly DateTime _now;

        public InventoryDueForMaintenanceSpecification(DateTime now)
        {
            _now = now;
        }

        public override Expression<Func<InventoryItem, bool>> ToExpression() =>
            i => i.NextMaintenanceAt.HasValue && 
                 i.NextMaintenanceAt.Value <= _now && 
                 !i.IsDeleted;
    }

    public class InventoryByLocationSpecification : Specification<InventoryItem>
    {
        private readonly string _location;

        public InventoryByLocationSpecification(string location)
        {
            _location = location;
        }

        public override Expression<Func<InventoryItem, bool>> ToExpression() =>
            i => i.Location == _location && !i.IsDeleted;
    }

    public class AssignedInventorySpecification : Specification<InventoryItem>
    {
        private readonly Guid _userId;

        public AssignedInventorySpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<InventoryItem, bool>> ToExpression() =>
            i => i.AssignedToUserId == _userId && !i.IsDeleted;
    }
}
