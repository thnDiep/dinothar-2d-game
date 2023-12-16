using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialBookBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image tutorialBookIcon;

    private void Awake()
    {
        tutorialBookIcon.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Thực hiện hành động khi chuột vào button
        tutorialBookIcon.color = new Color(1, 1, 1, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tutorialBookIcon.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        // Thực hiện hành động khi chuột rời khỏi button
    }
}