
## UI Breakdown: Learning & Knowledge Management (LKM) Feature Slices

This document details the UI/UX design for the primary feature slices supporting the Learning & Knowledge Management (LKM) Bounded Context.

### 1. `SkillManagement` Slice

**User Story:**

-   "As a user or administrator, I want to define, view, and manage skills relevant to my domain, including their relationships (e.g., prerequisites, related skills), so that we have a clear taxonomy for learning and development."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page, potentially within a settings or knowledge base area (`SkillsPage.razor` - `@page "/skills"` or `@page "/settings/skills"`).
    
-   **Organization:**
    
    -   Toolbar (`MudToolBar`): Add Skill button (`MudButton`), potentially filter/search options.
        
    -   Main Content: Could be a `MudTable` listing skills or a `MudTreeView` / `Syncfusion TreeGrid` if skills have a strong hierarchical or relational structure that needs visualization. Columns/Node info: Skill Name, Description (brief), Category?.
        
-   **Adding/Editing:** Via `MudDialog`. Fields: Name (`MudTextField`), Description (`MudTextField`), Category (`MudSelect`), Proficiency Level Definitions (complex component potentially needed), Skill Relationships (UI to search/select other skills and define relationship type - e.g., Prerequisite, Related).
    
-   **Primary Action(s):** Viewing skill list/hierarchy, CRUD operations on `SkillDefinition`s, defining relationships between skills.
    

**Component Breakdown & Interactions:**

-   **List/Tree View:**
    
    -   **UI Elements:**  `MudTable` or `MudTreeView`/`Syncfusion TreeGrid`. `MudButton` ("Add Skill"). Actions per item (`MudIconButton` Edit/Delete).
        
    -   **User Interaction:** View/Search/Filter skills. Add new skill (opens dialog). Edit/Delete existing skill (Edit opens dialog, Delete needs confirmation). Navigate hierarchy if tree view is used.
        
    -   **ADHD Considerations:** Clear presentation of skills. If using a tree, ensure smooth expand/collapse and clear hierarchy lines. Simple table view might be less overwhelming if hierarchy isn't the primary focus. Good search/filter is essential.
        
-   **`Add/Edit Skill Dialog (MudDialog):`**
    
    -   **UI Elements:**  `MudForm`, `MudTextField`, `MudSelect`. A potentially complex component for defining proficiency levels. A multi-select or linking component (`SkillSelector`) to define relationships. `MudButton` (Save/Cancel).
        
    -   **User Interaction:** Fill/select skill details. Define proficiency levels. Search and link related skills. Save/Cancel.
        
    -   **ADHD Considerations:** Break down complexity within the dialog using `MudTabs` or `MudExpansionPanels` (e.g., Basic Info, Proficiency Levels, Relationships). Make defining proficiency levels and relationships as intuitive as possible. Use clear language.
        

### 2. `LearningPlanManagement` Slice

**User Story:**

-   "As a user or manager, I want to create, view, and manage personalized learning plans, defining goals, milestones, and selecting relevant learning content, so that I have a structured path for skill development."
    

**Overall Layout & Structure:**

-   **Container:** List page (`LearningPlansPage.razor`) and Edit/View page (`LearningPlanDetailsPage.razor`).
    
-   **List Page:** Toolbar (Add Plan button), `MudTable` listing user's or team's learning plans (Name, Goal Summary, Status, Due Date?).
    
-   **Edit/View Page:** Uses `MudTabs` or sections:
    
    -   **Overview:** Plan Name, Description, Overall Goal, Status, Dates.
        
    -   **Goals/Milestones:**  `MudList` or `MudTable` editor to define specific `LearningPlanGoal`s and `LearningMilestone`s (Description, Target Skill/Proficiency, Due Date).
        
    -   **Content:** UI to link `LearningContent` items to the plan or specific goals/milestones (using a `LearningContentSelector` component).
        
    -   **Progress:** (View Mode Only) Shows aggregated progress based on `UserSkillProgress`.
        
-   **Primary Action(s):** CRUD operations on learning plans, defining goals/milestones, associating learning content.
    

**Component Breakdown & Interactions:**

-   **List Page:** Standard table interactions (Add, Edit, View, Delete).
    
-   **Edit/View Page:**
    
    -   **UI Elements:**  `MudTabs`, `MudTextField`, `MudDatePicker`, `MudList` editor, `LearningContentSelector` (custom component, likely dialog-based search/select). `MudButton` (Save Plan).
        
    -   **User Interaction:** Define plan details. Add/edit/remove goals and milestones. Search and link learning content. Save plan. View progress in view mode.
        
    -   **ADHD Considerations:** Use `MudStepper` for creating a new plan to guide the user. Clear separation between sections (Overview, Goals, Content). Make adding/editing goals/milestones intuitive. The content selector should be easy to use (good search/filter). Visualize plan structure clearly.
        

### 3. `UserSkillProgressView` Slice

**User Story:**

-   "As a user or manager, I want to view the current proficiency levels for my skills (or my team members'), see progress over time, and identify skill gaps based on target levels or roles, so that I can understand development status and focus learning efforts."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page or dashboard section (`SkillProgressPage.razor` - `@page "/skills/progress"`).
    
-   **Organization:**
    
    -   Filters: User selector (for managers), Skill Category (`MudSelect`).
        
    -   Main Content:
        
        -   **Skill List/Grid:**  `MudTable` or `MudGrid` with `MudCard`s showing skills, current proficiency level (`MudRating` or similar visual indicator), target level (if defined), status (On Track, Gap).
            
        -   **Visualizations:**  `Syncfusion Chart`s showing progress trends over time for specific skills, or visualizing skill gaps against required profiles.
            
-   **Primary Action(s):** Viewing skill proficiency, filtering views, identifying gaps.
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudTable`/`MudGrid`+`MudCard`, `MudRating`, `MudChip` (for status), `Syncfusion Chart`, `MudSelect`.
    
-   **User Interaction:** View skill list/grid. Apply filters. Hover over charts for details. Clicking a skill might navigate to `SkillManagement` details or related `LearningPlan`.
    
-   **ADHD Considerations:** Clear visual indicators for proficiency levels (e.g., progress bars, ratings). Use color effectively but subtly to highlight gaps or status without causing anxiety. Keep charts simple and focused on key trends. Ensure data loads smoothly.
    

### 4. `LearningContentManagement` Slice

**User Story:**

-   "As an administrator, L&D professional, or user, I want to add, organize, and manage learning content items (like articles, videos, courses, documents), so that relevant resources are available for learning plans and knowledge building."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page, similar to `ReferenceLibrary` (`LearningContentPage.razor` - `@page "/learning-content"`).
    
-   **Organization:**
    
    -   Toolbar: Search, Filter (`MudSelect` Type, Tags, Skill), Add Content button (`MudButton`).
        
    -   Main Content: `MudTable` or `MudGrid` card view listing `LearningContent` items (Title, Type, Source, Tags, Associated Skills).
        
-   **Adding/Editing:** Via `MudDialog` or dedicated page. Fields: Title, Type (`MudSelect`), Source/URL (`MudTextField`) or File Upload (`MudFileUpload`), Description, Tags (`TagInput`), Associated Skills (`SkillSelector`).
    
-   **Primary Action(s):** Browsing, searching, filtering content, CRUD operations on content items, tagging and linking content to skills.
    

**Component Breakdown & Interactions:**

-   **List/Grid View:** Standard interactions similar to `ReferenceLibrary`. Actions include View/Edit/Delete.
    
-   **Add/Edit Content Dialog/Page:**
    
    -   **UI Elements:**  `MudForm`, `MudTextField`, `MudSelect`, `MudFileUpload`, `TagInput`, `SkillSelector`. `MudButton` (Save).
        
    -   **User Interaction:** Fill details, upload file or provide link, add tags, link skills. Save.
        
    -   **ADHD Considerations:** Similar to `ReferenceUpload/Create`. Clear form, easy tagging/linking. Good feedback on save/upload.
        

### 5. `AssessmentTemplateManagement` Slice

**User Story:**

-   "As an L&D professional or subject matter expert, I want to create and manage assessment templates with specific criteria and questions (potentially including AI-generated ones), so that we can consistently evaluate skill proficiency."
    

**Overall Layout & Structure:**

-   **Container:** List page (`AssessmentTemplatesPage.razor`) and Edit page (`AssessmentTemplateEditPage.razor`).
    
-   **List Page:** Toolbar (Add Template button), `MudTable` listing templates (Name, Associated Skill, Question Count).
    
-   **Edit Page:**  `MudForm` with sections:
    
    -   **Basic Info:** Name, Description, Associated Skill (`SkillSelector`), Target Proficiency Level (`MudSelect`).
        
    -   **Criteria:** Define `AssessmentCriterion`.
        
    -   **Questions:**  `MudList` editor showing `AssessmentQuestion`s. Button ("Add Question Manually") opens dialog. Button ("Generate Questions") triggers LLM generation (`GenerateAssessmentQuestions` command) - shows progress indicator (`MudProgressCircular`) and potentially adds generated questions (`GeneratedQuestion`) to a review area before final inclusion. Interface to review and add/edit/remove questions.
        
-   **Primary Action(s):** CRUD on templates, defining criteria, adding/managing manual and generated questions.
    

**Component Breakdown & Interactions:**

-   **List Page:** Standard table interactions.
    
-   **Edit Page:**
    
    -   **UI Elements:**  `MudForm`, `MudTextField`, `SkillSelector`, `MudSelect`, `MudList` editor, `MudButton`s, `MudProgressCircular`. Dialog for manual question entry. Review area for generated questions.
        
    -   **User Interaction:** Define template details. Add/edit criteria. Add questions manually via dialog. Trigger AI generation, review generated questions, add selected ones to template. Reorder/remove questions. Save template.
        
    -   **ADHD Considerations:** Use `MudStepper` for template creation. Clear separation between manual and AI question management. Provide clear feedback during AI generation (progress indicator, success/failure message `MudSnackbar`). Make reviewing and adding generated questions straightforward.
        

### 6. `AssessmentTaking` Slice

**User Story:**

-   "As a user, I want to take an assigned or selected assessment in a clear, focused interface, answering questions sequentially, so that my understanding can be evaluated."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page, potentially full-screen or distraction-free mode (`TakeAssessmentPage.razor` - `@page "/assessments/take/{AssessmentInstanceId}"`).
    
-   **Organization:** Focused view displaying one question at a time.
    
    -   Header: Assessment Title, Progress indicator (e.g., "Question 5 of 20", `MudProgressLinear`). Optional Timer (`MudChip` countdown?).
        
    -   Question Area: Displays the `AssessmentQuestion` text.
        
    -   Answer Area: Renders appropriate input based on question type (e.g., `MudRadioGroup` for multiple choice, `MudCheckBox` group for multi-select, `MudTextField` for free text).
        
    -   Navigation: "Next Question" / "Previous Question" buttons (`MudButton`), "Submit Assessment" button (`MudButton` on last question).
        
-   **Primary Action(s):** Reading questions, selecting/entering answers, navigating between questions, submitting the completed assessment.
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudProgressLinear`, `MudText` (Question), Answer input controls (`MudRadioGroup`, `MudCheckBox`, `MudTextField`), `MudButton`s (Next/Previous/Submit).
    
