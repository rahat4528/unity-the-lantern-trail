using UnityEngine;
using UnityEngine.UI;
using LanternTrail.Core;

namespace LanternTrail.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Menu Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        [Header("Settings Screen")]
        [SerializeField] private SettingsMenu settingsMenu;

        private void Awake()
        {
            if (startButton != null) startButton.onClick.AddListener(OnStartClicked);
            if (settingsButton != null) settingsButton.onClick.AddListener(OnSettingsClicked);
            if (exitButton != null) exitButton.onClick.AddListener(OnExitClicked);
        }

        private void OnStartClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartNewRun();
            }

            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadSceneAsync("Location_01");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Location_01");
            }
        }

        private void OnSettingsClicked()
        {
            if (settingsMenu != null)
            {
                settingsMenu.Open();
            }
        }

        private void OnExitClicked()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
