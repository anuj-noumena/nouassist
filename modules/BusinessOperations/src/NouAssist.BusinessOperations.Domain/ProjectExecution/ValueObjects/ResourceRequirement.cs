namespace NouAssist.BusinessOperations.ProjectExecution.ValueObjects
{
    /// <summary>
    /// Value object representing a resource requirement for a project or task
    /// </summary>
    public class ResourceRequirement : ValueObject
    {
        /// <summary>
        /// Skill required for the resource
        /// </summary>
        public string Skill { get; private set; }
        
        /// <summary>
        /// Role required for the resource
        /// </summary>
        public string Role { get; private set; }
        
        /// <summary>
        /// Number of resources required
        /// </summary>
        public int Quantity { get; private set; }
        
        protected ResourceRequirement() { }
        
        public ResourceRequirement(
            string skill,
            string role,
            int quantity)
        {
            SetSkill(skill);
            SetRole(role);
            SetQuantity(quantity);
        }
        
        private void SetSkill(string skill)
        {
            Skill = Check.NotNullOrWhiteSpace(
                skill,
                nameof(skill),
                maxLength: 100
            );
        }
        
        private void SetRole(string role)
        {
            Role = Check.NotNullOrWhiteSpace(
                role,
                nameof(role),
                maxLength: 100
            );
        }
        
        private void SetQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new BusinessException("Quantity must be greater than zero");
            }
            
            Quantity = quantity;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Skill;
            yield return Role;
            yield return Quantity;
        }
    }
}
