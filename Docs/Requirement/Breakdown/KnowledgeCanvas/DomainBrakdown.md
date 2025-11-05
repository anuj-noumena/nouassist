
# Knowledge Canvas: Domain Layer Breakdown (DDD)

This document outlines the Domain Layer for the Knowledge Canvas application using Domain-Driven Design (DDD) principles. It focuses on identifying the core business logic, models, and their interactions.

## 1. Core Domain

The **Core Domain** of the Knowledge Canvas is **Knowledge Organization and Visual Interconnection**. This involves enabling users to:

-   Create and manage diverse pieces of information (Content Blocks).
    
-   Visually arrange these blocks on a flexible canvas.
    
-   Establish and represent meaningful relationships (Connections) between these blocks.
    
-   Structure and derive insights from the interconnected knowledge.
    

## 2. Bounded Contexts & Key Domain Elements

We can identify several Bounded Contexts, each with its own specific model and ubiquitous language.

### 2.1. Canvas Management Bounded Context

-   **Description:** Responsible for the overall workspace, its properties, and persistence.
    
-   **Ubiquitous Language:** Canvas, Workspace, Viewport, Grid, Background.
    
-   **Aggregates:**
    
    -   **`Canvas` (Aggregate Root)**
        
        -   **Entity ID:**  `CanvasId` (Value Object)
            
        -   **Entities (within Aggregate):**
            
            -   `CanvasVersion` (if versioning is a core feature, each with a unique `VersionId`, representing a distinct state or snapshot of the canvas content and properties over time)
                
        -   **Value Objects:**
            
            -   `CanvasName`
                
            -   `ViewportState` (zoom level, panX, panY)
                
            -   `CanvasSettings` (e.g., `BackgroundColor`, `GridConfiguration` {visibility, snapBehavior})
                
            -   `OwnerId` (references a User in a potential User Management Context)
                
            -   `Timestamp` (created, lastModified)
                
-   **Domain Services:**
    
    -   `CanvasLifecycleService`: Handles creation, saving, loading, and deletion of canvases.
        
    -   `CanvasSharingService`: Manages sharing permissions and access control (interacts with User Management BC if it exists).
        
-   **Domain Events:**
    
    -   `CanvasCreated`
        
    -   `CanvasSaved`
        
    -   `CanvasViewportChanged`
        
    -   `CanvasSettingsUpdated`
        
    -   `CanvasShared`
        
    -   `CanvasDeleted`
        

### 2.2. Content Block Management Bounded Context

-   **Description:** Manages the individual knowledge elements (blocks/nodes) that users place on the canvas. This is a central and rich context.
    
-   **Ubiquitous Language:** Block, Node, Element, Content, Position, Style.
    
