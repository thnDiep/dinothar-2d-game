using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteScreen : MonoBehaviour
{
    public Image fadeImage;
    public FillBar starBar;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI diamondText;

    public float fadeDuration = 2.0f;
    private float currentAlpha = 0f;

    void Start()
    {
        fadeImage = GetComponent<Image>();
        starBar.setFullNodes(PlayerManager.Instance.getStar());

        if(moneyText != null)
            moneyText.text = PlayerManager.Instance.getMoney().ToString();

        if(diamondText != null)
            diamondText.text = PlayerManager.Instance.getDiamond().ToString();

        SetAlpha(0);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            currentAlpha = Mathf.Lerp(0f, 0.5f, elapsedTime / fadeDuration);
            SetAlpha(currentAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Time.timeScale = 0;
    }

    void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
