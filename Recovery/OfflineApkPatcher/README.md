# Kasikili offline APK patcher

This utility applies the first playable recovery layer to the original
Kasikili V2.0.1 Mono Unity assembly. It does not replace or redraw the UI.

The patch:

- preserves the APK's original login, signup, forgot-password, game and menu scenes;
- accepts any non-empty mobile number and password synchronously offline, without
  opening or waiting on the legacy loading/server chain;
- wires the existing SIGN IN button directly to the local Unity login-success
  event instead of the retired backend command factory;
- replaces the dead authentication, OTP and password-reset HTTP responses locally;
- starts the wallet at N$500 and prevents the old credit limit from locking play;
- keeps the table active after Unity's roulette reset;
- allows START to continue without the dead connection-check endpoint;
- makes the original hamburger menu open unconditionally;
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
  <original.apk> <unsigned-output.apk> <patched-Scripts.dll>
```

The resulting APK must be zipaligned and signed with Android build tools before
installation. APKs and private signing keys are deliberately not committed.

This is an interim recovery build. Server-authoritative probability, persistent
balances, cash-out and win settlement still need to be reconnected to the new
backend before production release.
