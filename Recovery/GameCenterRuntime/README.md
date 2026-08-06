# Kasikili Game Centre runtime

This project compiles the API-ready reception UI into a standalone managed Unity
assembly for the recovered APK. It uses the same source file that is included in
the reconstructed Unity project.

The reception:

- opens after successful sign-in instead of exposing roulette immediately;
- keeps the original roulette scene intact behind the reception;
- launches roulette only after the user selects its game card;
- provides a partial-width animated left drawer from the round profile button;
- centralizes token transfers, cash-out, cash-in, leaderboard, referral rewards
  and notification/settings navigation;
- enforces game wallet to Main Balance and Main Balance to game transfers; and
- allows cash-out from the Main Balance only.

The catalog and wallet services are local fixtures behind API-shaped boundaries.
They must be replaced by authenticated server responses before production.

Set `KASIKILI_UNITY_MANAGED_DIR` to the recovered APK's
`assets/bin/Data/Managed` directory, or pass
`/p:UnityManagedDir=<path-to-assets/bin/Data/Managed>` when building.
