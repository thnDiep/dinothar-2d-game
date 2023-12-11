using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.Mathematics;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

public class ClueCollecitonBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image clueIcon;
    [SerializeField] Image clueCollection;
    [SerializeField] Image clue1, clue2, clue3;


    private Color originalColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Color shineColor = new Color(1f, 1f, 1f, 1f);
    private int collectedClueNumber = 0;

    private float shineTime;
    private float shineTimer = 7f;

    private bool collected;

    float elapsedTime = 0f;



    private void Awake()
    {
        clueIcon.color = originalColor;
        if (clueCollection != null)
        {
            clueCollection.gameObject.SetActive(false);
        }
        shineTime = shineTimer;
        collected = false;

        // elapsedTime = 0f;

    }

    private void Update()
    {
        if (collected)
        {
            shineTime -= Time.deltaTime;
            elapsedTime += Time.deltaTime;

            clueIcon.color = Color.Lerp(originalColor, shineColor, Mathf.PingPong(elapsedTime, 1f));

            if (shineTime < 0)
            {
                shineTime = shineTimer;
                collected = false;
                clueIcon.color = originalColor;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Thực hiện hành động khi chuột vào button
        Debug.Log("Mouse Enter");
        clueIcon.color = shineColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Thực hiện hành động khi chuột rời khỏi button
        Debug.Log("Mouse Exit");
        clueIcon.color = originalColor;
    }

    public void openClueCollection()
    {
        clueCollection.gameObject.SetActive(true);
    }

    public void closeClueCollection()
    {
        clueCollection.gameObject.SetActive(false);
    }

    public void unblockClue()
    {
        collected = true;
        clueIcon.color = shineColor;
        collectedClueNumber++;
        if (collectedClueNumber == 1)
        {
            clue1.gameObject.SetActive(true);
        }
        else if (collectedClueNumber == 2)
        {
            clue2.gameObject.SetActive(true);
        }
        else
        {
            clue3.gameObject.SetActive(true);
        }
    }

}