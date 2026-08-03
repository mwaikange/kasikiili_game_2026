# Kasikili Expo Go Preview (SDK 54)

An offline, interactive React Native preview rebuilt from the latest Kasikili APK's exported Unity scenes and original Texture2D assets.

## Run in Expo Go

```powershell
npm install
npx expo start --lan
```

Install Expo Go on the phone, connect it to the same Wi-Fi network as the computer, and scan the QR code printed by Expo.

The preview starts on the recovered white login screen. Tap **LOGIN** to enter the game, then use the upper-left menu to view the recovered Top 10 leaderboard and wallet/cash-out interface.

This remains an offline UI recovery: it uses demo data and deliberately makes no calls to the legacy HTTP backend. The artwork comes from the APK, while screen interaction and layout have been recreated in Expo so they can be inspected safely in Expo Go.
