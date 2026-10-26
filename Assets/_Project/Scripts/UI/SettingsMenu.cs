using UnityEngine;
using UnityEngine.UI;
using LanternTrail.Audio;
using LanternTrail.Save;
using LanternTrail.Player;

namespace LanternTrail.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        [Header("Audio Sliders")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        [Header("Control Toggles")]
        [SerializeField] private Toggle vibrationToggle;
        [SerializeField] private Toggle leftHandedToggle;
        [SerializeField] private Slider joystickSizeSlider;

        [Header("Graphics Dropdown / Toggle")]
        [SerializeField] private Dropdown graphicsTierDropdown;

        [Header("Buttons")]
        [SerializeField] private Button backButton;

        private void Awake()
        {
            if (backButton != null) backButton.onClick.AddListener(Close);
            gameObject.SetActive(false);
        }

        private void Start()
        {
            LoadAndApplySettings();
            RegisterListeners();
        }

        public void Open()
        {
            gameObject.SetActive(true);
            LoadAndApplySettings();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void LoadAndApplySettings()
        {
            float music = SaveSystem.GetMusicVolume();
            float sfx = SaveSystem.GetSfxVolume();
            bool vib = SaveSystem.GetVibrationEnabled();
            bool leftHanded = SaveSystem.GetLeftHanded();
            float joySize = SaveSystem.GetJoystickSize();
            int gfx = SaveSystem.GetGraphicsTier();

            if (musicSlider != null) musicSlider.value = music;
            if (sfxSlider != null) sfxSlider.value = sfx;
            if (vibrationToggle != null) vibrationToggle.isOn = vib;
            if (leftHandedToggle != null) leftHandedToggle.isOn = leftHanded;
            if (joystickSizeSlider != null) joystickSizeSlider.value = joySize;
            if (graphicsTierDropdown != null) graphicsTierDropdown.value = gfx;

            ApplyGraphics(gfx);
            ApplyControls(leftHanded, joySize);
        }

        private void RegisterListeners()
        {
            if (musicSlider != null)
                musicSlider.onValueChanged.AddListener(val => AudioManager.Instance?.SetMusicVolume(val));

            if (sfxSlider != null)
                sfxSlider.onValueChanged.AddListener(val => AudioManager.Instance?.SetSfxVolume(val));

            if (vibrationToggle != null)
                vibrationToggle.onValueChanged.AddListener(val => AudioManager.Instance?.SetVibration(val));

            if (leftHandedToggle != null)
                leftHandedToggle.onValueChanged.AddListener(val =>
                {
                    ApplyControls(val, joystickSizeSlider != null ? joystickSizeSlider.value : 1f);
                    SaveSystem.SaveControlSettings(val, joystickSizeSlider != null ? joystickSizeSlider.value : 1f);
                });

            if (joystickSizeSlider != null)
                joystickSizeSlider.onValueChanged.AddListener(val =>
                {
                    ApplyControls(leftHandedToggle != null && leftHandedToggle.isOn, val);
                    SaveSystem.SaveControlSettings(leftHandedToggle != null && leftHandedToggle.isOn, val);
                });

            if (graphicsTierDropdown != null)
                graphicsTierDropdown.onValueChanged.AddListener(val =>
                {
                    ApplyGraphics(val);
                    SaveSystem.SaveGraphicsTier(val);
                });
        }

        private void ApplyControls(bool leftHanded, float size)
        {
            if (VirtualJoystick.Instance != null)
            {
                VirtualJoystick.Instance.SetLeftHanded(leftHanded);
                VirtualJoystick.Instance.SetJoystickScale(size);
            }
        }

        private void ApplyGraphics(int tier)
        {
            // Tier 0 = Low (Fast mobile performance), Tier 1 = Medium
            if (tier == 0)
            {
                QualitySettings.shadows = ShadowQuality.Disable;
                QualitySettings.shadowCascades = 1;
                QualitySettings.antiAliasing = 0;
            }
            else
            {
                QualitySettings.shadows = ShadowQuality.HardOnly;
                QualitySettings.shadowCascades = 1;
                QualitySettings.antiAliasing = 2;
            }
        }
    }
}
