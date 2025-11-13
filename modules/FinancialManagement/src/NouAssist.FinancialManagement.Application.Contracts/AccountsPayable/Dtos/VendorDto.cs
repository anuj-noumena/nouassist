using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AccountsPayable.Dtos
{
    public class VendorDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public MonetaryValue CreditLimit { get; set; }
        public MonetaryValue CurrentBalance { get; set; }
    }

    public class CreateVendorDto
    {
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public int PaymentTermsNetDays { get; set; }
        public string PreferredPaymentMethod { get; set; }
    }

    public class UpdateVendorDto
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int PaymentTermsNetDays { get; set; }
    }
}
