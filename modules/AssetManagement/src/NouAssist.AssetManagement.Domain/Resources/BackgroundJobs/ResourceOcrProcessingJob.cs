using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.AssetManagement.Resources.Data;
using NouAssist.AssetManagement.Resources.Enums;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace NouAssist.AssetManagement.Resources.BackgroundJobs
{
    public class ResourceOcrProcessingJobArgs
    {
        public Guid ResourceId { get; set; }
        public Guid VersionId { get; set; }
    }

    public class ResourceOcrProcessingJob : AsyncBackgroundJob<ResourceOcrProcessingJobArgs>, ITransientDependency
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceOcrProcessingJob(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        [UnitOfWork]
        public override async Task ExecuteAsync(ResourceOcrProcessingJobArgs args)
        {
            Logger.LogInformation($"Processing OCR for resource {args.ResourceId}, version {args.VersionId}");

            try
            {
                var resource = await _resourceRepository.GetWithVersionsAsync(args.ResourceId);
                var version = resource.GetVersion(args.VersionId);

                if (version == null)
                {
                    Logger.LogWarning($"Version {args.VersionId} not found for resource {args.ResourceId}");
                    return;
                }

                if (version.OcrStatus != OcrStatus.Pending)
                {
                    Logger.LogInformation($"OCR already processed for version {args.VersionId}");
                    return;
                }

                // Update status to processing
                version.SetOcrProcessing();

                // TODO: Implement actual OCR processing using Azure Computer Vision, Tesseract, etc.
                await Task.Delay(5000); // Simulate processing

                // For demo, just set completed with empty text
                version.SetOcrCompleted("OCR text would go here...");

                await _resourceRepository.UpdateAsync(resource);

                Logger.LogInformation($"Successfully completed OCR for version {args.VersionId}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error processing OCR for resource {args.ResourceId}");
                
                // Mark as failed
                var resource = await _resourceRepository.GetWithVersionsAsync(args.ResourceId);
                var version = resource.GetVersion(args.VersionId);
                version?.SetOcrFailed(ex.Message);
                await _resourceRepository.UpdateAsync(resource);
                
                throw;
            }
        }
    }
}
