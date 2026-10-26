using UnityEngine;

namespace LanternTrail.Core
{
    [CreateAssetMenu(fileName = "NewLocationTheme", menuName = "LanternTrail/Location Theme")]
    public class LocationTheme : ScriptableObject
    {
        [Header("Identity")]
        public string locationName = "Village Paths";
        public GameLocation location = GameLocation.VillagePaths;

        [Header("Atmosphere")]
        public Material skyboxMaterial;
        public Color skyboxTint = Color.white;
        public bool enableFog = false;
        public Color fogColor = new Color(0.7f, 0.8f, 0.9f, 1f);
        public float fogDensity = 0.02f;

        [Header("Lighting")]
        public Color ambientLightColor = new Color(0.2f, 0.2f, 0.2f);
        public Color directionalLightColor = Color.white;
        public float directionalLightIntensity = 1.0f;
        public Vector3 directionalLightRotation = new Vector3(50f, -30f, 0f);

        [Header("Audio")]
        public AudioClip musicTrack;
        public AudioClip ambientAmbience;
    }
}
