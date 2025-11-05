# Avalonia UI Layer Development Guide

## 🎯 Objective

Create a comprehensive, production-ready implementation for a specific "feature slice" in an **Avalonia UI** desktop application that follows modern architectural patterns. The feature slice should include a complete UI layer (Views and UserControls) with appropriate business logic integration (ViewModels) that adheres to these guidelines:

1. Integrate with an **ABP Framework (v9+) backend** by invoking Application Services directly using the **ABP-generated Dynamic C# Client Proxies**.
2. Apply the **MVVM pattern** using **ReactiveUI** (latest version) for clean separation of concerns and reactive programming.
3. Create a responsive, accessible UI using **Avalonia UI** core controls.
4. Provide a complete set of files organized in a domain-driven folder structure, including clear separation for shared controls and services.
5. Include comprehensive error handling, loading states, and user feedback mechanisms suitable for a desktop application.
6. Demonstrate best practices for ABP backend integration **via dynamic proxies**, state management, UI/UX design, and performance in an **Avalonia** context.
7. Incorporate thoughtful UI transitions and animations using Avalonia's animation system to enhance usability, particularly for users with ADHD.
8. Organize each feature slice in its own self-contained folder with all necessary Views, UserControls, and ViewModels.
9. Integrate ABP's localization system for multi-language support, applying localized strings in both XAML and ViewModels.
10. Adhere to general ABP backend interaction and **Avalonia UI desktop application** best practices, specifically configuring and using dynamic client proxies correctly.

## 📝 Important Implementation Note

All code must be generated with complete file paths using the NouAssist.[Package].[Module].UI namespace structure, where [Package] represents a business domain (e.g., CRM, Finance, HRM) and [Module] represents a specific module within that domain (e.g., Contacts, Invoicing, Employees). All namespaces within the code should follow this structure.

## 🛠️ Technology Stack Details

* **Backend Framework**: ABP Framework v9+ (assumed, providing REST APIs and Application Services)
    * Built-in Localization System (resources accessible via API or shared project)
    * Authorization & Permission System (checked via backend calls or token claims)
    * Setting Management System (accessed via API)
    * Modular Architecture (Backend)
* **Target Framework**: .NET 9 (or latest stable)
* **UI Framework**: **Avalonia UI** (latest stable version)
* **UI Control Libraries**:
    * **Primary**: **Avalonia UI** core controls
* **MVVM Implementation**: **ReactiveUI** with:
    * `ReactiveObject` for property change notifications and reactive programming
    * `RaiseAndSetIfChanged` for property setters with change notifications
    * `ReactiveCommand` for UI action binding with async support
    * `MessageBus` for component communication and event handling
    * `WhenAnyValue` for reactive property observations
    * `ObservableAsPropertyHelper` for computed properties
* **Animation Libraries**:
    * **Avalonia UI's built-in Animation System** (`Transitions`, `KeyFrames`, `Storyboard`)
    * Custom animations where necessary.
* **Markdown Editor/Viewer**: https://github.com/whistyun/Markdown.Avalonia
* **Icons**: FontAwesome - https://github.com/Projektanker/Icons.Avalonia
* **Charting library**: LiveCharts2

## ✨ Additional UI Libraries and Tools

* **Markdown.Avalonia**: Enables rendering Markdown content in Avalonia UI.
* **Icons.Avalonia**: Facilitates the use of various FontAwesome icon packs within Avalonia applications.
* **LiveCharts2**: A charting library supporting Avalonia for data visualization.

## ✅ Overarching Best Practices & Guidelines

1.  **ABP Backend Interaction**: Communicate with the ABP backend via the injected Application Service interfaces (`IYourAppService`).
2.  **Permissions Integration**:
    * Inject a client-side service (e.g., `IPermissionService`) or use `IAuthorizationService` (if configured appropriately for client-side checks based on JWT claims) into ViewModels.
    * Check permissions asynchronously (e.g., `await PermissionService.IsGrantedAsync("PermissionName")`) before executing actions or enabling/disabling UI elements.
    * Use `IsEnabled` or `IsVisible` bindings in XAML, controlled by ViewModel properties reflecting permission status.
3.  **Exception Handling**:
    * Implement robust `try-catch` blocks around dynamic proxy calls (`IYourAppService.MethodAsync()`) within ViewModels or service layers.
    * Catch specific exceptions, including ABP's `UserFriendlyException` (often automatically deserialized by the proxy) or `BusinessException`, as well as potential `HttpRequestException` or `Volo.Abp.Http.Client.AbpRemoteCallException` for network/proxy issues.
    * Display user-friendly, **localized** error messages using mechanisms appropriate for Avalonia (e.g., message dialogs, status bar updates) potentially via a dedicated UI notification service.
