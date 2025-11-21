namespace NouAssist.IntelligenceStrategy.DecisionSupport.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity with the same name already exists
    /// </summary>
    public class DuplicateNameException : DecisionSupportDomainException
    {
        /// <summary>
        /// The duplicate name
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The type of entity with the duplicate name
        /// </summary>
        public string EntityType { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DuplicateNameException"/>
        /// </summary>
        /// <param name="entityType">The type of entity</param>
        /// <param name="name">The duplicate name</param>
        public DuplicateNameException(string entityType, string name)
            : base("DecisionSupport:ErrorMessage:DuplicateName")
        {
            Name = name;
            EntityType = entityType;
            WithData("entityType", entityType);
            WithData("name", name);
        }
    }
}
