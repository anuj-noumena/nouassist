## UI Breakdown: Personal Productivity Intelligence (PPI) Feature Slices

This document details the UI/UX design for the primary feature slices supporting the Personal Productivity Intelligence (PPI) Bounded Context, focusing on AI-driven personalization, predictions, recommendations, automation, and performance insights for the individual user.

### 1. `ProductivityProfileView` Slice (Optional User Slice)

**User Story:**

- "As a user interested in optimizing my work, I want to optionally view insights the system has learned about my productivity patterns, cognitive load tendencies, and learned preferences, so that I can gain deeper self-awareness about how I work best."

**Overall Layout & Structure:**

- **Container:** A dedicated page within user settings or a personal analytics area (`ProductivityProfilePage.razor` - `@page "/profile/productivity"`). Access should be optional and clearly labeled.
- **Organization:** Uses `MudCard`s or `MudList`s to present learned insights, grouped logically:

  - **Work Patterns:** Displays `LearnedWorkPatterns` (e.g., "Most focused work typically occurs between 9-11 AM", "Tends to batch communication tasks in the afternoon").
  - **Cognitive Load:** Shows trends or summaries based on `CognitiveLoadMetric` history (e.g., "Average cognitive load tends to increase during back-to-back meetings").
  - **Performance/Estimation:** Displays insights derived from `PerformanceDataPoint`s or `TaskCompletionRecord`s, including `EstimationBias` (e.g., "Accuracy on 'Development' tasks improving over last month").
  - **Learned Preferences:** Summarizes `UserPreferenceSetting`s learned by the system (e.g., "Prefers shorter focus blocks", "Responds well to checklist-style task breakdowns").

- **Primary Action(s):** Viewing the learned insights and patterns.

**Component Breakdown & Interactions:**

- **UI Elements:** `MudCard` (with `MudCardHeader`), `MudList`, `MudListItem`, `MudText`, potentially simple `Syncfusion Chart`s for trends.
- **User Interaction:** User navigates to the page and reads the displayed information. No direct editing is usually done here; preferences are typically learned or set elsewhere.
- **ADHD Considerations:**

  - **Clarity & Tone:** Present insights factually, clearly, and non-judgmentally. Use simple language and visuals. Focus on patterns and awareness, not scoring or ranking.
  - **Organization:** Group insights logically. Use headings and whitespace effectively. Avoid overwhelming the user with raw data; present synthesized patterns.
  - **Control/Opt-Out:** Ensure this view is optional. Clearly explain how the data is generated and used. Allow users to potentially reset or influence the profile if desired (though this might be handled via feedback mechanisms).

### 2. `Prediction/Recommendation UI` Slice (Integrated Component)

**User Story:**

- "As a user performing tasks like planning my schedule, estimating work, or prioritizing, I want to see context-aware, AI-driven predictions (e.g., task duration, completion likelihood) and recommendations (e.g., optimal time block, next best task, automation opportunity) presented unobtrusively, so that I can make more informed decisions and potentially improve my workflow."

**Overall Layout & Structure:**

- **Container:** Not a standalone slice. UI elements integrated directly into other feature slices where relevant:

  - **Task Management:** Near estimate fields (`PredictTaskCompletionTime`), in task details (recommendations for subtasks, prioritization `SuggestTaskPrioritization`).
  - **Time Management:** In `CalendarScheduleView` (suggested time blocks `OptimizeUserSchedule`), `FocusSession` setup (recommended duration).
  - **Workflow Optimization:** Suggesting `IdentifyRepetitiveTaskForAutomation`.

- **Organization:** Designed to be contextual and supplemental.

  - **Prediction Display:** `MudText` or `MudChip` near the relevant data point (e.g., "AI Est: 2.5h ± 0.5h", "Risk Prediction: Low"). May include confidence (`PredictionConfidenceLevel`) or an "Explain" link (`MudLink` triggering `GetPredictionExplanation`).
  - **Recommendation Display:** `MudAlert` (Severity="Info"), `MudList`, or contextual `MudButton`s (e.g., "Schedule Recommended Block", "Automate This Task?").

- **Primary Action(s):** Viewing predictions/recommendations, potentially accepting/applying them (triggering other commands), accessing explanations, providing feedback (via `FeedbackMechanism`), or ignoring them.

**Component Breakdown & Interactions:**

- **Prediction Display:**

  - **UI Elements:** `MudText`, `MudChip`, `MudLink`/`MudIconButton` ("Explain?").
  - **User Interaction:** User sees the prediction alongside their own input/data. Clicks "Explain" to see `ExplainabilityLog` / `FeatureImportance` data (in `MudPopover` or `MudDialog`).
  - **ADHD Considerations:** **Crucial:** Unobtrusive but clear. Label explicitly as AI prediction/suggestion. Don't make it look like a definite value. Explainability builds trust and understanding, reducing anxiety about "black box" AI. Must not block user input.

- **Recommendation Display:**

  - **UI Elements:** `MudAlert`, `MudList`, `MudListItem`, `MudButton` ("Accept Suggestion", "Automate?", "Schedule Now").
  - **User Interaction:** User sees recommendation. Clicks button to accept (triggers relevant command, e.g., `OptimizeUserSchedule`, `DefinePersonalAutomation`). Ignores or dismisses the recommendation.
  - **ADHD Considerations:** Present recommendations clearly and actionably. Make the suggested action obvious. Ensure recommendations are easy to ignore or dismiss without penalty. Avoid persistent or annoying recommendation popups.

### 3. `FeedbackMechanism` Slice (Integrated Component)

**User Story:**

- "As a user who received an AI prediction or recommendation, I want a very quick way to indicate if it was helpful, relevant, or accurate, so the system can learn and improve."

**Overall Layout & Structure:**

- **Container:** Integrated component, placed directly adjacent to the `Prediction/Recommendation UI` element.
- **Organization:** Extremely simple controls.
- **Primary Action(s):** Providing feedback (`RecordUserFeedback` command).

**Component Breakdown & Interactions:**

- **UI Elements:** `MudIconButton` pair (`Icons.Material.Filled.ThumbUp`, `Icons.Material.Filled.ThumbDown`), or possibly simple `MudChip`s ("Helpful", "Not Helpful").
- **User Interaction:** User clicks a feedback option. UI gives brief confirmation (e.g., icon highlights).
- **ADHD Considerations:** **Crucial:** One-click, zero-friction interaction. Universally understood icons. Optional, never mandatory. Doesn't interrupt workflow.

### 4. `PersonalAutomationManagement` Slice

**User Story:**

- "As a user, I want to define simple automations for my repetitive tasks (e.g., 'When I complete a 'Reporting' task, automatically log 1 hour to Project Y') and manage these automations, so I can reduce manual effort."

**Overall Layout & Structure:**

- **Container:** Dedicated page within settings or productivity tools (`AutomationsPage.razor` - `@page "/settings/automations"`).
- **Organization:**

  - **`Toolbar (MudToolBar):`** "Define New Automation" button (`MudButton`). Filter (`MudSelect` Status: Active/Inactive?).
  - **Main Content:** `MudTable` listing defined `PersonalAutomation`s. Columns: Name/Description, Trigger, Action(s), Status (`MudSwitch` for Active/Inactive), Effectiveness Metric (if tracked).

- **Adding/Editing:** Via `MudDialog` or dedicated page (`AutomationEditPage.razor`). Uses a simplified rule builder:

  - **Trigger:** Select event (`MudSelect`: e.g., Task Completed, Time Block Finished) + Conditions (e.g., "Task Tag IS 'Reporting'", "Project IS Project Y"). Uses filter-builder like UI.
  - **Action(s):** Select action (`MudSelect`: e.g., Log Time, Create Task, Send Notification) + Configure action parameters (e.g., Project for Log Time, Task details for Create Task). Can potentially chain simple actions.

- **Primary Action(s):** CRUD operations on personal automations, activating/deactivating automations (`DefinePersonalAutomation` command).

**Component Breakdown & Interactions:**

- **List View:**

  - **UI Elements:** `MudTable`, `MudTablePager`, `MudButton`, `MudSwitch`, `MudIconButton`s (Edit/Delete).
  - **User Interaction:** View automations. Activate/Deactivate using `MudSwitch`. Add/Edit/Delete via dialogs/pages.
  - **ADHD Considerations:** Clear list showing Trigger -> Action(s). Easy activation/deactivation.

- **Add/Edit Automation Interface:**

  - **UI Elements:** `MudDialog`/Page, `MudForm`, `MudSelect` (Trigger Event, Action), Condition Builder UI, Action Parameter inputs (dynamic based on selected action), `MudButton` ("Save Automation").
  - **User Interaction:** User selects trigger event, defines conditions, selects action(s), configures action parameters. Saves automation.
  - **ADHD Considerations:** **Crucial:** Keep the automation definition simple and visual if possible ("When [Trigger Event] matching [Conditions] happens, Then do [Action]"). Avoid complex coding-like interfaces. Use dropdowns with clear options. Break down definition into steps (`MudStepper`?) if needed (Trigger -> Conditions -> Action). Provide examples.

### 5. `PerformanceReportViewer` Slice

**User Story:**

