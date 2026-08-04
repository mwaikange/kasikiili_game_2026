# Kasikili Bergmann Roulette — Unity UI Handoff

This package recreates the approved **375 × 812** mobile game screen as editable Unity **uGUI** objects. The first 42 px in the visual reference belong to the real phone status bar, so the generated game canvas is **375 × 770** and does not draw fake time, network, Wi-Fi, or battery icons.

## Recommended Unity version

Unity **2020.3 LTS or newer**. The implementation uses only built-in `UnityEngine.UI` components and does not require TextMeshPro or third-party packages.

## Import and generate the screen

1. Copy `Assets/KasikiliRoulette` into the developer's Unity project, or import the supplied `.unitypackage`.
2. Open the target Unity scene.
3. Select **Tools → Kasikili Roulette → Create Main Game UI**.
4. Unity creates `Kasikili_Main_Game_UI`, a Canvas configured with a **375 × 770** app-content reference resolution below the real system status bar.
5. Save the scene or drag the generated root into the Project panel to create a prefab.

Running the menu command again replaces the previously generated Kasikili UI, so the layout can be regenerated cleanly.

## Included

- Exact scoreboard/wheel artwork cropped from the supplied approved design, excluding the illustrated status bar.
- Editable title, chip values, START/CANCEL controls, menu control, board lines, number fields and all 1–4 bet buttons.
- Responsive `CanvasScaler` using 375 × 812 as the source resolution.
- `KasikiliGameUIController` with UnityEvents for chip selection, bets, START, CANCEL and menu.
- Optional `KasikiliSafeArea` component for production devices with notches.
- Full-screen and content reference PNG files for comparison.

## Game-logic events

The root contains `KasikiliGameUIController` and exposes:

- `OnChipSelected(int chipValue)`
- `OnBetSelected(int rouletteNumber, int position, int chipValue)`
- `OnNumberSelected(int rouletteNumber, int chipValue)`
- `OnStartPressed()`
- `OnCancelPressed()`
- `OnMenuPressed()`

The UI package deliberately does **not** invent roulette rules, wallet logic, credit handling, API calls, win calculations or animations. The developer should connect these events to the existing game systems.

## Font matching

The reference uses **Halant** for most game typography. The operating system supplies the status treatment; it is not painted into the game UI. Font files are not redistributed in this handoff.

For closer editable-text matching, place licensed font files at:

- `Assets/KasikiliRoulette/Fonts/Halant-Regular.ttf`
- `Assets/KasikiliRoulette/Fonts/Halant-SemiBold.ttf`

Then rerun **Create Main Game UI**. Without those files, Unity's built-in font is used.

## Responsive behaviour

The game-content canvas uses `Scale With Screen Size`, reference resolution `375 × 770`, with a 0.5 width/height match. Together with a 42 px system bar this matches the supplied 375 × 812 reference. Keep the optional safe-area adjustment disabled when checking literal pixel alignment against the reference screenshot.

## Primary source files

- `Editor/KasikiliMainGameUIBuilder.cs` — creates the full editable layout.
- `Scripts/KasikiliGameUIController.cs` — developer-facing interaction events.
- `Art/KasikiliReference.png` — full approved reference.
- `Art/KasikiliTopHeader.png` — exact status bar, score panels and roulette-wheel artwork.
- `LAYOUT_SPEC.md` — measurements and visual tokens.
