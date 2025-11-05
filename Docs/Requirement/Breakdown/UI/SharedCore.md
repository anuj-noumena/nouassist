### UI Breakdowns: Shared Core Feature Slices

**Preamble:**

This document details the UI/UX design for feature slices related to the **Shared Core** domain. These components are designed to be reusable and provide a consistent experience for foundational concepts like tagging, attachments, and generic resources across the entire application. The implementation assumes an AvaloniaUI framework, leveraging standard and Eremex controls for optimal performance and user experience. ADHD considerations are integrated to ensure clarity, focus, and ease of use.

---

### 1. `TagManagement` Slice

This slice is broken into two key user experiences: the act of tagging an entity and the administrative management of all tags in the system.

#### 1.1 `EntityTagging` Component

**User Story:**

- "As a user, I want to apply existing tags or create new ones for any item (like a task, project, or document), so that I can classify and organize my work for easier filtering and retrieval."

**Overall Layout & Structure:**

- **Container:** A reusable `UserControl` (`EntityTaggingView.axaml`) designed to be embedded within other views (e.g., `TaskDetailsView`, `ResourceDetails`).
- **Organization:**
    - A `WrapPanel` to display a list of currently applied tags. Each tag is a `Border` or `Chip` control.
    - An "Add Tag" `TextBox` or `Button` that invokes a popup for tag selection/creation.
    - **Enhanced:** A type-ahead/autocomplete `TextBox` for inline tag addition, combining search and creation.
- **Primary Action(s):** Viewing applied tags, adding new or existing tags to an entity, removing tags from an entity.

**Component Breakdown & Interactions:**

- **Applied Tags Display:**
    - **UI Elements:** A `WrapPanel` containing an `ItemsControl` bound to the entity's tags. The `ItemTemplate` is a custom `Chip`-styled control (e.g., a `Border` with a `TextBlock` and a close `Button`). The `TagColor` property from the domain model is used for the background.
    - **User Interaction:** Users see a colorful, easy-to-scan list of tags. Clicking the 'x' icon on a chip immediately removes the tag from the entity, triggering a `ResourceUntaggedEto` or `EntityUntaggedEto` event. The `UsageCount` on the `Tag` aggregate would be decremented.
    - **ADHD Considerations:**
        - **Enhanced:** Animate the removal of a tag chip to provide smooth visual feedback.
        - Use color consistently based on the `TagColor` to aid in quick visual identification.
        - Ensure sufficient spacing between chips to prevent a cluttered appearance.

- **Add Tag Input/Popup:**
    - **UI Elements:** An `AutoCompleteBox` provides suggestions from existing tags as the user types. A "Create new tag" option appears if no match is found. Clicking this opens a small popup `Window` with fields for `TagName` and `TagColor`.
    - **User Interaction:** The user types a tag name. If it exists, they can select it to apply it. If not, they can trigger the creation flow. The business rule that a tag name must be unique within a tenant is enforced. A `ResourceTaggedEto` or `EntityTaggedEto` is published on success.
    - **ADHD Considerations:**
        - **Enhanced:** The suggestion list should highlight matching characters in the search results.
        - Keep the creation popup minimal, focusing only on the essential `Name` and `Color`.
        - The process should be keyboard-navigable (type, arrow down, enter to select; or type, enter to create).

---

#### 1.2 `TagAdministration` View

**User Story:**

- "As an administrator, I want to see all tags used in the system, edit their properties (like name and color), merge duplicates, and archive unused tags, so that I can maintain a clean and consistent classification system."

**Overall Layout & Structure:**

- **Container:** A dedicated `UserControl` (`TagAdminView.axaml`) within an administrative section of the application.
- **Organization:**
    - **Toolbar (`DockPanel`):** Search `TextBox` and a `Button` to create a new tag manually.
    - **Main Content:** An **`Eremex GridControl`** displaying all tags, with columns for Name, Color, Status, and Usage Count.
    - **Enhanced:** A detail panel or modal dialog for merging tags, showing a preview of the impact.
- **Primary Action(s):** Viewing all tags, searching, editing, merging, and archiving tags.

**Component Breakdown & Interactions:**

- **Tag Grid (`Eremex GridControl`):**
    - **UI Elements:** A grid with columns for `TagName`, `TagColor` (using a colored cell template), `TagStatus`, and `UsageCount`. Inline editing is enabled for the name and color.
    - **User Interaction:** The user can sort and filter the grid. They can edit a tag's name or color directly in the grid. A right-click context menu provides "Archive" and "Merge" actions. The business rule preventing the hard-deletion of a tag with `UsageCount > 0` is enforced with a disabled menu item and a tooltip explaining why.
    - **ADHD Considerations:**
        - **Enhanced:** Use data bars in the `UsageCount` column for quick visual comparison.
        - Clearly distinguish archived tags using a different row style (e.g., grayed out, strikethrough).
        - Ensure inline editing provides immediate and clear validation feedback (e.g., a red border if a duplicate name is entered).

- **Merge Tags Dialog:**
    - **UI Elements:** A `Window` that appears after selecting two or more tags and choosing "Merge." It has a "Merge into" selector and shows a summary of how many items will be re-tagged.
    - **User Interaction:** The user selects the primary tag to keep. Upon confirmation, the `TagDomainService` handles the logic of re-assigning entities and archiving the source tags.
    - **ADHD Considerations:**
        - **Enhanced:** Provide a clear, visual summary of the merge operation before confirmation (e.g., "Merging 'Tasking' (3 uses) into 'Tasks' (12 uses) will result in 'Tasks' (15 uses)").
        - The process should be undoable for a short period to prevent accidental data changes.