- "As a user, I want to view periodic or on-demand generated reports summarizing my personal performance, productivity patterns, and cognitive load trends, so I can reflect on my work habits and well-being."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`PerformanceReportsPage.razor` - `@page "/reports/performance"`) or section within Personal Analytics.
- **Organization:**

  - **`Toolbar (MudToolBar):`** `MudDateRangePicker` or `MudSelect` to choose report period. "Generate Report Now" button (`MudButton`, if on-demand generation is supported).
  - **Main Content:** Displays the `PerformanceSnapshot` content. This is likely a mix of:

    - Key Metrics (`MudCard`s with `MudText`: e.g., Tasks Completed, Average Focus Duration, Estimated vs Actual Time Ratio).
    - Charts (`Syncfusion Chart`): Trends over the period for key metrics (e.g., Cognitive Load trend, Task Throughput).
    - Pattern Summaries (`MudText` or `MudList`): Textual descriptions of detected `BehaviorPatternDescription`s during the period.
    - Insight Text (`MudText`): Generated `InsightText` summarizing findings.

- **Primary Action(s):** Selecting report period, viewing metrics, charts, and insights (`GetPerformanceReport` query). Triggering on-demand generation (`GeneratePerformanceReport` command).

**Component Breakdown & Interactions:**

- **UI Elements:** `MudDateRangePicker`/`MudSelect`, `MudButton`, `MudCard`, `MudText`, `Syncfusion Chart`, `MudList`.
- **User Interaction:** User selects period. Views report content (metrics, charts, text). Clicks button to generate a new report for the selected period.
- **ADHD Considerations:** Clear, concise presentation of information. Use visuals (charts, cards) effectively. Summarize key insights prominently. Avoid overly dense reports. Ensure charts are easy to read and understand. Frame performance data constructively.

### 6. `NlpTaskInput` Slice (Component/Interface)

**User Story:**

- "As a user, I want to quickly capture tasks or schedule items by typing or speaking in natural language (e.g., 'Remind me to call Sarah tomorrow afternoon about the proposal'), and have the system parse it into structured data for confirmation, so I can capture actions with less friction."

**Overall Layout & Structure:**

- **Container:** Could be:

  - A dedicated input field in a quick-add `MudDialog`.
  - An option within the main `TaskCreateEdit` or `CalendarScheduleView` slices.
  - A voice input button (`MudIconButton`) triggering transcription and parsing.

- **Organization:**

  - **Input:** `MudTextField` for typing, or voice input mechanism.
  - **Parsing Feedback:** After input, display the `ParsedTaskData` in a structured preview format (e.g., "Action: Call Sarah", "Due: Tomorrow Afternoon", "Context: Proposal"). Use `MudChip`s or labeled `MudText`.
  - **Confirmation/Correction:** Allow user to edit parsed fields if incorrect. `MudButton` ("Create Task/Event"), `MudButton` ("Cancel/Clear").

- **Primary Action(s):** Providing natural language input, reviewing parsed data, confirming or correcting, creating the structured item (`ProcessNlpTaskInput` command leading to `CreateTask`, `CreateTimeBlock`, etc.).

**Component Breakdown & Interactions:**

- **UI Elements:** `MudTextField` / Voice Input Button, Display area for parsed data (`MudChip`, `MudText`), potentially editable fields for correction, `MudButton`s.
- **User Interaction:** User types or speaks input. System processes (`ProcessNlpTaskInput`) and displays parsed preview. User reviews preview. If correct, clicks "Create". If incorrect, edits fields and then clicks "Create".
- **ADHD Considerations:**

  - **Low Friction Input:** Make typing or voice input very easy to access.
  - **Clear Parsing Feedback:** **Crucial:** Clearly show how the natural language was interpreted into structured fields. Highlight key extracted entities (action, date, person, topic).
  - **Easy Correction:** If parsing is imperfect (which it often is), make it very easy for the user to quickly correct the structured fields before final creation. Avoid making the user re-type the whole natural language input.
  - **Speed:** The parsing and feedback loop should be fast.

### 7. `ContextReferenceManager` Slice (Admin/Setting Slice - Optional)

- **User Story:** "As an administrator or advanced user, I want to view or potentially manage the external context references (like Project IDs, Team IDs) associated with my productivity profile, so I can ensure the system has the correct context for personalization."
- **Layout/Structure:** Likely a simple `MudTable` within user settings listing `ExternalContextRef`s (Context Type: Project/Team, ID, Name?). Actions to refresh or potentially manually link/unlink if automatic detection fails.
- **ADHD Considerations:** Keep this very simple and clear. Only expose if manual management is truly necessary, as it adds complexity. Automatic context detection is preferred.

## UI Breakdown: Decision Support Feature Slices

This document details the UI/UX design for the primary feature slices supporting the Decision Support Bounded Context, focusing on structured decision-making, analysis, idea management, and strategic proposal formulation.

### 1. `DecisionModelInterface` Slice

**User Story:**

- "As a user facing a complex decision, I want to use a structured model to define criteria, evaluate alternatives, incorporate analyses like SWOT or ROI, and compare options, so that I can make a more rational and well-informed choice."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`DecisionModelPage.razor` - `@page "/decisions/model/{ModelInstanceId}"` or `/new`) or potentially a multi-step `MudDialog` for simpler decisions.
- **Organization:** Likely uses `MudStepper` or `MudTabs` to guide the process:

  - **Step/Tab 1: Definition:** Define the decision (`MudTextField`), overall goal.
  - **Step 2: Criteria:** Define `Criterion` items (`MudList` editor or `MudTable`). For each criterion: Name (`MudTextField`), Weight (`MudSlider` or `MudNumericField`).
  - **Step 3: Alternatives:** Define `Alternative` items (`MudList` editor or `MudTable`). For each alternative: Name (`MudTextField`), Description.
  - **Step 4: Evaluation:** A grid (`MudTable` or custom layout) where alternatives are scored against criteria (e.g., `MudNumericField` or `MudRating` in each cell).
  - **Step 5: Analysis (Optional Panels):** Sections (`MudExpansionPanel`s?) to input:

    - Qualitative Analysis: SWOT items (`MudTextField` grid/list per alternative), Risk Assessment (`MudTextField` description, `MudSelect` Probability/Impact).
    - Quantitative Analysis: ROI/Cost-Benefit inputs (`MudNumericField`s).

  - **Step 6: Results/Comparison:** Displays calculated scores/ranks (`MudTable`), comparison charts (`Syncfusion Chart`), summaries of analyses. Highlights the recommended alternative based on scoring.

- **Primary Action(s):** Creating/editing decision models, defining criteria and alternatives, scoring alternatives, adding qualitative/quantitative analysis, viewing comparison results (`CreateDecisionModel`, `DefineCriterion`, `AddAlternative`, `PerformRiskAnalysis`, `GenerateSwot`, `CalculateCostBenefit`, `CompareAlternatives` commands/queries).

**Component Breakdown & Interactions:**

- **Stepper/Tabs & Forms:**

  - **UI Elements:** `MudStepper`/`MudTabs`, `MudForm`, `MudTextField`, `MudList` editor / `MudTable`, `MudSlider`/`MudNumericField`/`MudRating`, `MudExpansionPanel`, `Syncfusion Chart`. `MudButton` ("Save Model").
  - **User Interaction:** User progresses through steps/tabs. User defines criteria, alternatives. User enters scores in the evaluation grid. User optionally adds SWOT/Risk/ROI data. User views the final comparison/results. Saves the model.
  - **ADHD Considerations:**

    - **Structure:** The `MudStepper` provides crucial structure, breaking down the potentially complex modeling process into manageable stages. Clear titles and instructions for each step.
    - **Clarity:** Use intuitive controls for weighting (`MudSlider`) and scoring (`MudRating`, `MudNumericField`). Make the evaluation grid easy to navigate and fill.
    - **Visualization:** Use charts effectively in the results step to visually compare alternatives based on scores. Keep charts clean and easy to interpret.
    - **Focus:** Minimize distractions during the modeling process. Hide optional analysis sections initially using `MudExpansionPanel`s. Auto-save drafts.

### 2. `StrategicInitiativeProposalManagement` Slice

**User Story:**

- "As a user with an approved idea or decision outcome, I want to formally propose a strategic initiative, outlining its rationale, expected outcomes, and linking supporting analysis, and track its approval status, so that it can be considered for formal project creation."

**Overall Layout & Structure:**

- **Container:** List page (`InitiativeProposalsPage.razor`) and Edit/View page (`InitiativeProposalDetailsPage.razor`).
- **List Page:** Toolbar ("Propose New Initiative" button), `MudTable` listing `StrategicInitiativeProposal`s (Proposal Title, Status [`MudChip`: Proposed, Approved, Rejected], Proposer, Date). Filters (Status, Proposer?).
- **Edit/View Page:** `MudForm` (for editing) or display sections (for viewing):

  - **Core Info:** Proposal Title (`MudTextField`), Description/Rationale (`MudTextField` multiline), Link to originating Decision Model/Idea (`DecisionModelSelector` / `IdeaSelector`), Strategic Goal Alignment (`StrategicGoalSelector` from Strategic Planning BC?).
  - **Expected Outcomes:** `MudTextField` (multiline) or `MudList` editor.
  - **Supporting Analysis:** Links (`MudLink` or file attachments via Reference BC?) to relevant SWOT, ROI, Risk Analysis, Decision Models.
  - **Status & Actions (View Mode):** Display current `ProposalStatus`. If user has permission: "Approve" button (`MudButton`), "Reject" button (`MudButton`). Approval might trigger project creation flow.

