using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;

    public void StartScreenFade()
    {
        StartCoroutine(FadeInAndOut());
    }

    IEnumerator FadeInAndOut()
    {
        // Fade In
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Đặt alpha là 1

        while (elapsedTime < fadeInTime)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeInTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for a moment
        Debug.Log("FadeIn Complete");
        yield return new WaitForSeconds(0.5f);

        // Fade Out
        elapsedTime = 0f;
        startColor = fadeImage.color;
        targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Đặt alpha là 0

        while (elapsedTime < fadeOutTime)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeOutTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
        Debug.Log("FadeOut complete!");
    }
}
