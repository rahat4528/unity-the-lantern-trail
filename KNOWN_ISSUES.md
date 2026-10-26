# The Lantern Trail - Known Issues & Considerations 🔍

### 1. `file:///` Protocol vs HTTP Server for Service Worker
- **Issue**: Modern web browsers do not allow Service Workers to register over `file:///` URIs for security reasons.
- **Impact**: When opening `index.html` directly from Windows Explorer (as `file:///c:/...`), the game plays 100% fine, but offline PWA caching is disabled.
- **Resolution**: To test PWA installation, serve the directory via any local static server (e.g. `npx serve .` or VS Code Live Server) or host on GitHub Pages / Vercel / Netlify.

### 2. Audio Autoplay Restrictions
- **Issue**: Browsers enforce an audio autoplay restriction requiring user interaction (a tap or click) before Web Audio can output sound.
- **Resolution**: AudioContext is automatically resumed on the first click of "Start Adventure".

### 3. Vibration API on iOS Safari
- **Issue**: Apple iOS Safari does not support the Web Vibration API (`navigator.vibrate`).
- **Impact**: Haptic vibration feedback will work on Android devices, but will be silently ignored on iPhones/iPads without error.

### 4. Unity Native Binary Build
- **Issue**: Compiling the standalone `.apk` directly on this machine requires installing Unity 2022.3+ LTS and the Android NDK/SDK build tools.
- **Status**: The full C# source code and project configuration are provided and ready to be compiled immediately once opened in Unity Hub.