- **Primary Action(s):** CRUD operations on proposals, linking supporting data, viewing status, approving/rejecting proposals (`ProposeStrategicInitiative`, `ApproveInitiativeForProjectCreation`, `RejectInitiativeProposal` commands).

**Component Breakdown & Interactions:**

- **List Page:** Standard table interactions. Status chips provide quick overview.
- **Edit/View Page:**

  - **UI Elements:** `MudForm`/Display sections, `MudTextField`, `MudList` editor, Selectors (`DecisionModelSelector`, `IdeaSelector`, `StrategicGoalSelector`), `MudLink`, `MudChip` (Status), `MudButton`s (Save, Approve, Reject).
  - **User Interaction:** Define proposal details, link supporting info. Save. View existing proposal. Approvers click Approve/Reject (likely with confirmation `MudDialog` and optional comments).
  - **ADHD Considerations:** Provide a clear, structured template for proposals. Make the current status very obvious. Keep the approval/rejection actions simple and clear for authorized users. Ensure links to supporting documents/analyses are easy to access.

### 3. `ScenarioAnalysisInterface` Slice

**User Story:**

- "As a user planning for the future or assessing risks, I want to define different potential scenarios (e.g., Best Case, Worst Case, Most Likely), model their key variables and assumptions, and analyze their projected outcomes, so that I can make more resilient plans and decisions."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`ScenarioAnalysisPage.razor` - `@page "/analysis/scenarios/{AnalysisId}"` or `/new`) or potentially a feature within `DecisionModelInterface`.
- **Organization:**

  - **Toolbar:** "Add Scenario" button (`MudButton`). Maybe options to compare selected scenarios.
  - **Main Area:** Uses `MudGrid` or `MudTabs` to display/manage multiple `ScenarioModel`s within the `ScenarioAnalysis`.
  - **Scenario Card/Tab:** For each scenario:

    - Title/Name (`MudTextField`).
    - Description (`MudTextField`).
    - Key Variables/Assumptions (`MudList` editor or `MudTable` with Name/Value fields).
    - Projected Outcomes (`MudTextField` or structured fields).

  - **Comparison View (Optional):** A `MudTable` or side-by-side layout comparing key outcomes across scenarios.

- **Primary Action(s):** Creating/editing scenarios, defining variables/assumptions/outcomes for each, comparing scenarios (`DevelopScenarioModel`, `AnalyzeScenarioOutcomes` commands).

**Component Breakdown & Interactions:**

- **UI Elements:** `MudGrid`/`MudTabs`, `MudCard`/`MudTabPanel`, `MudTextField`, `MudList` editor / `MudTable`, `MudButton` ("Add Scenario", "Save Analysis"). Comparison `MudTable`.
- **User Interaction:** User adds/names scenarios. User defines assumptions and projected outcomes for each scenario. User views scenarios side-by-side or in a comparison table. Saves the analysis.
- **ADHD Considerations:** Clear separation between different scenarios (Tabs or distinct Cards). Structured input for assumptions/outcomes within each scenario. Make the comparison view easy to scan and understand key differences between scenarios.

### 4. `DecisionJournal` Slice

**User Story:**

- "As a user focused on improving my decision-making, I want to log important decisions, including the rationale, assumptions made, and context at the time, and later record the actual outcome, so that I can reflect on my process and learn from experience."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`DecisionJournalPage.razor` - `@page "/journal/decisions"`).
- **Organization:**

  - **Toolbar:** "Log New Decision" button (`MudButton`), Date filter (`MudDatePicker`), Search (`MudTextField`).
  - **Main Content:** `MudList` or `MudTable` displaying `DecisionJournalEntry` summaries (Decision Title/Summary, Date Made, Outcome Status [Pending/Recorded]).

- **Adding/Editing:** Via `MudDialog` or dedicated page (`DecisionJournalEntryPage.razor`). Structured `MudForm`:

  - Decision Made (`MudTextField`).
  - Rationale (`MudTextField` multiline).
  - Key Assumptions (`MudList` editor or `MudTextField`).
  - Context/Situation (`MudTextField`).
  - Expected Outcome (`MudTextField`).
  - Date Made (`MudDatePicker`).
  - **Later:** Actual Outcome (`MudTextField` - editable after decision outcome is known), Outcome Date (`MudDatePicker`), Lessons Learned (`MudTextField`).

- **Primary Action(s):** Logging new decisions, viewing past entries, searching/filtering, updating entries with actual outcomes and lessons learned (`RecordDecisionInJournal` command).

**Component Breakdown & Interactions:**

- **List View:** Standard list/table interactions. Filter by date or search. Click entry to view/edit details.
- **Add/Edit Entry Form:**

  - **UI Elements:** `MudForm`, `MudTextField`, `MudList` editor, `MudDatePicker`, `MudButton` ("Save Entry", "Log Outcome").
  - **User Interaction:** User fills in details about the decision when made. Saves. Later, user edits the entry to add the actual outcome and lessons learned.
  - **ADHD Considerations:** Provide a structured template to guide journaling, making it easier than free-form writing. Clear separation between initial logging and later outcome logging. Easy search/filtering to find past decisions for review.

### 5. `MentalModelLibrary` Slice

**User Story:**

- "As a user seeking better thinking tools, I want to browse a library of defined mental models (like 'Inversion' or 'Second-Order Thinking'), understand their descriptions and usage guidance, so that I can learn and apply them to my decision-making and problem-solving."

**Overall Layout & Structure:**

- **Container:** Dedicated page or section within a knowledge base (`MentalModelsPage.razor` - `@page "/library/mental-models"`).
- **Organization:**

  - **Toolbar:** Filter/Search (`MudTextField`). Maybe "Suggest New Model" button? If admins manage: "Add Model" button.
  - **Main Content:** `MudGrid` with `MudCard` per `MentalModelReference`, or `MudList`. Each item shows Model Name, brief description. Clicking expands or navigates to details.

- **Detail View:** (Opened via click or navigation) Displays full `ModelDescription` and usage guidance (`MudText` with formatting). Maybe links to related models or examples.
- **Primary Action(s):** Browsing models, searching, viewing model details. Admin: CRUD operations on models.

**Component Breakdown & Interactions:**

- **List/Grid View:**

  - **UI Elements:** `MudGrid`/`MudList`, `MudCard`/`MudListItem`, `MudTextField` (Search). Admin: `MudButton` ("Add Model").
  - **User Interaction:** Browse/Search models. Click a model to view details. Admin: Add/Edit/Delete models (via dialog).
  - **ADHD Considerations:** Visually appealing cards or clear list. Good search functionality. Easy navigation to details.

- **Detail View:**

  - **UI Elements:** `MudText` (displaying formatted description/guidance).
  - **User Interaction:** Read model details.
  - **ADHD Considerations:** High readability. Well-structured content using headings, lists, bold text to break up information and improve comprehension.

### 6. `InnovationIdeaManagement_DS` Slice

**User Story:**

- "As a user within the decision support context, I want to capture innovative ideas, view them in a pipeline, participate in their evaluation and prioritization, so that promising ideas can be identified and potentially developed into strategic initiative proposals."

**Overall Layout & Structure:** _(Note: Assumes this handles ideas if a separate IH BC doesn't exist or focuses specifically on ideas leading to strategic decisions)_

- **Container:** Dedicated page (`IdeasPage.razor` - `@page "/ideas"`).
- **Organization:** Similar to Innovation Hub (IH) slices, adapted for Decision Support context.

  - **Toolbar:** "Capture Idea" button (`MudButton`), Filters (`MudSelect` Status/Pipeline Stage, Tags?).
  - **Main Content:** `MudTable` or Kanban Board (`MudCard`s in columns representing `PipelineStage`: Captured, Evaluating, Prioritized, Proposed).
  - **Capture:** `IdeaCapture` slice/dialog (Simple form: Title, Description, Source).
  - **Details View:** `IdeaDetailsView` slice/dialog/page (Shows details, evaluation criteria/results, link to `StrategicInitiativeProposal` if created). Includes `IdeaEvaluation` component/form.
  - **Evaluation:** `IdeaEvaluation` slice/component (Structured form based on `IdeaEvaluationCriteria`).

- **Primary Action(s):** Capturing ideas, viewing idea pipeline/list, evaluating ideas, prioritizing ideas, potentially initiating `ProposeStrategicInitiative` from a prioritized idea.

**Component Breakdown & Interactions:**

- **Capture:** Simple dialog/form as described for IH `IdeaCapture`.
- **List/Board View:** Standard table or Kanban board interactions. Drag-and-drop on Kanban to change stage? Filter by stage/tags. Click idea for details.
- **Details View:** Displays idea info. Includes embedded `IdeaEvaluation` form/display. Button to "Propose Initiative".
- **Evaluation:** Structured form as described for IH `IdeaEvaluation`.
- **ADHD Considerations:** Kanban view can provide good visual flow for the pipeline. Low friction capture. Structured evaluation. Clear status indicators. Keep details view organized (Tabs/Panels).

### 7. `WisdomRepository` Slice

**User Story:**

- "As a user, I want to browse and contribute to a repository of core principles, insights, or 'laws' relevant to our work or decision-making, so that we can build and share collective wisdom."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`WisdomPage.razor` - `@page "/wisdom"`).
- **Organization:**

  - **Toolbar:** "Add Principle" button (`MudButton`), Search/Filter (`MudTextField`).
  - **Main Content:** `MudList` or `MudCard`s displaying `PrincipleDefinition`s (Principle Statement, Description/Context).