---

### 2. `AttachmentManagement` Slice (Reusable Component)

**User Story:**

- "As a user, I want to upload one or more files to an item, see the upload progress, manage the attached files, and preview them if possible, so that I can associate relevant documents and images with my work."

**Overall Layout & Structure:**

- **Container:** A reusable `UserControl` (`AttachmentView.axaml`) designed to be embedded in any view that supports attachments (e.g., `TaskDetailsView`, `ProjectDetailsView`).
- **Organization:**
    - **Upload Area:** A `Button` or a drag-and-drop enabled `Border` to initiate file uploads.
    - **File List:** An `ItemsControl` displaying the list of `Attachment` aggregates.
    - **Enhanced:** An upload queue panel that appears during active uploads, showing individual file progress.
- **Primary Action(s):** Uploading new files, viewing the list of attached files, downloading files, removing attachments.

**Component Breakdown & Interactions:**

- **Upload Area & Process:**
    - **UI Elements:** A `Border` with dashed styling and an icon to indicate a drop target. A `Button` provides an alternative click-to-open file dialog. During upload, a progress bar appears.
    - **User Interaction:** The user drags files onto the drop zone or selects them via the file dialog. The `AttachmentDomainService` validates the upload against the `File Upload Policy` (size, type, quota). The `Attachment` aggregate's status transitions from `Pending` -> `Uploading` -> `Available`/`Failed`. An `AttachmentUploadedEto` is published on success.
    - **ADHD Considerations:**
        - **Enhanced:** Provide instant visual feedback when a file is dragged over the drop zone (e.g., change of border color).
        - Show individual progress bars for multi-file uploads to give a clear sense of progress and avoid the feeling of a stalled UI.
        - Use distinct visual cues (e.g., green checkmark, red error icon) to indicate the final status of each upload.

- **Attachment List:**
    - **UI Elements:** An `ItemsControl` where each item template includes a file type icon, `OriginalFileName`, `FileSize`, and action buttons for "Download" and "Delete".
    - **User Interaction:** The user can view all associated files. Clicking "Download" initiates a file download. Clicking "Delete" shows a confirmation dialog and then archives or permanently deletes the attachment, publishing an `AttachmentDeletedEto`.
    - **ADHD Considerations:**
        - **Enhanced:** Implement a preview feature where clicking a file (if it's an image or PDF) opens a quick-look `Window` instead of immediately downloading.
        - Group attachments by `AttachmentType` (e.g., Images, Documents) to create structure in long lists.
        - Keep the layout for each file entry clean and uncluttered, with generous spacing.

---

### 3. `ResourceEditor` Slice

**User Story:**

- "As a content manager, I want a flexible editor to create and manage generic content resources, composing them from notes, links, and structured data, so that I can build a knowledge base or reference library."

**Overall Layout & Structure:**

- **Container:** A `UserControl` (`ResourceEditorView.axaml`) used for creating and editing `ResourceAggregate` instances.
- **Organization:**
    - **Toolbar:** `TextBox` for the `Title`, a `ComboBox` for `ResourceType`, and action buttons (`Publish`, `Save Draft`, `Archive`).
    - **Content Area:** A dynamic area where the user can add and edit different content parts (`NoteItem`, `LinkItem`, `JsonDataItem`, etc.) from the `ResourceContent` value object.
    - **Side Panel:** A panel for managing metadata, including tags (using the `EntityTagging` component) and attachments (using the `AttachmentManagement` component).
- **Primary Action(s):** Creating/editing a resource's title and content, publishing, archiving, and managing associated tags and attachments.

**Component Breakdown & Interactions:**

- **Toolbar and Lifecycle Actions:**
    - **UI Elements:** Standard form controls and `Button`s. The `Publish` button is disabled if validation fails.
    - **User Interaction:** The user sets the title and type. Clicking "Publish" triggers the `ResourceDomainService` to validate the resource against the `Resource Publishing Policy` (e.g., cannot be published without content). The action changes the `ResourceStatus` and publishes corresponding events like `ResourcePublishedEto`.
    - **ADHD Considerations:**
        - **Enhanced:** Provide a "quality analysis" score or checklist from the `ResourceDomainService` that shows what's needed for publishing.
        - Use clear visual states for the resource `Status` (e.g., a "Draft" or "Published" badge near the title).
        - Ensure destructive actions like "Archive" have clear confirmation dialogs.

- **Content Editor:**
    - **UI Elements:** A dynamic `ItemsControl` where the user can add different types of content blocks. A `NoteItem` uses a Markdown editor. A `LinkItem` has fields for URL and Title. An `AttachmentItem` allows picking from already uploaded attachments.
    - **User Interaction:** The user builds the resource by adding and arranging different content blocks. The entire `ResourceContent` object is saved as a single JSONB column for efficiency. Changes are saved by publishing a `ResourceContentUpdatedEto`.
    - **ADHD Considerations:**
        - **Enhanced:** Allow content blocks to be reordered via drag-and-drop to make composition more intuitive.
        - Use an `Expander` for each content block to allow users to collapse parts they aren't working on, reducing visual clutter.
        - Implement an auto-save feature to a draft state to prevent data loss.