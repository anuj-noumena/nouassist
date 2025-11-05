# Shared Core Bounded Context

-   **Responsibility**: Manages the lifecycle and rules for shared, cross-cutting domain concepts like resources, files, reminders, and notifications. It provides a common language and model for fundamental entities used across multiple bounded contexts, ensuring consistency and reusability.
    
-   **Subdomains:** Resource Management, Attachment Management, Tag Management, **Reminder Management**, **Notification Management**
    
-   **Aggregates (AR = Aggregate Root):**
    
    -   **`ResourceAggregate` (AR):** Acts as a generic, content-agnostic container for information.
        
        -   _Entities:_ None.
            
        -   _Value Objects:_  `ResourceId`, `ResourceTitle`, `ResourceContent` (Notes, Attachments, Links, JSON Data, References), `ResourceStatus` (Draft, Published, Archived), `ResourceType`.
            
    -   **`AttachmentAggregate` (AR):** Manages the complete lifecycle of a file, from upload to cloud storage.
        
        -   _Entities:_ None.
            
        -   _Value Objects:_  `AttachmentId`, `OriginalFileName`, `ContentType`, `FileSizeBytes`, `AttachmentStatus` (Pending, Available, Archived), `CloudStorageDetails`.
            
    -   **`TagAggregate` (AR):** Manages the definition of a tag and tracks its usage.
        
        -   _Entities:_  `TaggedEntityLink` (represents the link between the tag and a specific entity).
            
        -   _Value Objects:_  `TagId`, `TagName`, `TagColor`, `TagStatus` (Active, Archived).
            
    -   **`ReminderAggregate` (AR):** Represents a scheduled prompt for a specific entity and user.
        
        -   _Entities:_  `ReminderDeliveryAttempt` (tracks attempts to send the reminder).
            
        -   _Value Objects:_  `ReminderId`, `TargetEntityId`, `TargetEntityType`, `RecipientUserId`, `ReminderTime` (when it should trigger), `DeliveryChannel` (e.g., In-App, Email, Push), `MessagePayload` (content of the reminder), `ReminderStatus` (Pending, Sent, Failed, Snoozed), `SnoozeUntil` (nullable timestamp).
            
    -   **`NotificationAggregate` (AR):** Represents a message delivered to a user as a result of a system event.
        
        -   _Entities:_ None.
            
        -   _Value Objects:_  `NotificationId`, `RecipientUserId`, `Message`, `SourceEventId` (optional, what triggered it), `SourceEntityId`, `SourceEntityType`, `NotificationStatus` (Unread, Read, Archived), `Timestamp`, `NotificationType` (Informational, Warning, ActionRequired), `ActionUrl`.
            
-   **Commands:**
    
    -   `CreateResource`, `UpdateResourceContent`, `PublishResource`, `ArchiveResource`, `TagResource`, `UntagResource`, `CreateTag`, `ArchiveTag`, `CompleteAttachmentUpload`, `ArchiveAttachment`, **`ScheduleReminder`**, **`SnoozeReminder`**, **`CancelReminder`**, **`AcknowledgeReminder`**, **`CreateNotificationForUser`**, **`MarkNotificationAsRead`**, `MarkAllNotificationsAsRead`, `ArchiveNotification`.
        
-   **Queries:**
    
    -   `GetResourceById`, `GetResourcesByTag`, `GetAttachmentById`, `GetTagByName`, `ListTags`, **`GetReminderById`**, **`GetRemindersForEntity`**, **`GetRemindersForUser`**, **`GetDueReminders`** (internal for service), **`GetNotificationById`**, **`GetNotificationsForUser`**, **`GetUnreadNotificationCountForUser`**.
        
-   **Events:**
    
    -   `ResourceCreated`, `ResourcePublished`, `ResourceArchived`, `EntityTagged`, `EntityUntagged`, `AttachmentUploaded`, `AttachmentArchived`, `TagCreated`, **`ReminderScheduled`**, **`ReminderTriggered`**, **`ReminderSnoozed`**, **`ReminderCancelled`**, **`ReminderDeliveryFailed`**, **`NotificationGenerated`**, **`NotificationRead`**, **`NotificationArchived`**.
        
-   **Event Subscribers/Policies:**
    
    -   _Policy:_ A reminder must be scheduled for a future time.
        
    -   _Policy:_ An archived tag cannot be applied to an entity.
        
    -   _Policy:_ A notification must have a valid `RecipientUserId`.
        
    -   _Subscriber (Internal):_ A background job (`ReminderService`) periodically runs `GetDueReminders` and publishes a `ReminderTriggered` event for each due reminder.
        
    -   _Subscriber (Internal):_ On `ReminderTriggered`, a handler issues the `CreateNotificationForUser` command to generate the actual notification for the user.
        
    -   _Subscriber (External):_ Listens for events from other Bounded Contexts (e.g., `TaskAssigned`, `ProjectInvitationAccepted`, `CommentMentionedUser`) and issues `CreateNotificationForUser` commands. This is a primary integration point.
        
    -   _Subscriber (External):_ Publishes `NotificationGenerated` event, which could be consumed by a real-time service (e.g., SignalR, WebSockets) to push updates to the client UI. It could also trigger sending emails or push notifications via external gateways.