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
        Color currrentColor = fadeImage.color;

        float startAlpha = currrentColor.a;

        while (time < duration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);

            fadeImage.color = new Color(currrentColor.r, currrentColor.g, currrentColor.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(currrentColor.r, currrentColor.g, currrentColor.b, targetAlpha);
    }
}