- **Adding/Editing:** Via `MudDialog`. Fields: Principle Statement (`MudTextField`), Description/Context (`MudTextField` multiline).
- **Primary Action(s):** Browsing principles, searching, adding new principles, editing existing ones.

**Component Breakdown & Interactions:**

- **List View:** View principles. Search/Filter. Click "Add Principle". Click existing principle to view/edit (opens dialog).
- **Add/Edit Dialog:**

  - **UI Elements:** `MudDialog`, `MudForm`, `MudTextField`s, `MudButton` ("Save").
  - **User Interaction:** Enter/edit principle statement and description. Save.
  - **ADHD Considerations:** Simple list view. Clear, concise principle statements displayed prominently. Easy-to-use editor dialog.

### 8. `ClarityToolsInterface` Slice

**User Story:**

- "As a user seeking clarity on a complex problem or decision, I want to use guided tools like thought distillation or assumption excavation, so that I can break down complexity and gain deeper understanding."

**Overall Layout & Structure:**

- **Container:** Could be a dedicated page (`ClarityToolsPage.razor`) offering different tools, or specific tools launched as `MudDialog`s or guided flows when needed (e.g., from a Decision Model or Journal entry).
- **Organization (Example: Thought Distillation Tool):**

  - Uses `MudStepper` to guide the process.
  - Step 1: Input initial thought/problem (`MudTextField` multiline).
  - Step 2: Guided questions/prompts (`MudText`) with input fields (`MudTextField`) to break it down (e.g., "What's the core issue?", "What are the key components?").
  - Step 3: Synthesize/Summarize (`MudTextField` for `DistilledThought`).
  - Step 4: Review & Save (`ClaritySession` saved).

- **Organization (Example: Assumption Excavation Tool):**

  - Input the statement/decision (`MudTextField`).
  - Guided prompts (`MudText`) to list underlying assumptions (`MudList` editor with `MudTextField` per assumption).
  - Interface to challenge/test each assumption (`MudTextField` for counter-arguments/evidence).

- **Primary Action(s):** Engaging with specific clarity tools, following guided steps, inputting thoughts/assumptions, saving the session results (`DistillThoughts`, `ExcavateAssumptions` commands).

**Component Breakdown & Interactions:**

- **UI Elements:** `MudStepper`, `MudStep`, `MudText` (Prompts), `MudTextField` (Inputs), `MudList` editor, `MudButton`s ("Next", "Back", "Save Session").
- **User Interaction:** User follows the steps provided by the tool, entering text in response to prompts. User saves the completed session.
- **ADHD Considerations:**

  - **Structure:** `MudStepper` provides excellent structure for these guided thinking processes, preventing overwhelm.
  - **Focus:** Each step should focus on a single task/question. Keep prompts clear and concise.
  - **Guidance:** Provide clear instructions throughout the process.
  - **Saving:** Allow saving progress if the process is long. Provide clear confirmation upon completion.

## UI Breakdown: Workflow Engine Feature Slices

This document details the UI/UX design for the primary feature slices supporting the Workflow Engine Bounded Context, focusing on designing, managing, executing, and monitoring automated or semi-automated processes.

### 1. `WorkflowDesigner` Slice

**User Story:**

- "As a process designer or administrator, I want a visual interface to design, configure, and version workflow definitions using standard notation (like BPMN), including activities, conditional logic, automation rules, and task assignments, so that I can accurately model and automate business processes."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`WorkflowDesignerPage.razor` - `@page "/workflows/designer/{DefinitionId}"` or `/new`). This is typically a complex interface.
- **Organization:**

  - **`Toolbar (MudToolBar):`** Save Definition/Version (`MudButton`), Validate Workflow (`MudButton`), Zoom controls, potentially import/export BPMN (`MudButton`).
  - **`` `Palette/Toolbox Panel (MudPaper or MudDrawer):` ``** Contains draggable BPMN elements (Start Event, End Event, User Task, Service Task, Gateways, Sequence Flows, etc.).
  - **Main Canvas Area:** Large area where the visual modeling occurs. Requires integration with a JavaScript BPMN modeling library (like `bpmn-js`). Users drag elements from the palette onto the canvas and connect them.
  - **`Properties Panel (MudPaper typically docked to the side):`** Displays configuration options for the currently selected element on the canvas. Content changes dynamically based on selection.

    - **Activity Config:** Name, Description, Assignee (`AssigneeReference` selector: User/Team/Role), Form definition (if user task), Automation script/service call details (if service task).
    - **Gateway Config:** Condition expressions (`ConditionExpression` editor).
    - **Sequence Flow Config:** Conditions (if applicable).

- **Primary Action(s):** Visually designing workflows by dragging, dropping, and connecting elements; configuring properties of selected elements (activities, gateways, flows); defining task assignments; setting conditions; defining automation rules; saving and versioning workflow definitions (`DesignWorkflow`, `DefineActivity`, `SetCondition`, `DefineAutomationRule`, `VersionWorkflow` commands).

**Component Breakdown & Interactions:**

- **Toolbar:** Standard button interactions for Save, Validate, Zoom, Import/Export.
- **Palette/Toolbox:** User drags elements from here onto the canvas.
- **Main Canvas (JS Library Integration):**

  - **UI Elements:** Visual representation of the BPMN diagram. Nodes (events, tasks, gateways), Edges (sequence flows). Selection highlighting. Connection points.
  - **User Interaction:** Drag elements from palette. Drag elements on canvas to position. Drag connection points to create sequence flows. Select elements (single click) to show properties. Double-click elements (optional) for quick editing (e.g., rename).
  - **ADHD Considerations:**

    - **Visual Clarity:** The diagram must be clear and easy to read. Use standard BPMN icons consistently. Ensure connection lines are distinct and avoid unnecessary crossing. Good alignment tools help.
    - **Intuitive Interaction:** Drag-and-drop should be smooth and predictable. Element selection and property panel updates should be immediate.
    - **Complexity Management:** For large workflows, provide good zoom/pan capabilities. Consider features like collapsing sub-processes to manage visual complexity.

- **Properties Panel:**

  - **UI Elements:** `MudPaper` containing dynamic `MudForm` controls based on selected element type: `MudTextField`, `MudSelect`, `MudSwitch`, custom components for `AssigneeReference` selection (User/Team/Role selectors), condition expression editor (could be simple text or a visual builder), form builder link/integration.
  - **User Interaction:** User selects element on canvas. User modifies properties in the panel (e.g., enters name, selects assignee, writes condition). Changes might auto-save or require explicit save action.
  - **ADHD Considerations:**

    - **Focus:** Panel should only show properties relevant to the selected element. Clear labels and logical grouping of properties.
    - **Structure:** Use `MudExpansionPanel`s within the properties panel for complex elements with many settings (e.g., advanced automation rules).
    - **Guidance:** Provide tooltips (`MudTooltip`) or helper text for complex properties like condition expressions or assignee types.
    - **Validation:** Provide real-time validation feedback within the properties panel where possible. The main "Validate Workflow" button checks overall consistency.

- **Saving/Versioning:** Clear save button. Handle versioning explicitly (e.g., "Save as New Version" button).

### 2. `WorkflowTemplateManagement` Slice

**User Story:**

- "As a user or administrator, I want to browse, search, and manage reusable workflow templates, so that I can quickly start new workflow instances based on standardized processes or share best practices."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`WorkflowTemplatesPage.razor` - `@page "/workflows/templates"`).
- **Organization:**

  - **`Toolbar (MudToolBar):`** "Create Template" button (`MudButton` - might navigate to `WorkflowDesigner` in template mode), Filter/Search (`MudTextField`).
  - **Main Content:** `MudTable` or `MudGrid` card view listing `WorkflowTemplate`s. Columns/Card Info: Template Name, Description, Version, Last Updated, Actions.

- **Primary Action(s):** Browsing templates, searching/filtering, creating new templates, editing existing templates (opens `WorkflowDesigner`), creating an instance from a template (`ExecuteWorkflowInstance` command initiated from here or elsewhere), sharing/deleting templates (`CreateWorkflowTemplate`, `ManageWorkflowTemplates`, `ShareWorkflowTemplate` commands).

**Component Breakdown & Interactions:**

- **List/Grid View:**

  - **UI Elements:** `MudTable`/`MudGrid`+`MudCard`, `MudButton`, `MudTextField`. Actions per item (`MudMenu` or `MudIconButton`s: Use Template / Start Instance, Edit Template, Share, Delete).
  - **User Interaction:** View/Search/Filter templates. Click "Create Template". Click "Use Template" / "Start Instance" (might open dialog for instance-specific parameters before execution). Click "Edit Template". Click other actions (Share, Delete with confirmation).
  - **ADHD Considerations:** Clear listing with easy searching. Visual previews or good descriptions of templates are helpful. Make the "Use Template" action prominent.

### 3. `WorkflowInstanceMonitoring` Slice

**User Story:**

