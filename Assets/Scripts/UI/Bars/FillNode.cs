using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillNode : MonoBehaviour
{
    public Image fullImage;

    //private void Awake()
    //{
    //    fullStar.transform.localScale = Vector3.zero;
    //}

    public void setFull(bool isFull)
    {
        if(isFull)
            fullImage.transform.localScale = Vector3.one;
        else
            fullImage.transform.localScale = Vector3.zero;
    }
}
