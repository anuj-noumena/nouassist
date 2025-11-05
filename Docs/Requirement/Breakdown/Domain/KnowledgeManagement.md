**1. Knowledge Management Core (BC)**

* **Focus:** Central knowledge item, its content, versions, metadata, categories, tags, and **relationships defining the knowledge graph structure.**
* **Subdomains:** Core (Knowledge Representation & Graph)
* **Aggregates:**
    * `KnowledgeItem` (AR): The central piece of knowledge, managing its content, versions, metadata, **and defined relationships.**
* **Entities:** `KnowledgeItem` (AR), `KnowledgeItemVersion`, `KnowledgeCategory`, `KnowledgeTag`, **`KnowledgeRelationship`** (*Entity or VO associated with KnowledgeItem, defines link type and target KnowledgeItemID*).
* **Value Objects:** `RichContent`, `Metadata`, `TagName`, `CategoryName`, `RelationshipType`, `ConceptMapData` (Exportable representation of relationships).
* **User Stories:**
    * As a knowledge author, I want to `CreateKnowledgeItem` with rich text, images, and attachments, so I can capture information comprehensively.
    * As a knowledge manager, I want to `UpdateKnowledgeItemContent` and track `KnowledgeItemVersionHistory`, so I can ensure accuracy and audit changes.
    * As a user, I want to `AddMetadataToKnowledgeItem` like source or author, so I can provide context.
    * As a knowledge curator, I want to `DeleteKnowledgeItem` when it's no longer relevant.
    * As an editor, I want to `RevertKnowledgeItemVersion` if a recent change is incorrect.
    * As a knowledge architect, I want to `CreateKnowledgeCategory` and `AssignCategoryToKnowledgeItem`, so I can structure the knowledge base.
    * As a user, I want to `CreateKnowledgeTag` and `AssignTagToKnowledgeItem`, so I can find related information easily.
    * As a knowledge modeler, I want to `DefineKnowledgeRelationship` between two knowledge items, so I can build a connected knowledge graph.
    * As a knowledge modeler, I want to `RemoveKnowledgeRelationship` if the connection is no longer valid.
    * As a researcher, I want to `GenerateConceptMap` from knowledge items and their relationships, so I can visualize connections.
    * As a user, I want to `GetKnowledgeItemById` to view its details.
    * As an analyst, I want to `ListKnowledgeItemsByCategory` or `ListKnowledgeItemsByTag` to find specific sets of information.
    * As a user, I want to `GetRelatedKnowledgeItems` to explore connected concepts.
    * As a developer, I want to `GetKnowledgeGraphStructureData` so I can build visualization tools.
    * As an analyst, I want to `ExportConceptMap` for offline review or sharing.
* **Commands:** `CreateKnowledgeItem`, `UpdateKnowledgeItemContent`, `AddMetadataToKnowledgeItem`, `DeleteKnowledgeItem`, `RevertKnowledgeItemVersion`, `CreateKnowledgeCategory`, `AssignCategoryToKnowledgeItem`, `CreateKnowledgeTag`, `AssignTagToKnowledgeItem`, `DefineKnowledgeRelationship` (*associates two KIs*), `RemoveKnowledgeRelationship`, `GenerateConceptMap`.
* **Queries:** `GetKnowledgeItemById`, `GetKnowledgeItemVersionHistory`, `ListKnowledgeItemsByCategory`, `ListKnowledgeItemsByTag`, `GetRelatedKnowledgeItems` (*traverses KnowledgeRelationships*), **`GetKnowledgeGraphStructureData`** (*provides data for visualization by traversing relationships*), `ExportConceptMap`.
* **Events:** `KnowledgeItemCreated`, `KnowledgeItemUpdated`, `KnowledgeItemDeleted`, `KnowledgeItemVersionReverted`, `KnowledgeItemCategorized`, `KnowledgeItemTagged`, `KnowledgeRelationshipDefined`, `KnowledgeRelationshipRemoved`, `ConceptMapGenerated`.

**2. Semantic Search (BC)**

* **Focus:** Indexing content from various sources (KM Core, Notes, Docs) for **advanced semantic understanding *and* full-text search capabilities.**
* **Subdomains:** Core (Semantic Understanding & Search)
* **Aggregates:**
    * `SemanticIndexEntry` (AR): Represents the indexed version of a source item (KnowledgeItem, Note, Document).
