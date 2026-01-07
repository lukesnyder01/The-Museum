using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeSpeed = 2f;

    public static SceneFader Instance { get; private set; }

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
        Canvas canvas = GetComponentInChildren<Canvas>();
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
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        // Fade out
        fadeCanvasGroup.alpha = 0;
        while (fadeCanvasGroup.alpha < 1)
        {
            fadeCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
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
            fadeCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
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