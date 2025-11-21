using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using NouAssist.BusinessOperations.FreelanceOperations.Events;
using NouAssist.BusinessOperations.FreelanceOperations.Exceptions;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.FreelanceOperations.Aggregates
{
    /// <summary>
    /// Aggregate root representing a freelance project
    /// </summary>
    public class FreelanceProject : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Current status of the project
        /// </summary>
        public ProjectStatus Status { get; private set; }
        
        /// <summary>
        /// ID of the client for this project
        /// </summary>
        public Guid ClientId { get; private set; }
        
        /// <summary>
        /// Type of billing for this project
        /// </summary>
        public BillingType BillingType { get; private set; }
        
        /// <summary>
        /// Hourly rate if applicable
        /// </summary>
        public HourlyRate HourlyRate { get; private set; }
        
        /// <summary>
        /// Fixed price amount if applicable
        /// </summary>
        public decimal? FixedPrice { get; private set; }
        
        /// <summary>
        /// Currency for fixed price
        /// </summary>
        public string Currency { get; private set; }
        
        /// <summary>
        /// Detailed description of the project scope
        /// </summary>
        public ProjectScopeDescription Scope { get; private set; }
        
        /// <summary>
        /// Start date of the project
        /// </summary>
        public DateTime? StartDate { get; private set; }
        
        /// <summary>
        /// Expected end date of the project
        /// </summary>
        public DateTime? EndDate { get; private set; }
        
        /// <summary>
        /// Actual completion date of the project
        /// </summary>
        public DateTime? CompletionDate { get; private set; }
        
        /// <summary>
        /// ID of the proposal that was accepted for this project
        /// </summary>
        public Guid? AcceptedProposalId { get; private set; }
        
        /// <summary>
        /// Collection of time entries for this project
        /// </summary>
        public IReadOnlyCollection<TimeEntry> TimeEntries => _timeEntries.AsReadOnly();
        private readonly List<TimeEntry> _timeEntries;
        
        /// <summary>
        /// Collection of milestones for this project
        /// </summary>
        public IReadOnlyCollection<ProjectMilestone> Milestones => _milestones.AsReadOnly();
        private readonly List<ProjectMilestone> _milestones;

        protected FreelanceProject()
        {
            _timeEntries = new List<TimeEntry>();
            _milestones = new List<ProjectMilestone>();
        }

        /// <summary>
        /// Creates a new freelance project with hourly billing
        /// </summary>
        public static FreelanceProject CreateHourlyProject(
            Guid id,
            string name,
            Guid clientId,
            HourlyRate hourlyRate,
            ProjectScopeDescription scope,
            DateTime? startDate = null,
            DateTime? endDate = null,
            Guid? tenantId = null)
        {
            return new FreelanceProject(
                id,
                name,
                clientId,
                BillingType.Hourly,
                scope,
                startDate,
                endDate,
                tenantId)
            {
                HourlyRate = hourlyRate ?? throw new ArgumentNullException(nameof(hourlyRate))
            };
        }

        /// <summary>
        /// Creates a new freelance project with fixed price billing
        /// </summary>
        public static FreelanceProject CreateFixedPriceProject(
            Guid id,
            string name,
            Guid clientId,
            decimal fixedPrice,
            string currency,
            ProjectScopeDescription scope,
            DateTime? startDate = null,
            DateTime? endDate = null,
            Guid? tenantId = null)
        {
            if (fixedPrice < 0)
            {
                throw new BusinessException("FreelanceOperations:Validation:InvalidValue", "Fixed price");
            }
            
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new BusinessException("FreelanceOperations:Validation:Required", "Currency");
            }
            
            return new FreelanceProject(
                id,
                name,
                clientId,
                BillingType.FixedPrice,
                scope,
                startDate,
                endDate,
                tenantId)
            {
                FixedPrice = fixedPrice,
                Currency = currency.ToUpperInvariant()
            };
        }

        /// <summary>
        /// Creates a new freelance project with milestone-based billing
        /// </summary>
        public static FreelanceProject CreateMilestoneProject(
            Guid id,
            string name,
            Guid clientId,
            ProjectScopeDescription scope,
            DateTime? startDate = null,
            DateTime? endDate = null,
            Guid? tenantId = null)
        {
            return new FreelanceProject(
                id,
                name,
                clientId,
                BillingType.Milestone,
                scope,
                startDate,
                endDate,
                tenantId);
        }

        private FreelanceProject(
            Guid id,
            string name,
            Guid clientId,
            BillingType billingType,
            ProjectScopeDescription scope,
            DateTime? startDate = null,
            DateTime? endDate = null,
            Guid? tenantId = null) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: FreelanceOperationsConsts.Project.MaxNameLength
            );
            
            ClientId = clientId;
            BillingType = billingType;
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            StartDate = startDate;
            EndDate = endDate;
            TenantId = tenantId;
            Status = ProjectStatus.Draft;
            _timeEntries = new List<TimeEntry>();
            _milestones = new List<ProjectMilestone>();
            
            AddDistributedEvent(new FreelanceProjectCreatedEto
            {
                Id = Id,
                Name = Name,
                ClientId = ClientId,
                Status = Status,
                BillingType = BillingType,
                StartDate = StartDate,
                EndDate = EndDate,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Update the basic properties of this project
        /// </summary>
        public void UpdateBasicDetails(
            string name,
            ProjectScopeDescription scope,
            DateTime? startDate,
            DateTime? endDate)
        {
            // Status validation
            if (Status == ProjectStatus.Completed || Status == ProjectStatus.Canceled)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: FreelanceOperationsConsts.Project.MaxNameLength
            );
            
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            StartDate = startDate;
            EndDate = endDate;
            
            AddDistributedEvent(new FreelanceProjectUpdatedEto
            {
                Id = Id,
                Name = Name,
                ClientId = ClientId,
                Status = Status,
                BillingType = BillingType,
                UpdatedAt = DateTime.UtcNow,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Update hourly rate for hourly billing projects
        /// </summary>
        public void UpdateHourlyRate(HourlyRate hourlyRate)
        {
            if (BillingType != BillingType.Hourly)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            if (Status == ProjectStatus.Completed || Status == ProjectStatus.Canceled)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            HourlyRate = hourlyRate ?? throw new ArgumentNullException(nameof(hourlyRate));
        }

        /// <summary>
        /// Update fixed price for fixed-price projects
        /// </summary>
        public void UpdateFixedPrice(decimal fixedPrice, string currency)
        {
            if (BillingType != BillingType.FixedPrice)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            if (Status == ProjectStatus.Completed || Status == ProjectStatus.Canceled)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            if (fixedPrice < 0)
            {
                throw new BusinessException("FreelanceOperations:Validation:InvalidValue", "Fixed price");
            }
            
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new BusinessException("FreelanceOperations:Validation:Required", "Currency");
            }
            
            FixedPrice = fixedPrice;
            Currency = currency.ToUpperInvariant();
        }

        /// <summary>
        /// Change the status of this project
        /// </summary>
        public void ChangeStatus(ProjectStatus newStatus, DateTime statusChangeDate)
        {
            // Validate status transition
            ValidateStatusTransition(newStatus);
            
            Status = newStatus;
            
            // Set completion date if project is completed
            if (newStatus == ProjectStatus.Completed)
            {
                CompletionDate = statusChangeDate;
            }
            else
            {
                CompletionDate = null;
            }
            
            AddDistributedEvent(new FreelanceProjectUpdatedEto
            {
                Id = Id,
                Name = Name,
                ClientId = ClientId,
                Status = Status,
                BillingType = BillingType,
                UpdatedAt = statusChangeDate,
                TenantId = TenantId
            });
        }

        private void ValidateStatusTransition(ProjectStatus newStatus)
        {
            // Define valid transitions based on current status
            var invalidTransition = false;
            
            switch (Status)
            {
                case ProjectStatus.Draft:
                    // Draft can transition to Proposed, Active, Canceled, or Archived
                    invalidTransition = newStatus != ProjectStatus.Proposed && 
                                       newStatus != ProjectStatus.Active && 
                                       newStatus != ProjectStatus.Canceled && 
                                       newStatus != ProjectStatus.Archived;
                    break;
                case ProjectStatus.Proposed:
                    // Proposed can transition to Active, Canceled, or Archived
                    invalidTransition = newStatus != ProjectStatus.Active && 
                                       newStatus != ProjectStatus.Canceled && 
                                       newStatus != ProjectStatus.Archived;
                    break;
                case ProjectStatus.Active:
                    // Active can transition to OnHold, Completed, Canceled, or Archived
                    invalidTransition = newStatus != ProjectStatus.OnHold && 
                                       newStatus != ProjectStatus.Completed && 
                                       newStatus != ProjectStatus.Canceled && 
                                       newStatus != ProjectStatus.Archived;
                    break;
                case ProjectStatus.OnHold:
                    // OnHold can transition to Active, Completed, Canceled, or Archived
                    invalidTransition = newStatus != ProjectStatus.Active && 
                                       newStatus != ProjectStatus.Completed && 
                                       newStatus != ProjectStatus.Canceled && 
                                       newStatus != ProjectStatus.Archived;
                    break;
                case ProjectStatus.Completed:
                    // Completed can only transition to Archived
                    invalidTransition = newStatus != ProjectStatus.Archived;
                    break;
                case ProjectStatus.Canceled:
                    // Canceled can only transition to Archived
                    invalidTransition = newStatus != ProjectStatus.Archived;
                    break;
                case ProjectStatus.Archived:
                    // Archived is terminal state
                    invalidTransition = true;
                    break;
            }
            
            if (invalidTransition)
            {
                throw new InvalidStatusTransitionException(
                    "Project", 
                    Status.ToString(), 
                    newStatus.ToString()
                );
            }
        }

        /// <summary>
        /// Link this project to an accepted proposal
        /// </summary>
        public void LinkToProposal(Guid proposalId)
        {
            AcceptedProposalId = proposalId;
            
            // If project is still in draft, transition to proposed status
            if (Status == ProjectStatus.Draft)
            {
                Status = ProjectStatus.Proposed;
            }
        }

        /// <summary>
        /// Add a time entry to this project
        /// </summary>
        public TimeEntry AddTimeEntry(
            DateTime date,
            TimeSpan startTime,
            int durationMinutes,
            string description,
            bool isBillable = true)
        {
            if (Status == ProjectStatus.Completed || Status == ProjectStatus.Canceled || Status == ProjectStatus.Archived)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            var timeEntry = new TimeEntry(
                Id, 
                date, 
                startTime, 
                durationMinutes, 
                description, 
                isBillable
            );
            
            _timeEntries.Add(timeEntry);
            
            return timeEntry;
        }

        /// <summary>
        /// Update an existing time entry
        /// </summary>
        public void UpdateTimeEntry(
            DateTime date,
            TimeSpan startTime,
            int newDurationMinutes,
            string newDescription)
        {
            var timeEntry = GetTimeEntry(date, startTime);
            
            if (newDurationMinutes > 0)
            {
                timeEntry.UpdateDuration(newDurationMinutes);
            }
            
            if (!string.IsNullOrWhiteSpace(newDescription))
            {
                timeEntry.UpdateDescription(newDescription);
            }
        }

        /// <summary>
        /// Remove a time entry
        /// </summary>
        public void RemoveTimeEntry(DateTime date, TimeSpan startTime)
        {
            var timeEntry = GetTimeEntry(date, startTime);
            
            if (timeEntry.IsInvoiced)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            _timeEntries.Remove(timeEntry);
        }

        /// <summary>
        /// Get a specific time entry
        /// </summary>
        private TimeEntry GetTimeEntry(DateTime date, TimeSpan startTime)
        {
            var timeEntry = _timeEntries.FirstOrDefault(t => 
                t.Date.Date == date.Date && t.StartTime == startTime);
            
            if (timeEntry == null)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:EntityNotFound", "Time entry");
            }
            
            return timeEntry;
        }

        /// <summary>
        /// Add a milestone to this project
        /// </summary>
        public ProjectMilestone AddMilestone(
            Guid milestoneId,
            MilestoneDetails details,
            DateTime? deadline = null)
        {
            if (Status == ProjectStatus.Completed || Status == ProjectStatus.Canceled || Status == ProjectStatus.Archived)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            var milestone = new ProjectMilestone(
                milestoneId,
                Id,
                details,
                deadline
            );
            
            _milestones.Add(milestone);
            
            return milestone;
        }

        /// <summary>
        /// Update an existing milestone
        /// </summary>
        public void UpdateMilestone(
            Guid milestoneId,
            MilestoneDetails details,
            DateTime? deadline = null)
        {
            var milestone = GetMilestone(milestoneId);
            
            if (details != null)
            {
                milestone.UpdateDetails(details);
            }
            
            milestone.UpdateDeadline(deadline);
        }

        /// <summary>
        /// Complete a milestone
        /// </summary>
        public void CompleteMilestone(Guid milestoneId, DateTime completedDate)
        {
            var milestone = GetMilestone(milestoneId);
            milestone.MarkAsCompleted(completedDate);
        }

        /// <summary>
        /// Reopen a milestone
        /// </summary>
        public void ReopenMilestone(Guid milestoneId)
        {
            var milestone = GetMilestone(milestoneId);
            milestone.MarkAsIncomplete();
        }

        /// <summary>
        /// Remove a milestone
        /// </summary>
        public void RemoveMilestone(Guid milestoneId)
        {
            var milestone = GetMilestone(milestoneId);
            
            if (milestone.IsInvoiced)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            _milestones.Remove(milestone);
        }

        /// <summary>
        /// Get a specific milestone
        /// </summary>
        private ProjectMilestone GetMilestone(Guid milestoneId)
        {
            var milestone = _milestones.FirstOrDefault(m => m.Id == milestoneId);
            
            if (milestone == null)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:EntityNotFound", "Milestone");
            }
            
            return milestone;
        }

        /// <summary>
        /// Calculate total hours logged for this project
        /// </summary>
        public decimal GetTotalHours()
        {
            return _timeEntries.Sum(t => t.GetDurationInHours());
        }

        /// <summary>
        /// Calculate total billable hours logged for this project
        /// </summary>
        public decimal GetTotalBillableHours()
        {
            return _timeEntries
                .Where(t => t.IsBillable)
                .Sum(t => t.GetDurationInHours());
        }

        /// <summary>
        /// Calculate total amount due for project based on billing type
        /// </summary>
        public decimal CalculateTotalAmount()
        {
            switch (BillingType)
            {
                case BillingType.Hourly:
                    return GetTotalBillableHours() * HourlyRate.Amount;
                
                case BillingType.FixedPrice:
                    return FixedPrice ?? 0;
                
                case BillingType.Milestone:
                    return _milestones
                        .Where(m => m.IsCompleted && !m.IsInvoiced)
                        .Sum(m => m.Details.Price);
                
                default:
                    return 0;
            }
        }
    }
}