4.  **Performance (Avalonia UI)**:
    * **UI Thread Management**: Ensure proxy calls (which are I/O bound) are performed off the UI thread using `async/await` and ReactiveUI's `ReactiveCommand`.
    * **Efficient Data Binding**: Use `CompiledBindings` (`x:CompileBindings="True"`).
    * **Virtualization**: Utilize UI virtualization in list/grid controls (Avalonia).
    * **Control Instantiation/Templates**: Optimize complex templates and avoid excessive control creation.
    * **Resource Dictionaries**: Structure and merge efficiently.
5.  **State Management**:
    * Primarily use ViewModels (`ReactiveObject`, `RaiseAndSetIfChanged`) for state management.
    * Use `MessageBus` for decoupled communication between ViewModels and components.
    * Leverage `WhenAnyValue` for reactive property observations and derived computations.
    * Use `ObservableAsPropertyHelper` for computed properties that depend on other observables.
    * Avoid large global state stores.
6.  **Security**:
    * Never store sensitive information client-side.
    * Configure the `HttpClient` used by the dynamic proxies (during DI setup) to correctly attach authentication tokens (e.g., Bearer token from a secure store).
    * Rely on ABP's backend authorization.
    * Validate all data server-side.
7.  **API Integration (Dynamic Proxies)**:
    * **Inject Service Interfaces**: Directly inject the ABP Application Service interfaces (e.g., `IYourAppService`) into ViewModels or client-side services.
    * **DI Configuration**: Ensure the dynamic client proxies are correctly registered in the Avalonia application's DI container using extension methods from the `*.HttpApi.Client` project (e.g., `context.Services.AddHttpClientProxies(...)`).
    * **Invoke Methods**: Call the interface methods directly (e.g., `await _myAppService.CreateAsync(dto)`).
8.  **DTOs vs. ViewModels**: Maintain a clear separation.
9.  **Project References**: Ensure the Avalonia project (`NouAssist.[Package].[Module].UI`) references the necessary ABP projects (`*.Application.Contracts`, `*.HttpApi.Client`).

## 📁 Required File Structure (Example)

```
📁 NouAssist.[Package].[Module].UI       \# Avalonia Project
├── 📁 Assets
├── 📁 Features
│   └── 📁 [SliceName]          \# Self-contained feature slice
│       ├── 📁 Views
│       │   ├── [SliceName]ListView.axaml
│       │   ├── [SliceName]ListView.axaml.cs
│       │   └── [SliceName]EditView.axaml     \# Could be a Window or UserControl for a Dialog
│       │   └── [SliceName]EditView.axaml.cs
│       ├── 📁 UserControls         \# Reusable controls specific to this feature
│       │   ├── [SliceName]Toolbar.axaml
│       │   ├── [SliceName]Toolbar.axaml.cs
│       │   └── [Other feature-specific controls]
│       ├── 📁 ViewModels
│       │   ├── [SliceName]ListViewModel.cs
│       │   └── [SliceName]EditViewModel.cs
│       ├── 📁 Services         \# Optional: Feature-specific client services
│       │   └── ...
│       └── 📁 Styles           \# Optional: Feature-specific styles/animations
│           └── [SliceName]Styles.axaml
├── 📁 Shared                    \# Views/Controls/Services reused across features
│   ├── 📁 Controls             \# Reusable UserControls
│   │   ├── LoadingIndicator.axaml
│   │   ├── LoadingIndicator.axaml.cs
│   │   ├── ConfirmationDialog.axaml
│   │   ├── ConfirmationDialog.axaml.cs
│   │   └── [Other shared controls]
│   ├── 📁 Services             \# Shared client services (e.g., UI Notification, Navigation, Permissions)
│   │   └── NotificationService.cs
│   ├── 📁 ViewModels           \# Base ViewModels or shared small ViewModels
│   │   └── ViewModelBase.cs
│   ├── 📁 Converters           \# Shared Value Converters
│   │   └── ...
│   └── 📁 Styles               \# Global or shared styles/resources
│       └── SharedStyles.axaml
├── App.axaml
├── App.axaml.cs
├── Program.cs
├── ViewLocator.cs              \# If using View Locator pattern
└── ... etc ...
```

Namespaces should reflect this structure.

## 📊 Implementation Requirements (Feature Slice Specific)

### Feature Organization

1.  **Self-Contained Feature Slices**: As before.
2.  **Cross-Feature Communication**: Use `MessageBus` from ReactiveUI.