- "As an administrator or involved user, I want to monitor the status and progress of running workflow instances, view execution history and logs for completed or failed instances, and potentially intervene if issues arise, so that I can ensure workflows execute correctly and troubleshoot problems."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`WorkflowInstancesPage.razor` - `@page "/workflows/instances"`).
- **Organization:**

  - **`Toolbar (MudToolBar):`** Filters (`MudSelect` for Workflow Definition, `MudSelect` for Status [Running, Completed, Failed, etc.], `MudDateRangePicker` for Start/End Time). Auto-refresh toggle (`MudSwitch`)?
  - **Main Content:** `MudTable` listing `WorkflowInstance`s. Columns: Instance ID, Workflow Definition Name (`MudLink` to definition?), Start Time, End Time (if applicable), Duration, Status (`MudChip`), Current Activity/Step. Action button/link ("View Details") per row.

- **Details View:** (Opened via "View Details" - could be `MudDialog` or separate page `WorkflowInstanceDetailsPage.razor`)

  - **Header:** Instance ID, Definition Name, Status, Timestamps. Actions (Pause, Resume, Cancel - `MudButton`s, conditionally enabled).
  - **Visual Progress:** Display the workflow diagram (read-only view from `WorkflowDesigner`?) with highlighting indicating completed steps, the currently active step(s), and potentially error states.
  - **Execution Log:** `MudTable` showing `WorkflowExecutionLog` entries chronologically (Timestamp, Activity Name, Event Type, Details).
  - **Variables/Context:** (Optional) Display current workflow variable values.
  - **Exceptions:** If Status is Failed, display `WorkflowExceptionRecord` details (`MudAlert` or dedicated section).

- **Primary Action(s):** Viewing list of instances, filtering instances, viewing detailed status/log/visual progress of a specific instance, performing instance control actions (Pause, Resume, Cancel - `HandleWorkflowException` related commands).

**Component Breakdown & Interactions:**

- **List View:**

  - **UI Elements:** `MudTable`, `MudTablePager`, `MudSelect`, `MudDateRangePicker`, `MudSwitch` (optional refresh), `MudChip` (Status), `MudLink`, `MudButton` ("View Details").
  - **User Interaction:** View instance list. Filter/Sort list. Click "View Details".
  - **ADHD Considerations:** Clear status indicators (`MudChip` with colors) are essential. Effective filtering helps manage potentially large lists. Auto-refresh can be useful but should be controllable (`MudSwitch`) as constant updates can be distracting.

- **Details View:**

  - **UI Elements:** `MudDialog`/Page sections, `MudChip`, `MudButton`s (Pause/Resume/Cancel), Visual diagram component (read-only), `MudTable` (Logs), `MudAlert` (Exceptions).
  - **User Interaction:** View instance details. Review visual progress on diagram. Examine execution logs. View exception details if failed. Click Pause/Resume/Cancel buttons (show confirmation `MudDialog`).
  - **ADHD Considerations:**

    - **Visual Progress:** Highlighting progress on the workflow diagram provides an intuitive overview. Keep it clean and easy to understand.
    - **Logs/Errors:** Make logs easy to read and filter. Highlight errors clearly in logs and the dedicated exception section.
    - **Actions:** Make control actions (Pause/Resume/Cancel) clear and provide confirmation, explaining the consequences.

### 4. `WorkflowTaskInterface` Slice

**User Story:**

- "As a user assigned a task by an automated workflow, I want to see the task in my work queue, understand its context within the workflow, complete any required actions or forms, and mark the task as complete, so that the workflow can proceed."

**Overall Layout & Structure:**

- **Container:** This UI might be:

  - Integrated into a unified Task List (e.g., Task BC's `TaskList` slice, with a way to identify workflow tasks).
  - A separate queue/page dedicated to workflow tasks (`WorkflowTasksPage.razor` - `@page "/workflow-tasks"`).

- **Organization (Task List):**

  - `MudTable` listing `TaskAssignment`s. Columns: Task Name/Description, Workflow Instance ID (`MudLink` to `WorkflowInstanceMonitoring` details?), Due Date, Priority, Assigned Date, Status (`MudChip`). Action button ("Open Task").

- **Organization (Task Completion Form):** (Opened via "Open Task" - typically a `MudDialog` or dedicated page)

  - **Context:** Display information about the workflow instance and the specific activity that generated the task.
  - **Form (If required):** Dynamically render form fields defined in the `ActivityDefinition`. Uses standard `MudForm` controls (`MudTextField`, `MudSelect`, etc.).
  - **Instructions:** Clear instructions (`MudText`) on what needs to be done.
  - **Actions:** `MudButton` ("Complete Task"), `MudButton` ("Save Draft / Cancel"). Potentially other actions defined by the workflow (e.g., "Reject", "Request Clarification").

- **Primary Action(s):** Viewing assigned workflow tasks, understanding task context, completing required forms/actions, submitting task completion (`UpdateTaskStatus` command within Workflow context).

**Component Breakdown & Interactions:**

- **Task List:** Standard table interactions. Filter by workflow definition or status. Click "Open Task".
- **Task Completion Form:**

  - **UI Elements:** `MudDialog`/Page, `MudText` (Context/Instructions), `MudForm` with dynamic controls, `MudButton`s.
  - **User Interaction:** User reviews context and instructions. User fills in required form fields (if any). User clicks "Complete Task" (or other workflow-defined action).
  - **ADHD Considerations:**

    - **Clarity:** Clearly distinguish workflow tasks from other tasks if lists are combined. Provide sufficient context about _why_ this task exists (which workflow, what step). Instructions must be unambiguous.
    - **Simplicity:** Keep the required form fields focused on what's necessary for the workflow step. Avoid unnecessary complexity.
    - **Focus:** Minimize distractions within the task completion interface.
    - **Feedback:** Clear confirmation upon task completion (`MudSnackbar`) and update the task status in the list.

## 2. Team BC Feature Slices

### 2.1. `TeamListView` Slice

**User Story:**

- "As an administrator or manager, I want to see a list of all teams within the organization, so I can manage team structures and memberships."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`TeamsPage.razor` - `@page "/settings/teams"`).
- **Organization:**
    - **`Toolbar (MudToolBar):`** "Create Team" button (`MudButton`), Search field (`MudTextField`).
    - **Main Content:** `MudTable` listing teams. Columns: Team Name (`MudLink` to `TeamDetailView`), Purpose, Member Count. (`GetTeamDetails` for basic list).

- **Primary Action(s):** Viewing teams, searching, navigating to team details, initiating team creation (`CreateTeam`).

**Component Breakdown & Interactions:**

- **UI Elements:** `MudTable`, `MudToolBar`, `MudButton`, `MudTextField`, `MudLink`.
- **User Interaction:** User views list, searches, clicks team name link, clicks "Create Team".
- **ADHD Considerations:**
    - **Clear List:** Simple table layout.
    - **Easy Navigation:** Clear links to detail views.

### 2.2. `TeamDetailView` Slice

**User Story:**

- "As an administrator or manager, I want to view the details of a specific team, including its purpose and members with their roles, so I can manage the team's composition and assignments."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`TeamDetailPage.razor` - `@page "/settings/teams/{TeamId:guid}"`).
- **Organization:**
    - **Header:** Team Name (`MudText`), Purpose (`MudText`), Edit button (`MudIconButton` -> `TeamEditForm`), Disband button (`MudButton` with confirmation).
    - **Main Content:** `MudTable` listing `TeamMember`s. Columns: Member Name (`UserID`/`MemberName`), Assigned Role (`MudChip`), Date Joined. Add Member button (`MudButton` -> `AddMemberDialog`). (`GetTeamMembers`, `GetMemberRoles`)
- **Primary Action(s):** Viewing team details, adding/removing members (`AddMemberToTeam`, `RemoveMemberFromTeam`), assigning/revoking roles (`AssignRoleToMember`, `RevokeRoleFromMember` - likely via member edit action), updating team details (`UpdateTeamDetails`), disbanding team (`DisbandTeam`).

**Component Breakdown & Interactions:**

- **UI Elements:** `MudText`, `MudIconButton`, `MudButton`, `MudTable`, `MudChip`, `MudDialog`.
- **User Interaction:** User views details. Clicks "Add Member" (opens dialog to search/select users and assign role). Clicks actions on table rows (e.g., "Edit Role", "Remove Member"). Clicks "Edit" for team details, "Disband" to delete.
- **ADHD Considerations:**
    - **Clear Structure:** Clearly separate team details from the member list.
    - **Actionable List:** Make adding/removing/editing members straightforward from the list.
    - **Role Clarity:** Display assigned roles clearly (e.g., using chips).

### 2.3. `RoleManagementView` Slice

**User Story:**

- "As an administrator, I want to define and manage roles within the organization, specifying their descriptions and associated capabilities, so that roles can be consistently assigned across teams."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`RolesPage.razor` - `@page "/settings/roles"`).
- **Organization:**
    - **`Toolbar (MudToolBar):`** "Define Role" button (`MudButton`).
    - **Main Content:** `MudTable` listing defined `Role`s. Columns: Role Name (`MudLink` to `RoleDetailView`), Description. (`GetRoleDefinition` for list).

- **Primary Action(s):** Viewing defined roles, initiating role definition (`DefineRole`), navigating to role details.

**Component Breakdown & Interactions:**

