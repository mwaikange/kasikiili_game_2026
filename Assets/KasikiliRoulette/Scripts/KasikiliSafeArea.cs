using UnityEngine;

namespace Kasikili.Roulette
{
    /// <summary>
    /// Optional safe-area adapter. Keep disabled when reproducing the supplied
    /// 375x812 screenshot literally. Enable it for production devices with notches.
    /// </summary>
    [ExecuteAlways]
    public sealed class KasikiliSafeArea : MonoBehaviour
    {
        [SerializeField] private bool applySafeArea;
        private Rect lastSafeArea;
        private Vector2Int lastScreenSize;
        private RectTransform rectTransform;

        private void OnEnable()
        {
            rectTransform = transform as RectTransform;
            Apply();
        }

        private void Update()
        {
            if (!applySafeArea || rectTransform == null)
                return;

            if (lastSafeArea != Screen.safeArea ||
                lastScreenSize.x != Screen.width ||
                lastScreenSize.y != Screen.height)
            {
                Apply();
            }
        }

        private void Apply()
        {
            if (rectTransform == null || !applySafeArea || Screen.width <= 0 || Screen.height <= 0)
                return;

            Rect area = Screen.safeArea;
            Vector2 anchorMin = area.position;
            Vector2 anchorMax = area.position + area.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            lastSafeArea = area;
            lastScreenSize = new Vector2Int(Screen.width, Screen.height);
        }
    }
}
