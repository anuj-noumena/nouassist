namespace NouAssist.BusinessOperations.FreelanceOperations.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity with the same name already exists
    /// </summary>
    public class DuplicateNameException : FreelanceOperationsDomainException
    {
        public DuplicateNameException(string entityType, string name)
            : base("FreelanceOperations:ErrorMessage:DuplicateName")
        {
            WithData("entityType", entityType);
            WithData("name", name);
        }
    }
}
