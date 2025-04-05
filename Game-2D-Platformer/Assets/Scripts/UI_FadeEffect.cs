using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeEffect : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    public void ScreenFade(float targetAlpha, float duration)
    {
        StartCoroutine(FadeCoroutine(targetAlpha, duration));
    }

    private IEnumerator FadeCoroutine(float targetAlpha, float duration)
    {
        float time = 0;
        Color currentColor = fadeImage.color;

        float startAlpha = currentColor.a;

        while (time < duration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);

            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }
}
