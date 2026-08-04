# Kasikili Expo Go Preview (SDK 54)

An offline, interactive React Native preview rebuilt from the latest Kasikili APK and the supplied Figma screen references.

## Run in Expo Go

```powershell
npm install
npx expo start --lan
```

Install Expo Go on the phone, connect it to the same Wi-Fi network as the computer, and scan the QR code printed by Expo.

The preview starts on the recovered white login screen. Tap **LOGIN** to enter the game, then use the upper-left menu to view the recovered Top 10 leaderboard and wallet/cash-out interface.

This remains an offline UI recovery: it uses demo data and deliberately makes no calls to the legacy HTTP backend. Screens are implemented as real React Native controls and layouts—not screenshot backgrounds. Only recovered artwork such as the roulette wheel and leaderboard crown is used as imagery.