-   **User Interaction:** User reads question, selects/enters answer. User clicks Next/Previous. User clicks Submit on the final question (shows confirmation `MudDialog`?).
    
-   **ADHD Considerations:**  **Crucial:** Minimize distractions. Show only the current question. Clear progress indicator helps maintain focus and reduce anxiety. Ensure answer controls are simple and unambiguous. Provide clear navigation. If timed, make the timer visible but not overly stressful (avoid rapid ticking sounds/animations). Confirm before submission.
    

### 7. `AssessmentReview` Slice

**User Story:**

-   "As a user or manager, I want to view the results of a completed assessment, including my score, feedback, correct/incorrect answers, and potentially suggested learning resources, so that I can understand performance and identify areas for improvement."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`AssessmentResultPage.razor` - `@page "/assessments/results/{AssessmentInstanceId}"`).
    
-   **Organization:**
    
    -   Header: Assessment Title, User Name, Date Taken, Overall Score/Result (`MudText`, `MudChip`).
        
    -   Results Breakdown: `MudList` or `MudTable` showing each question, the user's answer, the correct answer (if applicable), and feedback/score per question. Correct/incorrect answers visually distinct.
        
    -   Feedback/Suggestions Section: Displays overall `AssessmentFeedback` and any `SuggestResourceBasedOnAssessment` (links to `LearningContent`).
        
-   **Primary Action(s):** Viewing assessment score and detailed results/feedback. Accessing suggested resources.
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudText`, `MudChip`, `MudList`/`MudTable`, `MudIcon` (Correct/Incorrect), `MudLink` (to resources).
    
-   **User Interaction:** User reviews score and question breakdown. User clicks links to suggested resources.
    
-   **ADHD Considerations:** Clear presentation of the overall score. Easy-to-scan breakdown of questions/answers. Use clear visual cues (icons, color) for correct/incorrect answers without being overly negative for incorrect ones. Group suggested resources clearly.
    

### 8. `LearningSessionLog` Slice

**User Story:**

-   "As a user, I want to view a log of my past learning sessions, including duration, topics covered, and any logged metrics or interruptions, so that I can reflect on my learning activities and patterns."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page or section within a user dashboard (`LearningLogPage.razor` - `@page "/learning/log"`).
    
-   **Organization:**
    
    -   Toolbar: Filter (`MudDateRangePicker`, `MudSelect` for Skill/Topic?).
        
    -   Main Content: `MudTable` listing `LearningSession` records. Columns: Date, Start/End Time, Duration, Topic/Skill Studied (`MudChip`), Cognitive Metrics summary (if logged), Interruptions count.
        
-   **Primary Action(s):** Viewing learning session history, filtering the log.
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudDateRangePicker`, `MudSelect`, `MudTable`, `MudTablePager`, `MudChip`.
    
-   **User Interaction:** View log. Apply filters. Sort table columns. Clicking a session might open a details dialog showing full metrics/interruptions.
    
-   **ADHD Considerations:** Clear table layout. Easy filtering by date or topic. Summarize key metrics concisely in the table view; show full details on demand.
    

### 9. `KnowledgeItemManagement` Slice

**User Story:**

-   "As a user, I want to create, view, edit, and organize individual knowledge items (like synthesized notes, summaries, concepts), link them to skills or other items, and tag them, so that I can build and manage my personal knowledge base."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page, potentially integrated with `ReferenceLibrary` or a separate knowledge base area (`KnowledgePage.razor` - `@page "/knowledge"`).
    
-   **Organization:**
    
    -   Toolbar: Search, Filter (Tags, Linked Skill/Area), Add Item button (`MudButton`).
        
    -   Main Content: `MudTable` or `MudGrid` card view listing `KnowledgeItem`s (Title, Type, Tags, Last Updated).
        
-   **Adding/Editing:** Via `MudDialog` or dedicated page. Fields: Title (`MudTextField`), Content (`MudTextField` multiline, rich text/Markdown), Tags (`TagInput`), Metadata, Links to other `KnowledgeItem`s (`KnowledgeItemSelector`), Links to `SkillDefinition`s (`SkillSelector`).
    
-   **Primary Action(s):** CRUD on knowledge items, tagging, linking items together or to skills.
    

**Component Breakdown & Interactions:**

-   **List/Grid View:** Standard interactions. Actions include View/Edit/Delete.
    
-   **Add/Edit Dialog/Page:**
    
    -   **UI Elements:**  `MudForm`, `MudTextField`, `MudMarkdownTextArea` / Rich Text Editor, `TagInput`, `KnowledgeItemSelector`, `SkillSelector`. `MudButton` (Save).
        
    -   **User Interaction:** Create/edit content. Add tags. Search and link related items/skills. Save.
        
    -   **ADHD Considerations:** Simple, clean editor for content. Easy tagging. Intuitive interface for linking related items/skills (good search/selection). Auto-save drafts could be beneficial.
        

### 10. `KnowledgeGraphView` Slice

**User Story:**

-   "As a user, I want to visualize the relationships between my knowledge items and skills as a graph, so that I can explore connections, discover related concepts, and understand the structure of my knowledge."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`KnowledgeGraphPage.razor` - `@page "/knowledge/graph"`).
    
-   **Organization:**
    
    -   Toolbar: Search (`MudTextField` to find/center node), Filter (limit depth, filter by tag/skill?), Layout options (`MudSelect` for different graph layouts).
        
    -   Main Content: **Graph Visualization Component** (`Syncfusion Diagram` or similar library). Nodes represent `KnowledgeItem`s or `SkillDefinition`s. Edges represent `KnowledgeGraphLink` relationships.
        
-   **Primary Action(s):** Exploring the graph, panning/zooming, clicking nodes to view details or navigate, filtering the view.
    

**Component Breakdown & Interactions:**

-   **Toolbar:** Search/Filter controls. Layout selection.
    
-   **Graph Component:**
    
    -   **UI Elements:** Graph visualization canvas. Nodes with labels. Edges showing relationships. Tooltips on hover.
        
    -   **User Interaction:** Pan and zoom the graph. Hover over nodes/edges for tooltips. Click a node (highlights connections, potentially opens `KnowledgeItemDetails` or `SkillDetails` in a side panel or dialog). Apply filters/layout changes from toolbar.
        
    -   **ADHD Considerations:**  **Crucial:** Keep the graph visualization clean and performant. Avoid node/edge clutter. Use distinct shapes/colors for different item types (Knowledge Item vs Skill). Ensure smooth panning/zooming. Highlighting connections on node selection helps focus. Provide options to filter or limit the graph complexity if it becomes too large.
        

### 11. `KnowledgePlaybookManagement` Slice

**User Story:**

-   "As a user or expert, I want to create and manage structured playbooks that sequence knowledge items and actions for specific scenarios or processes, so that knowledge can be applied consistently and effectively."
    

**Overall Layout & Structure:**

-   **Container:** List page (`PlaybooksPage.razor`) and Edit page (`PlaybookEditPage.razor`).
    
-   **List Page:** Toolbar (Add Playbook button), `MudTable` listing playbooks (Name, Description, Status).
    
-   **Edit Page:**  `MudForm`.
    
    -   Fields: Name, Description.
        
    -   **Steps Editor:**  `MudList` editor where each step can be defined:
        
        -   Step Description (`MudTextField`).
            
        -   Associated `KnowledgeItem` (`KnowledgeItemSelector`).
            
        -   Associated Action/Task Template? (`TaskTemplateSelector`).
            
        -   Order (Drag-and-drop reordering).
            
    -   `MudButton` ("Add Step").
        
    -   `MudButton` ("Save Playbook").
        
-   **Primary Action(s):** CRUD on playbooks, defining and ordering steps, linking knowledge/actions to steps.
    

**Component Breakdown & Interactions:**

-   **List Page:** Standard table interactions.
    
-   **Edit Page:**
    
    -   **UI Elements:**  `MudForm`, `MudTextField`, `MudList` editor with drag-and-drop, `KnowledgeItemSelector`, `TaskTemplateSelector`?, `MudButton`s.
        
    -   **User Interaction:** Define playbook details. Add/edit/reorder steps. Link knowledge/actions to steps using selectors. Save playbook.
        
    -   **ADHD Considerations:** Structured editor for steps is key. Use drag-and-drop for intuitive ordering. Clear interface for linking knowledge/actions. Break down complex playbook creation using a `MudStepper` if necessary.


## UI Breakdown: Personal Growth & Productivity (PGP) Feature Slices

This document details the UI/UX design for the primary feature slices supporting the Personal Growth & Productivity (PGP) Bounded Context.

### 1. `PersonalGoalManagement` Slice

**User Story:**

-   "As a user focused on self-improvement, I want to define meaningful personal goals using criteria like SMART, track my progress towards them, and review them regularly, so that I can stay motivated and achieve my aspirations."
    

**Overall Layout & Structure:**

-   **Container:** List page (`PersonalGoalsPage.razor`) and Edit/View page (`PersonalGoalDetailsPage.razor`).
    
-   **List Page:** Toolbar (Add Goal button), `MudTable` or `MudGrid` card view listing goals (Name, Status, Target Date?, Progress Indicator `MudProgressLinear`). Filters (`MudSelect` Status, Area/Domain?).
    
-   **Edit/View Page:**  `MudForm` (for editing) or display sections (for viewing). Uses `MudTabs` or `MudExpansionPanels`:
    
    -   **Definition:** Goal Name (`MudTextField`), Description (`MudTextField`), SMART Criteria fields (Specific `MudTextField`, Measurable `MudTextField`, Achievable `MudSwitch`?, Relevant `MudTextField`, Time-bound `MudDatePicker`). Link to `LifeDomainConfiguration`?
        
    -   **Progress:** Interface to log `GoalProgressUpdate`s (`MudTextField` for notes, `MudSlider` or `MudNumericField` for quantitative progress?), `MudList` displaying progress history.
        
    -   **Related Items:** Links to related `ImprovementInitiative`s, `Habit`s, or `Task`s.
        
