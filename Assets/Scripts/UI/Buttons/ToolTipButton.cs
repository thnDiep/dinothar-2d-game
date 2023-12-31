using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CanvasGroup canvasGroup;
    public Image iconButton;

    void Start()
    {
        canvasGroup.alpha = 0f;
        iconButton.enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; 
        iconButton.enabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canvasGroup.alpha = 0f;
        iconButton.enabled = true;
    }
}
