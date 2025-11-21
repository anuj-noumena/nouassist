using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services;
using NouAssist.BusinessOperations.FreelanceOperations.Localization;
using NouAssist.BusinessOperations.FreelanceOperations.Reminders.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.BusinessOperations.FreelanceOperations.Reminders
{
    [Authorize]
    public class ReminderAppService : ApplicationService, IReminderAppService
    {
        private readonly IRemindersRepository _remindersRepository;
        private readonly IAutomatedReminderScheduler _reminderScheduler;
        private readonly ICurrentUser _currentUser;
        
        public ReminderAppService(
            IRemindersRepository remindersRepository,
            IAutomatedReminderScheduler reminderScheduler,
            ICurrentUser currentUser)
        {
            _remindersRepository = remindersRepository;
            _reminderScheduler = reminderScheduler;
            _currentUser = currentUser;
            
            LocalizationResource = typeof(FreelanceOperationsResource);
        }

        public virtual async Task<PagedResultDto<ReminderDto>> GetListAsync(GetReminderListDto input)
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    throw new BusinessException(L["FreelanceOperations:ErrorMessage:UserNotAuthenticated"]);
                }
                
                // Get reminders
                var dueReminders = await _reminderScheduler.GetDueRemindersAsync(_currentUser.Id.Value);
                
                // Apply filtering
                var filteredReminders = dueReminders;
                
                if (input.IsCompleted.HasValue)
                {
                    filteredReminders = filteredReminders.Where(r => (bool)r["isCompleted"] == input.IsCompleted.Value).ToList();
                }
                
                if (input.IsOverdue.HasValue)
                {
                    var now = DateTime.UtcNow;
                    filteredReminders = filteredReminders.Where(r => 
                        input.IsOverdue.Value == (!(bool)r["isCompleted"] && (DateTime)r["dueDate"] < now)).ToList();
                }
                
                if (!string.IsNullOrEmpty(input.EntityType))
                {
                    filteredReminders = filteredReminders.Where(r => (string)r["entityType"] == input.EntityType).ToList();
                }
                
                // Calculate total count
                var totalCount = filteredReminders.Count;
                
                // Apply sorting
                var sortedReminders = input.Sorting switch
                {
                    "dueDate" => filteredReminders.OrderBy(r => (DateTime)r["dueDate"]).ToList(),
                    "dueDate DESC" => filteredReminders.OrderByDescending(r => (DateTime)r["dueDate"]).ToList(),
                    "title" => filteredReminders.OrderBy(r => (string)r["title"]).ToList(),
                    "title DESC" => filteredReminders.OrderByDescending(r => (string)r["title"]).ToList(),
                    _ => filteredReminders.OrderBy(r => (DateTime)r["dueDate"]).ToList() // Default sort
                };
                
                // Apply paging
                var pagedReminders = sortedReminders
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();
                
                // Map to DTOs
                var reminderDtos = pagedReminders.Select(r => new ReminderDto
                {
                    Id = (Guid)r["id"],
                    Title = (string)r["title"],
                    Message = (string)r["message"],
                    DueDate = (DateTime)r["dueDate"],
                    EntityId = (Guid)r["entityId"],
                    EntityType = (string)r["entityType"],
                    IsCompleted = (bool)r["isCompleted"],
                    // Map entity types to human-readable names
                    EntityTypeName = (string)r["entityType"] switch
                    {
                        "FreelanceProject" => "Project",
                        "FreelanceClient" => "Client",
                        "ProjectMilestone" => "Milestone",
                        "Proposal" => "Proposal",
                        "ClientCommunicationRecord" => "Communication",
                        _ => (string)r["entityType"]
                    },
                    // Create links based on entity type
                    EntityLink = (string)r["entityType"] switch
                    {
                        "FreelanceProject" => $"/projects/details/{r["entityId"]}",
                        "FreelanceClient" => $"/clients/details/{r["entityId"]}",
                        "ProjectMilestone" => $"/projects/milestones/{r["entityId"]}",
                        "Proposal" => $"/proposals/details/{r["entityId"]}",
                        "ClientCommunicationRecord" => $"/clients/communications/{r["entityId"]}",
                        _ => ""
                    },
                    // Set overdue flag
                    IsOverdue = !(bool)r["isCompleted"] && (DateTime)r["dueDate"] < DateTime.UtcNow
                }).ToList();
                
                return new PagedResultDto<ReminderDto>(totalCount, reminderDtos);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetListAsync for reminders");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        public virtual async Task<ReminderDto> GetAsync(Guid id)
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    throw new BusinessException(L["FreelanceOperations:ErrorMessage:UserNotAuthenticated"]);
                }
                
                // Get all reminders and find the matching one
                var reminders = await _reminderScheduler.GetDueRemindersAsync(_currentUser.Id.Value);
                var reminder = reminders.FirstOrDefault(r => (Guid)r["id"] == id);
                
                if (reminder == null)
                {
                    throw new BusinessException(L["FreelanceOperations:ErrorMessage:ReminderNotFound"]);
                }
                
                // Map to DTO
                var reminderDto = new ReminderDto
                {
                    Id = (Guid)reminder["id"],
                    Title = (string)reminder["title"],
                    Message = (string)reminder["message"],
                    DueDate = (DateTime)reminder["dueDate"],
                    EntityId = (Guid)reminder["entityId"],
                    EntityType = (string)reminder["entityType"],
                    IsCompleted = (bool)reminder["isCompleted"],
                    // Map entity types to human-readable names
                    EntityTypeName = (string)reminder["entityType"] switch
                    {
                        "FreelanceProject" => "Project",
                        "FreelanceClient" => "Client",
                        "ProjectMilestone" => "Milestone",
                        "Proposal" => "Proposal",
                        "ClientCommunicationRecord" => "Communication",
                        _ => (string)reminder["entityType"]
                    },
                    // Create links based on entity type
                    EntityLink = (string)reminder["entityType"] switch
                    {
                        "FreelanceProject" => $"/projects/details/{reminder["entityId"]}",
                        "FreelanceClient" => $"/clients/details/{reminder["entityId"]}",
                        "ProjectMilestone" => $"/projects/milestones/{reminder["entityId"]}",
                        "Proposal" => $"/proposals/details/{reminder["entityId"]}",
                        "ClientCommunicationRecord" => $"/clients/communications/{reminder["entityId"]}",
                        _ => ""
                    },
                    // Set overdue flag
                    IsOverdue = !(bool)reminder["isCompleted"] && (DateTime)reminder["dueDate"] < DateTime.UtcNow
                };
                
                return reminderDto;
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetAsync for reminder {ReminderId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        public virtual async Task MarkAsCompletedAsync(Guid id)
        {
            try
            {
                await _reminderScheduler.MarkReminderCompletedAsync(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in MarkAsCompletedAsync for reminder {ReminderId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }
    }
}
