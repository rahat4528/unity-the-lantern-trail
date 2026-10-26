using UnityEngine;
using LanternTrail.Audio;

namespace LanternTrail.Core
{
    public class ThemeApplier : MonoBehaviour
    {
        [Header("Theme Configuration")]
        [SerializeField] private LocationTheme currentTheme;
        [SerializeField] private Light directionalLight;

        private void Start()
        {
            ApplyTheme();
        }

        public void SetTheme(LocationTheme theme)
        {
            currentTheme = theme;
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (currentTheme == null) return;

            // Atmosphere & Fog
            RenderSettings.fog = currentTheme.enableFog;
            RenderSettings.fogColor = currentTheme.fogColor;
            RenderSettings.fogDensity = currentTheme.fogDensity;
            RenderSettings.fogMode = FogMode.ExponentialSquared;

            if (currentTheme.skyboxMaterial != null)
            {
                RenderSettings.skybox = currentTheme.skyboxMaterial;
            }

            // Ambient Light
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = currentTheme.ambientLightColor;

            // Directional Light
            if (directionalLight != null)
            {
                directionalLight.color = currentTheme.directionalLightColor;
                directionalLight.intensity = currentTheme.directionalLightIntensity;
                directionalLight.transform.rotation = Quaternion.Euler(currentTheme.directionalLightRotation);
            }

            // Music
            if (currentTheme.musicTrack != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusic(currentTheme.musicTrack);
            }
        }

        private void OnValidate()
        {
            if (currentTheme != null && !Application.isPlaying)
            {
                ApplyTheme();
            }
        }
    }
}
