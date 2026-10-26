using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LanternTrail.Core
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [Header("Fade Settings")]
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [SerializeField] private float fadeDuration = 0.4f;

        private bool isLoading = false;

        public bool IsLoading => isLoading;
        public event Action<float> OnFadeProgress;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public bool LoadSceneAsync(string sceneName)
        {
            // Section 3.3: Ignore second load request while one is running
            if (isLoading) return false;

            StartCoroutine(LoadSceneRoutine(sceneName));
            return true;
        }

        public bool LoadSceneAsync(int sceneIndex)
        {
            if (isLoading) return false;

            StartCoroutine(LoadSceneRoutine(sceneIndex));
            return true;
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            isLoading = true;
            yield return StartCoroutine(FadeRoutine(0f, 1f));

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            while (!op.isDone)
            {
                yield return null;
            }

            yield return StartCoroutine(FadeRoutine(1f, 0f));
            isLoading = false;
        }

        private IEnumerator LoadSceneRoutine(int sceneIndex)
        {
            isLoading = true;
            yield return StartCoroutine(FadeRoutine(0f, 1f));

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
            while (!op.isDone)
            {
                yield return null;
            }

            yield return StartCoroutine(FadeRoutine(1f, 0f));
            isLoading = false;
        }

        private IEnumerator FadeRoutine(float from, float to)
        {
            if (fadeCanvasGroup == null) yield break;

            float t = 0f;
            fadeCanvasGroup.blocksRaycasts = true;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                float progress = Mathf.Clamp01(t / fadeDuration);
                fadeCanvasGroup.alpha = Mathf.Lerp(from, to, progress);
                OnFadeProgress?.Invoke(progress);
                yield return null;
            }
            fadeCanvasGroup.alpha = to;
            fadeCanvasGroup.blocksRaycasts = (to > 0.5f);
        }
    }
}