- **UI Elements:** `MudTable`, `MudToolBar`, `MudButton`, `MudLink`.
- **User Interaction:** User views list, clicks "Define Role", clicks role name link.

### 2.4. `RoleDetailView` Slice (Could be combined with Edit)

**User Story:**

- "As an administrator, I want to view and edit the details of a specific role, including its description and the capabilities associated with it."

**Overall Layout & Structure:**

- **Container:** Dedicated page or dialog (`RoleDetailPage.razor` / `RoleEditDialog`).
- **Organization:** `MudForm` for editing:
    - `RoleName` (`MudTextField`)
    - `RoleDescription` (`MudTextField`)
    - Capabilities (`MudAutocomplete` or `MudChipGroup` with Add/Remove functionality, allowing selection/creation of `CapabilityTag`s). (`AddCapabilityToRole`, `UpdateRole`)
- **Primary Action(s):** Viewing, defining, or updating role details and associated capabilities.

**Component Breakdown & Interactions:**

- **UI Elements:** `MudForm`, `MudTextField`, `MudAutocomplete`/`MudChipGroup`, `MudButton` ("Save").
- **User Interaction:** User edits fields, adds/removes capability tags, saves changes.
- **ADHD Considerations:**
    - **Capability Management:** Make adding/viewing capabilities associated with a role intuitive (e.g., tag-like input).
    - **Clear Purpose:** Ensure the role's purpose and associated capabilities are clearly displayed.

## 3. Analytics & Reporting BC Feature Slices

### 3.1. `DashboardViewer` Slice

**User Story:**

- "As a user, I want to view my configured dashboards with various widgets displaying key metrics, charts, and reports, so I can get a quick overview of project progress, performance, or other relevant data."

**Overall Layout & Structure:**

- **Container:** Main application dashboard page or a dedicated dashboards section (`DashboardPage.razor` - `@page "/dashboards/{DashboardId:guid}"`).
- **Organization:**
    - **`Toolbar (MudToolBar):`** Dashboard selector (`MudSelect`), "Edit Layout" button (`MudButton`), "Add Widget" button (`MudButton`), Refresh button (`MudIconButton`).
    - **Main Content:** A grid system (`MudGrid`) displaying `WidgetInstance`s based on `DashboardLayoutSetting`. Each widget is a `MudCard`. Widget content varies based on `WidgetType` (KPI, Chart, Report Snippet, etc.). Data fetched via `GetWidgetData`.

- **Primary Action(s):** Viewing dashboards, switching between dashboards, initiating layout editing or widget addition.

**Component Breakdown & Interactions:**

- **UI Elements:** `MudGrid`, `MudItem`, `MudCard`, `MudCardHeader`, `MudCardContent`, `MudSelect`, `MudButton`, `MudIconButton`. Widgets themselves might contain `MudText`, `Syncfusion Chart`, `MudSimpleTable`, etc.
- **User Interaction:** User selects and views a dashboard. Widgets load data. User clicks buttons to enter edit mode or add new widgets.
- **ADHD Considerations:**
    - **Customization:** Allow users to arrange dashboards to show what's most important to them first.
    - **Visual Clarity:** Ensure widgets are well-designed, charts are easy to read, and KPIs stand out. Avoid overly dense dashboards.
    - **Loading States:** Provide clear loading indicators for widgets fetching data.
    - **Minimize Distractions:** Keep the dashboard focused on presenting information clearly without unnecessary animations or clutter.

### 3.2. `DashboardEditor` Slice

**User Story:**

- "As a user, I want to create or edit a dashboard, arranging widgets, adding new ones from a library, and configuring their data sources, so I can personalize my overview."

**Overall Layout & Structure:**

- **Container:** Same page as `DashboardViewer`, but in an "edit mode".
- **Organization:**
    - **`Toolbar (MudToolBar):`** "Save Layout" button, "Cancel" button, "Add Widget" button (opens `WidgetLibraryDialog`).
    - **Main Content:** `MudGrid` where `MudItem`s (widgets) can be resized and dragged-and-dropped (requires JS interop/library). Widgets show configuration options (`MudIconButton` "Configure") or remove buttons (`MudIconButton` "Remove"). (`ConfigureDashboardLayout`, `AddWidgetToDashboard`, `ConfigureWidgetDataSource`)

- **Primary Action(s):** Adding, removing, configuring, resizing, and repositioning widgets. Saving the dashboard layout.

**Component Breakdown & Interactions:**

- **UI Elements:** `MudGrid`, `MudItem`, `MudCard`, `MudButton`, `MudIconButton`, Drag-and-drop library integration. `WidgetLibraryDialog`, `WidgetConfigurationDialog`.
- **User Interaction:** User drags widgets to rearrange. Clicks "Add Widget" to select from a library. Clicks configure icon on a widget to open settings dialog (selecting data source, chart type, parameters). Clicks "Save Layout".
- **ADHD Considerations:**
    - **Intuitive Editing:** Make drag-and-drop and resizing intuitive. Provide visual cues during editing.
    - **Widget Library:** Organize the widget library clearly (by data source, type).
    - **Configuration Simplicity:** Keep widget configuration dialogs focused and step-by-step if complex.
    - **Undo/Cancel:** Provide easy ways to cancel edits or potentially undo actions.

### 3.3. `ReportConfigurationView` Slice

**User Story:**

- "As an analyst or manager, I want to define the configuration for a specific report, including its data source, parameters, format, and schedule, so that standardized reports can be generated automatically."

**Overall Layout & Structure:**

- **Container:** Dedicated page or section (`ReportConfigsPage.razor` - `@page "/reports/configure"`). List view + Detail/Edit view.
- **Organization (List View):**
    - **`Toolbar (MudToolBar):`** "Define Report" button.
    - **Main Content:** `MudTable` listing `ReportConfiguration`s. Columns: Name, Data Source, Format, Schedule.
- **Organization (Detail/Edit View - `MudDialog` or Page):**
    - `MudForm` with fields for:
        - Report Name/Description (`MudTextField`)
        - `DataSource` (`MudSelect` listing configured sources like event streams/views) (`ConfigureEventDataSource`)
        - `ReportParameter`s (dynamic fields based on source)
        - `DataTransformationStep`s (optional, simplified UI)
        - `ReportFormat` (`MudSelect` Enum: PDF, CSV)
        - `ReportSchedule` (`MudSelect` for frequency, `MudTimePicker`/`MudDatePicker` for time) (`ScheduleReportGeneration`)
        - Export Settings (`MudTextField` for destination, etc.)

- **Primary Action(s):** CRUD operations on `ReportConfiguration`s (`DefineReport`, `SetReportParameters`, `ScheduleReportGeneration`).

**Component Breakdown & Interactions:**

- **UI Elements:** `MudTable`, `MudToolBar`, `MudButton`, `MudDialog`/Page, `MudForm`, `MudTextField`, `MudSelect`, `MudTimePicker`, `MudDatePicker`.
- **User Interaction:** User views list, clicks "Define Report" or edits existing. Fills configuration form, selects data source, defines parameters, sets schedule, saves.
- **ADHD Considerations:**
    - **Guided Configuration:** Break down report definition into logical steps (`MudStepper` could be useful).
    - **Data Source Clarity:** Clearly explain what each data source represents.
    - **Scheduling Simplicity:** Use common language for scheduling options (Daily, Weekly, Monthly).
    - **Previews:** If possible, offer a preview of the report structure or data based on configuration.

### 3.4. `GeneratedReportViewer` Slice

**User Story:**

- "As a user, I want to view or download reports that have been generated based on predefined configurations, so I can access historical data and analysis."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`GeneratedReportsPage.razor` - `@page "/reports/view"`).
- **Organization:**
    - **`Toolbar (MudToolBar):`** Filter by Report Name (`MudSelect`), Date Range (`MudDateRangePicker`).
    - **Main Content:** `MudTable` listing `ReportGenerationLog` entries (or the generated reports themselves). Columns: Report Name, Generation Time, Status, Download Link (`MudLink`/`MudButton`). (`GetGeneratedReport`)

- **Primary Action(s):** Viewing list of generated reports, filtering, downloading a specific report.

**Component Breakdown & Interactions:**

- **UI Elements:** `MudTable`, `MudToolBar`, `MudSelect`, `MudDateRangePicker`, `MudLink`/`MudButton`.
- **User Interaction:** User filters list, finds the desired report instance, clicks download link/button.
- **ADHD Considerations:**
    - **Easy Access:** Make finding and downloading reports straightforward.
    - **Clear Naming:** Ensure report names and generation times are clear.
    - **Status Indication:** Clearly show if a report generation succeeded or failed.

### 3.5. `KpiDefinitionView` Slice

**User Story:**

- "As an analyst or manager, I want to define Key Performance Indicators (KPIs), specifying how they are calculated from the data and setting thresholds for alerts, so we can monitor critical metrics proactively."

**Overall Layout & Structure:**

- **Container:** Dedicated page or section (`KpiDefinitionsPage.razor` - `@page "/analytics/kpis"`). List + Detail/Edit view.
- **Organization (List View):**
    - **`Toolbar (MudToolBar):`** "Define KPI" button.
    - **Main Content:** `MudTable` listing `KpiDefinition`s. Columns: Name, Current Value (`GetKpiValue`), Status (`MudChip` based on thresholds), Trend Indicator (`MudIcon`).