* **Entities:** `SemanticIndexEntry` (AR), `ConceptDefinition`, `SynonymMapping`.
* **Value Objects:** `EmbeddingVector`, `QueryIntent`, `ConceptRelationshipData`, `SearchFilter`, `SearchOperator`, `PersonalizationFactor`, **`IndexedSourceReference`** (`SourceContext`: string, `SourceItemID`: string, `SourceType`: enum).
* **User Stories:**
    * As a system, I want to `IndexSourceItem` from KM Core, Notes, or Docs, so users can search across all content.
    * As a system, I want to `UpdateSemanticIndexForItem` when a source item changes, so search results remain current.
    * As a system, I want to `RemoveItemFromSemanticIndex` when a source item is deleted or archived, so it doesn't appear in search results.
    * As a subject matter expert, I want to `DefineConcept` and its properties, so the system understands domain-specific terms.
    * As a linguist, I want to `MapSynonyms` for concepts, so search can find relevant items even with different phrasing.
    * As a user, I want to `FindItemsBySemanticQuery` using natural language, so I can find information without knowing exact keywords.
    * As a user, I want to `FindItemsByKeywordAndFilter` with specific criteria (like source type, date) and operators (AND, OR, NOT), so I can perform precise searches.
    * As a researcher, I want to `GetRelatedConcepts` for a given term, so I can explore interconnected ideas.
    * As an advanced user, I want to `ExecuteKnowledgeGraphQuery` to find complex patterns and relationships within the indexed data.
    * As a frequent user, I want to `GetPersonalizedSearchResults` based on my past activity and preferences, so I find relevant information faster.
* **Commands:** `IndexSourceItem` (*handles different source types*), `UpdateSemanticIndexForItem`, `RemoveItemFromSemanticIndex`, `DefineConcept`, `MapSynonyms`.
* **Queries:** `FindItemsBySemanticQuery`, **`FindItemsByKeywordAndFilter`** (*Handles advanced full-text/keyword search across indexed sources*), `GetRelatedConcepts`, `ExecuteKnowledgeGraphQuery` (*Potentially uses indexed relationships or queries KM Core*), `GetPersonalizedSearchResults`.
* **Events:** `SourceItemIndexed`, `SemanticIndexUpdated`, `ItemRemovedFromSemanticIndex`, `ConceptDefined`, `SynonymMappingCreated`.

**3. Knowledge Curation (BC)**

* **Focus:** Managing knowledge quality, review processes, lifecycle (draft, published, archived).
* **Subdomains:** Supporting (Knowledge Curation & Lifecycle)
* **Aggregates:**
    * `KnowledgeReview` (AR)
    * `CurationTask` (AR)
* **Entities:** `KnowledgeReview` (AR), `KnowledgeQualityLog`, `CurationTask` (AR).
* **Value Objects:** `VerificationStatus`, `QualityMetric` (Accuracy, Relevance, Freshness), `LifecycleStage` (Draft, Published, Archived), `ReviewFeedback`.
* **User Stories:**
    * As a knowledge manager, I want to `InitiateKnowledgeReview` for a new or updated item, so its quality can be assessed.
    * As a reviewer, I want to `SubmitReviewFeedback` with comments and a verification status, so the author knows what to improve.
    * As a system, I want to `UpdateKnowledgeQualityMetrics` based on reviews and usage, so we can track the overall health of the knowledge base.
    * As a knowledge curator, I want to `AssignCurationTask` (e.g., "update outdated item", "find duplicate") to team members, so maintenance is actively managed.
    * As a team member, I want to `CompleteCurationTask` and mark it as done.
    * As a knowledge manager, I want to `UpdateKnowledgeLifecycleStage` (e.g., from Draft to Published, or Published to Archived), so content reflects its current relevance.
    * As a reviewer, I want to `GetPendingReviews` assigned to me, so I can manage my workload.
    * As a quality analyst, I want to `GetKnowledgeItemQualityHistory` to see how its accuracy and relevance have changed over time.
    * As a team lead, I want to `ListCurationTasks` to monitor ongoing maintenance efforts.
    * As a user, I want to `GetKnowledgeItemLifecycleStatus` to understand if an item is current, a draft, or archived.
