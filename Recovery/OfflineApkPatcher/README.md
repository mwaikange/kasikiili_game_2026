# Kasikili offline APK patcher

This utility applies the first playable recovery layer to the original
Kasikili V2.0.1 Mono Unity assembly. It does not replace or redraw the UI.

The patch:

- preserves the APK's original login, signup, forgot-password, game and menu scenes;
- accepts any non-empty mobile number and password synchronously offline, without
  opening or waiting on the legacy loading/server chain;
- wires the existing SIGN IN button to the Game Centre reception, hosted over
  Unity's configured Game scene, bypassing validation, authentication, loaders,
  callbacks and event listeners;
- replaces the dead authentication, OTP and password-reset HTTP responses locally;
- starts the wallet at N$500 and prevents the old credit limit from locking play;
- enables `gameActive` and `tableActive` only after confirming the initialized
  local credit is greater than zero;
- keeps the table active after Unity's roulette reset;
- allows START to continue without the dead connection-check endpoint;
- keeps the original hamburger, betting and START state guards so interaction is
  blocked correctly during spins, settlement, overlays and unavailable states;
- replaces the roulette hamburger destination with the Game Centre reception and
  signs out directly to login;
- replaces only the retired startup/balance and connectivity confirmations with
  the local N$500 wallet and immediate offline connection success;
- settles wins and losses into the local wallet, preserves that balance during
  reset, clears selection lights, and re-enables the guarded table lifecycle;
- supplies local probability, balance, leaderboard and prize data; and
- disables the obsolete Firebase registration call.

## Build and patch

Requires .NET 8.

```powershell
dotnet build -c Release
dotnet .\bin\Release\net8.0\OfflineApkPatcher.dll `
  <original-Scripts.dll> <patched-Scripts.dll>
```

To inject the patched assembly into a copy of the original APK:

```powershell
dotnet .\bin\Release\net8.0\OfflineApkPatcher.dll --inject-apk `
  <original.apk> <unsigned-output.apk> <patched-Scripts.dll> `
  <Kasikili.GameCenter.dll>
```

The resulting APK must be zipaligned and signed with Android build tools before
installation. APKs and private signing keys are deliberately not committed.

This is an interim recovery build. Server-authoritative probability, persistent
balances, cash-out and win settlement still need to be reconnected to the new
backend before production release.
