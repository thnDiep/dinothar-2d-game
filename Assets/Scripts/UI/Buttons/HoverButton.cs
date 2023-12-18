using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image iconButton;
    private void Start()
    {
        iconButton.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        iconButton.color = new Color(1, 1, 1, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        iconButton.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
}