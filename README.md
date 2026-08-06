# Kasikili Game Recovery 2026

This repository is the recovery baseline for the **Kasikili Game Centre** and its
first hosted game, **Kasikili Bergmann Roulette**.

It combines:

- the original Unity project assets, scene, packages, project settings, and design files;
- C# source reconstructed from the latest available Kasikili APK;
- forensic notes needed to reproduce and continue the restoration.

## Current status

The latest APK has been positively identified and recovered. A signed offline
test APK now preserves the original roulette while adding the Game Centre
reception and platform-level navigation.

- Latest Drive artifact: `Kasikili V2.0.1`
- APK manifest version: `2.0.0` (`versionCode` 1)
- Android package: `com.kasikili.bergmann.roulette`
- Unity version: `2020.3.5f1`
- Scripting backend: Mono
- APK SHA-256: `AA602BFCB98F0AC1D80108C0AAA7D50CF409347AC5CB7E74F697EFEABD3511CD`

The historical Unity project is the correct lineage and uses the exact same Unity editor version, but its source snapshot predates the APK's 2.0 features. Its project version is `0.4` and it does not contain the newer leaderboard code.

The reconstructed APK source does contain the owner's identifying features:

- leaderboard candidate list and current-player position;
- player upgrade state, firework particle animation, and upgrade audio;
- prize distribution;
- login, signup, OTP, password reset, cashout, and distributor flows;
- Firebase messaging and backend gateway code.

## Repository layout

- `Assets/` — original Unity assets and the historical `Game.unity` scene.
- `Packages/` — Unity package manifest and lock file.
- `ProjectSettings/` — original Unity 2020.3.5f1 settings.
- `design/` — original design assets.
- `docs/` — original project documents.
- `Recovery/LatestApkSource/` — ILSpy reconstruction of the latest APK's `Assembly-CSharp.dll` and `Scripts.dll`.
- `Recovery/Kasikili-recovery-report.md` — detailed APK, signing, backend, and restoration findings.
- `Recovery/GameCenterRuntime/` — standalone reception runtime used by the recovered APK.
- `docs/GAME_CENTRE_ARCHITECTURE.md` — reception, catalog, wallet-transfer, cash-out and referral boundaries.
- `expo-preview/` — Expo SDK 54 preview rebuilt with the latest APK's recovered login, game, menu, leaderboard, and account artwork.

## Important separation

`Recovery/LatestApkSource/` is deliberately outside `Assets/`.

Copying all reconstructed files directly into `Assets/` would create duplicate classes and would not recreate Unity's serialized scene/prefab references. The next recovery phase must merge scripts class-by-class while preserving old `.meta` GUIDs, then reconstruct new leaderboard/UI bindings from the latest APK's Unity asset data.

## Opening the historical base

1. Install Unity Hub and Unity Editor `2020.3.5f1` with Android Build Support.
2. Open the repository root as a Unity project.
3. Allow Unity to restore packages from `Packages/manifest.json`.
4. Open `Assets/Scenes/Game.unity`.
5. Keep the machine offline, or block the legacy endpoints, during initial testing.

The exact editor revision recorded by the project is `2020.3.5f1 (8095aa901b9b)`.

## Security and backend warning

The recovered APK contains hard-coded legacy HTTP endpoints. They have not been contacted during recovery and must not be treated as safe production services. Replace them with environment-based HTTPS configuration before live testing.

The historical signing certificate was recovered from the APK, but its private keystore was not found. A newly signed build will install as a fresh app; it cannot update an existing installation signed by the original key unless that keystore is recovered.

## Next restoration checkpoint

1. Continue diffing the recovered latest-APK scene/prefabs against `Assets/Scenes/Game.unity`.
2. Validate the Game Centre reception and drawer on a physical Android device.
3. Replace the local game catalog and wallet fixtures with authenticated APIs.
4. Merge the reconstructed 2.0 scripts into `Assets/` while preserving matching `.meta` GUIDs.
5. Repair missing serialized references and packages in Unity 2020.3.5f1.
6. Rebuild with a current Android target after the recovered baseline remains stable.

See [Recovery/Kasikili-recovery-report.md](Recovery/Kasikili-recovery-report.md) for the full findings.