-   **Aggregates:**
    
    -   **`ContentBlock` (Abstract Aggregate Root)** - Concrete block types inherit from this.
        
        -   **Entity ID:**  `BlockId` (Value Object, unique across all block types on a canvas)
            
        -   **Common Value Objects (for all block types):**
            
            -   `Position` (x, y coordinates)
                
            -   `Dimensions` (width, height)
                
            -   `BlockStyle` (e.g., `BackgroundColor`, `Border`, `FontSettings`)
                
            -   `ZOrder` (for layering)
                
            -   `CanvasId` (reference to the parent Canvas)
                
        -   **Concrete Entity Subtypes (each is an Aggregate Root instance, managed by this BC): This approach allows each block type to manage its own complex internal state, enforce its specific invariants independently, and evolve without directly impacting other block types.**
            
            -   **`RichTextBlock`**
                
                -   Value Objects: `RichTextContent` (stores formatted text, e.g., HTML or Markdown)
                    
            -   **`ImageBlock`**
                
                -   Value Objects: `ImageSource` (URL or internal reference), `AltText`, `DisplaySettings` (e.g., fit, fill)
                    
            -   **`MindMapBlock`**
                
                -   Entities (within aggregate): `MindMapBranch` (each with a unique `BranchId`)`
                    
                -   Value Objects: `MindMapNodeContent`, `MindMapStructure` (defines parent-child relationships, layout hints)
                    
            -   **`DiagramBlock`** (for free-form diagrams or specific types like flowcharts)
                
                -   Entities (within aggregate): `DiagramShape` (with `ShapeId`), `DiagramConnector` (with `InternalConnectorId`) (distinct from top-level `Connection` aggregates, as these internal connectors are part of the `DiagramBlock`'s own structure, describe relationships between its internal shapes, and are managed entirely within the `DiagramBlock` aggregate, not as independent domain objects linking separate `ContentBlock`s).
                    
                -   Value Objects: `ShapeType`, `ConnectorStyle`, `DiagramLayoutData`
                    
            -   **`GraphChartBlock`**
                
                -   Value Objects: `ChartType` (bar, line, pie), `ChartData` (series, labels, values), `ChartConfiguration` (axes, legends, colors)
                    
            -   **`TableBlock`**
                
                -   Value Objects: `TableStructure` (rows, columns), `CellData`, `TableStyle`
                    
            -   **`SketchBlock`**
                
                -   Value Objects: `SketchData` (vector paths, bitmap data)
                    
-   **Domain Services:**
    
    -   `ContentBlockFactory`: Responsible for creating instances of different block types.
        
    -   `ContentBlockInteractionService`: Handles operations like moving, resizing, updating content.
        
-   **Domain Events:**
    
    -   `ContentBlockAddedToCanvas`
        
    -   `ContentBlockRemovedFromCanvas`
        
    -   `ContentBlockMoved`
        
    -   `ContentBlockResized`
        
    -   `ContentBlockContentUpdated`
        
    -   `ContentBlockStyleUpdated`
        

### 2.3. Relationship Management Bounded Context

-   **Description:** Manages the explicit connections and relationships drawn between Content Blocks.
    
-   **Ubiquitous Language:** Connection, Link, Edge, Relationship, Arrow.
    
-   **Aggregates:**
    
    -   **`Connection` (Aggregate Root)**
        
        -   **Entity ID:**  `ConnectionId` (Value Object)
            
        -   **Value Objects:**
            
            -   `SourceBlockId` (references a ContentBlock)
                
            -   `TargetBlockId` (references a ContentBlock)
                
            -   `CanvasId` (reference to the parent Canvas)
                
            -   `ConnectionType` (e.g., directed, undirected)
                
            -   `ConnectionStyle` (e.g., `LineColor`, `LineThickness`, `ArrowheadStyle`)
                
            -   `Label` (optional text on the connection)
                
-   **Domain Services:**
    
    -   `ConnectionLifecycleService`: Handles creation, modification, and deletion of connections, ensuring validity (e.g., blocks exist).
        
-   **Domain Events:**
    
    -   `ConnectionCreated`
        
    -   `ConnectionModified`
        
    -   `ConnectionDeleted`
        

### 2.4. Organization & Structuring Bounded Context

-   **Description:** Deals with features that help users organize content on the canvas, such as grouping or explicit layouts.
    
-   **Ubiquitous Language:** Group, Cluster, Layout, Arrangement.
    
-   **Aggregates:**
    
    -   **`BlockGroup` (Aggregate Root)**
        
        -   **Entity ID:**  `GroupId` (Value Object)
            
        -   **Value Objects:**
            
            -   `GroupName`
                
            -   `MemberBlockIds` (Set of `BlockId`s)
                
            -   `CanvasId` (reference to the parent Canvas)
                
            -   `GroupStyle` (e.g., `BoundingBoxStyle`)
                
-   **Domain Services:**
    
    -   `GroupingService`: Manages creation of groups, adding/removing blocks from groups.
        
    -   `AutomatedLayoutService` (if advanced): Arranges blocks based on algorithms or relationships (could also be part of AI/Advanced BC).
        
-   **Domain Events:**
    
    -   `BlockGroupCreated`
        
    -   `BlocksAddedToGroup`
        
    -   `BlocksRemovedFromGroup`
        
    -   `BlockGroupDeleted`
        
    -   `LayoutApplied`
        

### 2.5. (Optional/Future) User & Collaboration Bounded Context

-   **Description:** Manages users, authentication, permissions, and collaborative features.
    
-   **Ubiquitous Language:** User, Collaborator, Session, Permissions, Comment.
    
-   **Aggregates:**  `User`, `CollaborationSession`, `CommentThread`.
    
-   **Domain Events:**  `UserJoinedCanvas`, `CommentAdded`.
    

### 2.6. (Optional/Future) Templating Bounded Context

-   **Description:** Manages pre-defined structures or layouts for canvases.
    
-   **Ubiquitous Language:** Template, Blueprint, Starter.
    
-   **Aggregates:**  `CanvasTemplate`.
    
-   **Domain Events:**  `TemplateCreated`, `TemplateAppliedToCanvas`.
    

### 2.7. (Optional/Future) AI & Advanced Interaction Bounded Context

-   **Description:** Handles features like semantic search, content suggestions, and automated insights.
    
-   **Ubiquitous Language:** Suggestion, Insight, Semantic Search.
    
-   **Domain Services:**  `KnowledgeGraphService`, `ContentSuggestionService`.
    
-   **Domain Events:**  `AISuggestionGenerated`, `SemanticSearchExecuted`.
    

## 3. Ubiquitous Language

Maintaining a consistent Ubiquitous Language across all discussions, code, and UI is crucial. Key terms include: `Canvas`, `Content Block` (or `Node`/`Element`), `Connection`, `Group`, `Position`, `Style`, `Viewport`. The specific name for each `Content Block` type (e.g., `MindMapBlock`, `TableBlock`) should also be part of this language.

## 4. Key Application Services (Examples)

Application services orchestrate use cases by coordinating domain objects. They do not contain business logic themselves.

-   `CreateNewCanvasApplicationService`
    
-   `AddBlockToCanvasApplicationService` (e.g., `AddRichTextBlockToCanvas`)
    
-   `UpdateBlockContentApplicationService`
    
-   `ConnectBlocksApplicationService`
    
-   `MoveBlockApplicationService`
    
-   `GroupBlocksApplicationService`
    
-   `SaveCanvasApplicationService`
    