using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects
{
    /// <summary>
    /// Value object that represents a definition of a funnel stage
    /// </summary>
    public class StageDefinition : ValueObject
    {
        /// <summary>
        /// Stage name
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Stage description
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Stage order in the funnel (1-based)
        /// </summary>
        public int Order { get; private set; }
        
        /// <summary>
        /// Criteria or conditions defining this stage
        /// </summary>
        public string Criteria { get; private set; }
        
        protected StageDefinition() { }
        
        public StageDefinition(string name, string description, int order, string criteria)
        {
            SetName(name);
            SetDescription(description);
            SetOrder(order);
            Criteria = criteria; // Can be null
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: GrowthExperimentationConstants.ConversionFunnels.MaxStageNameLength
            );
        }
        
        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description, 
                nameof(description), 
                maxLength: GrowthExperimentationConstants.ConversionFunnels.MaxFunnelDescriptionLength
            );
        }
        
        private void SetOrder(int order)
        {
            if (order <= 0)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", order)
                    .WithData("property", "Order");
            }
            
            Order = order;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description;
            yield return Order;
            
            if (!string.IsNullOrEmpty(Criteria))
            {
                yield return Criteria;
            }
        }
    }
}
