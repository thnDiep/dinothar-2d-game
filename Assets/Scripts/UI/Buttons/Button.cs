using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    private void Awake()
    {
        image.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(1, 1, 1, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
}