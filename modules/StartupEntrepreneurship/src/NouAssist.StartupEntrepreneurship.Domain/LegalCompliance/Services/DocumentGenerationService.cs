namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Services
{
    /// <summary>
    /// Implementation of the document generation service
    /// </summary>
    public class DocumentGenerationService : DomainService, IDocumentGenerationService
    {
        private readonly ILegalAgreementRepository _agreementRepository;
        private readonly ICorporateRecordRepository _recordRepository;
        private readonly ILogger<DocumentGenerationService> _logger;
        
        // This would be replaced with a proper template repository in a real implementation
        private readonly Dictionary<Guid, (string Name, TemplateContent Content)> _templates;

        public DocumentGenerationService(
            ILegalAgreementRepository agreementRepository,
            ICorporateRecordRepository recordRepository,
            ILogger<DocumentGenerationService> logger)
        {
            _agreementRepository = agreementRepository;
            _recordRepository = recordRepository;
            _logger = logger;
            _templates = new Dictionary<Guid, (string, TemplateContent)>();
        }

        /// <inheritdoc/>
        public Task<Guid> CreateTemplateAsync(
            string name,
            string content,
            string format,
            Dictionary<string, string> placeholders)
        {
            try
            {
                var templateId = GuidGenerator.Create();
                var templateContent = new TemplateContent(content, format, placeholders, Clock.Now);
                
                _templates[templateId] = (name, templateContent);
                
                return Task.FromResult(templateId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create document template {Name}", name);
                throw;
            }
        }

        /// <inheritdoc/>
        public Task UpdateTemplateAsync(
            Guid id,
            string name,
            string content,
            string format,
            Dictionary<string, string> placeholders)
        {
            try
            {
                if (!_templates.ContainsKey(id))
                {
                    throw new BusinessException("LegalCompliance:ErrorMessage:EntityNotFound", 
                        "Template not found");
                }
                
                var templateContent = new TemplateContent(content, format, placeholders, Clock.Now);
                _templates[id] = (name, templateContent);
                
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update document template {Id}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public Task<string> GenerateDocumentAsync(
            Guid templateId,
            Dictionary<string, string> values)
        {
            try
            {
                if (!_templates.ContainsKey(templateId))
                {
                    throw new BusinessException("LegalCompliance:ErrorMessage:EntityNotFound", 
                        "Template not found");
                }
                
                var template = _templates[templateId];
                var generatedContent = template.Content.ApplyPlaceholders(values);
                
                return Task.FromResult(generatedContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate document from template {Id}", templateId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<string> GenerateAgreementDocumentAsync(
            Guid agreementId,
            Guid templateId,
            Dictionary<string, string> additionalValues = null)
        {
            try
            {
                var agreement = await _agreementRepository.GetAsync(agreementId);
                
                var values = new Dictionary<string, string>
                {
                    { "title", agreement.Title },
                    { "description", agreement.Description },
                    { "type", agreement.Type.ToString() },
                    { "counterpartyName", agreement.CounterpartyName },
                    { "effectiveDate", agreement.EffectiveDate.ToString("yyyy-MM-dd") },
                    { "expirationDate", agreement.ExpirationDate?.ToString("yyyy-MM-dd") ?? "N/A" },
                    { "referenceNumber", agreement.ReferenceNumber ?? "N/A" },
                    { "jurisdiction", agreement.Jurisdiction.ToString() },
                    { "status", agreement.Status.Status.ToString() }
                };
                
                if (additionalValues != null)
                {
                    foreach (var kvp in additionalValues)
                    {
                        values[kvp.Key] = kvp.Value;
                    }
                }
                
                return await GenerateDocumentAsync(templateId, values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate agreement document for agreement {Id}", agreementId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<string> GenerateCorporateRecordDocumentAsync(
            Guid recordId,
            Guid templateId,
            Dictionary<string, string> additionalValues = null)
        {
            try
            {
                var record = await _recordRepository.GetAsync(recordId);
                
                var values = new Dictionary<string, string>
                {
                    { "title", record.Title },
                    { "description", record.Description },
                    { "type", record.Type.ToString() },
                    { "recordDate", record.RecordDate.ToString("yyyy-MM-dd") },
                    { "referenceNumber", record.ReferenceNumber ?? "N/A" },
                    { "status", record.Status.Status.ToString() }
                };
                
                if (additionalValues != null)
                {
                    foreach (var kvp in additionalValues)
                    {
                        values[kvp.Key] = kvp.Value;
                    }
                }
                
                return await GenerateDocumentAsync(templateId, values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate corporate record document for record {Id}", recordId);
                throw;
            }
        }
    }
}
