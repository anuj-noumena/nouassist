using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing a sharing link for portfolios
    /// </summary>
    public class SharingLink : ValueObject
    {
        public Guid LinkId { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public AccessPermissions Permissions { get; private set; }

        protected SharingLink() { }

        public SharingLink(Guid linkId, string token, DateTime expirationDate, AccessPermissions permissions)
        {
            LinkId = linkId;
            Token = Check.NotNullOrWhiteSpace(token, nameof(token));
            
            if (expirationDate <= DateTime.Now)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:DateRange")
                    .WithData("property", "ExpirationDate")
                    .WithData("reference", "current date");
            }
            
            ExpirationDate = expirationDate;
            Permissions = permissions ?? AccessPermissions.ReadOnly();
        }

        public bool IsExpired(DateTime currentTime)
        {
            return currentTime > ExpirationDate;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return LinkId;
            yield return Token;
            yield return ExpirationDate;
            yield return Permissions;
        }
    }
}
