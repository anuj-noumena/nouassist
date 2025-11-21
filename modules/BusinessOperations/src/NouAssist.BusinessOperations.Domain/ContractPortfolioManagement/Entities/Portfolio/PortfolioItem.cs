using System;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio
{
    /// <summary>
    /// Represents an item in a portfolio (reference to a contract, project, etc.)
    /// </summary>
    public class PortfolioItem : Entity<Guid>
    {
        public Guid PortfolioId { get; private set; }
        public ItemReference Reference { get; private set; }
        public Guid? CategoryId { get; private set; }
        public decimal? Progress { get; private set; }
        public string Notes { get; private set; }
        public DateTime AddedDate { get; private set; }
        public DateTime? LastUpdatedDate { get; private set; }

        protected PortfolioItem() { }

        public PortfolioItem(
            Guid id,
            Guid portfolioId,
            ItemReference reference,
            Guid? categoryId = null)
            : base(id)
        {
            PortfolioId = portfolioId;
            Reference = reference;
            CategoryId = categoryId;
            AddedDate = DateTime.Now;
        }

        public void UpdateCategory(Guid? categoryId)
        {
            CategoryId = categoryId;
            LastUpdatedDate = DateTime.Now;
        }

        public void UpdateProgress(decimal progress)
        {
            if (progress < 0 || progress > 100)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:InvalidValue")
                    .WithData("propertyName", "Progress");
            }

            Progress = progress;
            LastUpdatedDate = DateTime.Now;
        }

        public void UpdateNotes(string notes)
        {
            Notes = notes;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
