using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Events;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Exceptions;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Services
{
    /// <summary>
    /// Domain service for managing glossary terms
    /// </summary>
    public class GlossaryTermManager : DomainService, IGlossaryTermManager
    {
        private readonly IGlossaryTermRepository _glossaryTermRepository;
        private readonly ILogger<GlossaryTermManager> _logger;

        public GlossaryTermManager(
            IGlossaryTermRepository glossaryTermRepository,
            ILogger<GlossaryTermManager> logger)
        {
            _glossaryTermRepository = glossaryTermRepository;
            _logger = logger;
        }

        public async Task<GlossaryTerm> CreateAsync(
            string name,
            string definition,
            string? context = null,
            string? examples = null,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(definition, nameof(definition));

            // Check for existing term with the same name
            var existingTerm = await _glossaryTermRepository.FindByNameAsync(name, cancellationToken);
            if (existingTerm != null)
            {
                throw new GlossaryTermAlreadyExistsException(name);
            }

            var term = GlossaryTerm.Create(
                GuidGenerator.Create(),
                name,
                definition,
                CurrentTenant.Id,
                context,
                examples);

            term = await _glossaryTermRepository.InsertAsync(term, cancellationToken: cancellationToken);

            // Publish domain event
            term.AddDistributedEvent(new GlossaryTermCreatedEto
            {
                Id = term.Id,
                Name = term.Name,
                Definition = term.Definition,
                TenantId = term.TenantId,
                CreationTime = term.CreationTime
            });

            _logger.LogInformation(
                "Glossary term created: {Name} with ID {Id}",
                name,
                term.Id);

            return term;
        }

        public async Task<GlossaryTerm> UpdateDefinitionAsync(
            Guid termId,
            string newDefinition,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(newDefinition, nameof(newDefinition));

            var term = await _glossaryTermRepository.GetAsync(termId, cancellationToken: cancellationToken);
            term.UpdateDefinition(newDefinition);

            await _glossaryTermRepository.UpdateAsync(term, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Glossary term definition updated: {Name} (ID: {Id})",
                term.Name,
                termId);

            return term;
        }

        public async Task ApproveAsync(
            Guid termId,
            Guid approvedBy,
            DateTime approvedAt,
            CancellationToken cancellationToken = default)
        {
            var term = await _glossaryTermRepository.GetAsync(termId, cancellationToken: cancellationToken);
            term.Approve(approvedBy, approvedAt);

            await _glossaryTermRepository.UpdateAsync(term, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Glossary term approved: {Name} (ID: {Id}) by user {ApprovedBy}",
                term.Name,
                termId,
                approvedBy);
        }

        public async Task RevokeApprovalAsync(
            Guid termId,
            CancellationToken cancellationToken = default)
        {
            var term = await _glossaryTermRepository.GetAsync(termId, cancellationToken: cancellationToken);
            term.Revoke();

            await _glossaryTermRepository.UpdateAsync(term, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Glossary term approval revoked: {Name} (ID: {Id})",
                term.Name,
                termId);
        }

        public async Task SetRelatedTermsAsync(
            Guid termId,
            List<Guid> relatedTermIds,
            CancellationToken cancellationToken = default)
        {
            var term = await _glossaryTermRepository.GetAsync(termId, cancellationToken: cancellationToken);

            // Validate that all related term IDs exist
            if (relatedTermIds.Any())
            {
                var relatedTerms = await _glossaryTermRepository.GetRelatedTermsAsync(relatedTermIds, cancellationToken);
                var existingIds = relatedTerms.Select(t => t.Id).ToList();
                var missingIds = relatedTermIds.Except(existingIds).ToList();

                if (missingIds.Any())
                {
                    throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:RelatedTermsNotFound")
                        .WithData("missingIds", string.Join(", ", missingIds));
                }
            }

            term.SetRelatedTerms(relatedTermIds);
            await _glossaryTermRepository.UpdateAsync(term, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Related terms set for {Name} (ID: {Id}): {RelatedCount} terms",
                term.Name,
                termId,
                relatedTermIds.Count);
        }

        public async Task<bool> CanDeleteAsync(
            Guid termId,
            CancellationToken cancellationToken = default)
        {
            // Check if any other terms reference this term
            // This is a simplified check - in a real implementation,
            // you might also check if it's referenced in requirements or other entities
            var allTerms = await _glossaryTermRepository.GetListAsync(cancellationToken: cancellationToken);
            
            return !allTerms.Any(t => t.GetRelatedTermIds().Contains(termId));
        }

        public async Task<List<GlossaryTerm>> SearchAsync(
            string searchTerm,
            bool approvedOnly = true,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(searchTerm, nameof(searchTerm));

            var terms = await _glossaryTermRepository.SearchByNameAsync(searchTerm, cancellationToken: cancellationToken);

            if (approvedOnly)
            {
                terms = terms.Where(t => t.IsApproved).ToList();
            }

            _logger.LogInformation(
                "Glossary search for '{SearchTerm}' returned {ResultCount} terms",
                searchTerm,
                terms.Count);

            return terms;
        }
    }
}
