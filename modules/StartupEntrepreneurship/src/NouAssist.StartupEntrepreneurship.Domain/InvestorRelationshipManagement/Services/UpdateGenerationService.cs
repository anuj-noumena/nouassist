namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Services
{
    /// <summary>
    /// Domain service for generating investor updates from templates and data
    /// </summary>
    public class UpdateGenerationService : DomainService, IUpdateGenerationService
    {
        private readonly IInvestorUpdateRepository _updateRepository;
        private readonly ILogger<UpdateGenerationService> _logger;
        // Assuming you might have a service to fetch template details
        // private readonly ITemplateService _templateService; // Example: Inject if you have a template service

        public UpdateGenerationService(
            IInvestorUpdateRepository updateRepository,
            ILogger<UpdateGenerationService> logger /*, ITemplateService templateService */)
        {
            _updateRepository = updateRepository;
            _logger = logger;
            // _templateService = templateService; // Example
        }

        public async Task<InvestorUpdate> GenerateUpdateAsync(string templateId, Dictionary<string, object> data, DateTime scheduledDate)
        {
            try
            {
                // Create update based on template and data
                var update = InvestorUpdate.Create(
                    GuidGenerator.Create(),
                    data.GetValueOrDefault("title", "Investor Update").ToString(),
                    data.GetValueOrDefault("subject", "Monthly Investor Update").ToString(),
                    UpdateFrequency.Monthly, // This could also come from data or template
                    templateId,
                    CurrentTenant.Id
                );

                // Add sections based on template
                await AddTemplateSectionsAsync(update, templateId, data);

                // Add metrics if provided
                if (data.ContainsKey("metrics") && data["metrics"] is List<MetricValue> metrics)
                {
                    foreach (var metric in metrics)
                    {
                        update.AddMetric(metric);
                    }
                }

                // Schedule the update
                update.Schedule(scheduledDate);

                _logger.LogInformation("Generated update {UpdateId} from template {TemplateId} scheduled for {ScheduledDate}", update.Id, templateId, scheduledDate);
                return update;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate update from template {TemplateId}", templateId);
                throw; // Or handle more gracefully, perhaps returning null or a failed result object
            }
        }

        public async Task<InvestorUpdate> GenerateMetricsUpdateAsync(List<MetricValue> metrics, DateTime scheduledDate)
        {
            try
            {
                var update = InvestorUpdate.Create(
                    GuidGenerator.Create(),
                    "Key Metrics Performance Update", // Default title
                    $"Key Metrics Update - {scheduledDate:MMMM yyyy}", // Default subject
                    UpdateFrequency.Monthly, // Default frequency, consider making this configurable
                    tenantId: CurrentTenant.Id
                );

                // Add metrics section
                var metricsContent = await GenerateMetricsContentAsync(metrics);
                update.AddSection("Key Metrics Summary", metricsContent);

                // Add each metric as snapshot
                foreach (var metric in metrics)
                {
                    update.AddMetric(metric, scheduledDate); // Ensure snapshot date is consistent
                }

                update.Schedule(scheduledDate);

                _logger.LogInformation("Generated metrics update {UpdateId} scheduled for {ScheduledDate}", update.Id, scheduledDate);
                return update;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate metrics update scheduled for {ScheduledDate}", scheduledDate);
                throw;
            }
        }

        public async Task<List<InvestorUpdate>> GenerateScheduledUpdatesAsync(DateTime forDate)
        {
            var generatedUpdates = new List<InvestorUpdate>();
            _logger.LogInformation("Starting generation of scheduled updates for date: {ForDate}", forDate);

            // Placeholder logic: In a real scenario, you'd query for investors/groups
            // that are due for an update based on their preferences (UpdateFrequency)
            // and the last update sent date.

            // Example: Fetch investors who prefer monthly updates and haven't received one this month.
            // This would involve querying InvestorProfile entities and their CommunicationPreferences.
            // For simplicity, let's assume we have a list of target investor groups or a general update.

            // This is a very simplified example. A real implementation would be more complex.
            // It might involve:
            // 1. Identifying all investor profiles or groups that need an update.
            // 2. Checking their preferred UpdateFrequency and the date of their last update.
            // 3. Determining if a new update is due based on 'forDate'.
            // 4. Fetching relevant data/metrics for each investor/group.
            // 5. Using a specific template or a default one.

            // Example: Generate a generic monthly update if it's the first of the month
            if (forDate.Day == 1)
            {
                _logger.LogInformation("Generating a generic monthly update for {ForDate}", forDate);
                var monthlyUpdateData = new Dictionary<string, object>
                {
                    { "title", $"Monthly Review - {forDate:MMMM yyyy}" },
                    { "subject", $"Your Monthly Update - {forDate:MMMM yyyy}" },
                    // Potentially add some default metrics or sections
                };

                // Using a default template ID or null if no specific template
                string defaultMonthlyTemplateId = "default_monthly_template";

                var genericMonthlyUpdate = await GenerateUpdateAsync(
                    defaultMonthlyTemplateId,
                    monthlyUpdateData,
                    forDate // Schedule for the given date
                );
                if (genericMonthlyUpdate != null)
                {
                    generatedUpdates.Add(genericMonthlyUpdate);
                    // In a real system, you would then persist this update
                    // await _updateRepository.InsertAsync(genericMonthlyUpdate, autoSave: true);
                    _logger.LogInformation("Scheduled generic monthly update {UpdateId}", genericMonthlyUpdate.Id);
                }
            }

            // Add more sophisticated logic here based on your application's needs,
            // such as iterating through investor profiles and checking their update frequency.

            _logger.LogInformation("Completed generation of {Count} scheduled updates for date: {ForDate}", generatedUpdates.Count, forDate);
            return generatedUpdates;
        }

        private async Task AddTemplateSectionsAsync(InvestorUpdate update, string templateId, Dictionary<string, object> data)
        {
            // Placeholder for fetching template sections and populating them with data.
            // In a real application, this method would interact with a template engine or service.
            _logger.LogDebug("Attempting to add sections for template {TemplateId} to update {UpdateId}", templateId, update.Id);

            // Example: Simulate fetching sections from a template
            // var templateSections = await _templateService.GetSectionsAsync(templateId);
            // foreach (var sectionDefinition in templateSections)
            // {
            //     string populatedContent = PopulateTemplateContent(sectionDefinition.ContentTemplate, data);
            //     var updateContent = new UpdateContent(populatedContent, sectionDefinition.ContentType);
            //     update.AddSection(sectionDefinition.Title, updateContent, sectionDefinition.Order);
            // }

            // Simplified placeholder:
            if (templateId == "default_monthly_template" || data.ContainsKey("introduction_content"))
            {
                var introContent = data.GetValueOrDefault("introduction_content", "This is your monthly update overview.").ToString();
                update.AddSection(
                    "Introduction",
                    new UpdateContent(introContent, "text/html"),
                    1
                );
                _logger.LogDebug("Added 'Introduction' section to update {UpdateId}", update.Id);
            }

            if (data.ContainsKey("highlights_content"))
            {
                var highlights = data.GetValueOrDefault("highlights_content", "Key highlights for this period will be listed here.").ToString();
                update.AddSection(
                    "Key Highlights",
                    new UpdateContent(highlights, "text/html"),
                    2
                );
                _logger.LogDebug("Added 'Key Highlights' section to update {UpdateId}", update.Id);
            }

            // Simulate adding a financial summary section if financial data is present
            if (data.ContainsKey("financial_summary"))
            {
                 update.AddSection(
                    "Financial Summary",
                    new UpdateContent(data["financial_summary"].ToString(), "text/html"),
                    3
                );
                _logger.LogDebug("Added 'Financial Summary' section to update {UpdateId}", update.Id);
            }
            await Task.CompletedTask; // To make the method async as per typical service patterns
        }

        private async Task<UpdateContent> GenerateMetricsContentAsync(List<MetricValue> metrics)
        {
            // Placeholder for generating rich content (e.g., HTML table) from metrics.
            _logger.LogDebug("Generating content for {MetricsCount} metrics", metrics.Count);

            var stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("<h3>Key Metrics Overview</h3>");
            stringBuilder.AppendLine("<table border='1' style='width:100%; border-collapse: collapse;'>");
            stringBuilder.AppendLine("<tr><th>Metric Name</th><th>Value</th><th>Unit</th><th>Previous Value</th><th>Target Value</th><th>Growth %</th><th>Target Achievement %</th></tr>");

            foreach (var metric in metrics)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append($"<td>{System.Net.WebUtility.HtmlEncode(metric.Name)}</td>");
                stringBuilder.Append($"<td>{metric.Value}</td>");
                stringBuilder.Append($"<td>{System.Net.WebUtility.HtmlEncode(metric.Unit ?? "-")}</td>");
                stringBuilder.Append($"<td>{(metric.PreviousValue.HasValue ? metric.PreviousValue.Value.ToString() : "-")}</td>");
                stringBuilder.Append($"<td>{(metric.TargetValue.HasValue ? metric.TargetValue.Value.ToString() : "-")}</td>");
                stringBuilder.Append($"<td>{(metric.GetGrowthPercentage().HasValue ? $"{metric.GetGrowthPercentage():F2}%" : "-")}</td>");
                stringBuilder.Append($"<td>{(metric.GetTargetAchievementPercentage().HasValue ? $"{metric.GetTargetAchievementPercentage():F2}%" : "-")}</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.AppendLine("</table>");

            _logger.LogDebug("Generated metrics content string: {ContentString}", stringBuilder.ToString());
            await Task.CompletedTask; // To make the method async
            return new UpdateContent(stringBuilder.ToString(), "text/html");
        }
    }
}