### Localization (ABP Framework & Avalonia)

1.  **Inject Localizer**: Inject `IStringLocalizer<TResource>` into ViewModels.
2.  **Localize ViewModels**: Use `L["ResourceKey"]` with reactive property updates.
3.  **Localize XAML**: Use appropriate Avalonia mechanisms (binding, markup extension).
4.  **Assume Resource**: Assume `YourAppResource` exists and is configured.

### UI Views & Controls

1.  **List View (`[SliceName]ListView.axaml`)**: Responsive, toolbar (localized), Avalonia DataGrid (localized headers), loading/empty states (localized), search (localized).
2.  **Edit View/Window (`[SliceName]EditView.axaml`)**: Window or UserControl for dialog.
3.  **Control Modularity**: Extract reusable parts to `Shared` or feature `UserControls`.

### ViewModels (ReactiveUI Pattern)

1.  **List ViewModel (`[SliceName]ListViewModel.cs`)**: Inherit from `ReactiveObject`. Inject `IStringLocalizer`, **`I[SliceName]AppService`** (the dynamic proxy), `IPermissionService` (or similar), and use `MessageBus` for communication.
2.  **Edit ViewModel (`[SliceName]EditViewModel.cs`)**: Inherit from `ReactiveObject`. Inject dependencies including **`I[SliceName]AppService`**.
3.  **Property Implementation**: Use `RaiseAndSetIfChanged` for property setters.
4.  **Commands**: Use `ReactiveCommand.Create` and `ReactiveCommand.CreateFromTask` for synchronous and asynchronous operations.
5.  **Reactive Bindings**: Use `WhenAnyValue` for property observations and `ObservableAsPropertyHelper` for computed properties.

### Service Integration (Dynamic Proxy Specific)

1.  **ABP Dynamic Proxy Usage**: Inject the application service interface (e.g., **`I[FeatureName]AppService`**).
2.  **Data Transfer**: Use DTOs from `*.Application.Contracts` directly with the proxy methods.
3.  **Reactive Operations**: Wrap service calls in `ReactiveCommand` to handle loading states and error handling reactively.

### ADHD-Friendly UI Transitions & Animations (Avalonia)

1.  **Purpose-Driven Animations**: Subtle transitions, state indicators.
2.  **Focus-Enhancing Techniques**: Highlight active elements, smooth scrolling, subtle pulses, immediate feedback.
3.  **Animation Implementation**: Use Avalonia `Transitions`, `Animations`, `KeyFrames`, control `Duration`/`Easing`.
4.  **Cognitive Load Considerations**: Limit concurrency, offer settings, don't block interaction, maintain consistency.
5.  **Specific Animation Examples**: Fade-ins, scale on hover, translate for panels, progress indicators, validation cues.

### Advanced Requirements (Feature Specific)

1.  **State Management**: Loading indicators, localized feedback via shared service, reactive state updates.
2.  **Accessibility & UX**: AutomationProperties, keyboard navigation, contrast, responsiveness.
3.  **Performance**: Apply Avalonia best practices (virtualization, compiled bindings, threading) and ReactiveUI performance patterns.

## 📤 Output Format

Provide complete C# and AXAML files with clear naming, comments, demonstrating Localization, Permission Checks, ReactiveUI patterns, etc. **For each file, include the full file path using the NouAssist.[Package].[Module].UI namespace structure.**

```xml
// [FULL PATH INCLUDING NouAssist.[Package].[Module].UI] e.g., // src/NouAssist.CRM.Contacts.UI/Features/MyFeature/Views/MyFeatureListView.axaml
[COMPLETE FILE CONTENT]

---

// [FULL PATH INCLUDING NouAssist.[Package].[Module].UI] e.g., // src/NouAssist.Finance.Invoicing.UI/Features/MyFeature/ViewModels/MyFeatureListViewModel.cs
[COMPLETE FILE CONTENT]
```

## 🚀 Additional Requirements

1.  **Theme Support**: Avalonia styling integration.
2.  **Advanced UI Patterns**: Drag-and-drop, Expanders.
3.  **Attention Management**: Focus control.
4.  **Feature Navigation**: View activation, menu integration.
5.  **ABP Settings Integration**: Fetch settings via `ISettingProvider` proxy or dedicated service.
6.  **Reactive Programming**: Leverage ReactiveUI's reactive extensions for complex state management and UI updates.

Generate a complete, production-ready implementation for the requested Avalonia feature slice, adhering to all specified requirements, structures, best practices (including **correct usage and configuration of ABP dynamic C\# client proxies**, localization, permissions), ReactiveUI patterns, and ADHD-friendly design considerations.