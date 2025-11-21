using System;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract
{
    /// <summary>
    /// Represents a clause within a contract
    /// </summary>
    public class ContractClause : Entity<Guid>
    {
        public Guid ContractId { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Reference { get; private set; }
        public bool IsMandatory { get; private set; }

        protected ContractClause() { }

        public ContractClause(
            Guid id,
            Guid contractId,
            string title,
            string content,
            string reference,
            bool isMandatory)
            : base(id)
        {
            ContractId = contractId;
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: ContractPortfolioManagementConsts.ContractClause.MaxTitleLength);
            
            Content = Check.NotNullOrWhiteSpace(
                content,
                nameof(content),
                maxLength: ContractPortfolioManagementConsts.ContractClause.MaxContentLength);
            
            Reference = reference;
            IsMandatory = isMandatory;
        }

        public void UpdateContent(string content)
        {
            Content = Check.NotNullOrWhiteSpace(
                content,
                nameof(content),
                maxLength: ContractPortfolioManagementConsts.ContractClause.MaxContentLength);
        }
    }
}
