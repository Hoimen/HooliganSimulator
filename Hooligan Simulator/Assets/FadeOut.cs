using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    void Start()
    {
        // start transparent (can remove later) 
        SetAlpha(0f);
    }

    public void FadeToBlack()
    {
        StartCoroutine(Fade(0f, 1f));
    }

    public void FadeFromBlack()
    {
        StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float blend = Mathf.Clamp01(t / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, blend);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