* **Commands:** `InitiateKnowledgeReview`, `SubmitReviewFeedback`, `UpdateKnowledgeQualityMetrics`, `AssignCurationTask`, `CompleteCurationTask`, `UpdateKnowledgeLifecycleStage` (e.g., Archive).
* **Queries:** `GetPendingReviews`, `GetKnowledgeItemQualityHistory`, `ListCurationTasks`, `GetKnowledgeItemLifecycleStatus`.
* **Events:** `KnowledgeReviewInitiated`, `KnowledgeReviewCompleted`, `KnowledgeQualityUpdated`, `CurationTaskAssigned`, `CurationTaskCompleted`, `KnowledgeLifecycleStageChanged`.

**4. Knowledge Integration (BC)**

* **Focus:** Linking knowledge items to entities in other external modules/BCs.
* **Subdomains:** Core (Knowledge Linking & Integration)
* **Aggregates:**
    * `KnowledgeLink` (AR): Represents a link between a Knowledge Item (KM Core) and an external entity.
* **Entities:** `KnowledgeLink` (AR).
* **Value Objects:**
    * **`ExternalReference { ContextName: string, EntityType: string, ItemID: string, ItemName?: string }`** (*Explicit structure for robust linking*),
    * `LinkContext`, `RecommendationScore`, `LinkType`.
* **User Stories:**
    * As a user working on a project task, I want to `CreateKnowledgeLink` between the task and a relevant knowledge item, so I have quick access to information.
    * As an administrator, I want to `RemoveKnowledgeLink` if the external item is deleted or the link is no longer relevant.
    * As a system, I want to `UpdateKnowledgeLinkContext` if the name or details of the linked external item change.
    * As a user viewing an external item (e.g., a project, a customer case), I want the system to `GenerateKnowledgeRecommendationsForContext`, so I can discover relevant knowledge items automatically.
    * As a knowledge author, I want to `GetLinksForKnowledgeItem` to see all external entities it's connected to.
    * As a user viewing an external entity, I want to `GetLinksToExternalItem` to see all knowledge items linked to it.
    * As a user, I want to `GetContextualKnowledgeRecommendations` when working in a specific area (e.g., a specific module or task type) to find helpful information.
* **Commands:** `CreateKnowledgeLink`, `RemoveKnowledgeLink`, `UpdateKnowledgeLinkContext`, `GenerateKnowledgeRecommendationsForContext`.
* **Queries:** `GetLinksForKnowledgeItem`, `GetLinksToExternalItem`, `GetContextualKnowledgeRecommendations`.
* **Events:** `KnowledgeLinkCreated`, `KnowledgeLinkRemoved`, `KnowledgeLinkContextUpdated`, `KnowledgeRecommendationsGenerated`.

**5. Notes And Documentation (BC)**

* **Focus:** User-created rich notes, organization (collections/folders), linking *within* notes.
* **Subdomains:** Supporting (Note Taking & Organization)
* **Aggregates:**
    * `Note` (AR)
    * `NoteCollection` (AR)
* **Entities:** `Note` (AR), `NoteVersion`, `NoteCollection` (AR).
* **Value Objects:** `NoteRichContent`, `NoteTag`, `NoteCategory`, `BiDirectionalLinkTarget`, `AttachmentInfo`, `VoiceTranscription`, `ChangeSet`.
* **User Stories:**
    * As a user, I want to `CreateNote` with rich text and formatting, so I can capture my thoughts and information effectively.
    * As a user, I want to `UpdateNoteContent` easily, so my notes stay current.
    * As a user, I want to `DeleteNote` when it's no longer needed.
    * As a user, I want to `AddTagToNote` and `CategorizeNote`, so I can organize and find my notes later.
    * As a user, I want to `CreateNoteLink` to another note, so I can create connections between related ideas (bi-directionally).
    * As a user, I want to `AttachFileToNote`, so I can keep relevant documents with my notes.
    * As a user, I want to `TranscribeVoiceToNote` from an audio recording, so I can quickly capture spoken ideas.
    * As a user, I want to `RestoreNoteVersion` if I accidentally delete content or want to see a previous state.
    * As a user, I want to `CreateNoteCollection` (like a folder or notebook), so I can group related notes together.
    * As a user, I want to `AddNoteToCollection` and `RemoveNoteFromCollection` to manage my note organization.
    * As a user, I want to `GetNoteById` to quickly open and view a specific note.
    * As a user, I want to view `GetNoteVersionHistory` and `CompareNoteVersions` to understand changes over time.
    * As a user, I want to `SearchNotesByMetadata` (tags, category) to find notes even if I don't remember the exact title or content.
    * As a user, I want to `GetNotesByTag` or `GetNotesByCategory` for focused Browse.
    * As a user, I want to `ListNoteCollections` to see all my notebooks.
    * As a user, I want to `GetNotesInCollection` to view all notes within a specific notebook.