- **Organization (Detail/Edit View - `MudDialog` or Page):**
    - `MudForm` with fields for:
        - KPI Name/Description (`MudTextField`)
        - Data Source/Calculation Logic (Simplified input - e.g., select metric from `AggregatedDataView`, apply aggregation)
        - `KpiThreshold`s (Warning, Critical levels - `MudNumericField`) (`SetKpiAlert`)
        - Alerting Configuration (Notifications - simplified)

- **Primary Action(s):** CRUD operations on `KpiDefinition`s (`DefineKpi`, `SetKpiAlert`). Viewing current KPI values/status (`GetKpiValue`, `GetKpiAlerts`).

**Component Breakdown & Interactions:**

- **UI Elements:** `MudTable`, `MudToolBar`, `MudButton`, `MudDialog`/Page, `MudForm`, `MudTextField`, `MudSelect`, `MudNumericField`, `MudChip`, `MudIcon`.
- **User Interaction:** User views KPI list/dashboard widget. Clicks "Define KPI" or edits existing. Configures calculation, sets thresholds, saves.
- **ADHD Considerations:**
    - **Visual Status:** Use color-coded chips and trend icons for quick KPI assessment in the list view/dashboard widget.
    - **Simple Definition:** Abstract the complexity of KPI calculation logic behind a user-friendly interface.
    - **Clear Thresholds:** Make setting warning/critical thresholds intuitive.

## 4. Strategic Planning BC Feature Slices

*(Note: UI for this BC often benefits from more specialized visual interfaces beyond standard forms/tables. This section emphasizes incorporating **interactive canvases, mind maps, and structured visual editors** to handle the complexity and interconnectedness of strategic concepts.)*

### 4.1. `StrategicGoalMindMapView` Slice

**User Story:**

- "As a strategic planner or leader, I want to visually map out our high-level strategic goals, their relationships (e.g., parent/child goals, dependencies), and associated targets using an interactive mind map, so we can better understand the overall strategic landscape."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`StrategicGoalsMapPage.razor` - `@page "/strategy/goals-map"`).
- **Organization:**
    - **`Toolbar (MudToolBar):`** "Add Goal Node" button, Zoom controls, Layout options (e.g., tree, radial), Filter/Highlight options (e.g., by status, pillar).
    - **Main Content:** An interactive canvas (using a JS library like `vis.js`, `d3.js`, or a dedicated mind map component) displaying `StrategicGoalDefinition`s as nodes. Edges represent relationships. Nodes display goal description and key target info. Clicking a node opens a detail panel/dialog. (`GetStrategicGoalDefinition`, `GetGoalTargets`)

- **Primary Action(s):** Viewing the goal map, adding new goal nodes (`DefineStrategicGoal`), editing node details (`SetGoalTarget`), creating/editing relationships between nodes (visual linking), zooming/panning the map.

**Component Breakdown & Interactions:**

- **UI Elements:** Interactive Canvas Component (JS library integration), `MudToolBar`, `MudButton`, `MudIconButton`, Detail Panel (`MudDrawer` or `MudDialog`) containing a form for goal details (`MudTextField`, `MudNumericField`).
- **User Interaction:** User interacts with the canvas: drags nodes to rearrange (layout permitting), clicks "+" on nodes to add children, draws connections between nodes, clicks nodes to view/edit details in a panel/dialog. Uses toolbar for zoom/layout/filtering.
- **ADHD Considerations:**
    - **Visual Structure:** The mind map provides a strong visual structure, which can be helpful. Ensure layout options prevent overwhelming clutter.
    - **Focus Mode:** Allow users to focus/highlight a specific goal and its direct connections, fading out unrelated nodes.
    - **Clear Information:** Keep node labels concise. Use icons or colors for status. Ensure the detail panel is clean and focused.
    - **Interactivity:** Interactions (adding nodes, linking) should be intuitive and provide immediate visual feedback. Avoid overly complex interactions.

### 4.2. `HorizonPlanningCanvas` Slice (Enhanced `HorizonPlanView`)

**User Story:**

- "As a strategic planner, I want to use an interactive canvas to visually manage our integrated horizon plan (H1/H2/H3), mapping initiatives, milestones (`TimelineEventMarker`), dependencies, and resource needs across timeframes and strategic themes, so we can perform dynamic planning and scenario modeling."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`HorizonCanvasPage.razor` - `@page "/strategy/horizon-canvas"`).
- **Organization:**
    - **Visual Layout:** An interactive canvas (custom component or JS library like Miro/Mural embedding if possible, or built with `Konva.js`/`Fabric.js`).
        - Background represents time horizons (H1, H2, H3).
        - Swimlanes for strategic themes or departments.
        - Draggable cards (`MudCard`-like elements on canvas) represent initiatives/milestones. Card details include description, owner, status, resource needs (`ResourceID` refs).
        - Connection lines (draggable) represent dependencies.
    - **Toolbar:** "Add Initiative/Milestone" button, Filter/View options (by theme, status, resource), Zoom controls, potentially "Scenario Mode" toggle. (`MapIntegratedTimeline`, `PerformBackwardPlanning`)

- **Primary Action(s):** Visualizing and interacting with the multi-horizon plan, adding/editing/dragging initiative cards, drawing/editing dependency lines, filtering the view, potentially modeling scenarios by moving elements.

**Component Breakdown & Interactions:**

- **UI Elements:** Interactive Canvas Component, Canvas Cards/Elements, Connection Lines, `MudToolBar`, `MudButton`, `MudSelect`, `MudIconButton`. Detail editing via inline editing on cards or `MudDialog`/`MudDrawer`.
- **User Interaction:** User drags cards across horizons or between swimlanes. Clicks "Add" to create new cards. Draws lines between cards to set dependencies. Edits card details directly or via popups. Uses filters to focus the view.
- **ADHD Considerations:**
    - **Visual Clarity:** **Crucial.** Maintain clear visual separation between horizons and swimlanes. Use consistent card designs. Avoid visual clutter.
    - **Guided Interaction:** Provide clear visual cues for dragging, connecting, and editing.
    - **Filtering/Focus:** Essential for managing complexity. Allow users to filter by theme, status, or zoom into specific areas.
    - **Undo/History:** Important for complex canvases where accidental changes can occur.
    - **Simplicity:** While powerful, avoid making the canvas overly complex with too many features enabled by default.

### 4.3. `CrisisResponseFlowchartCanvas` Slice (Enhanced `CrisisPlanManager`)

**User Story:**

- "As a risk manager, I want to visually design and manage crisis response protocols using an interactive flowchart canvas, mapping out steps, decision points, responsible roles, and communication paths, so the response process is clear and actionable."

**Overall Layout & Structure:**

- **Container:** Dedicated page or tab within crisis planning (`CrisisProtocolCanvasPage.razor` - `@page "/strategy/crisis-protocols/{PlanId:guid}/canvas"`).
- **Organization:**
    - **Visual Layout:** An interactive flowchart canvas (JS library like `GoJS`, `Draw.io` embedding, or custom).
        - Nodes represent `CrisisProtocolStep`s (actions, decisions, communication points). Nodes contain step description, responsible role (`RoleID` ref).
        - Edges represent the flow/sequence between steps. Decision nodes have multiple outgoing paths based on conditions.
    - **Toolbar:** Add Step Node (Action, Decision, Comms) buttons, Connector tool, Zoom controls, Validate Flow button. (`CreateCrisisProtocol`)

- **Primary Action(s):** Visually designing the protocol flow, adding/editing step nodes, connecting nodes, defining decision logic, validating the flow.

**Component Breakdown & Interactions:**

- **UI Elements:** Flowchart Canvas Component, Node Palette, Connector Tool, `MudToolBar`, `MudButton`, `MudIconButton`. Node details edited inline or via `MudDialog`/`MudDrawer`.
- **User Interaction:** User drags step nodes from a palette onto the canvas. Connects nodes using a connector tool. Clicks nodes to edit details (description, role). Configures decision logic on decision nodes.
- **ADHD Considerations:**
    - **Structured Flow:** The flowchart inherently provides structure. Use standard flowchart shapes consistently.
    - **Clear Labels:** Ensure node text is clear and concise.
    - **Visual Cues:** Use color or icons to differentiate node types (action, decision, etc.).
    - **Validation:** Provide validation feedback (e.g., unconnected nodes, incomplete decision logic) clearly but non-intrusively.
    - **Simplicity:** Keep the node types and connection logic straightforward.

### 4.4. `OpportunityEvaluationCanvas` Slice (Enhanced `OpportunityFilterManager`)

**User Story:**

- "As a strategic planner or evaluator, I want to use a structured canvas (like a modified Business Model Canvas or a custom evaluation matrix) to visually assess new opportunities against defined filters, strategic alignment, potential costs, and benefits, so we can make informed decisions."

**Overall Layout & Structure:**

- **Container:** Dedicated page or tool (`OpportunityCanvasPage.razor` - `@page "/strategy/opportunities/{OpportunityId:guid}/canvas"`).
- **Organization:**
    - **Visual Layout:** An interactive canvas representing a structured evaluation framework. Examples:
        - **Evaluation Matrix:** Canvas divided into quadrants (e.g., Strategic Fit vs. Feasibility) or sections (Filters Checklist, Cost/Benefit Analysis, Risk Assessment). Users add sticky notes (`MudCard`-like elements) or text entries within sections.
        - **Visual Funnel:** Canvas showing stages (Initial Idea -> Filter Check -> Assessment -> Decision). Opportunity represented as an element moving through stages, with data added at each stage.
    - **Toolbar:** Add Note/Entry button, Link to Filter Rules button, Status update control, Export/Summary button. (`DefineOpportunityFilter` - used for reference, `CalculateOpportunityCost` - results displayed, `PlanStrategicExperiment` - potentially linked).

