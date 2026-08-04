using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kasikili.Roulette
{
    /// <summary>
    /// Lightweight interaction layer for the supplied UI handoff.
    /// It does not contain roulette business rules; it exposes clear events
    /// that the game developer can connect to the existing backend/game logic.
    /// </summary>
    public sealed class KasikiliGameUIController : MonoBehaviour
    {
        [Serializable]
        public sealed class ChipSelectedEvent : UnityEngine.Events.UnityEvent<int> { }

        [Serializable]
        public sealed class BetSelectedEvent : UnityEngine.Events.UnityEvent<int, int, int> { }

        [Serializable]
        public sealed class NumberSelectedEvent : UnityEngine.Events.UnityEvent<int, int> { }

        public ChipSelectedEvent OnChipSelected = new ChipSelectedEvent();
        public BetSelectedEvent OnBetSelected = new BetSelectedEvent();
        public NumberSelectedEvent OnNumberSelected = new NumberSelectedEvent();
        public UnityEngine.Events.UnityEvent OnStartPressed = new UnityEngine.Events.UnityEvent();
        public UnityEngine.Events.UnityEvent OnCancelPressed = new UnityEngine.Events.UnityEvent();
        public UnityEngine.Events.UnityEvent OnMenuPressed = new UnityEngine.Events.UnityEvent();

        public int SelectedChip { get; private set; } = 10;
        public int SelectedNumber { get; private set; } = -1;
        public int SelectedPosition { get; private set; } = -1;

        private readonly Dictionary<Graphic, Color> originalChipColors = new Dictionary<Graphic, Color>();
        private readonly List<Graphic> chipGraphics = new List<Graphic>();
        private readonly List<Outline> betHighlights = new List<Outline>();
        private readonly Dictionary<int, Outline> numberHighlights = new Dictionary<int, Outline>();

        private static readonly Color SelectedChipTint = new Color32(190, 39, 39, 255);

        private void Awake()
        {
            WireButtons();
        }

        private void WireButtons()
        {
            foreach (Button button in GetComponentsInChildren<Button>(true))
            {
                string objectName = button.gameObject.name;

                if (objectName.StartsWith("Chip_", StringComparison.Ordinal))
                {
                    if (!int.TryParse(objectName.Substring(5), out int chipValue))
                        continue;

                    Graphic graphic = button.targetGraphic;
                    if (graphic != null)
                    {
                        chipGraphics.Add(graphic);
                        originalChipColors[graphic] = graphic.color;
                    }

                    button.onClick.AddListener(() => SelectChip(chipValue, graphic));
                    continue;
                }

                if (objectName.StartsWith("Bet_", StringComparison.Ordinal))
                {
                    string[] pieces = objectName.Split('_');
                    if (pieces.Length == 3 &&
                        int.TryParse(pieces[1], out int number) &&
                        int.TryParse(pieces[2], out int position))
                    {
                        Outline highlight = CreateSelectionHighlight(button.targetGraphic, new Color32(255, 138, 0, 255));
                        betHighlights.Add(highlight);
                        button.onClick.AddListener(() => SelectBet(number, position, highlight));
                    }
                    continue;
                }

                if (objectName.StartsWith("Number_", StringComparison.Ordinal))
                {
                    if (!int.TryParse(objectName.Substring(7), out int number))
                        continue;

                    Outline highlight = CreateSelectionHighlight(button.targetGraphic, new Color32(255, 244, 60, 255));
                    numberHighlights[number] = highlight;
                    button.onClick.AddListener(() => SelectNumber(number));
                    continue;
                }

                switch (objectName)
                {
                    case "StartButton":
                        button.onClick.AddListener(() => OnStartPressed.Invoke());
                        break;
                    case "CancelButton":
                        button.onClick.AddListener(CancelSelection);
                        break;
                    case "MenuButton":
                        button.onClick.AddListener(() => OnMenuPressed.Invoke());
                        break;
                }
            }

            HighlightInitialChip();
        }

        private void HighlightInitialChip()
        {
            foreach (Button button in GetComponentsInChildren<Button>(true))
            {
                if (button.gameObject.name == "Chip_10")
                {
                    SelectChip(10, button.targetGraphic, false);
                    return;
                }
            }
        }

        private void SelectChip(int chipValue, Graphic selectedGraphic, bool raiseEvent = true)
        {
            SelectedChip = chipValue;

            foreach (Graphic graphic in chipGraphics)
            {
                if (graphic != null && originalChipColors.TryGetValue(graphic, out Color original))
                    graphic.color = original;
            }

            if (selectedGraphic != null)
                selectedGraphic.color = SelectedChipTint;

            if (raiseEvent)
                OnChipSelected.Invoke(chipValue);
        }

        private static Outline CreateSelectionHighlight(Graphic graphic, Color color)
        {
            if (graphic == null)
                return null;

            Outline outline = graphic.gameObject.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = new Vector2(2f, -2f);
            outline.useGraphicAlpha = false;
            outline.enabled = false;
            return outline;
        }

        private void SelectNumber(int number)
        {
            SetSelectedNumber(number);
            SelectedPosition = -1;
            DisableHighlights(betHighlights);
            OnNumberSelected.Invoke(number, SelectedChip);
        }

        private void SelectBet(int number, int position, Outline betHighlight)
        {
            SetSelectedNumber(number);
            SelectedPosition = position;
            DisableHighlights(betHighlights);
            if (betHighlight != null)
                betHighlight.enabled = true;
            OnBetSelected.Invoke(number, position, SelectedChip);
        }

        private void SetSelectedNumber(int number)
        {
            SelectedNumber = number;
            foreach (Outline highlight in numberHighlights.Values)
            {
                if (highlight != null)
                    highlight.enabled = false;
            }

            if (numberHighlights.TryGetValue(number, out Outline selected) && selected != null)
                selected.enabled = true;
        }

        private static void DisableHighlights(IEnumerable<Outline> highlights)
        {
            foreach (Outline highlight in highlights)
            {
                if (highlight != null)
                    highlight.enabled = false;
            }
        }

        private void CancelSelection()
        {
            SelectedNumber = -1;
            SelectedPosition = -1;
            DisableHighlights(betHighlights);
            DisableHighlights(numberHighlights.Values);
            OnCancelPressed.Invoke();
        }
    }
}
