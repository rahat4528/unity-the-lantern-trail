# The Lantern Trail 🏮

> An atmospheric, tranquil adventure game built for Android and Modern Web Browsers.

---

## 🌟 Overview

**The Lantern Trail** is a lightweight, polished adventure game. You play as a hooded wanderer guided by a luminous wisp companion through mystical landscapes, gathering forgotten lanterns, deciphering ancient stone tablets, solving pressure plate puzzles, and rekindling the Great Lighthouse Beacon.

---

## 🗺️ The Three Locations

1. **Village Paths (Morning/Green)**
   - *Goal*: Collect 3 ancient lanterns to unlock the gate to the river.
   - *Secrets*: Search behind the grove for the first hidden Golden Lantern (★).
   - *Lore*: Discover notes from the village elder explaining the sacred flame.

2. **Misty River (Foggy/Blue)**
   - *Goal*: Step on three ancient pressure plates in the correct sequence (**North → East → West**) to summon the river crossing bridge.
   - *Secrets*: A second Golden Lantern lies concealed near the abandoned fisherman's skiff.
   - *Hazards*: Crossing without the bridge is blocked by the turbulent river currents.

3. **Lighthouse Hill (Night/Deep Purple)**
   - *Goal*: Climb the stone staircase through crumbling ruins to reach the summit beacon.
   - *Secrets*: The rarest third Golden Lantern is hidden behind the ancient monolithic ruins.
   - *Climax*: Ignite the Great Beacon to conclude the journey and view your final star rating!

---

## 🎮 Controls

### Desktop (Browser / Editor)
| Action | Key / Input |
|---|---|
| **Move** | `W`, `A`, `S`, `D` or `Arrow Keys` or `Click & Drag` mouse |
| **Interact / Collect / Read** | `E`, `Spacebar`, or `Enter` |
| **Direct Click** | Click directly on any nearby lantern, note, plate, or gate |
| **Summon Wisp Hint** | `H` or click 💡 Hint button |
| **Pause / Journal** | `Esc` or click ⏸ Pause button |

### Mobile (Android Chrome / Installed PWA)
| Action | Touch Gesture |
|---|---|
| **Move** | Virtual joystick in the bottom-left corner (or drag anywhere on left half) |
| **Interact / Collect** | Tap the dynamic **✋ Interact** button or tap the object directly |
| **Wisp Hint** | Tap **💡 Hint** (wisp flies toward the nearest objective for 4 seconds) |
| **Pause Menu** | Tap **⏸ Pause** to view the Journal, adjust volume, or restart |

---

## 📱 How to Play on Android (PWA Installation)

This game is packaged as an installable **Progressive Web App (PWA)**:
1. Open [`index.html`](file:///c:/porject%2003/Wandering%20Game/index.html) in Android Google Chrome.
2. Tap the **"📱 Install Android App"** button on the main menu, or tap Chrome's three dots (`⋮`) and select **"Add to Home screen"** / **"Install app"**.
3. The game will launch full-screen with offline support, native touch controls, and zero browser UI distractions!

---

## 🛠️ Unity Project Setup (Source Code Included)

The complete Unity C# source code is included under [`Assets/Scripts/`](file:///c:/porject%2003/Wandering%20Game/Assets/Scripts/):
- **Unity Version Targeted**: Unity 2022.3 LTS (or 2023+)
- **Build Target**: Android (Target SDK 34 / Android 14, Minimum SDK 24 / Android 7.0)
- **Architecture**:
  - `Assets/Scripts/Core/`: `GameManager.cs`, `SaveSystem.cs`, `AudioManager.cs`
  - `Assets/Scripts/Player/`: `PlayerController.cs`, `VirtualJoystick.cs`
  - `Assets/Scripts/Interactables/`: `IInteractable.cs`, `CollectibleLantern.cs`, `SignNote.cs`, `PressurePlate.cs`, `PuzzleManager.cs`, `BridgeGate.cs`, `Beacon.cs`
  - `Assets/Scripts/Companion/`: `WispCompanion.cs`
  - `Assets/Scripts/UI/`: `UIManager.cs`, `NotePanelUI.cs`, `JournalUI.cs`, `EndingScreenUI.cs`

To open in Unity:
1. Open **Unity Hub** -> Click **Add project from disk** -> Select `c:\porject 03\Wandering Game`.
2. Ensure the Android Build Support module is installed.
3. In Unity Editor, go to `File -> Build Settings`, select `Android`, and click `Build`.

---

## 🏆 Scoring & Star Ratings

- ⭐ **1 Star**: Complete the journey and light the beacon.
- ⭐⭐ **2 Stars**: Complete the journey and find at least 1 Golden Lantern or read all lore notes.
- ⭐⭐⭐ **3 Stars**: Discover all 3 secret Golden Lanterns across the three locations!
