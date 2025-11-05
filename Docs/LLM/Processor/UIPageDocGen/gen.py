import base64
import os
from google import genai
from google.genai import types
import time
import re
import sys

sys_prompt = """You are a senior software architect specializing in Blazor WebAssembly, ABP Framework, and MudBlazor. However, **your task is to generate only a detailed UI/UX documentation** for a _single, complete vertical slice_ of a large-scale productivity application. The documentation should cover **all** relevant design, layout, component usage, **service calls**, **DTOs**, and user flows required in the vertical slices—**without generating any code**.

---

### **Project Context**

- **Framework:** ABP Framework v9+  
- **UI Technology:** Blazor WebAssembly (.NET 8)  
- **Component Library:** MudBlazor  
- **Architecture:** Modular, component-based, with a clear separation of concerns (UI, ViewModel, Application Services)  
- **API Integration:** Use ABP Framework’s dynamic C# proxy generation for all API calls (assume services and DTOs exist, but do **not** generate any code for them)  

---

Provide **detailed UI/UX documentation** for the following requirement. Each slice should be self-contained and demonstrate a complete user flow, including conceptual layout, navigation, form design, validation approach, loading and error states, **service calls**, **DTO usage**, and any relevant user interactions or states. You **must** provide documentation for **all** the slices and **not** skip or ignore any.

---

## **Detailed Instructions (UI/UX Documentation Only)**

1. **High-Level Overview**  
   - Describe each vertical slice’s purpose, the end-to-end user flow, and how it fits into the larger application’s architecture.

2. **Wireframes & Layout**  
   - Provide a descriptive layout or wireframe for each slice (e.g., how the page is structured, where components go, how forms and lists are arranged).  
   - Specify any **MudBlazor** components you would use (e.g., `MudForm`, `MudTextField`, `MudButton`, `MudTable`), but **do not** provide code. Instead, describe how they should appear and behave.

3. **Navigation and Routing**  
   - Explain how the user navigates to and from each vertical slice (e.g., through links, menus, or buttons).  
   - Indicate **route paths** conceptually (e.g., `/tasks/create`) but **do not** include code.

4. **Form Design and Validation**  
   - Outline each form’s fields, including labels, placeholders, data validation requirements (e.g., required fields, max lengths, etc.).  
   - Explain how validation errors should be displayed to the user (e.g., error messages under fields).

5. **Loading State and Error Handling**  
   - Describe the approach for showing a loading indicator (e.g., `MudProgressCircular` or a loading overlay) during data fetch or form submission.  
   - Document how errors (both client- and server-side) should be communicated to the user (e.g., using ABP’s `MessageService.Error(...)`, conceptually described).

6. **Service Call & DTO Usage**  
   - Conceptually describe the **service calls** required for each vertical slice (e.g., “Calls the `ITaskAppService.CreateAsync` method with a `CreateTaskDto`” or “Fetches data from `IUserAppService.GetListAsync`”).  
   - **Do not** include any actual C# or proxy generation code. Instead, provide a high-level description, including:  
     - **The purpose of the call** (e.g., “Creates a new task in the system.”)  
     - **The input DTO** fields and their conceptual structure (e.g., “`CreateTaskDto` has properties for `Title`, `Description`, `DueDate`, etc.”).  
     - **The expected output DTO** or result and how it fits into the UI.  
   - Emphasize how the UI should handle request/response states (loading, success notifications, error messages).

7. **Localization**  
   - For all user-facing text, specify the usage of ABP’s localization system (e.g., `@L["Save"]`).  
   - Provide an overview of which localization keys might be used (e.g., `Save`, `Cancel`, `CreateTask`, etc.), but **do not** write any code.

8. **Authorization**  
   - Indicate where `[Authorize]` or `<AuthorizeView>` would conceptually be placed for restricting access.  
   - Explain how unauthorized users should be handled in the UI (e.g., hide or disable certain buttons or sections).

9. **File/Folder Structure (Conceptual)**  
   - Provide a conceptual outline of the folder structure (e.g., `Pages/Tasks`, `Components/Tasks`, `ViewModels/Tasks`), **but do not** include any `.razor` or `.cs` code.  
   - Show how the structure promotes maintainability and separation of concerns.

10. **Error Logging and Handling**  
    - Recommend best practices for error handling (e.g., using `try...catch` blocks, calling `HandleErrorAsync` in ABP), but keep it at a conceptual level.

11. **No Code Generation**  
    - **Do not** generate or include **any** C#, Razor, or markup code.  
    - Provide all details in a descriptive, documentation-style format.

---

## **UI/UX Documentation Output Format**

Your output should be **detailed documentation** describing:

- **Page/Component Purpose**: What the page or component does.  
- **Layout**: Structure, sections, relevant MudBlazor components (described in words, no code).  
- **Interactions & Flows**: How users interact with the UI (clicks, form submits, error scenarios).  
- **Validation & Error Display**: Where validation messages appear and what they show.  
- **Service Calls & DTOs**: Conceptual details of which service method is called, input/output DTO fields, and how the result is used in the UI.  
- **Localization**: Which strings to localize, using a conceptual reference to `@L[...]`.  
- **Authorization Considerations**: Which roles or permissions can view or interact with this slice.  
- **Loading Indicators**: How and when loading states appear.  
- **Notification/Feedback**: How success/error messages are displayed.

**Do not** produce any code blocks (Razor, C#, or otherwise). Write in a **documentation style** that a UI/UX or development team could follow to implement these pages and vertical slices.
"""


def generate(index, section):
    client = genai.Client(
        api_key="AIzaSyAPmHgyWo3dbSwIyGP5taYwTrv4AzH_d2c",
    )

    model = "gemini-2.0-flash-thinking-exp-01-21"
    contents = [
        types.Content(
            role="user",
            parts=[
                types.Part.from_text(text=section),
            ],
        ),
    ]
    generate_content_config = types.GenerateContentConfig(
        temperature=1.6,
        top_p=0.95,
        top_k=64,
        max_output_tokens=65536,
        response_mime_type="text/plain",
        system_instruction=[
            types.Part.from_text(text=sys_prompt),
        ],
    )
    with open(f"{index}.txt", "w") as file:
        for chunk in client.models.generate_content_stream(
            model=model,
            contents=contents,
            config=generate_content_config,
        ):
            print(chunk.text, end="")
            file.write(chunk.text)
            file.flush()


def extract_sections(text):
    matches = list(re.finditer(r'(?m)^###\s+(.+)', text))

    return [
        {
            "title": match.group(1).strip(),
            "content": text[match.start():matches[i + 1].start()].strip()
            if i + 1 < len(matches) else text[match.start():].strip()
        }
        for i, match in enumerate(matches)
    ]
    
if __name__ == "__main__":
    with open("req.md", "r") as file:
        text = file.read()
        sections = extract_sections(text)
        print("Identified sections:")
        for index, section in enumerate(sections):
            print(section["title"])
        print(f"Total pages: {len(sections)}")
        char = input("Press Y to continue: ")[0]  # Takes only the first character
        if char.strip().lower() != "y":
            sys.exit(1)
        else:
            for index, section in enumerate(sections):
                generate(index, section["content"])
                print("\n\n\n")
                time.sleep(5)
