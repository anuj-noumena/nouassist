using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Events;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Exceptions;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates
{
    /// <summary>
    /// Aggregate root representing a customer interview with its associated data
    /// </summary>
    public class CustomerInterview : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public InterviewType Type { get; private set; }
        public InterviewStatus Status { get; private set; }
        public DateTime ScheduledDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }
        public string InterviewerName { get; private set; }
        public string CustomerName { get; private set; }
        public string CustomerEmail { get; private set; }
        public Guid? CustomerSegmentId { get; private set; }
        public InterviewGuide Guide { get; private set; }

        // Entities within the aggregate
        private readonly List<InterviewTranscript> _transcripts;
        private readonly List<Insight> _insights;

        // Public read-only access
        public IReadOnlyCollection<InterviewTranscript> Transcripts => _transcripts.AsReadOnly();
        public IReadOnlyCollection<Insight> Insights => _insights.AsReadOnly();

        protected CustomerInterview()
        {
            _transcripts = new List<InterviewTranscript>();
            _insights = new List<Insight>();
        }

        public static CustomerInterview Create(
            Guid id,
            string title,
            string description,
            InterviewType type,
            DateTime scheduledDate,
            string interviewerName,
            string customerName,
            string customerEmail,
            InterviewGuide guide,
            Guid? customerSegmentId = null,
            Guid? tenantId = null)
        {
            return new CustomerInterview(
                id, title, description, type, scheduledDate,
                interviewerName, customerName, customerEmail,
                guide, customerSegmentId, tenantId);
        }

        private CustomerInterview(
            Guid id,
            string title,
            string description,
            InterviewType type,
            DateTime scheduledDate,
            string interviewerName,
            string customerName,
            string customerEmail,
            InterviewGuide guide,
            Guid? customerSegmentId,
            Guid? tenantId) : base(id)
        {
            SetTitle(title);
            SetDescription(description);
            Type = type;
            SetScheduledDate(scheduledDate);
            SetInterviewerName(interviewerName);
            SetCustomerName(customerName);
            SetCustomerEmail(customerEmail);
            Guide = guide ?? throw new ArgumentNullException(nameof(guide));
            CustomerSegmentId = customerSegmentId;
            TenantId = tenantId;
            Status = InterviewStatus.Scheduled;
            _transcripts = new List<InterviewTranscript>();
            _insights = new List<Insight>();
        }

        public void UpdateDetails(string title, string description)
        {
            if (Status != InterviewStatus.Scheduled)
            {
                throw new InvalidInterviewStatusTransitionException(Status, Status);
            }

            SetTitle(title);
            SetDescription(description);
        }

        public void UpdateSchedule(DateTime scheduledDate)
        {
            if (Status == InterviewStatus.Completed || Status == InterviewStatus.Cancelled)
            {
                throw new InvalidInterviewStatusTransitionException(Status, InterviewStatus.Rescheduled);
            }

            SetScheduledDate(scheduledDate);
            Status = InterviewStatus.Rescheduled;
        }

        public void StartInterview(DateTime startTime)
        {
            if (Status != InterviewStatus.Scheduled && Status != InterviewStatus.Rescheduled)
            {
                throw new InvalidInterviewStatusTransitionException(Status, InterviewStatus.InProgress);
            }

            Status = InterviewStatus.InProgress;
        }

        public void CompleteInterview(DateTime completedTime)
        {
            if (Status != InterviewStatus.InProgress)
            {
                throw new InvalidInterviewStatusTransitionException(Status, InterviewStatus.Completed);
            }

            Status = InterviewStatus.Completed;
            CompletedDate = completedTime;

            AddDistributedEvent(new CustomerInterviewCompletedEto
            {
                Id = Id,
                Title = Title,
                Type = Type,
                CompletedDate = completedTime,
                InsightCount = _insights.Count,
                TenantId = TenantId
            });
        }

        public void CancelInterview()
        {
            if (Status == InterviewStatus.Completed)
            {
                throw new InvalidInterviewStatusTransitionException(Status, InterviewStatus.Cancelled);
            }

            Status = InterviewStatus.Cancelled;
        }

        public void AddTranscript(string content, DateTime createdAt, string createdBy)
        {
            if (Status != InterviewStatus.InProgress && Status != InterviewStatus.Completed)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:InvalidOperation",
                    "Transcripts can only be added during or after the interview");
            }

            var transcript = new InterviewTranscript(
                GuidGenerator.Create(),
                Id,
                content,
                createdAt,
                createdBy);

            _transcripts.Add(transcript);
        }

        public void UpdateTranscript(Guid transcriptId, string content)
        {
            var transcript = _transcripts.FirstOrDefault(t => t.Id == transcriptId);
            if (transcript == null)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:EntityNotFound",
                    "Transcript not found");
            }

            transcript.UpdateContent(content);
        }

        public void AddInsight(
            string description,
            string category,
            int importance,
            DateTime extractedAt,
            string extractedBy)
        {
            if (Status != InterviewStatus.Completed)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:InvalidOperation",
                    "Insights can only be added after the interview is completed");
            }

            var insight = new Insight(
                GuidGenerator.Create(),
                Id,
                description,
                category,
                importance,
                extractedAt,
                extractedBy);

            _insights.Add(insight);
        }

        public void UpdateInsight(Guid insightId, string description, int importance)
        {
            var insight = _insights.FirstOrDefault(i => i.Id == insightId);
            if (insight == null)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:EntityNotFound",
                    "Insight not found");
            }

            insight.UpdateDescription(description);
            insight.UpdateImportance(importance);
        }

        public void RemoveInsight(Guid insightId)
        {
            _insights.RemoveAll(i => i.Id == insightId);
        }

        public void AddTagToInsight(Guid insightId, FeedbackTag tag)
        {
            var insight = _insights.FirstOrDefault(i => i.Id == insightId);
            if (insight == null)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:EntityNotFound",
                    "Insight not found");
            }

            insight.AddTag(tag);
        }

        public int GetHighImportanceInsightCount()
        {
            return _insights.Count(i => i.Importance >= 4);
        }

        public List<string> GetUniqueCategories()
        {
            return _insights.Select(i => i.Category).Distinct().ToList();
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: ProductDiscoveryConsts.MaxInterviewTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ProductDiscoveryConsts.MaxInterviewDescriptionLength
            );
        }

        private void SetScheduledDate(DateTime scheduledDate)
        {
            if (scheduledDate <= DateTime.UtcNow.AddMinutes(-30))
            {
                throw new BusinessException("ProductDiscovery:Validation:FutureDate",
                    "Scheduled date must be in the future");
            }

            ScheduledDate = scheduledDate;
        }

        private void SetInterviewerName(string interviewerName)
        {
            InterviewerName = Check.NotNullOrWhiteSpace(
                interviewerName,
                nameof(interviewerName),
                maxLength: 100
            );
        }

        private void SetCustomerName(string customerName)
        {
            CustomerName = Check.NotNullOrWhiteSpace(
                customerName,
                nameof(customerName),
                maxLength: 100
            );
        }

        private void SetCustomerEmail(string customerEmail)
        {
            CustomerEmail = Check.NotNullOrWhiteSpace(
                customerEmail,
                nameof(customerEmail),
                maxLength: 255
            );
        }
    }
}
