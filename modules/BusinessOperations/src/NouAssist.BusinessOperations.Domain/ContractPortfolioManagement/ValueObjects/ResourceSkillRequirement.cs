using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing a resource skill requirement
    /// </summary>
    public class ResourceSkillRequirement : ValueObject
    {
        public string SkillName { get; private set; }
        public int RequiredLevel { get; private set; } // 1-5
        public bool IsMandatory { get; private set; }
        public string Description { get; private set; }

        protected ResourceSkillRequirement() { }

        public ResourceSkillRequirement(string skillName, int requiredLevel, bool isMandatory, string description = null)
        {
            SkillName = Check.NotNullOrWhiteSpace(skillName, nameof(skillName));
            
            if (requiredLevel < 1 || requiredLevel > 5)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:InvalidValue")
                    .WithData("propertyName", "RequiredLevel");
            }
            
            RequiredLevel = requiredLevel;
            IsMandatory = isMandatory;
            Description = description;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return SkillName;
            yield return RequiredLevel;
            yield return IsMandatory;
            yield return Description ?? string.Empty;
        }
    }
}
