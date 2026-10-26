using UnityEngine;
using LanternTrail.Save;

namespace LanternTrail.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("SFX Clips")]
        [SerializeField] private AudioClip collectSfx;
        [SerializeField] private AudioClip goldenCollectSfx;
        [SerializeField] private AudioClip gateUnlockSfx;
        [SerializeField] private AudioClip plateStepSfx;
        [SerializeField] private AudioClip plateResetSfx;
        [SerializeField] private AudioClip beaconIgniteSfx;
        [SerializeField] private AudioClip noteReadSfx;

        [Header("Footsteps by Surface")]
        [SerializeField] private AudioClip grassFootstep;
        [SerializeField] private AudioClip woodFootstep;
        [SerializeField] private AudioClip stoneFootstep;

        private float masterVolume = 1.0f;
        private float musicVolume = 0.7f;
        private float sfxVolume = 0.85f;
        private bool vibrationEnabled = true;

        public float MasterVolume => masterVolume;
        public float MusicVolume => musicVolume;
        public float SfxVolume => sfxVolume;
        public bool VibrationEnabled => vibrationEnabled;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
            if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();

            musicSource.loop = true;
            musicSource.playOnAwake = false;
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;

            LoadSettings();
        }

        private void LoadSettings()
        {
            musicVolume = SaveSystem.GetMusicVolume();
            sfxVolume = SaveSystem.GetSfxVolume();
            vibrationEnabled = SaveSystem.GetVibrationEnabled();

            UpdateVolumes();
        }

        private void UpdateVolumes()
        {
            if (musicSource != null) musicSource.volume = musicVolume * masterVolume;
            if (sfxSource != null) sfxSource.volume = sfxVolume * masterVolume;
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null || musicSource == null) return;
            if (musicSource.clip == clip && musicSource.isPlaying) return;

            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlaySFX(AudioClip clip, float volumeScale = 1.0f)
        {
            if (clip != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(clip, sfxVolume * masterVolume * volumeScale);
            }
        }

        public void PlayCollect(bool isGolden)
        {
            PlaySFX(isGolden ? goldenCollectSfx : collectSfx, isGolden ? 1.0f : 0.85f);
            TriggerVibration();
        }

        public void PlayGateUnlock()
        {
            PlaySFX(gateUnlockSfx, 1.0f);
            TriggerVibration();
        }

        public void PlayPlateStep(bool isCorrect)
        {
            PlaySFX(isCorrect ? plateStepSfx : plateResetSfx, 0.9f);
            TriggerVibration();
        }

        public void PlayBeaconIgnite()
        {
            PlaySFX(beaconIgniteSfx, 1.0f);
            TriggerVibration();
        }

        public void PlayNoteRead()
        {
            PlaySFX(noteReadSfx, 0.75f);
        }

        public void PlayFootstep(string surfaceTag)
        {
            AudioClip clip = surfaceTag switch
            {
                "Wood" => woodFootstep ?? grassFootstep,
                "Stone" => stoneFootstep ?? grassFootstep,
                _ => grassFootstep
            };

            if (clip != null)
            {
                // Subtle random pitch variation
                sfxSource.pitch = Random.Range(0.92f, 1.08f);
                PlaySFX(clip, 0.35f);
                sfxSource.pitch = 1.0f;
            }
        }

        public void TriggerVibration()
        {
            if (vibrationEnabled)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                Handheld.Vibrate();
#endif
            }
        }

        public void SetMasterVolume(float vol)
        {
            masterVolume = Mathf.Clamp01(vol);
            UpdateVolumes();
        }

        public void SetMusicVolume(float vol)
        {
            musicVolume = Mathf.Clamp01(vol);
            UpdateVolumes();
            SaveSystem.SaveAudioSettings(musicVolume, sfxVolume, vibrationEnabled);
        }

        public void SetSfxVolume(float vol)
        {
            sfxVolume = Mathf.Clamp01(vol);
            UpdateVolumes();
            SaveSystem.SaveAudioSettings(musicVolume, sfxVolume, vibrationEnabled);
        }

        public void SetVibration(bool enabled)
        {
            vibrationEnabled = enabled;
            SaveSystem.SaveAudioSettings(musicVolume, sfxVolume, vibrationEnabled);
        }
    }
}