- **Primary Action(s):** Visually documenting the evaluation of an opportunity within a structured framework, adding notes/data points, checking against filters, assessing costs/benefits, updating status.

**Component Breakdown & Interactions:**

- **UI Elements:** Interactive Canvas Component (Matrix, Funnel, etc.), Canvas Elements (Sticky Notes, Text Boxes), `MudToolBar`, `MudButton`, `MudSelect`. Data entry via inline editing or `MudDialog`.
- **User Interaction:** User adds notes/data to relevant sections of the canvas. Links to or checks off filter criteria. Updates cost/benefit fields. Moves the opportunity element through funnel stages (if applicable).
- **ADHD Considerations:**
    - **Clear Structure:** The canvas framework itself must be clear and well-defined (labeled sections/stages).
    - **Guided Input:** Prompt users for specific information within each section/stage.
    - **Visual Progress:** Clearly show the status or stage of the evaluation on the canvas.
    - **Minimize Clutter:** Ensure notes and data entries within the canvas remain organized and readable. Avoid overwhelming the user with too much information at once. Use collapsible sections if needed.

### 4.5. `SustainableGrowthCanvas` Slice

**User Story:**

- "As a sustainability lead or strategist, I want to use a canvas to visually map our ESG (Environmental, Social, Governance) goals, link them to specific initiatives or projects, track key `GrowthMetric`s and `ImpactMeasurementData`, and visualize progress towards `EsgTargetValue`s."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`SustainableGrowthCanvasPage.razor` - `@page "/strategy/esg-canvas"`).
- **Organization:**
    - **Visual Layout:** An interactive canvas (potentially using a relationship-mapping library or custom layout).
        - Nodes represent `ESGGoalRecord`s (grouped by E/S/G pillars).
        - Nodes represent specific initiatives/projects (potentially linking to `Project` BC).
        - Nodes represent `GrowthMetric`s or `LongTermMetricData`.
        - Edges connect goals to supporting initiatives and relevant metrics. Node details show status, `EsgTargetValue`, current `ImpactMeasurementData`.
    - **Toolbar:** Add Goal/Initiative/Metric Node buttons, Link Nodes tool, Filter by Pillar (E/S/G), View Mode (Goals View, Initiatives View). (`EstablishGrowthFramework`, `SetEsgGoal`, `MeasureImpact`, `TrackLongTermMetric`)

- **Primary Action(s):** Visually mapping the ESG strategy, defining goals, linking initiatives, adding/updating metrics, tracking progress visually.

**Component Breakdown & Interactions:**

- **UI Elements:** Interactive Canvas Component, Node Types (Goal, Initiative, Metric), Edges/Connectors, `MudToolBar`, `MudButton`, `MudIconButton`, Detail Panel/Dialog (`MudDrawer`/`MudDialog`) for editing node details (forms with `MudTextField`, `MudNumericField`, `MudSelect`).
- **User Interaction:** User adds nodes for goals, initiatives, metrics. Uses connector tool to link related items. Clicks nodes to view/edit details (e.g., update metric values, change goal status). Uses filters to focus on specific pillars.
- **ADHD Considerations:**
    - **Clear Categorization:** Use visual cues (color, icons, layout) to clearly distinguish E, S, and G pillars, goals, initiatives, and metrics.
    - **Relationship Clarity:** Ensure connection lines are easy to follow and understand (e.g., goal -> initiative -> metric). Avoid overly complex "spider web" layouts.
    - **Progress Visualization:** Use clear visual indicators (e.g., progress bars, color changes) on nodes to show progress towards targets.
    - **Focused Views:** Allow filtering not just by pillar but potentially by status or initiative to reduce visual load.

### 4.6. `TransitionPlanRoadmapCanvas` Slice

**User Story:**

- "As an individual planning a career transition or a leader planning a business exit, I want to use a visual roadmap canvas to map out `TransitionMilestoneRecord`s, key `IdentityEvolutionStrategy` elements or `ExitScenarioModel` steps, dependencies, timelines, and required capabilities (`CapabilityRoadmapItem`), so I can create a clear and actionable transition plan."

**Overall Layout & Structure:**

- **Container:** Dedicated page (`TransitionRoadmapPage.razor` - `@page "/strategy/transition-roadmap/{PlanId:guid}"`).
- **Organization:**
    - **Visual Layout:** An interactive timeline or roadmap canvas (JS library like `vis.js` Timeline or custom).
        - Horizontal axis represents time (months, quarters, years).
        - Swimlanes might represent different areas (e.g., Skills, Networking, Financials for personal; Operations, Legal, Comms for business).
        - Draggable elements represent `TransitionMilestoneRecord`s, `CapabilityRoadmapItem`s, or key steps from `ExitScenarioModel`. Elements contain description, dates, status.
        - Connection lines show dependencies between milestones/steps.
    - **Toolbar:** Add Milestone/Capability/Step button, View options (Timeline, List), Zoom controls, Filter by Swimlane/Status. (`PlanIdentityEvolution`, `SimulateExitScenario`, `DefineLegacyTransfer`, `TrackTransitionMilestone`, `BuildCapabilityRoadmap`)

- **Primary Action(s):** Visually creating the transition roadmap, defining milestones/steps, setting timelines, mapping dependencies, tracking progress on the roadmap.

**Component Breakdown & Interactions:**

- **UI Elements:** Interactive Timeline/Roadmap Canvas Component, Draggable Milestone/Step Elements, Connection Lines, Swimlanes, `MudToolBar`, `MudButton`, `MudSelect`, `MudIconButton`. Detail editing via inline or `MudDialog`/`MudDrawer`.
- **User Interaction:** User adds milestones/steps to the timeline/swimlanes. Drags elements to adjust timing. Draws dependency lines. Updates status of elements as progress is made. Zooms/pans the roadmap.
- **ADHD Considerations:**
    - **Timeline Clarity:** Ensure the time axis is clear and easy to understand. Provide options for different time scales (zoom levels).
    - **Visual Structure:** Swimlanes help organize different aspects of the transition. Keep element design clean and consistent.
    - **Dependency Visibility:** Make dependency lines clear but not overwhelming. Highlight critical paths if possible.
    - **Progress Tracking:** Use clear visual indicators (color, completion percentage) on elements to show status.
    - **Actionability:** Make it easy to add, edit, and update items on the roadmap.

### 4.7. `FutureSelfReflectionCanvas` Slice

**User Story:**

- "As a user engaging in long-term personal planning, I want a structured reflection canvas to facilitate `FutureSelfDialogueLog`s, capture `AspirationalStatement`s, define `FutureSelfAttribute`s, and link them to potential `PersonalEvolutionGoal`s, so I can clarify my long-term vision."

**Overall Layout & Structure:**

- **Container:** Dedicated tool/page (`FutureSelfCanvasPage.razor` - `@page "/strategy/future-self"`).
- **Organization:**
    - **Visual Layout:** An interactive canvas divided into predefined sections or using guided prompts (potentially like a template in Miro/Mural). Sections might include:
        - "Future Self Vision" (for `AspirationalStatement`s - text/notes).
        - "Key Attributes" (for `FutureSelfAttribute`s - sticky notes/tags).
        - "Dialogue Prompts/Responses" (structured text areas for `FutureSelfDialogueLog`).
        - "Potential Goals" (area to brainstorm `PersonalEvolutionGoal`s linked to attributes/vision).
        - "Legacy Impact" (for `LegacyImpactGoal`s).
    - **Toolbar:** Add Note/Text Entry button, Link Elements tool (e.g., link attribute to goal), Template selector (if multiple reflection frameworks offered), Save/Export button. (`FacilitateFutureSelfDialogue`, `DesignFutureSelf`, `DesignLegacyImpact`)

- **Primary Action(s):** Engaging in structured reflection, adding text/notes to different sections, defining future attributes, brainstorming goals, potentially linking related ideas visually.

**Component Breakdown & Interactions:**

- **UI Elements:** Interactive Canvas Component with predefined zones/templates, Text Input Elements, Sticky Note Elements, Connection Lines (optional), `MudToolBar`, `MudButton`, `MudIconButton`.
- **User Interaction:** User types text into designated areas based on prompts. Adds sticky notes for attributes or ideas. Optionally draws connections between related thoughts (e.g., an attribute leading to a potential goal). Saves the reflection session.
- **ADHD Considerations:**
    - **Guided Structure:** The predefined sections and prompts provide essential structure for reflection, reducing overwhelm from a blank canvas.
    - **Low Pressure:** Frame the tool as exploratory and non-judgmental. Avoid metrics or scoring.
    - **Clear Prompts:** Use clear, open-ended questions or prompts in each section.
    - **Flexible Input:** Allow input via typing or adding simple visual notes.
    - **Save Progress:** Allow users to save incomplete reflections and return later.
    - **Minimalism:** Keep the visual design clean and calming, avoiding distracting elements.