-   **Primary Action(s):** CRUD operations on personal goals, defining SMART criteria, logging progress updates, viewing progress history.
    

**Component Breakdown & Interactions:**

-   **List Page:** Standard table/grid interactions (Add, View, Edit, Delete/Archive). Progress bars provide quick status.
    
-   **Edit/View Page:**
    
    -   **UI Elements:**  `MudTabs`/`MudExpansionPanels`, `MudForm`, `MudTextField`, `MudSwitch`, `MudDatePicker`, `MudSlider`/`MudNumericField`, `MudList`, `MudButton` ("Save Goal", "Log Progress"). Selectors for linking related items.
        
    -   **User Interaction:** Define goal using SMART fields. Log progress updates via a dedicated input area or dialog. View history. Link related work. Save changes.
        
    -   **ADHD Considerations:** The SMART criteria fields provide structure for goal definition, reducing ambiguity. Use `MudStepper` for initial goal creation to guide the process. Visualize progress clearly (e.g., `MudProgressLinear` or simple chart). Make logging progress updates quick and easy. Break down complex goals visually if possible.
        

### 2. `Journal` Slice

**User Story:**

-   "As a user, I want to easily log journal entries about my thoughts, experiences, reflections, or learnings, and be able to review past entries, so that I can practice self-reflection and track my personal journey."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`JournalPage.razor` - `@page "/journal"`). Could have different views.
    
-   **Organization:**
    
    -   **Entry View:** Default view might show entries chronologically (`MudList` or `MudCard`s per entry). Toolbar with "New Entry" button, Date filter (`MudDatePicker`), Search (`MudTextField`), Tag filter (`TagInput`).
        
    -   **Calendar View:** (Optional) `Syncfusion Scheduler` or `MudDatePicker` highlighting days with entries. Clicking a day shows entries for that day.
        
-   **Adding/Editing:** Via a dedicated editor area on the page or a `MudDialog`.
    
    -   Editor: `MudTextField` (multiline, supporting Markdown or Rich Text), `TagInput` for tags, potentially Mood tracker (`MudSelect` with icons?), Link selectors (Tasks, Projects, Goals?).
        
-   **Primary Action(s):** Creating new journal entries, viewing past entries (list/calendar), searching/filtering entries, editing entries.
    

**Component Breakdown & Interactions:**

-   **Entry/Calendar View:**
    
    -   **UI Elements:**  `MudList`/`MudCard`s or `Syncfusion Scheduler`/`MudDatePicker`. `MudButton` ("New Entry"), `MudDatePicker`, `MudTextField` (Search), `TagInput`.
        
    -   **User Interaction:** View entries chronologically or via calendar. Filter by date/tag/search. Click "New Entry". Click existing entry to view/edit.
        
    -   **ADHD Considerations:** Provide a clean, inviting interface for viewing entries. Calendar view can be helpful for visualizing frequency. Powerful search/tagging is essential for finding past entries.
        
-   **Add/Edit Entry Editor:**
    
    -   **UI Elements:**  `MudTextField` (rich text/markdown), `TagInput`, `MudSelect` (Mood?), Link selectors. `MudButton` ("Save Entry").
        
    -   **User Interaction:** Write entry content. Add tags/mood/links. Save.
        
    -   **ADHD Considerations:**  **Crucial:** Make the writing experience low-friction and pleasant. Use a clean editor (consider `MudMarkdownTextArea` or integrating a simple rich text editor). Auto-save drafts. Easy tagging. Avoid overly complex formatting options unless desired. Keep the interface calm and focused.
        

### 3. `FailureAnalysis` Slice

**User Story:**

-   "As a user committed to learning from mistakes, I want to document failures or setbacks using a structured format, analyzing the root cause and identifying lessons learned, so that I can avoid repeating errors and foster resilience."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`FailureAnalysisPage.razor` - `@page "/analysis/failures"`).
    
-   **Organization:**
    
    -   Toolbar: "Document New Analysis" button (`MudButton`), Filters (`MudSelect` Category?, Date Range `MudDateRangePicker`).
        
    -   Main Content: `MudTable` listing `FailureAnalysisReport`s (Title/Situation Summary, Date, Key Lesson Learned).
        
-   **Adding/Editing:** Via dedicated page (`FailureAnalysisEditPage.razor`) using a structured `MudForm`. Sections for:
    
    -   Situation Description (`MudTextField`).
        
    -   What Happened (`MudTextField`).
        
    -   Root Cause Analysis (`MudTextField` - maybe guided questions?).
        
    -   Lessons Learned (`MudTextField`).
        
    -   Action Plan Items (`MudList` editor for simple text items or links to `Task`s).
        
-   **Primary Action(s):** CRUD operations on failure analysis reports, filling structured analysis fields.
    

**Component Breakdown & Interactions:**

-   **List Page:** Standard table interactions.
    
-   **Edit Page:**
    
    -   **UI Elements:**  `MudForm`, `MudTextField` (multiline), `MudList` editor. `MudButton` ("Save Analysis").
        
    -   **User Interaction:** Fill in the structured analysis fields. Add action plan items. Save.
        
    -   **ADHD Considerations:** The structured template itself aids focus and prevents rambling. Use clear headings for each section. Keep the interface non-judgmental and focused on learning. Break down the analysis into manageable text areas.
        

### 4. `HabitTracking` Slice

**User Story:**

-   "As a user, I want to define habits I want to build or break, easily log whether I performed them each day, and visualize my streaks and progress, so that I can stay consistent and motivated."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page or dashboard section (`HabitsPage.razor` - `@page "/habits"`).
    
-   **Organization:**
    
    -   Toolbar: "Define New Habit" button (`MudButton`), View options (Daily Log, Weekly View, Monthly Calendar?).
        
    -   Main Content: Displays habits, often in a grid or list format suitable for daily logging.
        
        -   **Daily Log View:** List (`MudList`) or Cards (`MudCard`) for each habit defined for today. Each item shows Habit Name, Trigger, and a logging mechanism (e.g., `MudCheckBox`, `MudButton` "Log Done", `MudNumericField` if tracking quantity).
            
        -   **Streak/History View:**  `MudTable` or `MudCard`s showing Habit Name, Current Streak (`MudChip`), Longest Streak, recent history visualization (e.g., mini calendar heatmap, simple bar chart).
            
