using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Data
{
    public class CRMDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Lead, Guid> _leadRepository;
        private readonly IRepository<Contact, Guid> _contactRepository;
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IRepository<CommunicationTemplate, Guid> _templateRepository;
        private readonly ILeadManager _leadManager;
        private readonly IContactManager _contactManager;
        private readonly IOrganizationManager _organizationManager;
        private readonly ICommunicationTemplateManager _templateManager;
        private readonly IGuidGenerator _guidGenerator;
        
        public CRMDataSeedContributor(
            IRepository<Lead, Guid> leadRepository,
            IRepository<Contact, Guid> contactRepository,
            IRepository<Organization, Guid> organizationRepository,
            IRepository<CommunicationTemplate, Guid> templateRepository,
            ILeadManager leadManager,
            IContactManager contactManager,
            IOrganizationManager organizationManager,
            ICommunicationTemplateManager templateManager,
            IGuidGenerator guidGenerator)
        {
            _leadRepository = leadRepository;
            _contactRepository = contactRepository;
            _organizationRepository = organizationRepository;
            _templateRepository = templateRepository;
            _leadManager = leadManager;
            _contactManager = contactManager;
            _organizationManager = organizationManager;
            _templateManager = templateManager;
            _guidGenerator = guidGenerator;
        }
        
        public async Task SeedAsync(DataSeedContext context)
        {
            // Only seed if we have no data
            if (await _leadRepository.GetCountAsync() > 0)
            {
                return;
            }
            
            // Seed sample organizations
            var acmeOrgId = _guidGenerator.Create();
            await _organizationManager.CreateAsync(
                "Acme Corporation",
                new ContactInfo(
                    "info@acme.com",
                    "+1-555-123-4567",
                    website: "https://www.acme.com",
                    address: "123 Main St",
                    city: "Metropolis",
                    state: "NY",
                    postalCode: "10001",
                    country: "USA"),
                new OrganizationInfo(
                    OrganizationType.Customer,
                    "Technology",
                    employeeCount: 500,
                    annualRevenue: 10000000,
                    taxId: "123456789"),
                "ACM-12345");
                
            var contosouOrgId = _guidGenerator.Create();
            await _organizationManager.CreateAsync(
                "Contoso Ltd",
                new ContactInfo(
                    "info@contoso.com",
                    "+1-555-987-6543",
                    website: "https://www.contoso.com",
                    address: "456 Oak Ave",
                    city: "Springfield",
                    state: "IL",
                    postalCode: "62701",
                    country: "USA"),
                new OrganizationInfo(
                    OrganizationType.Prospect,
                    "Manufacturing",
                    employeeCount: 1000,
                    annualRevenue: 25000000,
                    taxId: "987654321"),
                "CON-67890");
                
            // Seed sample contacts
            await _contactManager.CreateAsync(
                "John Smith",
                new ContactInfo(
                    "john.smith@acme.com",
                    "+1-555-111-2222",
                    alternativePhone: "+1-555-333-4444",
                    address: "123 Main St",
                    city: "Metropolis",
                    state: "NY",
                    postalCode: "10001",
                    country: "USA"),
                "CEO",
                acmeOrgId,
                true,
                "Met at tech conference");
                
            await _contactManager.CreateAsync(
                "Jane Doe",
                new ContactInfo(
                    "jane.doe@contoso.com",
                    "+1-555-555-6666",
                    address: "456 Oak Ave",
                    city: "Springfield",
                    state: "IL",
                    postalCode: "62701",
                    country: "USA"),
                "CTO",
                contosouOrgId,
                true,
                "Potential partner for AI project");
                
            // Seed sample leads
            await _leadManager.CreateAsync(
                "Michael Johnson",
                new ContactInfo(
                    "michael.johnson@example.com",
                    "+1-555-777-8888",
                    website: "https://www.example.com"),
                new LeadDetails(
                    "IT Director",
                    "Example Inc",
                    "Healthcare",
                    LeadSource.WebForm,
                    campaignId: "SUMMER-2023",
                    notes: "Interested in our enterprise solution",
                    estimatedBudget: 50000),
                LeadStatus.New,
                score: 75);
                
            await _leadManager.CreateAsync(
                "Sarah Williams",
                new ContactInfo(
                    "sarah.williams@startup.co",
                    "+1-555-999-0000"),
                new LeadDetails(
                    "Founder",
                    "Startup Co",
                    "Finance",
                    LeadSource.Referral,
                    referralSource: "John Smith",
                    notes: "Looking for early access to beta"),
                LeadStatus.Contacted,
                score: 60);
                
            // Seed sample email templates
            await _templateManager.CreateAsync(
                "WELCOME",
                "Welcome Email",
                InteractionType.Email,
                "Welcome to our platform, {{Name}}!",
                "Dear {{Name}},\n\nThank you for your interest in our products and services.\n\nWe're excited to have you on board and look forward to helping you achieve your goals.\n\nBest regards,\nThe Team",
                "First email sent to new leads",
                "Onboarding");
                
            await _templateManager.CreateAsync(
                "FOLLOW_UP",
                "Follow-up Email",
                InteractionType.Email,
                "Following up on our conversation, {{Name}}",
                "Dear {{Name}},\n\nI wanted to follow up on our recent conversation about {{Topic}}.\n\nDo you have any additional questions I can help with?\n\nBest regards,\n{{SenderName}}",
                "Email for following up after initial contact",
                "Sales");
        }
    }
}