* **Commands:** `CreateNote`, `UpdateNoteContent`, `DeleteNote`, `AddTagToNote`, `CategorizeNote`, `CreateNoteLink`, `AttachFileToNote`, `TranscribeVoiceToNote`, `RestoreNoteVersion`, `CreateNoteCollection`, `AddNoteToCollection`, `RemoveNoteFromCollection`.
* **Queries:** `GetNoteById`, `GetNoteVersionHistory`, `CompareNoteVersions`, **`SearchNotesByMetadata`** (*Internal search by tag/category*), `GetNotesByTag`, `GetNotesByCategory`, `ListNoteCollections`, `GetNotesInCollection`. *(Note: For full-text/semantic search, query the Semantic Search BC)*.
* **Events:** `NoteCreated`, `NoteUpdated`, `NoteDeleted`, `NoteTagged`, `NoteCategorized`, `NoteLinkCreated`, `FileAttachedToNote`, `VoiceTranscribedToNote`, `NoteVersionRestored`, `NoteCollectionCreated`, `NoteAddedToCollection`, `NoteRemovedFromCollection`.

**6. Document Management (BC)**

* **Focus:** Managing discrete files/documents, versions, metadata, permissions, workflows, templates.
* **Subdomains:** Supporting (Document Storage & Versioning), Generic (File Storage, Permissions)
* **Aggregates:**
    * `Document` (AR)
    * `DocumentTemplate` (AR)
    * `DocumentWorkflowInstance` (AR)
* **Entities:** `Document` (AR), `DocumentVersion`, `DocumentTemplate` (AR), `DocumentWorkflowInstance` (AR), `DocumentShare`.
* **Value Objects:** `DocumentMetadata`, `DocumentTag`, `DocumentCategory`, `Permission`, `StorageReference`, `FileVersionHash`, `WorkflowStep`, `WorkflowState`, `ShareLink`.
* **User Stories:**
    * As a user, I want to `UploadDocument` into the system, so I can store and manage it.
    * As a user, I want to `CreateNewDocumentVersion` when I make changes, so I can track its history and revert if needed.
    * As a user, I want to `UpdateDocumentMetadata` (like author, keywords), so the document is easier to find and understand.
    * As a user, I want to `DeleteDocument` when it's no longer required.
    * As an administrator, I want to `CategorizeDocument` and `TagDocument`, so documents are well-organized and searchable.
    * As a document owner, I want to `SetDocumentPermissions`, so I can control who can view or edit it.
    * As a user, I want to `ShareDocument` with specific colleagues or create a `ShareLink`, so others can access it.
    * As a document owner, I want to `RevokeShare` access when it's no longer needed.
    * As a user, I want to `CreateDocumentFromTemplate`, so I can ensure consistency and save time.
    * As a process manager, I want to `InitiateDocumentWorkflow` (e.g., for approval), so documents follow a defined process.
    * As a reviewer, I want to `AdvanceDocumentWorkflowStep` after completing my part of the process.
    * As a process manager, I want to `CancelDocumentWorkflow` if it's no longer necessary.
    * As a user, I want to `GetDocumentById` to access a specific file.
    * As a user, I want to `GetDocumentMetadata` to understand its properties.
    * As a user, I want to view `GetDocumentVersionHistory` to see how it has evolved.
    * As a user, I want to `SearchDocumentsByMetadata` (tags, category, custom fields) to find files efficiently.
    * As a user, I want to `ListDocumentsByCategory` or `ListDocumentsByTag`.
    * As an administrator, I want to `GetDocumentPermissions` to review access rights.
    * As a user, I want to see `GetSharedDocumentLinks` I've created or have access to.
    * As a user, I want to `ListDocumentTemplates` to choose from available templates.
    * As a user involved in a workflow, I want to `GetDocumentWorkflowStatus` to track its progress.
