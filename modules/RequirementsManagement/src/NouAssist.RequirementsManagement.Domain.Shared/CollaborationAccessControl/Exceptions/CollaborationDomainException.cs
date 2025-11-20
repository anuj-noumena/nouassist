namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Exceptions
{
    /// <summary>
    /// Base exception for Collaboration domain.
    /// </summary>
    public class CollaborationDomainException : BusinessException
    {
        public CollaborationDomainException(string code)
            : base(code)
        {
        }

        public CollaborationDomainException(string code, string message)
            : base(code, message)
        {
        }

        public CollaborationDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
