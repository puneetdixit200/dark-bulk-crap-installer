# BCUninstaller Industrial Glass UI Refresh Design

Date: 2026-05-13

## Goal

Modernize the full Bulk Crap Uninstaller desktop interface with a dark "Industrial Precision Glass" visual system while preserving the existing application behavior. The final deliverable is a Release build that includes `BCUninstaller.exe`.

## Approved Direction

Use the heavier owner-drawn WinForms shell approach. This keeps the app as a native WinForms application but adds a shared theme layer, custom renderers, rounded control painting, and a full-app styling sweep.

This path was selected over a lighter native skin because the target look needs stronger polish, and over an embedded web frontend because replacing the UI shell would be more likely to disturb behavior boundaries.

## Visual Foundation

The target visual language is dark industrial glassmorphism:

- Main backdrop: ultra-deep navy, based on `#050A10`, with subtle navy and blue gradient depth.
- Primary glass surfaces: deep slate glass, visually matching `rgba(22, 32, 44, 0.65)`.
- Elevated glass surfaces: brighter slate glass, visually matching `rgba(28, 41, 56, 0.80)`.
- Glass edges: 1px light strokes at low opacity, visually matching `rgba(255, 255, 255, 0.12)`.
- Primary action color: Safety Orange `#FF6B00`.
- Secondary active/glow color: Industrial Blue `#3E92FF`.
- Success color: `#4CAF50`.
- Critical color: `#FF4D4D`.
- High emphasis text: white.
- Secondary text: translucent off-white.

WinForms does not provide CSS-style backdrop blur. The implementation will emulate the glass effect with translucent color blends, painted gradients, highlight strokes, shadows, and focused glow states rather than introducing a web rendering layer.

## Typography

Prefer Hanken Grotesk if it is available on the user system. Fall back to Segoe UI or the existing application font when unavailable. The app must remain readable and functional on systems without the preferred font.

Use tighter but practical WinForms typography:

- Main headings and large labels: semi-bold, visually close to 32px where the layout has room.
- Toolbar/dialog/action labels: bold, compact, and clear.
- Body and dense utility text: readable at existing desktop utility sizes.

Do not reduce density in the application list to the point that the tool becomes inefficient for bulk operations.

## Component System

Create a shared UI theme layer for common colors, dimensions, and painting helpers. The intended reusable pieces include:

- Dark glass color palette constants.
- Rounded rectangle drawing helpers.
- Focus glow and highlight stroke helpers.
- ToolStrip/MenuStrip/StatusStrip renderers.
- Recursive form/control styling utilities.
- ObjectListView styling helpers.

### Buttons

Buttons will use 8px-inspired rounding when they are custom-painted or can be safely styled at runtime. Native controls that cannot be rounded without fragile rewrites will receive matching dark glass colors, borders, and focus treatment instead.

- Primary buttons use Safety Orange and are reserved for main forward actions such as Uninstall and Continue.
- Secondary buttons use slate glass fills with low-opacity borders.
- Critical or destructive secondary actions use red glass treatment rather than orange.
- Focus states use a soft Industrial Blue glow.
- Hover and pressed states increase surface opacity and edge brightness where owner drawing allows it.

### Toolbars and Menus

Toolbars and menus receive custom dark glass renderers. Existing toolbar button actions and menu item event handlers must remain unchanged.

Toolbar icons must remain recognizable, and their surrounding surfaces will adopt the glass shell. The implementation may tint or visually frame existing icon resources only when clarity is preserved.

### Main App List

The ObjectListView remains the central dense work surface. It must retain:

- Existing columns and saved column behavior.
- Sorting/filtering behavior.
- Selection, checkbox, group, and context menu behavior.
- Virtual list behavior.
- Inline edit behavior.

Visual changes will target:

- Dark list background.
- Modernized headers.
- Softer grid or separator treatment.
- Alternating row surfaces.
- Orange/blue selected and focused states.
- Clear disabled/locked/protected visual states if already present in the existing list logic.

### Panels and Sidebars

Main window sidebars, filter areas, tree map container, properties sidebar, and status areas will be styled as glass panels with consistent border, fill, and shadow treatment. Avoid changing the layout model except where minor spacing adjustments are needed for the new visual shell.

### Dialogs and Wizards

Apply the same dark glass system across major windows:

- Settings.
- First-run setup.
- Begin uninstall wizard.
- Uninstall confirmation.
- Uninstall progress.
- Junk cleanup.
- Properties.
- List legend.
- Target picker.
- Feedback/debug/helper windows.

The pass will use shared styling utilities so dialogs do not require large one-off rewrites.

## Behavior Boundary

This is a UI-only modernization. Do not change:

- Uninstall command execution.
- Quiet/manual/MSI uninstall behavior.
- Registry scanning or cleanup decisions.
- Installed application detection.
- Leftover cleanup logic.
- Startup manager behavior.
- Ratings behavior.
- Update checks.
- Hotkeys.
- Settings semantics.
- Saved filter/list meanings.
- Import/export output formats.
- Automation or helper process behavior.

Existing event handlers must stay wired. If a visual change requires touching a designer file, preserve event subscriptions and control names.

## Implementation Risks

### WinForms Glass Limitations

True backdrop blur is not native to WinForms controls. The design intentionally uses painted approximations, not WebView or wholesale shell replacement.

### Localization and Overflow

The repository recently fixed localization-related UI overflow. Rounder controls and changed padding must not create new overflow in common dialogs.

### DPI and Accessibility

Owner drawing must respect high DPI scaling and keep sufficient contrast. Focus states must remain visible for keyboard users.

### Designer Fragility

WinForms designer files are large and event-heavy. Prefer centralized runtime styling and small targeted designer changes over broad designer regeneration.

## Testing and Verification

Verification before completion must include:

- Restore/build or publish using Visual Studio MSBuild.
- Release publish that produces `BCUninstaller.exe`.
- Attempted smoke launch of the produced executable. If local launch is blocked by environment or safety constraints, document the blocker.
- Manual visual inspection of the main window and representative dialogs where safe.

Do not test by running real uninstall or cleanup operations. Use safe UI navigation and startup checks only.

## Build Output

Use the repository's MSBuild publish pattern. The expected local final artifact is a published Release folder containing `BCUninstaller.exe` and its supporting files.

Visual Studio 2022 MSBuild is available locally at:

`C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe`

## Acceptance Criteria

- The app has a consistent dark Industrial Precision Glass look across the main window and major dialogs.
- Custom-painted controls and containers have visibly rounder corners, with fallback dark glass styling for native controls that cannot be safely rounded.
- Primary actions use Safety Orange sparingly and consistently.
- Active/focused surfaces use Industrial Blue glow or highlight treatment.
- Existing application workflows remain wired and unchanged.
- The solution publishes successfully in Release configuration.
- A final executable is available in the publish output.
