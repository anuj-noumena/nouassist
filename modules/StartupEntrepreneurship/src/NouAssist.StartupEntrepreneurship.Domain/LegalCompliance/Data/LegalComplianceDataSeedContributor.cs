using System;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Repositories;
using NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Data
{
    /// <summary>
    /// Seed contributor for Legal & Compliance data
    /// </summary>
    public class LegalComplianceDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIntellectualPropertyRepository _ipRepository;
        private readonly ILegalAgreementRepository _agreementRepository;
        private readonly IComplianceObligationRepository _obligationRepository;
        private readonly ICorporateRecordRepository _recordRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public LegalComplianceDataSeedContributor(
            IIntellectualPropertyRepository ipRepository,
            ILegalAgreementRepository agreementRepository,
            IComplianceObligationRepository obligationRepository,
            ICorporateRecordRepository recordRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            _ipRepository = ipRepository;
            _agreementRepository = agreementRepository;
            _obligationRepository = obligationRepository;
            _recordRepository = recordRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // Only seed if the repositories are empty
            if (await _ipRepository.GetCountAsync() > 0 ||
                await _agreementRepository.GetCountAsync() > 0 ||
                await _obligationRepository.GetCountAsync() > 0 ||
                await _recordRepository.GetCountAsync() > 0)
            {
                return;
            }

            await SeedIntellectualPropertyAssetsAsync();
            await SeedLegalAgreementsAsync();
            await SeedComplianceObligationsAsync();
            await SeedCorporateRecordsAsync();
        }

        private async Task SeedIntellectualPropertyAssetsAsync()
        {
            // Create sample IP assets
            var samplePatent = IntellectualPropertyAsset.Create(
                _guidGenerator.Create(),
                "Sample AI Algorithm Patent",
                "A machine learning algorithm for predictive analytics in startup environments",
                IntellectualPropertyType.Patent,
                new IPIdentifier("US12345678", IntellectualPropertyType.Patent, "USPTO"),
                new Jurisdiction("United States", "California"),
                DateTime.Now.AddYears(-1),
                FilingStatusType.Pending,
                null,
                DateTime.Now.AddYears(19),
                _currentTenant.Id);

            var sampleTrademark = IntellectualPropertyAsset.Create(
                _guidGenerator.Create(),
                "NouAssist Brand Trademark",
                "The primary brand and logo trademark for the NouAssist platform",
                IntellectualPropertyType.Trademark,
                new IPIdentifier("TM87654321", IntellectualPropertyType.Trademark, "USPTO"),
                new Jurisdiction("United States"),
                DateTime.Now.AddMonths(-8),
                FilingStatusType.Registered,
                DateTime.Now.AddMonths(-2),
                DateTime.Now.AddYears(9).AddMonths(10),
                _currentTenant.Id);

            await _ipRepository.InsertAsync(samplePatent);
            await _ipRepository.InsertAsync(sampleTrademark);
        }

        private async Task SeedLegalAgreementsAsync()
        {
            // Create sample legal agreements
            var sampleNda = LegalAgreement.Create(
                _guidGenerator.Create(),
                "Standard NDA Template",
                "Non-disclosure agreement for use with potential customers and partners",
                LegalAgreementType.NDA,
                "Various Counterparties",
                DateTime.Now.AddMonths(-3),
                DateTime.Now.AddYears(2).AddMonths(-3),
                "NDA-TEMPLATE-001",
                new Jurisdiction("United States"),
                null,
                null,
                _currentTenant.Id);

            var sampleServiceAgreement = LegalAgreement.Create(
                _guidGenerator.Create(),
                "Cloud Services Provider Agreement",
                "Service agreement with our primary cloud infrastructure provider",
                LegalAgreementType.ServiceAgreement,
                "CloudProviderCo",
                DateTime.Now.AddMonths(-2),
                DateTime.Now.AddYears(1).AddMonths(-2),
                "SVC-CLOUD-001",
                new Jurisdiction("United States"),
                10000.00m,
                "USD",
                _currentTenant.Id);

            await _agreementRepository.InsertAsync(sampleNda);
            await _agreementRepository.InsertAsync(sampleServiceAgreement);
        }

        private async Task SeedComplianceObligationsAsync()
        {
            // Create sample compliance obligations
            var annualTaxFiling = ComplianceObligation.Create(
                _guidGenerator.Create(),
                "Annual Corporate Tax Filing",
                new ObligationDescription(
                    "Annual corporate tax return filing requirement",
                    "Complete and submit Form 1120 or relevant corporate tax return",
                    "Consult with tax advisor before filing"),
                new Jurisdiction("United States", "California"),
                "Internal Revenue Service",
                new DateTime(DateTime.Now.Year, 4, 15),
                "TAX-CORP-001",
                true,
                "Annual",
                _currentTenant.Id);

            var dataPrivacyCompliance = ComplianceObligation.Create(
                _guidGenerator.Create(),
                "Privacy Policy Compliance Review",
                new ObligationDescription(
                    "Periodic review of privacy policies for compliance with current regulations",
                    "Review and update privacy policy and data handling practices to ensure compliance with applicable laws",
                    "Focus on GDPR, CCPA, and other relevant regulations"),
                new Jurisdiction("United States"),
                "Various",
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(3),
                "PRIV-REV-001",
                true,
                "Quarterly",
                _currentTenant.Id);

            await _obligationRepository.InsertAsync(annualTaxFiling);
            await _obligationRepository.InsertAsync(dataPrivacyCompliance);
        }

        private async Task SeedCorporateRecordsAsync()
        {
            // Create sample corporate records
            var boardMeeting = CorporateRecord.Create(
                _guidGenerator.Create(),
                "Q1 Board Meeting Minutes",
                "Minutes from the Q1 board meeting discussing product roadmap and funding strategy",
                CorporateRecordType.BoardMeetingMinutes,
                new DateTime(DateTime.Now.Year, 3, 15),
                "BOARD-MIN-Q1",
                _currentTenant.Id);

            var bylawsRecord = CorporateRecord.Create(
                _guidGenerator.Create(),
                "Company Bylaws",
                "Official company bylaws as adopted by the board of directors",
                CorporateRecordType.Bylaws,
                new DateTime(DateTime.Now.Year - 1, 1, 1),
                "BYLAWS-001",
                _currentTenant.Id);

            await _recordRepository.InsertAsync(boardMeeting);
            await _recordRepository.InsertAsync(bylawsRecord);
        }
    }
}
