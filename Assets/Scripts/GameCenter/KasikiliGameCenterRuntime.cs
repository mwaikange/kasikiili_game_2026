using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kasikili.GameCenter
{
    /// <summary>
    /// Public bridge used by the recovered login/menu assembly. The reception is
    /// drawn over the original Game scene, so the roulette scene and its serialized
    /// object references remain untouched.
    /// </summary>
    public static class GameCenterBridge
    {
        private const string PendingKey = "kasikili.gamecenter.pending";
        private const string RootName = "Kasikili_Game_Centre_Reception";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnSceneLoaded()
        {
            if (PlayerPrefs.GetInt(PendingKey, 0) == 1)
                ShowReception();
        }

        public static void MarkReceptionPending()
        {
            PlayerPrefs.SetInt(PendingKey, 1);
            PlayerPrefs.Save();
        }

        public static void ShowReception()
        {
            MarkReceptionPending();
            Time.timeScale = 0f;

            GameObject existing = GameObject.Find(RootName);
            if (existing != null)
            {
                existing.SetActive(true);
                existing.transform.SetAsLastSibling();
                return;
            }

            EnsureEventSystem();
            GameObject root = new GameObject(RootName, typeof(RectTransform), typeof(Canvas),
                typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(GameCenterController));
            Canvas canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 500;

            CanvasScaler scaler = root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(375f, 812f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            root.GetComponent<GameCenterController>().Build();
        }

        public static void OpenRoulette()
        {
            PlayerPrefs.SetInt(PendingKey, 0);
            PlayerPrefs.Save();
            Time.timeScale = 1f;

            GameObject root = GameObject.Find(RootName);
            if (root != null)
                UnityEngine.Object.Destroy(root);
        }

        public static void ReloadReception()
        {
            MarkReceptionPending();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void LogoutToLogin()
        {
            PlayerPrefs.SetInt(PendingKey, 0);
            PlayerPrefs.Save();
            Time.timeScale = 1f;
            SceneManager.LoadScene(2);
        }

        private static void EnsureEventSystem()
        {
            if (UnityEngine.Object.FindObjectOfType<EventSystem>() == null)
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
    }

    [Serializable]
    public sealed class GameCatalogItem
    {
        public string id;
        public string displayName;
        public string sceneName;
        public bool available;

        public GameCatalogItem(string idValue, string nameValue, string sceneValue, bool isAvailable)
        {
            id = idValue;
            displayName = nameValue;
            sceneName = sceneValue;
            available = isAvailable;
        }
    }

    /// <summary>
    /// Local fixture behind an API-shaped boundary. A future catalog endpoint can
    /// replace GetGames without changing reception navigation.
    /// </summary>
    public static class GameCatalogService
    {
        public static IList<GameCatalogItem> GetGames()
        {
            return new List<GameCatalogItem>
            {
                new GameCatalogItem("kasikili-roulette", "Kasikili Roulette", "Game", true),
                new GameCatalogItem("lucky-6-deluxe", "My Lucky 6 Deluxe", string.Empty, false),
                new GameCatalogItem("kasikili-predictions", "Kasikili Predictions", string.Empty, false)
            };
        }
    }

    /// <summary>
    /// Temporary local wallet ledger. Transfers are deliberately hub-and-spoke:
    /// one side must always be MAIN. Cash-out may debit MAIN only.
    /// </summary>
    public static class GameCenterWallet
    {
        public const string MainWallet = "MAIN";
        private const string MainKey = "kasikili.wallet.main";
        private static readonly Dictionary<string, decimal> GameBalances = new Dictionary<string, decimal>();

        public static decimal MainBalance
        {
            get { return ReadDecimal(MainKey, 500m); }
            private set { WriteDecimal(MainKey, value); }
        }

        public static decimal GetBalance(string walletId)
        {
            if (walletId == MainWallet)
                return MainBalance;

            decimal value;
            return GameBalances.TryGetValue(walletId, out value) ? value : 0m;
        }

        public static bool TryTransfer(string from, string to, decimal amount, out string error)
        {
            error = string.Empty;
            if (amount <= 0m)
            {
                error = "Enter an amount greater than zero.";
                return false;
            }

            if (from == to || (from != MainWallet && to != MainWallet))
            {
                error = "Transfers must move through the Main Balance.";
                return false;
            }

            if (GetBalance(from) < amount)
            {
                error = "Insufficient balance in the selected wallet.";
                return false;
            }

            SetBalance(from, GetBalance(from) - amount);
            SetBalance(to, GetBalance(to) + amount);
            return true;
        }

        public static bool TryCashOut(decimal amount, out string error)
        {
            error = string.Empty;
            if (amount <= 0m)
            {
                error = "Enter an amount greater than zero.";
                return false;
            }

            if (MainBalance < amount)
            {
                error = "Cash-out is limited to the Main Balance.";
                return false;
            }

            MainBalance -= amount;
            return true;
        }

        private static void SetBalance(string walletId, decimal value)
        {
            if (walletId == MainWallet)
                MainBalance = value;
            else
                GameBalances[walletId] = value;
        }

        private static decimal ReadDecimal(string key, decimal fallback)
        {
            decimal value;
            return decimal.TryParse(PlayerPrefs.GetString(key, fallback.ToString(CultureInfo.InvariantCulture)),
                NumberStyles.Number, CultureInfo.InvariantCulture, out value) ? value : fallback;
        }

        private static void WriteDecimal(string key, decimal value)
        {
            PlayerPrefs.SetString(key, value.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.Save();
        }
    }

    public sealed class GameCenterController : MonoBehaviour
    {
        private static readonly Color White = new Color32(250, 250, 247, 255);
        private static readonly Color Ink = new Color32(15, 23, 20, 255);
        private static readonly Color Charcoal = new Color32(37, 50, 45, 255);
        private static readonly Color Green = new Color32(21, 91, 61, 255);
        private static readonly Color DarkGreen = new Color32(3, 53, 33, 255);
        private static readonly Color DeepGreen = new Color32(2, 39, 25, 255);
        private static readonly Color CardGreen = new Color32(10, 72, 48, 255);
        private static readonly Color Red = new Color32(139, 15, 15, 255);
        private static readonly Color Gold = new Color32(245, 194, 42, 255);
        private static readonly Color Accent = new Color32(197, 201, 22, 255);

        private RectTransform screen;
        private RectTransform drawer;
        private GameObject scrim;
        private Text balanceText;
        private GameObject modal;
        private Font font;
        private Sprite circleSprite;
        private Sprite roundedSmallSprite;
        private Sprite roundedLargeSprite;
        private bool drawerOpen;
        private Coroutine drawerMotion;

        public void Build()
        {
            font = LoadFont();
            circleSprite = MakeCircleSprite();
            roundedSmallSprite = MakeRoundedSprite(64, 12);
            roundedLargeSprite = MakeRoundedSprite(64, 22);

            RectTransform canvasRect = GetComponent<RectTransform>();
            canvasRect.anchorMin = Vector2.zero;
            canvasRect.anchorMax = Vector2.one;
            canvasRect.offsetMin = Vector2.zero;
            canvasRect.offsetMax = Vector2.zero;

            screen = Rect("SafeArea", transform, 0f, 0f, 375f, 812f);
            screen.anchorMin = new Vector2(0.5f, 0.5f);
            screen.anchorMax = new Vector2(0.5f, 0.5f);
            screen.pivot = new Vector2(0.5f, 0.5f);
            screen.anchoredPosition = Vector2.zero;
            screen.gameObject.AddComponent<GameCenterSafeArea>();
            GameObject background = AddImage("Background", screen, 0f, 0f, 375f, 812f, DarkGreen, false);
            RectTransform backgroundRect = background.GetComponent<RectTransform>();
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;

            AddProfileButton(screen, 18f, 21f, 54f, OpenDrawer);
            AddText("AppTitle", screen, "KASIKILI GAME CENTRE", 82f, 18f, 270f, 32f, 21, Color.white,
                TextAnchor.MiddleLeft, FontStyle.Bold);
            AddText("AppSubtitle", screen, "PLAY  •  WIN  •  DISCOVER", 83f, 48f, 250f, 18f, 10, Gold,
                TextAnchor.MiddleLeft, FontStyle.Bold);

            GameObject balanceCard = AddButton("BalanceCard", screen, 18f, 92f, 339f, 105f, DeepGreen, null,
                roundedLargeSprite);
            AddShadow(balanceCard, new Color(0f, 0f, 0f, 0.32f), new Vector2(0f, -5f));
            AddImage("BalanceAccent", balanceCard.transform, 0f, 0f, 7f, 105f, Gold, false, roundedSmallSprite);
            AddText("BalanceLabel", balanceCard.transform, "MAIN BALANCE", 24f, 14f, 285f, 24f, 12, Gold,
                TextAnchor.MiddleLeft, FontStyle.Bold);
            balanceText = AddText("Balance", balanceCard.transform, FormatBalance(), 24f, 36f, 290f, 40f, 27,
                Color.white, TextAnchor.MiddleLeft, FontStyle.Bold);
            AddText("BalanceHint", balanceCard.transform, "Available for transfers and cash-out", 24f, 76f, 290f, 18f,
                11, new Color(1f, 1f, 1f, 0.68f), TextAnchor.MiddleLeft, FontStyle.Normal);

            AddText("ChooseTitle", screen, "CHOOSE YOUR GAME", 18f, 221f, 260f, 28f, 17, Color.white,
                TextAnchor.MiddleLeft, FontStyle.Bold);
            AddImage("ChooseRule", screen, 18f, 254f, 48f, 3f, Gold, false, roundedSmallSprite);

            IList<GameCatalogItem> games = GameCatalogService.GetGames();
            for (int i = 0; i < games.Count; i++)
            {
                int row = i / 2;
                int column = i % 2;
                AddGameCard(games[i], 18f + column * 175f, 276f + row * 234f);
            }

            BuildDrawer();
        }

        private string FormatBalance()
        {
            return "N$ " + GameCenterWallet.MainBalance.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private void AddGameCard(GameCatalogItem game, float x, float y)
        {
            Color cardColor = game.available ? CardGreen : new Color(0.06f, 0.24f, 0.17f, 1f);
            GameObject card = AddButton("Game_" + game.id, screen, x, y, 164f, 212f, cardColor,
                delegate { SelectGame(game); }, roundedLargeSprite);
            AddShadow(card, new Color(0f, 0f, 0f, 0.28f), new Vector2(0f, -4f));
            Outline border = card.AddComponent<Outline>();
            border.effectColor = game.available ? new Color(Gold.r, Gold.g, Gold.b, 0.75f) : new Color(1f, 1f, 1f, 0.1f);
            border.effectDistance = new Vector2(1.5f, -1.5f);

            GameObject icon = AddImage("Icon", card.transform, 18f, 16f, 128f, 128f,
                game.available ? Color.white : new Color(0.65f, 0.68f, 0.66f, 1f), true, circleSprite);
            Sprite wheel = FindLoadedSprite("kasikili-logo");
            if (wheel == null)
                wheel = FindLoadedSprite("wheel");
            if (game.id == "kasikili-roulette" && wheel != null)
            {
                Image image = icon.GetComponent<Image>();
                image.sprite = wheel;
                image.preserveAspect = true;
                image.color = Color.white;
            }
            else
            {
                string mark = game.id == "lucky-6-deluxe" ? "L6" : "KP";
                AddImage("IconInner", icon.transform, 13f, 13f, 102f, 102f,
                    game.id == "lucky-6-deluxe" ? Red : Green, false, circleSprite);
                AddText("GameMark", icon.transform, mark, 13f, 13f, 102f, 102f, 32, Gold,
                    TextAnchor.MiddleCenter, FontStyle.Bold);
            }

            AddText("Name", card.transform, game.displayName, 10f, 151f, 144f, 42f, 16, Color.white,
                TextAnchor.MiddleCenter, FontStyle.Bold);
            if (!game.available)
            {
                GameObject badge = AddImage("AvailabilityBadge", card.transform, 34f, 188f, 96f, 19f, Red, false,
                    roundedSmallSprite);
                AddText("Availability", badge.transform, "COMING SOON", 0f, 0f, 96f, 19f, 9, Color.white,
                    TextAnchor.MiddleCenter, FontStyle.Bold);
            }
        }

        private void SelectGame(GameCatalogItem game)
        {
            if (game.available && game.id == "kasikili-roulette")
            {
                GameCenterBridge.OpenRoulette();
                return;
            }

            ShowInfo(game.displayName, "This game is registered in the Game Centre catalog and will open when its API/scene is connected.");
        }

        private void BuildDrawer()
        {
            scrim = AddButton("DrawerScrim", screen, 0f, 0f, 375f, 812f, new Color(0f, 0f, 0f, 0.56f), CloseDrawer);
            scrim.SetActive(false);

            drawer = Rect("NavigationDrawer", screen, 0f, 0f, 302f, 812f);
            AddImage("DrawerBackground", drawer, 0f, 0f, 302f, 812f, DeepGreen, false);
            AddImage("DrawerAccent", drawer, 0f, 0f, 7f, 812f, Gold, false);
            AddProfileButton(drawer, 24f, 35f, 66f, CloseDrawer);
            AddText("Title", drawer, "KASIKILI", 105f, 33f, 170f, 30f, 22, Color.white,
                TextAnchor.MiddleLeft, FontStyle.Bold);
            AddText("Subtitle", drawer, "GAME CENTRE", 105f, 62f, 170f, 22f, 12, Gold,
                TextAnchor.MiddleLeft, FontStyle.Bold);
            AddText("MenuHeading", drawer, "ACCOUNT & SERVICES", 24f, 130f, 240f, 22f, 11,
                new Color(1f, 1f, 1f, 0.55f), TextAnchor.MiddleLeft, FontStyle.Bold);

            string[] labels = { "TOKEN TRANSFERS", "CASH-OUT", "CASH-IN", "LEADERBOARD", "REWARDS", "SETTINGS" };
            Action[] actions = { OpenTransfers, OpenCashOut, OpenCashIn, OpenLeaderboard, OpenRewards, OpenSettings };
            for (int i = 0; i < labels.Length; i++)
            {
                GameObject item = AddButton("Menu_" + labels[i], drawer, 20f, 166f + i * 76f, 262f, 58f,
                    new Color(1f, 1f, 1f, 0.075f), actions[i], roundedSmallSprite);
                AddImage("Icon", item.transform, 12f, 9f, 40f, 40f, i == 0 ? Gold : Red, false, circleSprite);
                string iconText = i == 0 ? "T" : (i == 1 ? "$" : (i == 2 ? "+" : (i == 3 ? "#" : (i == 4 ? "R" : "S"))));
                AddText("IconMark", item.transform, iconText, 12f, 9f, 40f, 40f, 19, Color.white,
                    TextAnchor.MiddleCenter, FontStyle.Bold);
                AddText("Label", item.transform, labels[i], 66f, 0f, 184f, 58f, 17, Color.white,
                    TextAnchor.MiddleLeft, FontStyle.Bold);
            }

            GameObject logout = AddButton("Logout", drawer, 20f, 632f, 262f, 52f, Red,
                GameCenterBridge.LogoutToLogin, roundedSmallSprite);
            AddText("LogoutIcon", logout.transform, "←", 15f, 0f, 36f, 52f, 22, Color.white,
                TextAnchor.MiddleCenter, FontStyle.Bold);
            AddText("LogoutLabel", logout.transform, "LOG OUT", 66f, 0f, 170f, 52f, 16, Color.white,
                TextAnchor.MiddleLeft, FontStyle.Bold);

            AddText("Footer", drawer, "Kasikili Virtual Gaming", 24f, 704f, 250f, 25f, 11,
                new Color(1f, 1f, 1f, 0.42f), TextAnchor.MiddleLeft, FontStyle.Normal);
            drawer.anchoredPosition = new Vector2(-302f, 0f);
        }

        private void OpenDrawer()
        {
            SetDrawer(true);
        }

        private void CloseDrawer()
        {
            SetDrawer(false);
        }

        private void SetDrawer(bool open)
        {
            drawerOpen = open;
            scrim.SetActive(true);
            scrim.transform.SetAsLastSibling();
            drawer.transform.SetAsLastSibling();
            if (drawerMotion != null)
                StopCoroutine(drawerMotion);
            drawerMotion = StartCoroutine(AnimateDrawer(open));
        }

        private IEnumerator AnimateDrawer(bool open)
        {
            float start = drawer.anchoredPosition.x;
            float end = open ? 0f : -302f;
            float elapsed = 0f;
            const float duration = 0.22f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                t = 1f - Mathf.Pow(1f - t, 3f);
                drawer.anchoredPosition = new Vector2(Mathf.Lerp(start, end, t), 0f);
                yield return null;
            }

            drawer.anchoredPosition = new Vector2(end, 0f);
            if (!open)
                scrim.SetActive(false);
            drawerMotion = null;
        }

        private void OpenTransfers()
        {
            CloseDrawer();
            BuildTransferPanel();
        }

        private void BuildTransferPanel()
        {
            GameObject body = OpenModal("TOKEN TRANSFERS");
            string[] walletIds = { GameCenterWallet.MainWallet, "kasikili-roulette", "lucky-6-deluxe", "kasikili-predictions" };
            int fromIndex = 0;
            int toIndex = 1;

            Text fromLabel = AddText("FromLabel", body.transform, WalletName(walletIds[fromIndex]), 20f, 82f, 295f, 54f,
                18, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold);
            Text toLabel = AddText("ToLabel", body.transform, WalletName(walletIds[toIndex]), 20f, 160f, 295f, 54f,
                18, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold);
            AddButton("From", body.transform, 20f, 82f, 295f, 54f, Charcoal, delegate
            {
                fromIndex = (fromIndex + 1) % walletIds.Length;
                fromLabel.text = "FROM: " + WalletName(walletIds[fromIndex]);
            }, roundedSmallSprite);
            AddButton("To", body.transform, 20f, 160f, 295f, 54f, Charcoal, delegate
            {
                toIndex = (toIndex + 1) % walletIds.Length;
                toLabel.text = "TO: " + WalletName(walletIds[toIndex]);
            }, roundedSmallSprite);
            fromLabel.transform.SetAsLastSibling();
            toLabel.transform.SetAsLastSibling();

            InputField amount = AddInput(body.transform, "Amount", 20f, 240f, 295f, 56f, "Amount (N$)");
            Text status = AddText("Status", body.transform, "Game balances return to MAIN before cash-out.", 20f, 306f,
                295f, 58f, 14, Color.white, TextAnchor.MiddleCenter, FontStyle.Normal);
            GameObject transfer = AddButton("Transfer", body.transform, 65f, 380f, 205f, 58f, Red, delegate
            {
                decimal value;
                string error;
                if (!decimal.TryParse(amount.text, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                    status.text = "Enter a valid amount.";
                else if (!GameCenterWallet.TryTransfer(walletIds[fromIndex], walletIds[toIndex], value, out error))
                    status.text = error;
                else
                {
                    balanceText.text = FormatBalance();
                    status.text = "Transfer complete.";
                    amount.text = string.Empty;
                }
            }, roundedSmallSprite);
            AddShadow(transfer, new Color(0f, 0f, 0f, 0.24f), new Vector2(0f, -3f));
            AddText("Label", transfer.transform, "TRANSFER", 0f, 0f, 205f, 58f, 20, Color.white,
                TextAnchor.MiddleCenter, FontStyle.Bold);
        }

        private void OpenCashOut()
        {
            CloseDrawer();
            GameObject body = OpenModal("CASH-OUT");
            AddText("Rule", body.transform, "Only funds in your Main Balance can be cashed out.", 20f, 78f, 295f, 70f,
                16, Color.white, TextAnchor.MiddleCenter, FontStyle.Normal);
            InputField amount = AddInput(body.transform, "Amount", 20f, 172f, 295f, 58f, "Cash-out amount (N$)");
            Text status = AddText("Status", body.transform, string.Empty, 20f, 242f, 295f, 66f, 15, Color.white,
                TextAnchor.MiddleCenter, FontStyle.Normal);
            GameObject action = AddButton("CashOut", body.transform, 65f, 330f, 205f, 58f, Red, delegate
            {
                decimal value;
                string error;
                if (!decimal.TryParse(amount.text, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                    status.text = "Enter a valid amount.";
                else if (!GameCenterWallet.TryCashOut(value, out error))
                    status.text = error;
                else
                {
                    balanceText.text = FormatBalance();
                    status.text = "Cash-out request recorded locally.";
                    amount.text = string.Empty;
                }
            }, roundedSmallSprite);
            AddShadow(action, new Color(0f, 0f, 0f, 0.24f), new Vector2(0f, -3f));
            AddText("Label", action.transform, "CASH-OUT", 0f, 0f, 205f, 58f, 20, Color.white,
                TextAnchor.MiddleCenter, FontStyle.Bold);
        }

        private void OpenCashIn()
        {
            CloseDrawer();
            ShowInfo("CASH-IN", "PaySme in-app purchase will be connected here when you provide the integration code.");
        }

        private void OpenLeaderboard()
        {
            CloseDrawer();
            GameObject body = OpenModal("LEADERBOARD");
            string[] rows = { "1   26481 XXX 2569      97%", "2   26481 XXX 2569      92%",
                "3   26481 XXX 2569      88%", "4   26481 XXX 2569      76%", "5   26481 XXX 2569      75%",
                "6   26481 XXX 2569      69%", "7   26481 XXX 2569      35%", "8   26481 XXX 2569      30%" };
            for (int i = 0; i < rows.Length; i++)
            {
                GameObject row = AddImage("Rank" + (i + 1), body.transform, 16f, 72f + i * 48f, 303f, 40f,
                    i == 0 ? Gold : Charcoal, false, roundedSmallSprite);
                AddText("Value", row.transform, rows[i], 10f, 0f, 283f, 40f, 15,
                    i == 0 ? Ink : Color.white, TextAnchor.MiddleCenter, FontStyle.Bold);
            }
        }

        private void OpenRewards()
        {
            CloseDrawer();
            GameObject body = OpenModal("REFERRAL REWARDS");
            string code = PlayerPrefs.GetString("kasikili.referral.code", "KASIKILI-LOCAL");
            AddText("Info", body.transform, "Invite friends with your referral code. Rewards will be credited after the referral API verifies qualifying activity.",
                20f, 80f, 295f, 120f, 16, Color.white, TextAnchor.MiddleCenter, FontStyle.Normal);
            GameObject codeBox = AddImage("CodeBox", body.transform, 30f, 224f, 275f, 66f, White, false,
                roundedSmallSprite);
            AddText("Code", codeBox.transform, code, 8f, 0f, 259f, 66f, 19, Ink, TextAnchor.MiddleCenter, FontStyle.Bold);
            GameObject copy = AddButton("Copy", body.transform, 65f, 318f, 205f, 56f, Red, delegate
            {
                GUIUtility.systemCopyBuffer = code;
            }, roundedSmallSprite);
            AddText("Label", copy.transform, "COPY REFERRAL CODE", 0f, 0f, 205f, 56f, 16, Color.white,
                TextAnchor.MiddleCenter, FontStyle.Bold);
        }

        private void OpenSettings()
        {
            CloseDrawer();
            GameObject body = OpenModal("SETTINGS");
            AddToggle(body.transform, "Push notifications", "kasikili.settings.push", 82f);
            AddToggle(body.transform, "Notification sounds", "kasikili.settings.sound", 154f);
            AddToggle(body.transform, "Vibrations", "kasikili.settings.vibration", 226f);
            AddText("Permissions", body.transform, "Android notification and app permissions remain controlled by the device settings.",
                20f, 322f, 295f, 90f, 15, Color.white, TextAnchor.MiddleCenter, FontStyle.Normal);
        }

        private GameObject OpenModal(string title)
        {
            if (modal != null)
                Destroy(modal);

            modal = AddImage("ReceptionPanel", screen, 0f, 0f, 375f, 812f, DarkGreen, true);
            GameObject header = AddImage("Header", modal.transform, 0f, 0f, 375f, 88f, DeepGreen, false);
            GameObject back = AddButton("Back", header.transform, 12f, 15f, 52f, 52f, Accent, CloseModal, circleSprite);
            AddText("Icon", back.transform, "<", 0f, 0f, 52f, 52f, 30, Ink, TextAnchor.MiddleCenter, FontStyle.Bold);
            AddText("Title", header.transform, title, 68f, 0f, 290f, 82f, 22, Color.white,
                TextAnchor.MiddleCenter, FontStyle.Bold);
            AddImage("HeaderRule", header.transform, 84f, 74f, 258f, 2f, Gold, false, roundedSmallSprite);
            GameObject body = AddImage("PanelBody", modal.transform, 18f, 112f, 339f, 520f, CardGreen, false,
                roundedLargeSprite);
            AddShadow(body, new Color(0f, 0f, 0f, 0.28f), new Vector2(0f, -5f));
            return body;
        }

        private void ShowInfo(string title, string message)
        {
            GameObject body = OpenModal(title);
            AddText("Message", body.transform, message, 24f, 120f, 287f, 180f, 18, Color.white,
                TextAnchor.MiddleCenter, FontStyle.Normal);
        }

        private void CloseModal()
        {
            if (modal != null)
                Destroy(modal);
            modal = null;
        }

        private void AddToggle(Transform parent, string label, string key, float y)
        {
            bool enabled = PlayerPrefs.GetInt(key, 1) == 1;
            Text state = AddText("State", parent, enabled ? "ON" : "OFF", 250f, y, 62f, 44f, 16,
                enabled ? Accent : Color.gray, TextAnchor.MiddleCenter, FontStyle.Bold);
            AddText("Label", parent, label, 18f, y, 220f, 44f, 17, Color.white, TextAnchor.MiddleLeft, FontStyle.Bold);
            AddButton("Toggle", parent, 244f, y, 74f, 44f, Color.clear, delegate
            {
                enabled = !enabled;
                PlayerPrefs.SetInt(key, enabled ? 1 : 0);
                PlayerPrefs.Save();
                state.text = enabled ? "ON" : "OFF";
                state.color = enabled ? Accent : Color.gray;
            });
            state.transform.SetAsLastSibling();
        }

        private void AddProfileButton(Transform parent, float x, float y, float size, Action onClick)
        {
            GameObject button = AddButton("ProfileMenuButton", parent, x, y, size, size, Gold, onClick, circleSprite);
            GameObject inner = AddImage("ProfileInner", button.transform, size * 0.08f, size * 0.08f,
                size * 0.84f, size * 0.84f, Red, false, circleSprite);
            float head = size * 0.26f;
            AddImage("ProfileHead", inner.transform, size * 0.29f, size * 0.17f, head, head,
                White, false, circleSprite);
            AddImage("ProfileBody", inner.transform, size * 0.18f, size * 0.47f, size * 0.48f, size * 0.29f,
                White, false, circleSprite);
        }

        private InputField AddInput(Transform parent, string name, float x, float y, float width, float height, string hint)
        {
            GameObject box = AddImage(name, parent, x, y, width, height, White, true, roundedSmallSprite);
            InputField field = box.AddComponent<InputField>();
            Text value = AddText("Value", box.transform, string.Empty, 12f, 0f, width - 24f, height, 18, Ink,
                TextAnchor.MiddleLeft, FontStyle.Normal);
            Text placeholder = AddText("Placeholder", box.transform, hint, 12f, 0f, width - 24f, height, 16,
                Color.gray, TextAnchor.MiddleLeft, FontStyle.Normal);
            field.textComponent = value;
            field.placeholder = placeholder;
            field.contentType = InputField.ContentType.DecimalNumber;
            return field;
        }

        private GameObject AddButton(string name, Transform parent, float x, float y, float width, float height,
            Color color, Action action, Sprite sprite = null)
        {
            GameObject go = AddImage(name, parent, x, y, width, height, color, true, sprite);
            Button button = go.AddComponent<Button>();
            button.targetGraphic = go.GetComponent<Image>();
            button.navigation = new Navigation { mode = Navigation.Mode.None };
            if (action != null)
                button.onClick.AddListener(delegate { action(); });
            return go;
        }

        private GameObject AddImage(string name, Transform parent, float x, float y, float width, float height,
            Color color, bool raycast, Sprite sprite = null)
        {
            RectTransform rect = Rect(name, parent, x, y, width, height);
            Image image = rect.gameObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = raycast;
            image.sprite = sprite;
            if (sprite != null && sprite.border.sqrMagnitude > 0f)
            {
                image.type = Image.Type.Sliced;
                image.preserveAspect = false;
            }
            else
            {
                image.preserveAspect = sprite != null;
            }
            return rect.gameObject;
        }

        private static void AddShadow(GameObject target, Color color, Vector2 distance)
        {
            Shadow shadow = target.AddComponent<Shadow>();
            shadow.effectColor = color;
            shadow.effectDistance = distance;
            shadow.useGraphicAlpha = true;
        }

        private Text AddText(string name, Transform parent, string value, float x, float y, float width, float height,
            int size, Color color, TextAnchor alignment, FontStyle style)
        {
            RectTransform rect = Rect(name, parent, x, y, width, height);
            Text text = rect.gameObject.AddComponent<Text>();
            text.font = font;
            text.text = value;
            text.fontSize = size;
            text.fontStyle = style;
            text.color = color;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            text.raycastTarget = false;
            return text;
        }

        private static RectTransform Rect(string name, Transform parent, float x, float y, float width, float height)
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
            return rect;
        }

        private static string WalletName(string id)
        {
            if (id == GameCenterWallet.MainWallet) return "MAIN BALANCE";
            if (id == "kasikili-roulette") return "KASIKILI ROULETTE";
            if (id == "lucky-6-deluxe") return "MY LUCKY 6 DELUXE";
            return "KASIKILI PREDICTIONS";
        }

        private static Font LoadFont()
        {
            Font result = Resources.GetBuiltinResource<Font>("Arial.ttf");
            return result;
        }

        private static Sprite FindLoadedSprite(string spriteName)
        {
            Sprite[] sprites = Resources.FindObjectsOfTypeAll<Sprite>();
            for (int i = 0; i < sprites.Length; i++)
            {
                if (string.Equals(sprites[i].name, spriteName, StringComparison.OrdinalIgnoreCase))
                    return sprites[i];
            }
            return null;
        }

        private static Sprite MakeCircleSprite()
        {
            const int size = 64;
            Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false);
            texture.name = "GameCenterCircle";
            texture.wrapMode = TextureWrapMode.Clamp;
            Color clear = new Color(1f, 1f, 1f, 0f);
            Color solid = Color.white;
            float center = (size - 1) * 0.5f;
            float radius = center;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = x - center;
                    float dy = y - center;
                    texture.SetPixel(x, y, dx * dx + dy * dy <= radius * radius ? solid : clear);
                }
            }
            texture.Apply();
            return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        private static Sprite MakeRoundedSprite(int size, int radius)
        {
            Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false);
            texture.name = "GameCenterRounded" + radius;
            texture.wrapMode = TextureWrapMode.Clamp;
            Color clear = new Color(1f, 1f, 1f, 0f);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int nearestX = Mathf.Clamp(x, radius, size - radius - 1);
                    int nearestY = Mathf.Clamp(y, radius, size - radius - 1);
                    int dx = x - nearestX;
                    int dy = y - nearestY;
                    texture.SetPixel(x, y, dx * dx + dy * dy <= radius * radius ? Color.white : clear);
                }
            }
            texture.Apply();
            Vector4 border = new Vector4(radius, radius, radius, radius);
            return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f,
                0u, SpriteMeshType.FullRect, border);
        }
    }

    /// <summary>Keeps app content below the real Android/iOS status bar.</summary>
    public sealed class GameCenterSafeArea : MonoBehaviour
    {
        private Rect lastSafeArea;
        private Vector2Int lastScreenSize;

        private void OnEnable()
        {
            Apply();
        }

        private void Update()
        {
            if (lastSafeArea != Screen.safeArea || lastScreenSize.x != Screen.width || lastScreenSize.y != Screen.height)
                Apply();
        }

        private void Apply()
        {
            Rect safe = Screen.safeArea;
            RectTransform rect = GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(safe.xMin / Screen.width, safe.yMin / Screen.height);
            rect.anchorMax = new Vector2(safe.xMax / Screen.width, safe.yMax / Screen.height);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            lastSafeArea = safe;
            lastScreenSize = new Vector2Int(Screen.width, Screen.height);
        }
    }
}
