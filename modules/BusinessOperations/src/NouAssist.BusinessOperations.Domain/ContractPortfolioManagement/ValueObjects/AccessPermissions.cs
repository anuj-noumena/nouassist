namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing access permissions for sharing portfolios
    /// </summary>
    public class AccessPermissions : ValueObject
    {
        public bool CanView { get; private set; }
        public bool CanEdit { get; private set; }
        public bool CanShare { get; private set; }
        public bool CanDelete { get; private set; }

        protected AccessPermissions() { }

        public AccessPermissions(bool canView, bool canEdit, bool canShare, bool canDelete)
        {
            CanView = canView;
            CanEdit = canEdit;
            CanShare = canShare;
            CanDelete = canDelete;
        }

        public static AccessPermissions ReadOnly()
        {
            return new AccessPermissions(true, false, false, false);
        }

        public static AccessPermissions ReadWrite()
        {
            return new AccessPermissions(true, true, false, false);
        }

        public static AccessPermissions FullAccess()
        {
            return new AccessPermissions(true, true, true, true);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CanView;
            yield return CanEdit;
            yield return CanShare;
            yield return CanDelete;
        }
    }
}
