#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kasikili.Roulette.Editor
{
    /// <summary>
    /// Generates the supplied 375x812 Kasikili Bergmann Roulette screen as
    /// editable Unity uGUI objects. Run from Tools > Kasikili Roulette.
    /// </summary>
    public static class KasikiliMainGameUIBuilder
    {
        private const float ReferenceWidth = 375f;
        // The approved 375x812 reference reserves its first 42 px for the real
        // Android/iOS status bar. This canvas only owns the app content below it.
        private const float SystemStatusBarHeight = 42f;
        private const float ReferenceHeight = 812f - SystemStatusBarHeight;

        private const string RootName = "Kasikili_Main_Game_UI";
        private const string ArtPath = "Assets/KasikiliRoulette/Art/";
        private const string FontsPath = "Assets/KasikiliRoulette/Fonts/";

        private static readonly Color32 ContentGreen = new Color32(3, 53, 33, 255);       // #033521
        private static readonly Color32 ChipRed = new Color32(139, 15, 15, 255);           // #8B0F0F
        private static readonly Color32 NumberRed = new Color32(139, 15, 15, 255);         // #8B0F0F
        private static readonly Color32 NumberBlack = new Color32(0, 0, 0, 255);
        private static readonly Color32 NumberBlue = new Color32(15, 57, 59, 255);
        private static readonly Color32 BetYellow = new Color32(197, 201, 22, 255);        // #C5C916
        private static readonly Color32 GridLine = new Color32(110, 135, 125, 255);        // #6E877D

        private static Font regularFont;
        private static Font boldFont;

        [MenuItem("Tools/Kasikili Roulette/Create Main Game UI")]
        public static void CreateMainGameUI()
        {
            EnsureEventSystem();
            DeleteExistingRoot();
            LoadFonts();

            GameObject canvasObject = new GameObject(
                RootName,
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster),
                typeof(KasikiliGameUIController));

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;
            canvas.sortingOrder = 0;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(ReferenceWidth, ReferenceHeight);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            scaler.referencePixelsPerUnit = 100f;

            RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
            canvasRect.anchorMin = Vector2.zero;
            canvasRect.anchorMax = Vector2.one;
            canvasRect.offsetMin = Vector2.zero;
            canvasRect.offsetMax = Vector2.zero;

            GameObject screen = CreateRectTransform("GameContent_375x770", canvasObject.transform, 0, 0, ReferenceWidth, ReferenceHeight);
            RectTransform screenRect = screen.GetComponent<RectTransform>();
            screenRect.anchorMin = new Vector2(0.5f, 0.5f);
            screenRect.anchorMax = new Vector2(0.5f, 0.5f);
            screenRect.pivot = new Vector2(0.5f, 0.5f);
            screenRect.anchoredPosition = Vector2.zero;
            screenRect.sizeDelta = new Vector2(ReferenceWidth, ReferenceHeight);

            AddSolidRect("ContentBackground", screen.transform, 0, 0, ReferenceWidth, ReferenceHeight, ContentGreen, false);
            AddTopHeader(screen.transform);
            AddTitle(screen.transform);
            AddMenuButton(screen.transform);
            AddChipButtons(screen.transform);
            AddActionButtons(screen.transform);
            AddBetBoard(screen.transform);

            Selection.activeGameObject = canvasObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("Kasikili Main Game UI created. Connect KasikiliGameUIController events to the game logic.");
        }

        [MenuItem("Tools/Kasikili Roulette/Delete Main Game UI")]
        public static void DeleteMainGameUI()
        {
            DeleteExistingRoot();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        private static void AddTopHeader(Transform parent)
        {
            Texture2D header = AssetDatabase.LoadAssetAtPath<Texture2D>(ArtPath + "KasikiliTopHeader.png");
            if (header == null)
                throw new InvalidOperationException("KasikiliTopHeader.png is missing from " + ArtPath);

            const float sourceHeaderHeight = 219f;
            float gameHeaderHeight = sourceHeaderHeight - SystemStatusBarHeight;
            GameObject headerObject = CreateRectTransform("ExactGameHeader", parent, 0, 0, 375, gameHeaderHeight);
            RawImage image = headerObject.AddComponent<RawImage>();
            image.texture = header;
            // Unity UVs start at the bottom. Keeping the lower 177 px removes the
            // illustrated time/network/battery row without resampling the artwork.
            image.uvRect = new Rect(0f, 0f, 1f, gameHeaderHeight / sourceHeaderHeight);
            image.color = Color.white;
            image.raycastTarget = false;
        }

        private static void AddTitle(Transform parent)
        {
            AddText(
                "GameTitle",
                parent,
                "KASIKILI BERGMANN ROULETTE",
                57,
                177,
                262,
                28,
                17,
                Color.white,
                TextAnchor.MiddleCenter,
                FontStyle.Bold);
        }

        private static void AddMenuButton(Transform parent)
        {
            Texture2D mask = LoadTexture("MenuCircleMask.png");
            GameObject menu = AddRoundedButton("MenuButton", parent, mask, 7, 203, 34, 34, BetYellow);

            AddSolidRect("MenuLine_1", menu.transform, 6, 9, 22, 3, Color.black, false);
            AddSolidRect("MenuLine_2", menu.transform, 6, 15, 22, 3, Color.black, false);
            AddSolidRect("MenuLine_3", menu.transform, 6, 21, 22, 3, Color.black, false);
        }

        private static void AddChipButtons(Transform parent)
        {
            int[] values = { 10, 50, 1000, 2000, 25, 100 };
            float[] x = { 58, 97, 136, 188, 240, 279 };
            float[] widths = { 40, 40, 53, 53, 40, 40 };

            for (int i = 0; i < values.Length; i++)
            {
                AddChipButton(parent, values[i], x[i], 204, widths[i], 36);
            }
        }

        private static void AddChipButton(Transform parent, int value, float x, float y, float width, float height)
        {
            GameObject outer = AddSolidRect("Chip_" + value, parent, x, y, width, height, Color.white, true);
            Button button = outer.AddComponent<Button>();
            button.navigation = new Navigation { mode = Navigation.Mode.None };

            GameObject inner = AddSolidRect("Fill", outer.transform, 1, 1, width - 2, height - 2, ChipRed, true);
            Image innerImage = inner.GetComponent<Image>();
            button.targetGraphic = innerImage;

            AddText(
                "Label",
                outer.transform,
                value.ToString(),
                0,
                0,
                width,
                height,
                20,
                Color.white,
                TextAnchor.MiddleCenter,
                FontStyle.Normal);
        }

        private static void AddActionButtons(Transform parent)
        {
            Texture2D mask = LoadTexture("ActionButtonMask.png");

            GameObject cancel = AddRoundedButton("CancelButton", parent, mask, 88, 247, 57, 30, BetYellow);
            AddText("Label", cancel.transform, "CANCEL", 0, 0, 57, 30, 12, Color.black, TextAnchor.MiddleCenter, FontStyle.Bold);

            GameObject start = AddRoundedButton("StartButton", parent, mask, 231, 247, 57, 30, ChipRed);
            AddText("Label", start.transform, "START", 0, 0, 57, 30, 12, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold);
        }

        private static void AddBetBoard(Transform parent)
        {
            GameObject board = CreateRectTransform("BetBoard", parent, 10, 283, 355, 481);

            // Exact outer border and grid lines from the 375x812 reference.
            AddSolidRect("BorderTop", board.transform, 0, 0, 355, 1, GridLine, false);
            AddSolidRect("BorderBottom", board.transform, 0, 480, 355, 1, GridLine, false);
            AddSolidRect("BorderLeft", board.transform, 0, 0, 1, 481, GridLine, false);
            AddSolidRect("BorderRight", board.transform, 354, 0, 1, 481, GridLine, false);

            AddSolidRect("Vertical_1", board.transform, 120, 0, 1, 481, GridLine, false);
            AddSolidRect("Vertical_2", board.transform, 242, 0, 1, 481, GridLine, false);

            AddSolidRect("Horizontal_0", board.transform, 0, 99, 355, 1, GridLine, false);
            AddSolidRect("Horizontal_1", board.transform, 0, 194, 355, 1, GridLine, false);
            AddSolidRect("Horizontal_2", board.transform, 0, 289, 355, 1, GridLine, false);
            AddSolidRect("Horizontal_3", board.transform, 0, 385, 355, 1, GridLine, false);

            // Coordinates below are local to the board (content origin is x=10, y=283).
            AddNumberCell(board.transform, 0, 150, 30, 129, 192, 12, 60, NumberBlue);

            AddNumberCell(board.transform, 1, 28, 125, 6, 66, 108, 157, NumberRed);
            AddNumberCell(board.transform, 2, 148, 125, 123, 186, 108, 157, NumberBlack);
            AddNumberCell(board.transform, 3, 271, 125, 243, 306, 108, 157, NumberRed);

            AddNumberCell(board.transform, 4, 28, 222, 6, 66, 202, 251, NumberBlack);
            AddNumberCell(board.transform, 5, 148, 222, 123, 186, 202, 251, NumberRed);
            AddNumberCell(board.transform, 6, 271, 222, 243, 306, 202, 251, NumberBlack);

            AddNumberCell(board.transform, 7, 28, 310, 6, 66, 296, 345, NumberRed);
            AddNumberCell(board.transform, 8, 147, 310, 123, 186, 296, 345, NumberBlack);
            AddNumberCell(board.transform, 9, 271, 310, 243, 306, 296, 345, NumberRed);

            AddNumberCell(board.transform, 10, 28, 407, 6, 66, 391, 440, NumberBlack);
            AddNumberCell(board.transform, 11, 148, 407, 123, 186, 391, 440, NumberRed);
            AddNumberCell(board.transform, 12, 268, 407, 243, 306, 391, 440, NumberBlack);
        }

        private static void AddNumberCell(
            Transform board,
            int number,
            float ellipseX,
            float ellipseY,
            float leftButtonX,
            float rightButtonX,
            float topButtonY,
            float bottomButtonY,
            Color numberColor)
        {
            Texture2D ellipseMask = LoadTexture("NumberEllipseMask.png");
            Texture2D betMask = LoadTexture("BetButtonMask.png");

            GameObject ellipse = CreateRectTransform("Number_" + number, board, ellipseX, ellipseY, 53, 44);
            RawImage ellipseImage = ellipse.AddComponent<RawImage>();
            ellipseImage.texture = ellipseMask;
            ellipseImage.color = numberColor;
            ellipseImage.raycastTarget = true;

            Button numberButton = ellipse.AddComponent<Button>();
            numberButton.targetGraphic = ellipseImage;
            numberButton.navigation = new Navigation { mode = Navigation.Mode.None };

            AddText(
                "NumberLabel",
                ellipse.transform,
                number.ToString(),
                0,
                0,
                53,
                44,
                30,
                Color.white,
                TextAnchor.MiddleCenter,
                FontStyle.Bold);

            AddBetButton(board, betMask, number, 1, leftButtonX, topButtonY);
            AddBetButton(board, betMask, number, 2, rightButtonX, topButtonY);
            AddBetButton(board, betMask, number, 3, leftButtonX, bottomButtonY);
            AddBetButton(board, betMask, number, 4, rightButtonX, bottomButtonY);
        }

        private static void AddBetButton(Transform parent, Texture2D mask, int number, int position, float x, float y)
        {
            GameObject buttonObject = AddRoundedButton("Bet_" + number + "_" + position, parent, mask, x, y, 40, 32, BetYellow);
            AddText(
                "Label",
                buttonObject.transform,
                position.ToString(),
                0,
                0,
                40,
                32,
                16,
                Color.black,
                TextAnchor.MiddleCenter,
                FontStyle.Bold);
        }

        private static GameObject AddRoundedButton(
            string name,
            Transform parent,
            Texture2D mask,
            float x,
            float y,
            float width,
            float height,
            Color color)
        {
            GameObject go = CreateRectTransform(name, parent, x, y, width, height);
            RawImage rawImage = go.AddComponent<RawImage>();
            rawImage.texture = mask;
            rawImage.color = color;
            rawImage.raycastTarget = true;

            Button button = go.AddComponent<Button>();
            button.targetGraphic = rawImage;
            button.navigation = new Navigation { mode = Navigation.Mode.None };

            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(1f, 1f, 1f, 0.88f);
            colors.pressedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
            colors.selectedColor = Color.white;
            colors.disabledColor = new Color(1f, 1f, 1f, 0.45f);
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.08f;
            button.colors = colors;

            return go;
        }

        private static GameObject AddSolidRect(
            string name,
            Transform parent,
            float x,
            float y,
            float width,
            float height,
            Color color,
            bool raycastTarget)
        {
            GameObject go = CreateRectTransform(name, parent, x, y, width, height);
            Image image = go.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = raycastTarget;
            return go;
        }

        private static Text AddText(
            string name,
            Transform parent,
            string value,
            float x,
            float y,
            float width,
            float height,
            int fontSize,
            Color color,
            TextAnchor alignment,
            FontStyle style)
        {
            GameObject go = CreateRectTransform(name, parent, x, y, width, height);
            Text text = go.AddComponent<Text>();
            text.text = value;
            text.font = style == FontStyle.Bold ? boldFont : regularFont;
            text.fontSize = fontSize;
            text.fontStyle = style;
            text.color = color;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;
            text.supportRichText = false;
            return text;
        }

        private static GameObject CreateRectTransform(
            string name,
            Transform parent,
            float x,
            float y,
            float width,
            float height)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(x, -y);
            rect.sizeDelta = new Vector2(width, height);
            rect.localScale = Vector3.one;
            return go;
        }

        private static Texture2D LoadTexture(string fileName)
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(ArtPath + fileName);
            if (texture == null)
                throw new InvalidOperationException(fileName + " is missing from " + ArtPath);
            return texture;
        }

        private static void LoadFonts()
        {
            // Drop licensed font files into the Fonts directory using these names,
            // then rerun the builder. The package intentionally does not redistribute fonts.
            regularFont = AssetDatabase.LoadAssetAtPath<Font>(FontsPath + "Halant-Regular.ttf");
            boldFont = AssetDatabase.LoadAssetAtPath<Font>(FontsPath + "Halant-SemiBold.ttf");

            if (regularFont == null)
                regularFont = GetBuiltinFont();
            if (boldFont == null)
                boldFont = regularFont;
        }

        private static Font GetBuiltinFont()
        {
            Font font = null;
            try { font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); }
            catch { /* Older Unity versions use Arial.ttf. */ }

            if (font == null)
            {
                try { font = Resources.GetBuiltinResource<Font>("Arial.ttf"); }
                catch { /* Handled below. */ }
            }

            if (font == null)
                throw new InvalidOperationException("Unity built-in UI font could not be loaded.");

            return font;
        }

        private static void EnsureEventSystem()
        {
            if (UnityEngine.Object.FindObjectOfType<EventSystem>() != null)
                return;

            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        private static void DeleteExistingRoot()
        {
            GameObject existing = GameObject.Find(RootName);
            if (existing != null)
                UnityEngine.Object.DestroyImmediate(existing);
        }
    }
}
#endif
