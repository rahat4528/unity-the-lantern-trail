using System;
using System.IO;
using UnityEngine;

namespace LanternTrail.Save
{
    public static class SaveSystem
    {
        private const string PREF_MUSIC = "LT_MusicVolume";
        private const string PREF_SFX = "LT_SfxVolume";
        private const string PREF_VIBRATION = "LT_Vibration";
        private const string PREF_LEFT_HANDED = "LT_LeftHanded";
        private const string PREF_JOYSTICK_SIZE = "LT_JoystickSize";
        private const string PREF_GRAPHICS_TIER = "LT_GraphicsTier";

        private static readonly string ResultFilePath = Path.Combine(Application.persistentDataPath, "best_result.json");

        [Serializable]
        public class BestResultData
        {
            public int bestStars = 0;
            public int bestLanterns = 0;
            public int bestGolden = 0;
            public int bestNotes = 0;
            public float bestTime = 999999f;
            public string completionDate = "";
        }

        public static void SaveBestResult(int stars, int lanterns, int golden, int notes, float time)
        {
            try
            {
                BestResultData current = LoadBestResult();
                if (stars > current.bestStars || (stars == current.bestStars && time < current.bestTime))
                {
                    current.bestStars = stars;
                    current.bestLanterns = lanterns;
                    current.bestGolden = golden;
                    current.bestNotes = notes;
                    current.bestTime = time;
                    current.completionDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                    string json = JsonUtility.ToJson(current, true);
                    File.WriteAllText(ResultFilePath, json);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[SaveSystem] Failed to save best result JSON: {ex.Message}");
            }
        }

        public static BestResultData LoadBestResult()
        {
            try
            {
                if (File.Exists(ResultFilePath))
                {
                    string json = File.ReadAllText(ResultFilePath);
                    return JsonUtility.FromJson<BestResultData>(json) ?? new BestResultData();
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[SaveSystem] Failed to load best result JSON: {ex.Message}");
            }
            return new BestResultData();
        }

        public static void SaveAudioSettings(float music, float sfx, bool vibration)
        {
            PlayerPrefs.SetFloat(PREF_MUSIC, music);
            PlayerPrefs.SetFloat(PREF_SFX, sfx);
            PlayerPrefs.SetInt(PREF_VIBRATION, vibration ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void SaveControlSettings(bool leftHanded, float joystickSize)
        {
            PlayerPrefs.SetInt(PREF_LEFT_HANDED, leftHanded ? 1 : 0);
            PlayerPrefs.SetFloat(PREF_JOYSTICK_SIZE, joystickSize);
            PlayerPrefs.Save();
        }

        public static void SaveGraphicsTier(int tier) // 0=Low, 1=Medium
        {
            PlayerPrefs.SetInt(PREF_GRAPHICS_TIER, tier);
            PlayerPrefs.Save();
        }

        public static float GetMusicVolume() => PlayerPrefs.GetFloat(PREF_MUSIC, 0.7f);
        public static float GetSfxVolume() => PlayerPrefs.GetFloat(PREF_SFX, 0.85f);
        public static bool GetVibrationEnabled() => PlayerPrefs.GetInt(PREF_VIBRATION, 1) == 1;
        public static bool GetLeftHanded() => PlayerPrefs.GetInt(PREF_LEFT_HANDED, 0) == 1;
        public static float GetJoystickSize() => PlayerPrefs.GetFloat(PREF_JOYSTICK_SIZE, 1.0f);
        public static int GetGraphicsTier() => PlayerPrefs.GetInt(PREF_GRAPHICS_TIER, 1);
    }
}
