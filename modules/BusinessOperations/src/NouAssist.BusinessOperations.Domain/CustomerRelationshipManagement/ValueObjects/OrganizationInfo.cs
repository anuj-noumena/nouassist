using System.Collections.Generic;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Contains detailed information about an organization
    /// </summary>
    public class OrganizationInfo : ValueObject
    {
        /// <summary>
        /// The type of organization
        /// </summary>
        public OrganizationType Type { get; private set; }
        
        /// <summary>
        /// The industry the organization operates in
        /// </summary>
        public string Industry { get; private set; }
        
        /// <summary>
        /// The number of employees in the organization
        /// </summary>
        public int? EmployeeCount { get; private set; }
        
        /// <summary>
        /// Annual revenue of the organization
        /// </summary>
        public decimal? AnnualRevenue { get; private set; }
        
        /// <summary>
        /// Tax ID or business registration number
        /// </summary>
        public string TaxId { get; private set; }
        
        /// <summary>
        /// Additional notes about the organization
        /// </summary>
        public string Notes { get; private set; }

        protected OrganizationInfo()
        {
        }

        public OrganizationInfo(
            OrganizationType type,
            string industry = null,
            int? employeeCount = null,
            decimal? annualRevenue = null,
            string taxId = null,
            string notes = null)
        {
            Type = type;
            Industry = industry;
            EmployeeCount = employeeCount;
            AnnualRevenue = annualRevenue;
            TaxId = taxId;
            Notes = notes;
        }

        public OrganizationInfo Update(
            OrganizationType? type = null,
            string industry = null,
            int? employeeCount = null,
            decimal? annualRevenue = null,
            string taxId = null,
            string notes = null)
        {
            if (type.HasValue)
            {
                Type = type.Value;
            }

            if (industry != null)
            {
                Industry = industry;
            }

            if (employeeCount.HasValue)
            {
                EmployeeCount = employeeCount;
            }

            if (annualRevenue.HasValue)
            {
                AnnualRevenue = annualRevenue;
            }

            if (taxId != null)
            {
                TaxId = taxId;
            }

            if (notes != null)
            {
                Notes = notes;
            }

            return this;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Industry ?? string.Empty;
            yield return EmployeeCount ?? 0;
            yield return AnnualRevenue ?? 0m;
            yield return TaxId ?? string.Empty;
            yield return Notes ?? string.Empty;
        }
    }
}
