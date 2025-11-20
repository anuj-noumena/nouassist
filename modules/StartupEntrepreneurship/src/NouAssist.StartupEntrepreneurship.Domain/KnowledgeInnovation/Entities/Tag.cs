namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities
{
    /// <summary>
    /// Represents a tag for categorizing content
    /// </summary>
    public class Tag : Entity
    {
        public string Name { get; private set; }
        public Guid? TenantId { get; private set; }

        protected Tag() { }

        public Tag(string name, Guid? tenantId = null)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Tag);
            
            TenantId = tenantId;
        }

        public void UpdateName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Tag);
        }

        public override object[] GetKeys()
        {
            return new object[] { Name, TenantId };
        }
    }
}
