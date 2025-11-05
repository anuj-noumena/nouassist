## ResourceAggregate Acceptance Criteria

### Resource Creation
**AC-R001**: Given a user creates a new resource, when the resource is created, then it should have a unique ResourceId, Draft status, and empty content.

**AC-R002**: Given a user creates a resource, when no title is provided, then the creation should fail with a validation error.

### Content Management
**AC-R003**: Given a resource in Draft or Published status, when content is updated, then the ResourceContent should reflect the changes and ResourceContentUpdatedEto event should be published.

**AC-R004**: Given a resource in Archived status, when attempting to update content, then the operation should fail with a business rule violation.

**AC-R005**: Given a resource in Deleted status, when attempting to update content, then the operation should fail with a business rule violation.

### Publishing
**AC-R006**: Given a resource with content and valid title, when publish() is called, then the status should change to Published and ResourcePublishedEto event should be emitted.

**AC-R007**: Given a resource without content, when publish() is called, then the operation should fail with "cannot publish without content" error.

**AC-R008**: Given a resource in Deleted status, when publish() is called, then the operation should fail.

### Status Transitions
**AC-R009**: Given a resource in Draft status, when transitioning to Published, then the transition should succeed.

**AC-R010**: Given a resource in Published status, when transitioning to Archived, then the transition should succeed.

**AC-R011**: Given a resource in any status, when transitioning to Deleted, then the transition should succeed.

**AC-R012**: Given a resource in Archived status, when attempting to transition to Draft, then the operation should fail.

### Tag Management
**AC-R013**: Given a resource in Draft or Published status, when a valid tag is assigned to it, then a ResourceTaggedEntity record should be created, and a ResourceTaggedEto event should be emitted.

**AC-R014**: Given a resource with an existing tag, when attempting to add the same tag again, then the operation should fail with "duplicate tag" error.

**AC-R015**: Given a resource in Archived status, when attempting to add a tag, then the operation should fail.

**AC-R016**: Given a resource with an existing tag, when that tag is removed from it, then the corresponding ResourceTaggedEntity record should be deleted, and a ResourceUntaggedEto event should be emitted.

**AC-R017**: Given a resource, when attempting to remove a non-existent tag, then the operation should fail gracefully.

---

## Attachment Aggregate Acceptance Criteria

### Upload Lifecycle
**AC-A001**: Given a new attachment is created, when initialized, then it should have Pending status and a unique AttachmentId.

**AC-A002**: Given an attachment in Pending status, when upload starts, then the status should transition to Uploading.

**AC-A003**: Given an attachment in non-Pending status, when attempting to start upload, then the operation should fail.

### Upload Completion
**AC-A004**: Given an attachment in Uploading status, when completeUpload() is called with valid CloudStorageDetails, then the status should change to Available and AttachmentUploadedEto event should be emitted.

**AC-A005**: Given an attachment in non-Uploading status, when completeUpload() is called, then the operation should fail.

**AC-A006**: Given an attachment in Uploading status, when upload fails, then the status should change to Failed.

### File Validation
**AC-A007**: Given a file upload request, when FileSize is zero or negative, then the validation should fail.

**AC-A008**: Given a file upload request, when FileSize exceeds system maximum, then the validation should fail.

**AC-A009**: Given an attachment with EntityId, when EntityType is not provided, then the validation should fail.

**AC-A010**: Given an attachment with EntityType, when EntityId is not provided, then the validation should fail.

### Archival and Deletion
**AC-A011**: Given an attachment in Available status, when archive() is called, then the status should change to Archived.

**AC-A012**: Given an attachment in non-Available status, when archive() is called, then the operation should fail.

**AC-A013**: Given an attachment in Available status, when deleted, then AttachmentDeletedEto event should be emitted.

---

## Tag Aggregate Acceptance Criteria

### Tag Creation
**AC-T001**: Given a user creates a new tag, when the tag is created, then it should have a unique TagId, Active status, and zero UsageCount.

**AC-T002**: Given a tag name that already exists in the tenant, when creating a new tag, then the operation should fail with "tag name must be unique" error.

**AC-T003**: Given an empty or whitespace tag name, when creating a tag, then the validation should fail.

### Entity Tagging
**AC-T004**: Given an active tag and a valid target entity, when an entity tagging request is successfully processed by the system, then a new TaggedEntity record (e.g., ResourceTaggedEntity) should be created. An EntityTaggedEto event should be emitted, which will subsequently trigger the Tag's UsageCount to increment (via an event handler).

**AC-T005**: Given an entity already tagged with a specific tag, when attempting to tag with the same tag again, then the attempting to tag should fail with "entity cannot be tagged more than once with the same tag" error.

**AC-T006**: Given an archived tag, when an entity tagging request is made using an archived tag, then the operation should fail.

### Entity Untagging
**AC-T007**: Given a successfully tagged entity, when an entity untagging request is processed, then the corresponding TaggedEntity record should be removed. An EntityUntaggedEto event should be emitted, which will subsequently trigger the Tag's UsageCount to decrement (via an event handler).

**AC-T008**: Given an entity not tagged with a specific tag, when attempting to untag, then the operation should handle gracefully without error.

### Tag Lifecycle
**AC-T009**: Given an active tag, when updating name or color, then the changes should be applied and TagUpdatedEto event should be emitted.

**AC-T010**: Given an archived tag, when attempting to update, then the operation should fail.

**AC-T011**: Given a tag with UsageCount > 0, when attempting hard delete, then the operation should fail with "tag in use cannot be deleted" error.

**AC-T012**: Given a tag with UsageCount = 0, when attempting hard delete, then the deletion should succeed.

### Tag Uniqueness
**AC-T013**: Given two tags with names differing only in case (e.g., "Important" vs "important"), when creating the second tag, then the operation should fail due to case-insensitive uniqueness rule.

**AC-T014**: Given tags in different tenants, when they have the same name, then both should be allowed to exist (tenant isolation).
