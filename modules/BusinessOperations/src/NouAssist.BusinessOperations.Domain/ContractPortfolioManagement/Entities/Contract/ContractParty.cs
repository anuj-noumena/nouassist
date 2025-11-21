using System;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Constants;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract
{
    /// <summary>
    /// Represents a party involved in a contract
    /// </summary>
    public class ContractParty : Entity<Guid>
    {
        public Guid ContractId { get; private set; }
        public string Name { get; private set; }
        public string Identifier { get; private set; }
        public string ContactInfo { get; private set; }
        public ContractPartyType PartyType { get; private set; }

        protected ContractParty() { }

        public ContractParty(
            Guid id,
            Guid contractId,
            string name,
            string identifier,
            string contactInfo,
            ContractPartyType partyType)
            : base(id)
        {
            ContractId = contractId;
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ContractPortfolioManagementConsts.ContractParty.MaxNameLength);
            
            Identifier = Check.NotNullOrWhiteSpace(
                identifier,
                nameof(identifier),
                maxLength: ContractPortfolioManagementConsts.ContractParty.MaxIdentifierLength);
            
            ContactInfo = Check.Length(
                contactInfo,
                nameof(contactInfo),
                maxLength: ContractPortfolioManagementConsts.ContractParty.MaxContactInfoLength);
            
            PartyType = partyType;
        }

        public void UpdatePartyDetails(
            string name,
            string identifier,
            string contactInfo,
            ContractPartyType partyType)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ContractPortfolioManagementConsts.ContractParty.MaxNameLength);
            
            Identifier = Check.NotNullOrWhiteSpace(
                identifier,
                nameof(identifier),
                maxLength: ContractPortfolioManagementConsts.ContractParty.MaxIdentifierLength);
            
            ContactInfo = Check.Length(
                contactInfo,
                nameof(contactInfo),
                maxLength: ContractPortfolioManagementConsts.ContractParty.MaxContactInfoLength);
            
            PartyType = partyType;
        }
    }
}
