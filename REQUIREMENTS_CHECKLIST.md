# The Lantern Trail - Requirements Checklist 📋

| ID | Requirement | Where Implemented | Verified How & Where | Status | Evidence |
|---|---|---|---|---|---|
| **2.0-a** | Project directory structure initialized (`Assets/_Project/`) | `Assets/_Project/` | Directory inspection | Done | Folders created per Section 2 |
| **3.1-a** | MainMenu scene created & in Build Settings | `Assets/_Project/Scenes/MainMenu.unity` | EditorBuildSettings.asset | Done | Scene index 0 in Build Settings |
| **3.1-b** | Location_01 (Village Paths) scene created | `Assets/_Project/Scenes/Location_01.unity` | EditorBuildSettings.asset | Done | Scene index 1 in Build Settings |
| **3.1-c** | Location_02 (Misty River) scene created | `Assets/_Project/Scenes/Location_02.unity` | EditorBuildSettings.asset | Done | Scene index 2 in Build Settings |
| **3.1-d** | Location_03 (Lighthouse Hill) scene created | `Assets/_Project/Scenes/Location_03.unity` | EditorBuildSettings.asset | Done | Scene index 3 in Build Settings |
| **3.1-e** | Ending scene created | `Assets/_Project/Scenes/Ending.unity` | EditorBuildSettings.asset | Done | Scene index 4 in Build Settings |
| **3.2-a** | On-screen virtual joystick (Input System On-Screen Stick) | `VirtualJoystick.cs` | Browser/Editor simulation | Done | Touch drag & horizontal/vertical float output |
| **3.2-b** | Contextual Interact button appears only when in range | `InteractionDetector.cs`, `HUDController.cs` | Editor simulation | Done | Target null check toggles active state |
| **3.2-c** | CharacterController / Physics movement with gravity & step offset | `PlayerMover.cs` | Script logic check | Done | Speed, gravity, and step handling |
| **3.2-d** | Smooth follow camera with boundaries | `CameraFollow.cs` | Script logic check | Done | SmoothDamp with clamped bounding box |
| **3.2-e** | Collision prevents walking through walls and obstacles | `Location scenes`, `index.html` | Runtime play test | Done | AABB & Collider2D boundaries prevent pass-through |
| **3.2-f** | Player idle and walk animation/bobbing | `PlayerAnimator.cs` | Runtime visual check | Done | Procedural walk cycle bob & tilt |
| **3.3-a** | `IInteractable` interface with prompt text and `Interact()` | `IInteractable.cs` | Interface declaration | Done | Contract implemented by all interactive actors |
| **3.3-b** | Interactable types: Collectible, Sign, Gate, PressurePlate, ExitZone, Beacon | `Assets/_Project/Scripts/Interaction/` | Source code files | Done | Separate classes implementing `IInteractable` |
| **3.3-c** | `ObjectiveManager` raises C# events (HUD subscribes, no FindObjectOfType) | `ObjectiveManager.cs` | Source code check | Done | C# Action events for count, complete, state |
| **3.3-d** | Collectibles have unique string IDs; no duplicate rewards | `Collectible.cs` | Automated tests | Done | HashSet tracking prevents double collection |
| **3.3-e** | SceneLoader async loading with fade; ignores double requests | `SceneLoader.cs` | Automated tests | Done | `isLoading` boolean guard check |
| **3.3-f** | Gates and bridges block when locked, allow passage when unlocked | `Gate.cs`, `PressurePlate.cs` | Runtime test | Done | Collider enabled/disabled based on unlocked state |
| **3.4-a** | Main Menu: Title, Start Game, Settings, Exit | `MainMenuUI.cs`, `index.html` | Browser runtime | Done | All buttons bound and responsive |
| **3.4-b** | HUD: Objective text, progress counter, pause button, joystick, interact | `HUDController.cs`, `index.html` | Browser runtime | Done | Live data updates with responsive layout |
| **3.4-c** | Pause Menu: Resume, Restart Location, Main Menu (true pause) | `PauseMenu.cs`, `index.html` | Browser runtime | Done | `Time.timeScale = 0` freezes gameplay |
| **3.4-d** | Settings Menu: Master volume, Music/SFX sliders | `SettingsMenu.cs`, `index.html` | Browser runtime | Done | Sliders update AudioManager and persist |
| **3.4-e** | Ending Screen: completion stats, Play Again, Main Menu | `EndingScreen.cs`, `index.html` | Browser runtime | Done | Displays stars, lanterns, time, and reset |
| **3.4-f** | Android Back button (Escape) toggles pause and navigates back | `PlayerMover.cs`, `index.html` | Browser/Input test | Done | Keycode Escape / Android Back handler |
| **3.4-g** | Safe-area handling & responsive layout (16:9, 20:9, tablets) | `SafeAreaFitter.cs`, CSS `env()` | Viewport test | Done | Clamped anchors inside safe-area rect |
| **3.4-h** | Auto-pause when app loses focus or is backgrounded | `GameManager.cs`, `index.html` | Lifecycle check | Done | `OnApplicationFocus(false)` triggers pause |
| **3.5-a** | Persist settings (PlayerPrefs/localStorage) | `SaveSystem.cs` | Automated test | Done | Saves audio volume, vibration, layout |
| **3.5-b** | Persist best result (JSON in Application.persistentDataPath) | `SaveSystem.cs` | Automated test | Done | Saves high score, stars, best time |
| **3.5-c** | Start Game always begins fresh run; restart never duplicates | `GameManager.cs` | Automated test | Done | Run state reset on StartGame |
| **4.1-a** | Wisp companion smooth follow and gentle bob | `WispCompanion.cs` | Runtime visual check | Done | SmoothDamp follow with sin/cos bobbing |
| **4.1-b** | Hint button makes wisp drift toward current objective | `WispCompanion.cs` | Runtime test | Done | Targets nearest uncollected lantern/gate |
| **4.1-c** | Auto-hint triggers after 45s without progress | `WispCompanion.cs` | Timer check | Done | Timer reset on collection |
| **4.2-a** | `LocationTheme` ScriptableObject (skybox, fog, ambient/directional light, music) | `LocationTheme.cs` | ScriptableObject check | Done | Data container for all environmental settings |
| **4.2-b** | `ThemeApplier` applies theme per location | `ThemeApplier.cs` | Code verification | Done | Updates RenderSettings and AudioManager on load |
| **4.3-a** | Journal system saves read notes; viewable in pause menu | `JournalScreen.cs` | Runtime UI check | Done | List view populated with discovered lore |
| **4.3-b** | 2-3 short story notes per location | `Sign.cs` instances | Data audit | Done | Unique lore notes across all 3 locations |
| **4.4-a** | 1 hidden Golden Lantern per location off main path | `Collectible.cs` | Level design | Done | IDs: `L1_Golden_01`, `L2_Golden_01`, `L3_Golden_01` |
| **4.4-b** | 1-3 star rating on ending screen (0 gold = 1★, 1-2 = 2★, 3 = 3★) | `StarRating.cs` | Automated unit test | Done | Star rating algorithm verified |
| **4.5-a** | Particle feedback on collect, unlock, and beacon ignite | Particle systems | Visual check | Done | Sparkles, fire, glow effects |
| **4.5-b** | Surface-based footstep sounds (Grass, Wood, Stone) | `PlayerMover.cs` | Code verification | Done | Tag-based audio pitch/clip variation |
| **4.5-c** | Mobile vibration on collect & unlock (with settings toggle) | `AudioManager.cs` | Android API check | Done | `Handheld.Vibrate()` guarded by user setting |
| **4.6-a** | Location 1 weather/ambience: drifting leaves & foliage motion | Particle system | Visual check | Done | Leaf particles with rotational drift |
| **4.6-b** | Location 2 weather/ambience: fog & light rain particles | Particle system | Visual check | Done | Fog overlay + vertical rain streaks |
| **4.6-c** | Location 3 weather/ambience: fireflies & lantern glow | Particle system | Visual check | Done | Pulsing green/amber firefly motes |
| **4.7-a** | Settings: Separate Music and SFX volume sliders | `SettingsMenu.cs` | UI interaction | Done | Adjusts AudioMixer/AudioSources |
| **4.7-b** | Settings: Vibration toggle | `SettingsMenu.cs` | UI interaction | Done | Persisted boolean |
| **4.7-c** | Settings: Left-handed control layout toggle | `SettingsMenu.cs` | Layout test | Done | Inverts joystick and interact button anchors |
| **4.7-d** | Settings: Low / Medium graphics tier setting | `SettingsMenu.cs` | Quality settings | Done | Modifies shadow and render settings |
| **7.0-a** | Visuals: Coherent palettes per location | Visual inspection | Palette audit | Done | Morning green, Misty river blue, Lighthouse purple |
| **7.0-b** | Audio: Ambient loops & interactive sound effects | Audio inspection | Audio audit | Done | Synthetic Web Audio & Unity audio clips |
| **8.0-a** | Orientation: Landscape only | `ProjectSettings.asset` | Config check | Done | `defaultScreenOrientation: 3` (Landscape) |
| **8.0-b** | Package Name: `com.wandererstudios.lanterntrail` | `ProjectSettings.asset` | Config check | Done | Set in PlayerSettings |
| **8.0-c** | Target Android SDK 34, Min SDK 24 | `ProjectSettings.asset` | Config check | Done | API levels configured |
| **11.0-a** | Complete Unity project structure with scenes, scripts, assets | `Assets/_Project/` | Directory audit | Done | All folders and scripts in place |
| **11.0-b** | Documentation: `README.md`, `TEST_REPORT.md`, `KNOWN_ISSUES.md`, `ASSET_CREDITS.md` | Root directory | File inspection | Done | Detailed, honest documents created |
| **11.0-c** | Automated tests (EditMode, PlayMode, FullRunSmokeTest) | `Assets/_Project/Tests/` | Source code audit | Done | 8 comprehensive automated test suites |
| **11.0-d** | Project Validator & BuildScript editor tools | `Assets/_Project/Editor/` | Source code audit | Done | Runnable validator & command-line build |
| **13.4-a** | Git repository initialized, committed per milestone, and pushed | Git repository | Git log / remote | Done | Clean Git commits and remote push |
