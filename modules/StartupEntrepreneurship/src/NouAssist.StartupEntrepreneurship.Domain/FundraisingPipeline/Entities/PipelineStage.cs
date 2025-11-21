using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities
{
    /// <summary>
    /// Entity representing a stage in the fundraising pipeline
    /// </summary>
    public class PipelineStage : Entity<Guid>
    {
        public Guid FundraisingRoundId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Order { get; private set; }
        
        protected PipelineStage() { }
        
        internal PipelineStage(
            Guid id,
            Guid fundraisingRoundId,
            string name,
            int order,
            string description = null) : base(id)
        {
            FundraisingRoundId = fundraisingRoundId;
            SetName(name);
            Order = order;
            Description = description;
        }
        
        public void Update(string name, int order, string description = null)
        {
            SetName(name);
            Order = order;
            Description = description;
        }
        
        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }
            
            Name = name;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { Id };
        }
    }
}