* **Commands:** `UploadDocument`, `CreateNewDocumentVersion`, `UpdateDocumentMetadata`, `DeleteDocument`, `CategorizeDocument`, `TagDocument`, `SetDocumentPermissions`, `ShareDocument`, `RevokeShare`, `CreateDocumentFromTemplate`, `InitiateDocumentWorkflow`, `AdvanceDocumentWorkflowStep`, `CancelDocumentWorkflow`.
* **Queries:** `GetDocumentById`, `GetDocumentMetadata`, `GetDocumentVersionHistory`, **`SearchDocumentsByMetadata`** (*Internal search by metadata/tags/category*), `ListDocumentsByCategory`, `ListDocumentsByTag`, `GetDocumentPermissions`, `GetSharedDocumentLinks`, `ListDocumentTemplates`, `GetDocumentWorkflowStatus`. *(Note: For full-text/semantic search, query the Semantic Search BC; may require text extraction service interaction before indexing)*.
* **Events:** `DocumentUploaded`, `DocumentVersionCreated`, `DocumentMetadataUpdated`, `DocumentDeleted`, `DocumentCategorized`, `DocumentTagged`, `DocumentPermissionsUpdated`, `DocumentShared`, `ShareRevoked`, `DocumentCreatedFromTemplate`, `DocumentWorkflowInitiated`, `DocumentWorkflowStepCompleted`, `DocumentWorkflowCompleted`, `DocumentWorkflowCancelled`.

---

**Updated Event Subscribers and Policies (Examples):**

* **Event:** `KnowledgeItemCreated` / `KnowledgeItemUpdated` (from `KM Core`)
    * **Subscriber (`Semantic Search`):** Triggers `IndexSourceItem` / `UpdateSemanticIndexForItem`.
    * **Subscriber (`Knowledge Curation`):** May trigger `InitiateKnowledgeReview` based on rules.
    * **Subscriber (`Knowledge Integration`):** May re-evaluate `GenerateKnowledgeRecommendationsForContext`.
* **Event:** `NoteCreated` / `NoteUpdated` (from `Notes And Documentation`)
    * **Subscriber (`Semantic Search`):** Triggers `IndexSourceItem` / `UpdateSemanticIndexForItem`.
    * **Subscriber (`Knowledge Integration`):** Listens to potentially update `KnowledgeLinkContext` if a linked note changes.
* **Event:** `DocumentUploaded` / `DocumentVersionCreated` (from `Document Management`)
    * **Subscriber (`Document Management` internal or external service):** May perform text extraction for applicable types.
    * **Subscriber (`Semantic Search`):** Listens (potentially after text extraction is signaled) and triggers `IndexSourceItem`.
    * **Subscriber (`Knowledge Integration`):** Listens to potentially update `KnowledgeLinkContext`.
* **Event:** `KnowledgeLifecycleStageChanged` (to Archived) (from `Knowledge Curation`)
    * **Subscriber (`Semantic Search`):** Triggers `RemoveItemFromSemanticIndex`.
    * **Subscriber (`KM Core`):** Updates the `KnowledgeItem`'s status flag.
    * **Subscriber (`Knowledge Integration`):** May trigger `RemoveKnowledgeLink` for links pointing *from* the archived item.
* **Policy:** *Cross-Reference Content on Creation*
    1.  Listens for `KnowledgeItemCreated`, `NoteCreated`, `DocumentUploaded`.
    2.  When an event occurs, triggers queries to `Semantic Search` (`FindItemsByKeywordAndFilter` or `FindItemsBySemanticQuery` based on new item's content/keywords).
    3.  If relevant existing items are found (across KM, Notes, Docs via the unified index), triggers `CreateKnowledgeLink` (in `Knowledge Integration` BC) to link the new item to the existing ones.
* **Policy:** *Document Approval Workflow*
    1.  User triggers `InitiateDocumentWorkflow` (`Document Management`).
    2.  `Document Management` BC emits `DocumentWorkflowInitiated`, sets initial state in `DocumentWorkflowInstance`.
    3.  An external workflow engine or a service within `Document Management` BC listens/reacts.
    4.  Assigns tasks (e.g., via notifications or task list integration).
    5.  Reviewer action triggers `AdvanceDocumentWorkflowStep` (`Document Management`).
    6.  Workflow engine updates `DocumentWorkflowInstance` state.
    7.  If completed, emits `DocumentWorkflowCompleted`.
    8.  `Document Management` BC listens for completion event, updates the `Document` status/permissions.

---