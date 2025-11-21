using System;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities
{
    /// <summary>
    /// Entity representing a clause in a term sheet
    /// </summary>
    public class TermSheetClause : Entity<Guid>
    {
        public Guid TermSheetId { get; private set; }
        public string Name { get; private set; }
        public ClauseText Text { get; private set; }
        public string Category { get; private set; }
        public int Order { get; private set; }
        public string Notes { get; private set; }
        
        protected TermSheetClause() { }
        
        internal TermSheetClause(
            Guid id,
            Guid termSheetId,
            string name,
            ClauseText text,
            string category,
            int order,
            string notes = null) : base(id)
        {
            TermSheetId = termSheetId;
            SetName(name);
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Order = order;
            Notes = notes;
        }
        
        public void Update(
            string name,
            ClauseText text,
            string category,
            int order,
            string notes = null)
        {
            SetName(name);
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Order = order;
            Notes = notes;
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: FundraisingPipelineConsts.TermSheet.MaxClauseNameLength);
        }
    }
}
