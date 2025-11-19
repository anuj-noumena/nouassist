using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AccountsPayable.Dtos
{
    public class VendorDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int PaymentTermsDays { get; set; }
        public string PreferredPaymentMethod { get; set; }
        public decimal CreditLimit { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateVendorDto
    {
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int PaymentTermsDays { get; set; } = 30;
        public string PreferredPaymentMethod { get; set; } = "ACH";
    }

    public class UpdateVendorDto
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int PaymentTermsDays { get; set; }
        public string PreferredPaymentMethod { get; set; }
    }

    public class GetVendorListDto : PagedAndSortedResultRequestDto
    {
        public string SearchTerm { get; set; }
        public bool? ActiveOnly { get; set; } = true;
    }
}
