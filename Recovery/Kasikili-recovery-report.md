# Kasikili APK recovery report

## Offline playable recovery

`Recovery/OfflineApkPatcher` contains the reproducible Mono.Cecil patch used to
make the original V2.0.1 Unity APK testable without its retired backend. The
patched build retains the original artwork, scenes, sounds and interaction code,
starts at N$500, restores offline authentication flows, keeps the betting table
interactive, bypasses the obsolete START connection probe and opens the original
hamburger menu. See the patcher's README for build and signing boundaries.

Date: 2026-08-03

## Selected recovery target

- Drive file: `Kasikili V2.0.1`
- Drive modified time: 2023-12-30 15:14:52 UTC
- APK size: 38,475,251 bytes
- SHA-256: `AA602BFCB98F0AC1D80108C0AAA7D50CF409347AC5CB7E74F697EFEABD3511CD`
- Android package: `com.kasikili.bergmann.roulette`
- App label: `Kasikili Bergmann Roulette`
- APK manifest version: `2.0.0` (`versionCode` 1)
- Unity version: `2020.3.5f1`
- Scripting backend: Mono/.NET managed assemblies
- Android SDK: minimum 19, target/compile 30
- Native ABI: `armeabi-v7a` only

The Drive filename says V2.0.1, but the APK's own manifest reports 2.0.0. The APK manifest is authoritative for the installed application version.

## Why this is the correct latest build

The APK contains the features described by the owner:

- Leaderboard models and API flow: `Leaderboard`, `LeaderboardPerson`, `LeaderboardRequest`, `LeaderboardResponse`, and `GetLeaderboardResponse`.
- Dynamic ranked candidate list, current-player position, mobile number, score, and upgrade state.
- Upgrade animation using a firework particle system and audio, stopped after 12 seconds.
- Prize-distribution display.
- New authentication, OTP, password-reset, cashout, distributor-list, Firebase messaging, loading, and error flows.

The older uploaded Unity source uses the same exact Unity editor version and product name, proving it is the correct project lineage. It is not the latest code snapshot:

- Old `Scripts.dll`: 90 types and 44,032 bytes.
- Latest APK `Scripts.dll`: 225 types and 124,416 bytes.
- Old `Assembly-CSharp.dll`: 5 types and 9,216 bytes.
- Latest APK `Assembly-CSharp.dll`: 15 types and 13,824 bytes.

## Recovered source

ILSpy 9.1 successfully decompiled the latest APK's two application assemblies into 171 editable C# and project files:

- `Assembly-CSharp.dll`
- `Scripts.dll`

This is reconstructed source. It preserves application logic, class names, fields, and most method structure, but does not recreate Unity scene and prefab object references by itself. The original Unity project remains necessary for scenes, prefabs, sprites, animations, materials, audio, and `.meta` GUIDs.

## Backend dependencies found in the APK

The client is hard-coded to legacy plain-HTTP endpoints:

- Test service: `http://15.207.43.56:3008`
- Main service: `http://13.127.146.55/kaslkili`
- Leaderboard: `/gamification/leaderboard-mobile/{month}`
- Prize distribution: `/gamification/get-prize-distribution/{month}`
- Probability: `/coefficients/probabilites-mobile`
- Firebase token update: `/portability/update-fcm-token`

These endpoints were recovered but not contacted. They should be treated as legacy configuration until ownership, availability, and data-safety are verified. The restored app should move to HTTPS and environment-based configuration.

## Signing

- APK signature schemes: v1 and v2
- Signer subject: `O=Kasikili`
- Signer certificate SHA-256: `A4:38:A0:8E:FC:52:45:8B:96:6F:C1:06:3A:E6:EA:AB:6B:27:8A:5B:DA:38:0F:A1:65:A8:3C:E8:2B:04:F8:01`
- Signature algorithm: SHA1withRSA (obsolete/weak)

The original signing keystore is not present in the inspected Drive folder. A rebuilt app can be signed with a new key, but it cannot update an installed copy signed by the old key unless the original keystore is recovered.

## Recommended restoration path

1. Obtain the full original Unity project as the scene/asset base.
2. Preserve its `Assets`, `Packages`, `ProjectSettings`, and all `.meta` files.
3. Replace or merge the older application scripts with the decompiled 2.0 logic.
4. Recover serialized scene and prefab bindings from the latest APK's Unity asset data where the older project lacks the leaderboard and animation objects.
5. Replace legacy endpoints with configurable HTTPS services and revive the backend contract.
6. Upgrade Android target requirements carefully after producing a baseline build in Unity 2020.3.5f1.
7. Create a new signing strategy and document whether upgrade compatibility with the historical APK is required.
8. Build, install on a test device/emulator, and verify offline UI before enabling backend calls.

## Immediate next checkpoint

The next deliverable should be a complete Unity recovery project that opens in Unity 2020.3.5f1 and compiles far enough to reveal missing packages, assets, and serialized references. It should not call the recovered production IP addresses during initial testing.
