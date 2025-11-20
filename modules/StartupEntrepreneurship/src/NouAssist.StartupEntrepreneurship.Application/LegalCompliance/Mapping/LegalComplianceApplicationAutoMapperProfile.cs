using AutoMapper;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Compliance.Dtos;
using NouAssist.StartupEntrepreneurship.LegalCompliance.CorporateRecords.Dtos;
using NouAssist.StartupEntrepreneurship.LegalCompliance.DocumentGeneration.Dtos;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Entities;
using NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos;
using NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos;
using NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects;
using System;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Mapping
{
    public class LegalComplianceApplicationAutoMapperProfile : Profile
    {
        public LegalComplianceApplicationAutoMapperProfile()
        {
            // Intellectual Property mappings
            CreateMap<IntellectualPropertyAsset, IPAssetDto>()
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier.Value))
                .ForMember(dest => dest.IssuingAuthority, opt => opt.MapFrom(src => src.Identifier.IssuingAuthority))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Jurisdiction.Country))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Jurisdiction.State))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Status))
                .ForMember(dest => dest.StatusDate, opt => opt.MapFrom(src => src.Status.StatusDate))
                .ForMember(dest => dest.StatusNotes, opt => opt.MapFrom(src => src.Status.StatusNotes))
                .ForMember(dest => dest.ValuationAmount, opt => opt.MapFrom(src => src.Valuation != null ? src.Valuation.EstimatedValue : (decimal?)null))
                .ForMember(dest => dest.ValuationCurrency, opt => opt.MapFrom(src => src.Valuation != null ? src.Valuation.Currency : null))
                .ForMember(dest => dest.ValuationDate, opt => opt.MapFrom(src => src.Valuation != null ? src.Valuation.ValuationDate : (DateTime?)null))
                .ForMember(dest => dest.ValuationMethod, opt => opt.MapFrom(src => src.Valuation != null ? src.Valuation.ValuationMethod : null));

            CreateMap<IntellectualPropertyAsset, IPAssetListDto>()
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier.Value))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Jurisdiction.Country))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Status));

            CreateMap<FilingDeadline, IPDeadlineDto>();

            // Legal Agreement mappings
            CreateMap<LegalAgreement, LegalAgreementDto>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Jurisdiction.Country))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Jurisdiction.State))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Status))
                .ForMember(dest => dest.StatusDate, opt => opt.MapFrom(src => src.Status.StatusDate))
                .ForMember(dest => dest.StatusNotes, opt => opt.MapFrom(src => src.Status.StatusNotes))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive(DateTime.Now)))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired(DateTime.Now)));

            CreateMap<LegalAgreement, LegalAgreementListDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Status))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive(DateTime.Now)))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired(DateTime.Now)));

            CreateMap<AgreementTerms, AgreementTermDto>();
            CreateMap<AgreementReminder, AgreementReminderDto>();

            // Compliance Obligation mappings
            CreateMap<ComplianceObligation, ComplianceObligationDto>()
                .ForMember(dest => dest.Overview, opt => opt.MapFrom(src => src.Description.Overview))
                .ForMember(dest => dest.Requirements, opt => opt.MapFrom(src => src.Description.Requirements))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Description.Notes))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Jurisdiction.Country))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Jurisdiction.State))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Status))
                .ForMember(dest => dest.StatusDate, opt => opt.MapFrom(src => src.Status.StatusDate))
                .ForMember(dest => dest.StatusNotes, opt => opt.MapFrom(src => src.Status.Notes))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue(DateTime.Now)));

            CreateMap<ComplianceObligation, ComplianceObligationListDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Status))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Jurisdiction.Country))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue(DateTime.Now)));

            // Corporate Record mappings
            CreateMap<CorporateRecord, CorporateRecordDto>()
                .ForMember(dest => dest.RecordTypeDescription, opt => opt.MapFrom(src => src.RecordType.Description))
                .ForMember(dest => dest.RequiresSignature, opt => opt.MapFrom(src => src.RecordType.RequiresSignature))
                .ForMember(dest => dest.RequiresNotarization, opt => opt.MapFrom(src => src.RecordType.RequiresNotarization))
                .ForMember(dest => dest.RequiresFiling, opt => opt.MapFrom(src => src.RecordType.RequiresFiling))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Status))
                .ForMember(dest => dest.StatusDate, opt => opt.MapFrom(src => src.Status.StatusDate))
                .ForMember(dest => dest.StatusNotes, opt => opt.MapFrom(src => src.Status.StatusNotes));

            CreateMap<CorporateRecord, CorporateRecordListDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Status))
                .ForMember(dest => dest.RequiresFiling, opt => opt.MapFrom(src => src.RecordType.RequiresFiling));
        }
    }
}
