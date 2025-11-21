using System;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities
{
    /// <summary>
    /// Entity representing a section within an investor update
    /// </summary>
    public class UpdateSection : Entity<Guid>
    {
        public Guid UpdateId { get; private set; }
        public string Title { get; private set; }
        public UpdateContent Content { get; private set; }
        public int Order { get; private set; }
        public bool IsVisible { get; private set; }

        protected UpdateSection() { }

        internal UpdateSection(
            Guid id,
            Guid updateId,
            string title,
            UpdateContent content,
            int order) : base(id)
        {
            UpdateId = updateId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Content = Check.NotNull(content, nameof(content));
            Order = order;
            IsVisible = true;
        }

        public void UpdateTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
        }

        public void UpdateContent(UpdateContent content)
        {
            Content = Check.NotNull(content, nameof(content));
        }

        public void UpdateOrder(int order)
        {
            Order = order;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
        }
    }
}
