using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeSpeed = 4f;
    private Canvas canvas;

    public static SceneFader Instance { get; private set; }

    private bool isTransitioning = false;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            fadeCanvasGroup.alpha = 1;
            StartCoroutine(InitialFade());
        }

        SetupCanvas();
    }

    void SetupCanvas()
    {
        canvas = GetComponentInChildren<Canvas>();
        if (canvas == null)
        {
            if (fadeCanvasGroup != null)
                canvas = fadeCanvasGroup.GetComponent<Canvas>();
        }

        if (canvas != null)
        {
            canvas.sortingOrder = 9999;
            canvas.overrideSorting = true; // This ensures it ignores parent canvas sorting
        }
    }

    public void TransitionToScene(string sceneName)
    {
        if (isTransitioning) return;

        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isTransitioning = true;
        Time.timeScale = 0f;

        // Fade out
        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 0;

        while (fadeCanvasGroup.alpha < 1)
        {
            fadeCanvasGroup.alpha += Time.unscaledDeltaTime * fadeSpeed;
            yield return null;
        }

        // Load scene
        SceneManager.LoadScene(sceneName);

        // CRITICAL: Wait for scene to fully initialize
        yield return null; // Wait one frame for scene to start
        yield return null; // Wait another frame for objects to initialize

        // Now fade in
        fadeCanvasGroup.alpha = 1;
        while (fadeCanvasGroup.alpha > 0)
        {
            fadeCanvasGroup.alpha -= Time.unscaledDeltaTime * fadeSpeed;
            yield return null;
        }
        isTransitioning = false;
        Time.timeScale = 1f;
    }

    private IEnumerator InitialFade()
    {
        fadeCanvasGroup.alpha = 1;
        while (fadeCanvasGroup.alpha > 0)
        {
            fadeCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }
}