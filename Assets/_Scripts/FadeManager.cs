using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private GameObject fadeCanvas;

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

    private void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("FadeManager: Fade Image is not assigned.");
        }
        StartCoroutine(FadeInOutRoutine());
    }

    /// <summary>
    /// Fades the screen from transparent to opaque and back to transparent.
    /// </summary>
    public void FadeInOut()
    {
        StartCoroutine(FadeInOutRoutine());
    }

    private IEnumerator FadeInOutRoutine()
    {
        if (fadeImage == null) yield break;

        fadeCanvas.SetActive(true);

        // Fade from 0 to 1 (transparent to opaque)
        yield return FadeRoutine(1f);

        // Fade from 1 to 0 (opaque to transparent)
        yield return FadeRoutine(0f);
        fadeCanvas.SetActive(false);
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
}