-   **Adding/Editing Habit:** Via `MudDialog`. Fields: Name (`MudTextField`), Description/Motivation (`MudTextField`), Trigger (`MudTextField`), Frequency/Schedule (e.g., Daily, Weekdays, Specific Days `MudCheckBoxGroup`), Goal (e.g., Do/Don't Do, Quantity `MudNumericField`).
    
-   **Primary Action(s):** Defining habits, logging daily execution, viewing streaks and history.
    

**Component Breakdown & Interactions:**

-   **Daily Log View:**
    
    -   **UI Elements:**  `MudList`/`MudCard`, `MudCheckBox`/`MudButton`/`MudNumericField` for logging.
        
    -   **User Interaction:** User views habits for the day. User clicks checkbox/button or enters quantity to log execution (`LogHabitExecution` command).
        
    -   **ADHD Considerations:**  **Crucial:** Make daily logging extremely simple and fast (ideally one click per habit). Clear visual feedback upon logging (e.g., checkmark appears, button state changes). Group habits logically (e.g., Morning/Evening). Consider mobile-friendliness for quick logging.
        
-   **Streak/History View:**
    
    -   **UI Elements:**  `MudTable`/`MudCard`, `MudChip` (for streak count), simple charts/visualizations.
        
    -   **User Interaction:** View streaks and history.
        
    -   **ADHD Considerations:** Visualizing streaks (`MudChip` with fire icon?) can be highly motivating. Keep history visualizations simple and easy to understand (e.g., github-style contribution graph). Celebrate streak milestones (via Notifications BC?).
        
-   **Add/Edit Habit Dialog:**
    
    -   **UI Elements:**  `MudDialog`, `MudForm`, `MudTextField`, `MudCheckBoxGroup`, `MudNumericField`. `MudButton` ("Save Habit").
        
    -   **User Interaction:** Define habit details, schedule, goal. Save.
        
    -   **ADHD Considerations:** Simple, clear form for defining habits. Make scheduling options intuitive.
        

### 5. `MotivationProfileView` Slice (Optional)

**User Story:**

-   "As a user, I want to optionally review the motivation factors and potential obstacles related to my goals or habits that I've previously identified, so that I can remind myself of my 'why' and anticipate challenges."
    

**Overall Layout & Structure:**

-   **Container:** Settings page section or dedicated page (`MotivationProfilePage.razor`). Read-only focus.
    
-   **Organization:** Use `MudCard`s or `MudList`s to display:
    
    -   Identified Motivation Factors (linked to Goals/Habits?).
        
    -   Documented Obstacle Plans.
        
-   **Primary Action(s):** Viewing the profile information.
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudCard`, `MudText`, `MudList`.
    
-   **User Interaction:** Read information.
    
-   **ADHD Considerations:** Clear, concise presentation. Frame information positively and constructively. Ensure it's easy to find but not intrusive if the user doesn't frequently access it.
    

### 6. `PGPInboxProcessing` Slice

**User Story:**

-   "As a user, I want a dedicated space to capture and process personal thoughts, ideas, or reflections that don't fit elsewhere, deciding if they need action, journaling, or should be discarded, so that I can maintain mental clarity."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`PGPInboxPage.razor` - `@page "/personal-inbox"`). If this functionality is distinct from the main GTD inbox. If not distinct, this slice isn't needed and the GTD `InboxProcessing` slice handles it. Assuming it's distinct for now.
    
-   **Organization:** Similar to GTD `InboxProcessing`. Focused, one-item-at-a-time processing.
    
    -   Display current `InboxItem` (`MudCard`).
        
    -   Action buttons: "Delete", "Journal" (opens `Journal` entry), "Make Actionable" (creates `Task` or links?), "Make Goal" (opens `PersonalGoalManagement`?), "Defer", "Reference".
        
-   **Primary Action(s):** Processing personal inbox items sequentially.
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudCard`, `MudText`, `MudButton`s for processing actions. Potentially opens other slice dialogs/pages upon action selection.
    
-   **User Interaction:** View item, click action button. Next item loads.
    
-   **ADHD Considerations:** Same as GTD `InboxProcessing`: Focus on one item, clear actions, smooth transitions, minimize distractions. Ensure clear feedback on where the item went after processing.
    

### 7. `SOPManagement` Slice

**User Story:**

-   "As a user, I want to create, view, and update Standard Operating Procedures (SOPs) for my recurring personal or professional processes, so that I can execute them consistently and efficiently."
    

**Overall Layout & Structure:**

-   **Container:** List page (`SOPListPage.razor`) and Viewer/Editor page (`SOPDetailsPage.razor`).
    
-   **List Page:** Toolbar (Add SOP button), `MudTable` listing SOPs (Name, Version, Last Updated).
    
-   **Viewer/Editor Page:**
    
    -   Header: SOP Name, Version selector (`MudSelect`), Edit button.
        
    -   Content Area: Displays `SOPContent` (likely using `MudMarkdownTextArea` for editing or Markdown rendering component for viewing). Supports steps, checklists, embedded images/links.
        
-   **Primary Action(s):** CRUD operations on SOPs, viewing content, managing versions.
    

**Component Breakdown & Interactions:**

-   **List Page:** Standard table interactions.
    
-   **Viewer/Editor Page:**
    
    -   **UI Elements:**  `MudSelect` (Version), Markdown/Rich Text viewer/editor component, `MudButton` ("Save New Version").
        
    -   **User Interaction:** View SOP content. Select older versions to view. Click Edit, modify content, Save (creates new version).
        
    -   **ADHD Considerations:** Clear viewer with good formatting (headings, lists, checklists). Simple editor. Easy version navigation. Consider checklist interactivity directly in the viewer for executing an SOP.
        

### 8. `WorkflowOptimization` Slice

**User Story:**

-   "As a user seeking efficiency, I want to identify and view points of friction in my personal workflows and see potential optimization suggestions, so that I can streamline how I get things done."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page or section (`WorkflowOptimizationPage.razor`).
    
-   **Organization:**
    
    -   Maybe a way to _log_ friction points (`MudTextField` + `MudButton`).
        
    -   Main view: `MudList` or `MudTable` listing identified `WorkflowFrictionPoint`s (Description, Context/Workflow, Date Identified).
        
    -   For each friction point, potentially display `OptimizationSuggestion`s (generated by system or manually added).
        
-   **Primary Action(s):** Logging friction points, viewing friction points and suggestions.
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudTextField`, `MudButton` (Log Friction), `MudList`/`MudTable`, `MudCard` (for suggestions?).
    
-   **User Interaction:** User logs friction points. User views the list and associated suggestions.
    
-   **ADHD Considerations:** Simple interface for logging friction. Clear presentation of friction points and related suggestions. Make suggestions actionable if possible (e.g., link to create a related `ImprovementInitiative`).
    

### 9. `FeedbackManagement` Slice

**User Story:**

-   "As a user, I want to aggregate feedback received from various sources, view it in one place, and potentially see analysis or patterns, so that I can learn from feedback and identify areas for growth."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`FeedbackPage.razor` - `@page "/feedback"`).
    
-   **Organization:**
    
    -   Toolbar: Add Feedback Manually button (`MudButton`), Filters (`MudSelect` Source, Date Range `MudDateRangePicker`).
        
    -   Main Content:
        
        -   `MudTable` listing individual `FeedbackItem`s (Content Summary, Source, Date).
            
        -   Section displaying `FeedbackAnalysisReport`s or key `FeedbackPattern`s (`MudCard`s, `MudList`).
            
-   **Adding Manually:** Via `MudDialog`. Fields: Feedback Content (`MudTextField`), Source (`MudTextField` or `MudSelect`), Date (`MudDatePicker`).
    
-   **Primary Action(s):** Viewing aggregated feedback, adding manual feedback, viewing analysis/patterns.
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudButton`, `MudSelect`, `MudDateRangePicker`, `MudTable`, `MudCard`, `MudList`. Dialog with `MudForm`, `MudTextField`, `MudDatePicker`.
    
-   **User Interaction:** View feedback list/patterns. Filter feedback. Add manual feedback via dialog. Click items for full details.
    
-   **ADHD Considerations:** Organize feedback clearly. Summarize key patterns concisely. Avoid presenting raw feedback in a way that feels overwhelming or overly critical. Allow filtering by source or theme.
    

### 10. `ImprovementInitiativeTracking` Slice

**User Story:**

-   "As a user, I want to define specific improvement initiatives based on goals, feedback, or analysis, and track their progress and outcomes, so that I have a structured approach to making changes."
    

**Overall Layout & Structure:**

-   **Container:** List page (`InitiativesPage.razor`) and Details page (`InitiativeDetailsPage.razor`).
    
-   **List Page:** Toolbar (Start Initiative button), `MudTable` listing initiatives (Name, Goal Link?, Status, Start Date).
    
-   **Details Page:** Sections for:
    
    -   Initiative Definition (Name, Goal, Plan `MudTextField`).
        
    -   Progress Log (`MudList` editor for adding `LogInitiativeProgress` updates).
        
    -   Related Items (Links to `Task`s, `PersonalExperiment`s?).
        
-   **Primary Action(s):** CRUD on initiatives, logging progress, viewing details.
    

**Component Breakdown & Interactions:**

-   **List Page:** Standard table interactions.
    
-   **Details Page:**
    
    -   **UI Elements:**  `MudTextField`, `MudList` editor, Link selectors. `MudButton` ("Log Progress", "Complete Initiative").
        
    -   **User Interaction:** Define initiative details. Log progress updates. Link related items. Mark complete.
        
    -   **ADHD Considerations:** Clear structure for defining initiatives. Simple progress logging. Visualize progress if possible.
        

### 11. `PersonalExperimentManagement` Slice

**User Story:**

-   "As a user wanting to test improvements systematically, I want to define personal experiments with clear hypotheses, methods, and metrics, track their execution, and log the results, so that I can make data-informed decisions about changes to my habits or workflows."
    

**Overall Layout & Structure:**

-   **Container:** List page (`ExperimentsPage.razor`) and Edit/View page (`ExperimentDetailsPage.razor`).
    
-   **List Page:** Toolbar (Design Experiment button), `MudTable` listing experiments (Name, Hypothesis Summary, Status).
    
-   **Edit/View Page:**  `MudForm` (for editing) with structured fields:
    
    -   Hypothesis (`MudTextField`).
        
    -   Method/Procedure (`MudTextField`).
        
    -   Metrics to Track (`MudTextField` or `TagInput`).
        
    -   Duration/Schedule (`MudDateRangePicker`).
        
    -   Results Logging Area (`MudTextField` multiline).
        
    -   Conclusion (`MudTextField`).
        
-   **Primary Action(s):** CRUD on experiments, defining structured fields, logging results and conclusions.
    

**Component Breakdown & Interactions:**

-   **List Page:** Standard table interactions.
    
-   **Edit/View Page:**
    
    -   **UI Elements:**  `MudForm`, `MudTextField`, `TagInput`, `MudDateRangePicker`. `MudButton` ("Save Experiment", "Log Results").
        
    -   **User Interaction:** Define experiment using structured fields. Log results/conclusions. Save.
        
    -   **ADHD Considerations:** The structured template helps guide the experimental design process. Clear separation between hypothesis, method, results, conclusion. Keep the interface focused on the experiment details.


## UI Breakdown: Life Domain & Priority (LDP) Feature Slices

This document details the UI/UX design for the primary feature slices supporting the Life Domain & Priority (LDP) Bounded Context.

### 1. `LifeDomainConfiguration` Slice

**User Story:**

-   "As a user striving for balance, I want to define my key life domains (e.g., Work, Health, Family, Personal Growth), establish boundaries between them, and note important connections, so that I can structure my life intentionally and manage overlaps."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page within settings or a core planning area (`LifeDomainsPage.razor` - `@page "/settings/domains"` or `@page "/life-domains"`).
    
-   **Organization:** Uses `MudGrid` or `MudCard`s to manage different aspects:
    
    -   **Domains List/Editor:** A section to define and view `LifeDomain`s. Could be:
        
        -   A `MudList` where each item represents a domain, allowing inline editing or opening a dialog (`MudDialog`).
            
        -   `MudCard`s for each domain, showing Name, Description, maybe an Icon (`MudIcon`).
            
        -   Includes an "Add Domain" button (`MudButton`).
            
    -   **Connections Editor:** UI to define `CrossDomainConnection`s (e.g., "Work Project impacts Family Time"). Might use a visual graph editor (simple) or a list-based approach (Select Domain A -> Relationship Type -> Select Domain B).
        
    -   **Boundaries Editor:** UI to define `BoundaryRule`s (e.g., "Rule: No work emails after 7 PM"). A `MudTable` or `MudList` listing rules, with an "Add Rule" button opening a dialog. Rule definition might involve selecting domains, time constraints (`MudTimePicker`), activity types, etc.
        
-   **Primary Action(s):** CRUD operations on Life Domains, defining connections between domains, defining boundary rules.
    

**Component Breakdown & Interactions:**

-   **Domains List/Editor:**
    
    -   **UI Elements:**  `MudList`/`MudCard`s, `MudButton` ("Add Domain"), `MudTextField` (Name, Description), `MudIconPicker` (optional). Edit/Delete actions (`MudIconButton`).
        
    -   **User Interaction:** View existing domains. Click "Add Domain" (opens dialog/inline form). Edit domain details. Delete domain (with confirmation).
        
    -   **ADHD Considerations:** Keep the domain definition simple. Using icons can help with quick visual identification. Clear separation between domains in the list/card view.
        
-   **Connections Editor:**
    
    -   **UI Elements:** Could use two `MudSelect` components to pick domains and a `MudTextField` or `MudSelect` for the connection type/description. A `MudList` displays existing connections. `MudButton` ("Add Connection").
        
    -   **User Interaction:** User selects two domains and describes/categorizes the connection. Saves the connection. Views existing connections.
        
    -   **ADHD Considerations:** Make defining connections straightforward. Avoid overly complex graph editors unless essential. A simple list-based approach might be less overwhelming.
        
-   **Boundaries Editor:**
    
    -   **UI Elements:**  `MudTable`/`MudList`, `MudButton` ("Add Rule"). Dialog (`MudDialog`) for adding/editing rules containing `MudTextField` (Rule Description), `MudSelect` (Domains involved), potentially time/activity type selectors.
        
    -   **User Interaction:** View existing rules. Click "Add Rule" (opens dialog). Define rule parameters in the dialog. Save rule. Edit/Delete existing rules.
        
    -   **ADHD Considerations:** Simple rule definition interface. Clearly display existing rules. Use natural language where possible when defining rule conditions.
        

### 2. `PriorityManagement` Slice

**User Story:**

-   "As a user juggling multiple commitments, I want to view my prioritized activities across different life domains, identify potential conflicts, and make conscious decisions about resolving them, so that I can focus my energy on what matters most at any given time."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page or dashboard section (`PrioritiesPage.razor` - `@page "/priorities"`).
    
-   **Organization:**
    
    -   **Priority Set View:** Displays the current set of `PrioritizedActivity` items, potentially grouped by `LifeDomainConfiguration` or urgency/importance matrix (e.g., Eisenhower Matrix). Could use:
        
        -   A `MudList` with visual priority indicators.
            
        -   A `MudTable` sortable by priority score, domain, due date.
            
        -   Drag-and-drop interface for manual prioritization within/across domains?
            
    -   **Conflict Identification Area:** A distinct section (`MudAlert` or `MudCard`) highlighting identified `PriorityConflict`s (e.g., "Conflict: 'Work Project Deadline' clashes with 'Family Event' on [Date]").
        
    -   **Conflict Resolution Interface:** For each conflict, provide tools to resolve it (`ResolvePriorityConflict` command). This might involve:
        
        -   Buttons: "Defer Activity A", "Delegate Activity B", "Adjust Boundary Rule", "Accept Conflict (Manual Override)".
            
        -   Opening related items (Tasks, Events) to reschedule directly.
            
    -   **Dependency Mapping (Optional):** UI to view/define `ActivityDependency` links (e.g., "Task C depends on Task A").
        
-   **Primary Action(s):** Viewing prioritized activities, identifying conflicts, resolving conflicts using guided actions, potentially mapping dependencies.
    

**Component Breakdown & Interactions:**

-   **Priority Set View:**
    
    -   **UI Elements:**  `MudList`/`MudTable`, `MudChip` (for domain), Priority indicators (icons, numbers, color?). Drag-and-drop library if manual prioritization is enabled.
        
    -   **User Interaction:** View prioritized list. Drag items to re-prioritize (if enabled). Click items to view details (Task, Goal, etc.).
        
    -   **ADHD Considerations:** Clear visualization of priorities is key. Avoid overly complex displays. If using drag-and-drop, ensure it's smooth and intuitive. Grouping by domain helps contextualize.
        
-   **Conflict Identification & Resolution:**
    
    -   **UI Elements:**  `MudAlert` or `MudList` of conflicts. `MudButton`s for resolution actions. Links to conflicting items.
        
    -   **User Interaction:** User reviews identified conflicts. User clicks resolution action buttons (may trigger further dialogs or navigation).
        
    -   **ADHD Considerations:** Clearly highlight conflicts without causing undue stress (use clear text, maybe warning icons, avoid alarming colors unless critical). Provide simple, actionable resolution options. Guide the user through resolving the conflict; don't just present the problem.
        
-   **Dependency Mapping:**
    
    -   **UI Elements:** Could be a simple list view showing dependencies or a basic graph visualization. Interface (dialog?) to select two activities and define the dependency link.
        
    -   **User Interaction:** View dependencies. Define new dependencies.
        
    -   **ADHD Considerations:** Keep dependency mapping simple unless complex project management features are required. Clear visualization of links.
        

### 3. `CrossDomainReporting` Slice

**User Story:**

-   "As a user reflecting on my life balance, I want to view reports showing how my time and resources are allocated across different life domains, see how well my activities align with my goals, and get an overall sense of balance, so that I can make adjustments towards a more fulfilling life."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated reporting page or dashboard section (`LifeBalanceReportPage.razor` - `@page "/reports/life-balance"`).
    
-   **Organization:** Uses `MudCard`s and charts to display key metrics and visualizations:
    
    -   **Time Allocation:** Pie chart (`Syncfusion Chart`) or Bar chart showing percentage/hours of `TimeAllocationData` spent per `LifeDomainConfiguration` over a selected period (`MudDateRangePicker`).
        
    -   **Resource Allocation:** Similar chart/table showing allocation of key `Resource`s (if tracked this way) across domains.
        
    -   **Goal Alignment:** Scorecard (`MudCard` with `MudText`) or chart showing `GoalAlignmentScore` per domain, indicating how activities contribute to goals defined in PGP or LKM.
        
    -   **Life Balance Score:** A composite score or visual indicator (`MudProgressCircular` with text, custom gauge?) representing overall balance based on predefined or user-defined metrics.
        
    -   **Boundary Adherence:** Report/Metric showing how often defined `BoundaryRule`s were respected or violated (based on TimeTracking data?).
        
-   **Primary Action(s):** Viewing reports and visualizations, selecting date ranges for reports.
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudCard`, `MudDateRangePicker`, `Syncfusion Chart`, `MudText`, `MudProgressCircular` (or other gauge visualization).
    
-   **User Interaction:** User selects date range. User views charts and scorecards. User hovers over chart elements for details.
    
-   **ADHD Considerations:**
    
    -   **Clear Visualizations:** Use simple, easy-to-understand charts (pie, bar, line). Ensure clear labels, legends, and titles.
        
    -   **Meaningful Metrics:** Present scores (Goal Alignment, Balance) clearly and explain how they are calculated (via `MudTooltip` or info icon).
        
    -   **Avoid Judgment:** Frame metrics neutrally (e.g., "Time Allocation" rather than "Time Wasted"). Focus on awareness rather than judgment.
        
    -   **Simplicity:** Avoid overwhelming the user with too many charts or complex data points on one screen. Use whitespace effectively. Allow users to focus on specific metrics if desired (e.g., using tabs or filters).


## UI Breakdown: Personal Analytics (PA) Feature Slices

This document details the UI/UX design for the primary feature slices supporting the Personal Analytics (PA) Bounded Context, focusing on user-centric insights derived from their activities across other contexts.

### 1. `PersonalAnalyticsDashboardView` Slice

**User Story:**

-   "As a user interested in self-understanding, I want to view a personalized dashboard that highlights my productivity patterns, performance trends, potential correlations, and predictions based on my own data, so that I can gain actionable insights into my habits and effectiveness."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`PersonalDashboardPage.razor` - `@page "/personal-analytics"` or integrated as a section within a main user dashboard).
    
-   **Organization:** Similar structure to the general `DashboardView` (Analytics Platform BC), but widgets are specifically focused on _personal_ metrics derived from LKM, PGP, LDP, and GM data.
    
    -   **`Toolbar (MudToolBar):`** May include dashboard selection (`MudSelect` if multiple personal dashboards allowed), Configure button (`MudButton`), Date Range filter (`MudDateRangePicker`).
        
    -   **Main Content Area:**  `MudGrid` populated by widgets (`MudCard`) displaying personal insights:
        
        -   **Productivity Patterns:** Charts (`Syncfusion Chart`) or text summaries (`MudText`) showing focus duration trends, most productive times/days, task completion velocity.
            
        -   **Performance Trends:** Line charts showing skill progress (from LKM), habit consistency (from PGP), goal achievement rate (from PGP) over time.
            
        -   **Correlations:** Widgets highlighting discovered relationships (e.g., "Higher focus session completion correlates with better goal progress", "Logging Journal entries correlates with improved habit consistency").
            
        -   **Predictions:** Displaying `PerformancePrediction`s (e.g., "Predicted project completion likelihood based on current velocity").
            
        -   **Gamification Summary:** Key metrics from GM (Points trend, recent achievements).
            
        -   **Life Balance Snapshot:** Key metrics from LDP (Time Allocation summary, Balance Score trend).
            
-   **Primary Action(s):** Viewing personal analytics widgets, interacting with charts, filtering by date range, navigating to configuration.
    

**Component Breakdown & Interactions:**

-   **Dashboard Grid & Widgets:**
    
    -   **UI Elements:**  `MudGrid`, `MudCard`, `Syncfusion Chart`, `MudTable` (for specific data points), `MudText`.
        
    -   **User Interaction:** View insights. Hover over charts for details. Click on a widget might link to the source data context (e.g., clicking habit consistency widget links to `HabitTracking`).
        
    -   **ADHD Considerations:**
        
        -   **Clarity & Simplicity:** Present insights clearly and concisely. Avoid jargon. Use simple charts that are easy to interpret.
            
        -   **Actionable Insights:** Frame insights constructively. Focus on patterns and trends rather than raw, potentially overwhelming data.
            
        -   **Non-Judgmental Tone:** Ensure the language used is neutral and supportive, focusing on awareness rather than performance evaluation.
            
        -   **Customization:** Allow users to configure which insights are most relevant to them via `PersonalAnalyticsDashboardConfiguration`.
            
        -   **Loading:** Use `MudSkeleton` loaders for a smooth experience.
            
-   **Toolbar:**
    
    -   **UI Elements:**  `MudSelect`, `MudButton`, `MudDateRangePicker`.
        
    -   **User Interaction:** Standard toolbar interactions.
        
    -   **ADHD Considerations:** Simple controls, clear indication of current dashboard/filters.
        

### 2. `PersonalAnalyticsDashboardConfiguration` Slice

**User Story:**

-   "As a user, I want to customize my personal analytics dashboard by selecting, arranging, and configuring widgets that display the patterns, trends, and insights most meaningful to me, so that I can focus on the areas of self-understanding I care about."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`PersonalDashboardConfigPage.razor`) or an edit mode toggled on the `PersonalAnalyticsDashboardView`.
    
-   **Organization:** Similar to general `DashboardConfiguration`.
    
    -   **Editable Grid:**  `MudGrid` with drag-and-drop for layout. Widgets (`MudCard`) show resize handles, Configure/Delete icons (`MudIconButton`).
        
    -   **Widget Library/Adder:**  `MudDrawer` or `MudDialog` listing available _personal_ analytics widget types (e.g., "Habit Streak Trend", "Focus Session Analysis", "Estimation Bias Chart", "Goal Alignment Score").
        
    -   **Widget Configuration:** Clicking "Configure" on a widget opens a `MudDialog` specific to that insight type (e.g., select time frame, specific habit/skill/goal to focus on).
        
-   **Primary Action(s):** Adding, removing, arranging, resizing, and configuring personal analytics widgets; saving the dashboard layout.
    

**Component Breakdown & Interactions:**

-   **Editable Grid:** Standard drag-and-drop interactions for layout. Configure/Delete actions per widget.
    
-   **Widget Library/Adder:** Browse available personal insight widgets, add selected ones to the dashboard.
    
-   **Widget Configuration Dialog:**  `MudDialog` with `MudForm`. Controls are specific to the personal insight (e.g., `MudSelect` to choose a specific habit for trend analysis, `MudDateRangePicker` for time frame).
    
-   **Saving Dashboard:**  `MudButton` ("Save Personal Dashboard").
    
-   **ADHD Considerations:**
    
    -   **Intuitive Customization:** Make adding, removing, and configuring widgets straightforward.
        
    -   **Clear Widget Options:** Provide clear descriptions of what each personal analytics widget shows.
        
    -   **Focus:** Allow users to create a dashboard that isn't overwhelming by letting them choose only the insights they find valuable. Avoid forcing too many metrics.
        
    -   **Simplicity:** Keep configuration options per widget relatively simple.
        
    -   **Feedback:** Clear save confirmation. Consider autosave.
        

### 3. `InsightReportViewer` Slice

**User Story:**

-   "As a user, I want to view generated insight reports that provide deeper analysis or summaries of specific personal patterns, trends, or correlations discovered by the system, so that I can gain a more thorough understanding beyond the dashboard widgets."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`InsightReportPage.razor` - `@page "/reports/insights/{ReportId}"`). Accessed perhaps via a notification or a link from the dashboard.
    
-   **Organization:** Designed to present a specific, system-generated `InsightReport`.
    
    -   **Header:** Report Title (e.g., "Analysis of Focus Patterns - Q1 2025"), Generation Date. Export button (`MudButton`).
        
    -   **Content Area:** Displays the report content, which could be:
        
        -   Primarily text-based (`MudText` with good formatting) explaining a pattern or correlation.
            
        -   Specific charts (`Syncfusion Chart`) illustrating the insight.
            
        -   A combination of text and visuals within `MudCard`s or sections.
            
-   **Primary Action(s):** Viewing the insight report, potentially exporting it.
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudText`, `Syncfusion Chart`, `MudCard`, `MudButton` (Export).
    
-   **User Interaction:** User reads the report content and views associated charts. User clicks Export.
    
-   **ADHD Considerations:**
    
    -   **Clarity:** Present the insight clearly and directly. Use headings, bullet points, and simple language.
        
    -   **Visual Support:** Use charts effectively to illustrate the point, but ensure they are simple and easy to understand.
        
    -   **Conciseness:** Avoid overly long or dense reports. Summarize key findings upfront.
        
    -   **Actionability:** If possible, conclude the report with potential implications or questions for reflection, framed constructively.
        

### 4. `DataPrivacySettings` Slice

**User Story:**

-   "As a user concerned about privacy, I want to view and control how my personal data is used for analytics and learning, including options for anonymization, data export, or deletion, so that I can trust the system and manage my information."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page within user settings (`PrivacySettingsPage.razor` - `@page "/settings/privacy"`).
    
-   **Organization:**  `MudForm` with sections for different privacy controls:
    
    -   **Data Usage Consent:**  `MudCheckBox` or `MudSwitch` controls for opting in/out of specific types of analysis (e.g., "Allow use of task data for estimation prediction", "Allow use of time tracking data for productivity pattern analysis"). Clear explanations for each.
        
    -   **Anonymization:** Options (`MudRadioGroup`?) for data anonymization levels if applicable.
        
    -   **Data Export:**  `MudButton` ("Request Data Export") to trigger the `ExportAnalyticsData` command. Information about the process/format.
        
    -   **Data Deletion:**  `MudButton` ("Request Data Deletion") - with significant warnings and confirmation (`MudDialog`).
        
-   **Primary Action(s):** Viewing current settings, modifying consent/anonymization settings, requesting data export or deletion.
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudForm`, `MudCheckBox`/`MudSwitch`, `MudRadioGroup`, `MudButton`, `MudDialog` (for confirmations). Explanatory `MudText`.
    
-   **User Interaction:** User reviews current settings. User toggles switches/checkboxes to change consent. User selects anonymization level. User clicks buttons to request export or deletion (requiring confirmation). User saves preference changes.
    
-   **ADHD Considerations:**
    
    -   **Transparency:** Clearly explain what each setting does and what data is involved using simple, unambiguous language. Avoid legal jargon.
        
    -   **Control:** Provide granular control where meaningful, but avoid an overwhelming number of options. Group related settings logically.
        
    -   **Defaults:** Use privacy-preserving defaults where appropriate.
        
    -   **Confirmation:** Use clear confirmation dialogs (`MudDialog`) for irreversible actions like data deletion, explaining the consequences fully.
        
    -   **Assurance:** Design the page to feel trustworthy and give the user a sense of control over their data.


## UI Breakdown: Gamification (GM) Feature Slices

This document details the UI/UX design for the primary feature slices supporting the Gamification (GM) Bounded Context, focusing on engaging users through points, achievements, challenges, and leaderboards.

### 1. `UserGamificationProfileView` Slice

**User Story:**

-   "As a user, I want to see my current gamification status, including my points, earned achievements or badges, and progress in any active challenges, so that I can track my engagement and feel motivated by my progress."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`GamificationProfilePage.razor` - `@page "/profile/gamification"`) or a prominent section within the main user profile or dashboard.
    
-   **Organization:** Uses `MudCard`s or sections to display key gamification elements:
    
    -   **Points Summary:**  `MudCard` displaying current `PointsBalance` (`MudText` with large typography), maybe recent points history (`MudList` or mini-chart).
        
    -   **Recent Achievements:** Section displaying recently `EarnedAchievement`s, perhaps using `MudChipList` with icons or small `MudCard`s per achievement. Link to full `AchievementList`.
        
    -   **Active Challenges:**  `MudList` or `MudCard`s showing challenges the user has joined, including their progress towards the goal (`MudProgressLinear`), remaining time, and link to challenge details.
        
-   **Primary Action(s):** Viewing points, recent achievements, and active challenge status. Navigating to full lists/details.
    

**Component Breakdown & Interactions:**

-   **Points Summary:**
    
    -   **UI Elements:**  `MudCard`, `MudText`, `MudList` / `Syncfusion Chart` (optional).
        
    -   **User Interaction:** View current points and recent activity.
        
    -   **ADHD Considerations:** Make the points total prominent and visually appealing. Keep recent history concise. Avoid overly complex charts here.
        
-   **Recent Achievements:**
    
    -   **UI Elements:**  `MudChipList` / `MudGrid` with `MudCard`s. Each item shows Achievement Icon (`MudIcon` or image), Name. `MudLink` to `AchievementList`.
        
    -   **User Interaction:** View recent achievements. Click link to see all. Hovering might show achievement description (`MudTooltip`).
        
    -   **ADHD Considerations:** Visually appealing display of achievements (badges/icons are key). Celebrate recent unlocks clearly but subtly. Keep the section focused on recent items to avoid overwhelm.
        
-   **Active Challenges:**
    
    -   **UI Elements:**  `MudList` / `MudGrid` with `MudCard`s. Each item shows Challenge Name, Goal, User Progress (`MudProgressLinear`), Time Remaining. `MudLink` or `MudButton` to view challenge details (`ChallengeList` slice, filtered).
        
    -   **User Interaction:** View active challenges and progress. Click link/button for details.
        
    -   **ADHD Considerations:** Clear progress visualization (`MudProgressLinear`) is motivating. Keep the display focused on active/joined challenges. Make time remaining clear.
        

### 2. `AchievementList` Slice

**User Story:**

-   "As a user, I want to browse all available achievements or badges, see which ones I have unlocked, and understand the criteria for unlocking others, so that I know what goals I can strive for within the gamification system."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`AchievementsPage.razor` - `@page "/achievements"`).
    
-   **Organization:**
    
    -   **`Toolbar (MudToolBar):`** Filter (`MudToggleGroup` or `MudSelect`: All / Earned / Not Earned), Sort (`MudSelect`: Name, Points, Rarity?).
        
    -   **Main Content Area:**  `MudGrid` displaying achievements as `MudCard`s. Each card visually represents an `AchievementDefinition`.
        
-   **Primary Action(s):** Browsing achievements, filtering the view, viewing achievement details (criteria, points).
    

**Component Breakdown & Interactions:**

-   **Achievement Grid:**
    
    -   **UI Elements:**  `MudGrid`, `MudCard`. Each card contains:
        
        -   Achievement Icon/Badge (`MudIcon`, image, or SVG). Visually distinct style for earned vs. not earned (e.g., full color vs. grayscale).
            
        -   Achievement Name (`MudText`).
            
        -   Brief Description (`MudText`).
            
        -   Points Awarded (`MudChip`?).
            
        -   (On hover or click): `MudTooltip` or expanded section showing detailed `AchievementCriteria`.
            
    -   **User Interaction:** User browses the grid. User applies filters/sorting. User hovers or clicks on a card to see unlock criteria for unearned achievements or earned timestamp for earned ones.
        
    -   **ADHD Considerations:**
        
        -   **Visual Appeal:** Make the achievement icons/badges visually distinct and appealing. This is core to engagement.
            
        -   **Clarity:** Clearly differentiate between earned and unearned achievements. Make unlock criteria easy to understand.
            
        -   **Organization:** Filtering (Earned/Not Earned) is essential to manage potentially long lists. Grouping by category might also help.
            
        -   **Positive Framing:** Focus on celebrating earned achievements and presenting unearned ones as opportunities.
            

### 3. `ChallengeList` Slice

**User Story:**

-   "As a user, I want to discover and join active challenges, track my progress in challenges I've joined, and see past challenge results, so that I can participate in time-bound or objective-based gamified activities."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`ChallengesPage.razor` - `@page "/challenges"`).
    
-   **Organization:** Uses `MudTabs` to separate views:
    
    -   **Tab 1: Active Challenges:**  `MudGrid` or `MudList` showing currently running `Challenge`s.
        
    -   **Tab 2: My Challenges:** Shows challenges the user has joined (`ChallengeParticipant`). Includes progress display.
        
    -   **Tab 3: Past Challenges:** Archive view of completed challenges and results.
        
-   **Challenge Display:** Each challenge shown using a `MudCard`. Includes Name, Description/Goal, Duration/End Date, Reward, Participant Count?, User's Progress (if in "My Challenges" tab - `MudProgressLinear`), "Join Challenge" button (`MudButton` - conditionally shown) or "View Details" button.
    
-   **Primary Action(s):** Browsing active challenges, joining challenges, viewing progress in joined challenges, viewing past challenges.
    

**Component Breakdown & Interactions:**

-   **Tabs & Challenge Cards:**
    
    -   **UI Elements:**  `MudTabs`, `MudTabPanel`, `MudGrid`/`MudList`, `MudCard`, `MudText`, `MudProgressLinear`, `MudButton`.
        
    -   **User Interaction:** User switches between tabs. User browses challenges. User clicks "Join Challenge" (triggers `JoinChallenge` command, moves challenge to "My Challenges" tab visually). User views progress on joined challenges. User clicks "View Details" (might open a dialog with more info, participant list, or rules).
        
    -   **ADHD Considerations:** Clear separation between available, joined, and past challenges via Tabs. Make challenge goals and deadlines very clear on each card. Visual progress bars (`MudProgressLinear`) are important for motivation in the "My Challenges" tab. Keep card information concise. Provide clear feedback when joining a challenge.
        

### 4. `LeaderboardView` Slice

**User Story:**

-   "As a user interested in friendly competition, I want to view leaderboards based on points or specific activities over different time periods, so that I can see how I rank among peers."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`LeaderboardPage.razor` - `@page "/leaderboard"`).
    
-   **Organization:**
    
    -   **`Toolbar (MudToolBar):`** Selectors for Leaderboard Scope (`MudSelect`: Overall Points, Weekly Habit Points, Monthly Challenge Points, etc.) and Time Period (`MudSelect`: All Time, This Month, This Week).
        
    -   **Main Content:**  `MudTable` displaying the `LeaderboardEntry` data. Columns: Rank (`#`), User (`MudAvatar` + Name), Score, potentially change in rank. User's own entry should be highlighted.
        
-   **Primary Action(s):** Selecting leaderboard scope/period, viewing rankings.
    

**Component Breakdown & Interactions:**

-   **Toolbar:**
    
    -   **UI Elements:**  `MudSelect` components.
        
    -   **User Interaction:** User selects the desired leaderboard scope and time period. Table updates.
        
    -   **ADHD Considerations:** Simple selectors. Clear labels for scope/period.
        
-   **Leaderboard Table:**
    
    -   **UI Elements:**  `MudTable`, `MudTablePager` (if leaderboard is long), `MudAvatar`.
        
    -   **User Interaction:** View rankings. Paginate if necessary.
        
    -   **ADHD Considerations:** Clearly highlight the user's own rank. Keep the table clean and easy to scan. Consider the potential downsides of leaderboards for some users; ensure it doesn't create undue pressure or demotivation. Offering options to hide leaderboards or focus on personal bests could be beneficial. Ensure good performance when loading/sorting.
        

### 5. `AchievementDefinitionManagement` Slice (Admin Focus)

**User Story:**

-   "As an administrator or game designer, I want to define and manage achievements or badges, including their names, descriptions, visual icons, unlock criteria, and point rewards, so that I can create engaging goals for users within the gamification system."
    

**Overall Layout & Structure:**

-   **Container:** Admin settings page (`AchievementDefinitionsPage.razor` - `@page "/admin/gamification/achievements"`).
    
-   **Organization:** Toolbar (Add Achievement button). Main Content (`MudTable` listing `AchievementDefinition`s: Icon Preview, Name, Criteria Summary, Points).
    
-   **Adding/Editing:** Via `MudDialog`. Fields: Name (`MudTextField`), Description (`MudTextField`), Icon Uploader/Selector (`MudFileUpload`/`MudIconPicker`), Criteria Builder (UI to select Trigger Events, set Conditions/Thresholds), Points (`MudNumericField`).
    
-   **Primary Action(s):** CRUD operations on achievement definitions.
    

**Component Breakdown & Interactions:**

-   **List View:** Standard table interactions. Icon preview in the table is helpful.
    
-   **Add/Edit Dialog:**
    
    -   **UI Elements:**  `MudDialog`, `MudForm`, `MudTextField`, `MudFileUpload`/`MudIconPicker`, Criteria Builder UI (similar to Notification Rule conditions), `MudNumericField`, `MudButton` ("Save").
        
    -   **User Interaction:** Define achievement details. Build criteria logic. Set points. Save.
        
    -   **ADHD Considerations:** The criteria builder needs to be clear and structured. Use dropdowns for events/attributes. Provide examples. Simple icon selection/upload.
        

### 6. `ChallengeManagement` Slice (Admin Focus)

**User Story:**

-   "As an administrator or game designer, I want to create and manage challenges, defining their goals, duration, rules, and rewards, so that I can provide structured, time-bound activities to engage users."
    

**Overall Layout & Structure:**

-   **Container:** Admin settings page (`ChallengeDefinitionsPage.razor` - `@page "/admin/gamification/challenges"`).
    
-   **Organization:** Toolbar (Create Challenge button). Main Content (`MudTable` listing `Challenge`s: Name, Goal Summary, Duration, Status [Upcoming/Active/Ended]).
    
-   **Adding/Editing:** Via `MudDialog`. Fields: Name (`MudTextField`), Goal Description (`MudTextField`), Duration (`MudDateRangePicker` or Start Date + Duration field), Rules (`MudTextField` multiline), Reward Description (`MudTextField`, potentially link to points/achievements).
    
-   **Primary Action(s):** CRUD operations on challenges.
    

**Component Breakdown & Interactions:**

-   **List View:** Standard table interactions. Clear status indication.
    
-   **Add/Edit Dialog:**
    
    -   **UI Elements:**  `MudDialog`, `MudForm`, `MudTextField`, `MudDateRangePicker`, `MudButton` ("Save").
        
    -   **User Interaction:** Define challenge details. Save.
        
    -   **ADHD Considerations:** Simple, clear form for defining challenges. Make duration/dates unambiguous.


## UI Breakdown: Innovation Hub (IH) Feature Slices

This document details the UI/UX design for the primary feature slices supporting the Innovation Hub (IH) Bounded Context, focusing on capturing, incubating, evaluating, and collaborating on ideas.

### 1. `IdeaCapture` Slice

**User Story:**

-   "As a user with an idea, I want to quickly capture its essence, including a title, description, and potential tags or source, with minimal friction, so that the idea enters the hub for later development."
    

**Overall Layout & Structure:**

-   **Container:** Typically a `MudDialog` launched via an "Add Idea" button (perhaps globally available or within the `IdeaList` slice), or a simple dedicated page (`CaptureIdeaPage.razor`).
    
-   **Organization:** A simple `MudForm`.
    
    -   `MudTextField` (Label: "Idea Title / Summary" - Required).
        
    -   `MudTextField` (Label: "Description" - Multiline, optional but encouraged).
        
    -   `MudTextField` or `MudSelect` (Label: "Source" - Optional, e.g., "Brainstorm Session", "User Feedback").
        
    -   `TagInput` (Shared component for adding relevant tags - Optional).
        
    -   `MudButton` ("Capture Idea"), `MudButton` ("Cancel").
        
-   **Primary Action(s):** Entering idea details, adding optional metadata, submitting the idea (`CaptureIdea` command).
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudDialog`/Page, `MudForm`, `MudTextField`, `MudSelect` (optional), `TagInput` (optional), `MudButton`s.
    
-   **User Interaction:** User fills in the title and optionally other fields. User clicks "Capture Idea".
    
-   **ADHD Considerations:**
    
    -   **Low Friction:** Keep the initial capture form extremely simple with very few required fields (perhaps only Title). Make it quick to access and submit.
        
    -   **Clarity:** Clear labels for fields.
        
    -   **Feedback:** Immediate confirmation (`MudSnackbar`) that the idea was captured successfully.
        
    -   **Optional Fields:** Clearly distinguish optional fields (tags, source, detailed description) so users don't feel obligated to fill everything initially.
        

### 2. `IdeaList` Slice

**User Story:**

-   "As a user or facilitator, I want to browse, search, and filter all captured ideas based on their status, tags, or source, so that I can get an overview of the idea pipeline and find specific ideas."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`IdeasListPage.razor` - `@page "/ideas"`).
    
-   **Organization:**
    
    -   **`Toolbar (MudToolBar):`** "Capture New Idea" button (`MudButton`), Search (`MudTextField`), Filters (`MudSelect` for Status [e.g., Captured, Incubating, Evaluated, Prioritized], `TagInput` or `MudSelect` for Tags, `MudSelect` for Source?).
        
    -   **Main Content Area:**  `MudTable` or `MudGrid` (card view) displaying `Idea` summaries.
        
        -   **List View:**  `MudTable` columns: Title (`MudLink` to details), Status (`MudChip`), Tags (`MudChipList`), Source, Capture Date. Supports sorting, pagination.
            
        -   **Card View:**  `MudGrid` with `MudCard` per idea, showing Title, Status, Tags, brief description snippet.
            
-   **Primary Action(s):** Viewing the list/grid of ideas, searching, filtering, sorting, navigating to `IdeaDetailsView`, initiating idea capture.
    

**Component Breakdown & Interactions:**

-   **Toolbar & Filters:** Standard interactions for search, filter selection, and triggering the "Capture New Idea" flow.
    
-   **List/Grid View:**
    
    -   **UI Elements:**  `MudTable`/`MudGrid`+`MudCard`, `MudLink`, `MudChip`, `MudChipList`. Actions per item (`MudMenu` or `MudIconButton`s: View Details, maybe quick status change if applicable).
        
    -   **User Interaction:** View ideas. Use search/filters. Sort table columns. Click idea title/card to navigate to `IdeaDetailsView`. Use actions menu.
        
    -   **ADHD Considerations:**
        
        -   **Clear Status:** Use colored `MudChip`s or icons to make the idea status (Captured, Incubating, Evaluated, etc.) immediately clear.
            
        -   **Filtering:** Effective filtering by status and tags is crucial for managing the list.
            
        -   **Conciseness:** Keep the information shown in the list/card view concise to allow for quick scanning. Show more details on hover (`MudTooltip`) or in the details view.
            
        -   **Loading:** Standard skeleton loaders for smooth loading/filtering.
            

### 3. `IdeaDetailsView` Slice

**User Story:**

-   "As a user or evaluator, I want to view all the details of a specific idea, including its description, incubation progress, evaluation results, and related discussions, so that I can fully understand the idea and contribute to its development."
    

**Overall Layout & Structure:**

-   **Container:** Dedicated page (`IdeaDetailsPage.razor` - `@page "/ideas/details/{IdeaId}"`).
    
-   **Organization:** Uses `MudTabs` or `MudGrid` with `MudCard`s/`MudExpansionPanel`s to organize information:
    
    -   **Header:** Idea Title (`MudText` Typo.h5), Status (`MudChip`), Key Actions (`MudButton`s: "Start Incubation", "Evaluate", "Edit Details", etc. - conditionally shown based on status/permissions).
        
    -   **Core Details Panel:** Description, Source, Tags, Capture Date, Owner.
        
    -   **Incubation Panel:** Displays `IncubationStep`s (if applicable). Might be a `MudList` showing step name, status, notes. UI to add/update incubation steps.
        
    -   **Evaluation Panel:** Displays `IdeaEvaluation` results. Could show scores based on `EvaluationFrameworkResult` (using `MudRating`, `MudProgressLinear`, or `MudText`), evaluator comments. Button/Link to trigger `IdeaEvaluation` slice/component.
        
    -   **Collaboration Panel:** Embeds the `Commenting` component (from Collaboration BC) for discussions related to this specific idea.
        
    -   **Related Links Panel:** Shows `CrossPollinationLink`s to other ideas, tasks, projects.
        
-   **Primary Action(s):** Viewing all idea details, initiating incubation steps, triggering evaluation, participating in collaboration, viewing related items.
    

**Component Breakdown & Interactions:**

-   **Header & Core Details:** Display `MudText`, `MudChip`. Action buttons trigger commands or navigate to edit views.
    
-   **Incubation Panel:**
    
    -   **UI Elements:**  `MudList` displaying steps. `MudButton` ("Add/Update Step").
        
    -   **User Interaction:** View incubation progress. Add or update step status/notes (potentially via dialog).
        
    -   **ADHD Considerations:** Clearly visualize the incubation process/steps if applicable. Keep status updates simple.
        
-   **Evaluation Panel:**
    
    -   **UI Elements:**  `MudCard`s or `MudList` showing evaluation summaries/scores. `MudButton` ("Evaluate Idea") triggers evaluation flow.
        
    -   **User Interaction:** View evaluation results. Click "Evaluate Idea".
        
    -   **ADHD Considerations:** Present evaluation results clearly and concisely. Use visuals (ratings, progress bars) where appropriate.
        
-   **Collaboration Panel:**
    
    -   **UI Elements:** Embedded `Commenting` component.
        
    -   **User Interaction:** Read and add comments/replies related to the idea.
        
    -   **ADHD Considerations:** Inherited from `Commenting` component (clear threads, easy input). Ensure it's clearly labeled as discussion _about the idea_.
        
-   **Related Links Panel:**  `MudList` or `MudChipList` with `MudLink`s.
    

### 4. `IdeaEvaluation` Slice (Component/Form)

**User Story:**

-   "As an evaluator, I want to provide a structured evaluation for an idea based on predefined criteria (like impact, feasibility, alignment), so that ideas can be assessed consistently and objectively."
    

**Overall Layout & Structure:**

-   **Container:** Typically launched as a `MudDialog` or a dedicated section within the `IdeaDetailsView` when a user initiates an evaluation.
    
-   **Organization:** A `MudForm` structured around the `EvaluationFrameworkResult` criteria.
    
    -   Instructions/Guidance (`MudText`).
        
    -   Criteria Sections: For each criterion (e.g., Impact, Feasibility, Alignment, Cost):
        
        -   Criterion Name (`MudText`).
            
        -   Rating Input (e.g., `MudSlider` 1-5, `MudRating`, or `MudSelect`).
            
        -   Rationale/Comments (`MudTextField` multiline).
            
    -   Overall Recommendation (`MudSelect` or `MudRadioGroup`: Recommend, Consider, Reject?).
        
    -   Overall Comments (`MudTextField` multiline).
        
    -   `MudButton` ("Submit Evaluation"), `MudButton` ("Cancel").
        
-   **Primary Action(s):** Rating the idea against criteria, providing rationale and overall recommendation, submitting the evaluation (`EvaluateIdea` command).
    

**Component Breakdown & Interactions:**

-   **UI Elements:**  `MudDialog`/Section, `MudForm`, `MudText`, `MudSlider`/`MudRating`/`MudSelect`, `MudTextField`, `MudRadioGroup`, `MudButton`s.
    
-   **User Interaction:** User reviews the idea (context likely visible). User provides ratings/scores for each criterion. User adds comments/rationale. User provides overall recommendation. User submits the evaluation.
    
-   **ADHD Considerations:**
    
    -   **Structure:** The predefined framework provides essential structure, guiding the evaluation process. Clear headings for each criterion.
        
    -   **Clarity:** Use clear rating scales (sliders or ratings are often more intuitive than just numbers). Explain criteria briefly if needed (`MudTooltip`).
        
    -   **Focus:** Keep the form focused on the evaluation task. Avoid distractions within the dialog/section.
        
    -   **Feedback:** Confirmation (`MudSnackbar`) upon successful submission.
        

### 5. `IdeaCollaboration` Slice (Uses `Commenting` Component)

-   **User Story:** (Covered by `Commenting` component's User Story).
    
-   **Overall Layout & Structure:** This slice is implemented by embedding the reusable `Commenting` component (defined in Collaboration BC breakdown) within the `IdeaDetailsView` slice. It's configured with the `IdeaId` as the `ThreadId`.
    
-   **Component Breakdown & Interactions:** As defined for the `Commenting` component.
    
-   **ADHD Considerations:** As defined for the `Commenting` component. Ensure it's clearly placed within the Idea Details context.


## UI Breakdown: System Infrastructure (SI) Feature Slice

This document details the UI/UX design for the primary feature slice supporting the System Infrastructure (SI) Bounded Context. As this BC deals with system-level configuration, its UI is typically consolidated into a single administrative area.

### 1. `SystemConfigurationManagement` Slice (Admin Focus)

**User Story:**

-   "As a system administrator, I want to configure and manage core system settings, including storage, data processing, integrations (like LLM APIs), security protocols, backup schedules, and notification templates, so that the application runs securely, efficiently, and according to defined operational parameters."
    

**Overall Layout & Structure:**

-   **Container:** A dedicated page or section within the main administrative area (`SystemConfigurationPage.razor` - `@page "/admin/system/configuration"`).
    
-   **Organization:** Uses `MudTabs` or potentially nested `MudExpansionPanels` to organize the various configuration categories, preventing an overwhelming single page. Example Tabs/Sections:
    
    -   **Storage:** Configure `StoragePolicy` (e.g., storage endpoints `MudTextField`, quotas `MudNumericField`, encryption standards `MudSelect`). View status.
        
    -   **Processing Rules:** Manage `DataProcessingRule`s. `MudTable` listing rules, "Add Rule" button (`MudButton`) opening a `MudDialog` with a rule builder UI (similar to Notification/Achievement rules, defining triggers/conditions/actions).
        
    -   **Integrations:** Sub-sections or panels for:
        
        -   Internal Integrations (`ListInternalIntegrations` query results, potentially UI to register/configure).
            
        -   External Services / LLM (`ConfigureLLMIntegration`): Forms with `MudTextField` for API keys, endpoints, model selection (`MudSelect`), enable/disable `MudSwitch`.
            
    -   **Security:** Configure `SecurityProtocolConfig` (e.g., `MudSelect` for protocols, `MudTextField` for parameters, session timeouts `MudNumericField`).
        
    -   **Backups:** Configure `BackupSchedule` (`RecurrencePatternEditor` or simple `MudSelect`), view last backup status/timestamp (`GetBackupStatus` query), "Trigger Manual Backup" button (`MudButton`).
        
    -   **Notification Templates:** Manage system-level `NotificationTemplate`s (if not managed elsewhere). `MudTable` listing templates, "Add/Edit Template" button opening a `MudDialog` with `MudTextField` (Name) and `MudTextField` (Template Content - multiline, supporting placeholders).
        
    -   **Engines (Optional):** Configure `CalculationRule`s, `PatternDetectionAlgorithmConfig` if applicable via UI.
        
-   **Primary Action(s):** Viewing current system configurations across different categories, modifying settings, defining rules/templates, managing integrations, triggering manual actions (like backups). Commands like `ConfigureSecureStorage`, `DefineAutomatedProcessingRule`, `ConfigureLLMIntegration`, `ConfigureBackup`, `ConfigureNotificationTemplate` are triggered via these interactions.
    

**Component Breakdown & Interactions:**

-   **Tabs/Panels Structure:**
    
    -   **UI Elements:**  `MudTabs` / `MudExpansionPanels` to structure the different configuration areas.
        
    -   **User Interaction:** Admin navigates between different configuration sections using the tabs or expanding panels.
        
    -   **ADHD Considerations:**  **Crucial:** Breaking down the numerous system settings into logical tabs or panels is essential to reduce cognitive load and make specific settings easier to find. Use clear, descriptive titles for each section.
        
-   **Configuration Forms (General):**
    
    -   **UI Elements:** Within each section, use appropriate `MudForm` controls: `MudTextField`, `MudNumericField`, `MudSelect`, `MudSwitch`, `MudFileUpload` (if needed for certs etc.), `RecurrencePatternEditor`, custom rule builders, `MudTable`s for lists (rules, templates, integrations). `MudButton` ("Save Settings") often per section or a global save.
        
    -   **User Interaction:** Admin interacts with standard form controls to view and modify settings. Saves changes. Uses tables to manage lists of rules/templates/integrations (Add/Edit/Delete actions usually opening `MudDialog`s).
        
    -   **ADHD Considerations:**
        
        -   **Consistency:** Use consistent layout and control types across different configuration sections where possible.
            
        -   **Clarity:** Provide clear labels for all settings. Use `MudTooltip`s or helper text (`HelperText` property) to explain complex or non-obvious settings.
            
        -   **Grouping:** Group related settings visually within each section (e.g., using `MudCard` or `MudPaper` with borders).
            
        -   **Feedback:** Provide clear visual feedback (`MudSnackbar`) upon saving changes. Indicate which settings require an application restart if applicable.
            
        -   **Safety:** Use confirmation `MudDialog`s for potentially disruptive actions (e.g., deleting a rule, changing security settings, triggering a manual backup). Disable save buttons if no changes have been made in a section.
            
        -   **Complexity Hiding:** For highly technical or rarely used settings, consider placing them within a nested `MudExpansionPanel` labeled "Advanced Settings".
            