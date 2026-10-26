# The Lantern Trail - Asset & Technical Credits 🎨

### Graphics & Visual Art
- **Procedural Canvas Rendering**: All environmental art (trees, huts, water ripple effects, bridges, monoliths, lighthouse, and fireflies) is procedurally generated at runtime via HTML5 Canvas 2D API for lightweight zero-dependency execution.
- **PWA App Icons**: Vector-accurate RGBA PNG icons (`icon-192.png`, `icon-512.png`) featuring a stylized amber lantern and mystical glow, generated natively via custom compression pipeline.
- **Color Palettes**:
  - *Location 1 (Village Paths)*: Organic sage green (`#3d6e2a`, `#8db85a`) with warm amber accents (`#f5c842`).
  - *Location 2 (Misty River)*: Deep twilight river blues (`#1a3a5c`, `#4a7090`) with soft fog overlays (`#b4d2e6`).
  - *Location 3 (Lighthouse Hill)*: Nocturnal amethyst and obsidian (`#0d0028`, `#2a1050`) with beacon fire amber (`#ff8c42`).

### Audio Architecture
- **Web Audio API Engine**: Dynamic synthetic audio using oscillator nodes (Sine, Triangle) and gain envelopes.
- **Sound Effects**:
  - Regular Lantern Chime: High resonant dual-tone bell (`880Hz` -> `1100Hz`).
  - Golden Lantern Fanfare: Triad harmonic chime with extended decay.
  - Ancient Gate & Bridge Unlock: Rising harmonic triad (`440Hz` -> `660Hz` -> `880Hz`).
  - Pressure Plate Activation: Crisp metallic step tone.
  - Beacon Ignition: Deep resonance sweep with sustained warmth.

### Code & Frameworks
- **Web Engine**: Custom Vanilla HTML5 / ES6 Canvas Game Engine (Zero external dependencies).
- **Unity C# Engine**: C# (.NET Standard 2.1) structured for Unity 2022.3 LTS and Unity 2023+.
