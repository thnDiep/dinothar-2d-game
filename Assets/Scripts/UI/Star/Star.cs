using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    public Image fullStar;

    //private void Awake()
    //{
    //    fullStar.transform.localScale = Vector3.zero;
    //}

    public void setStar(bool isFull)
    {
        if(isFull)
            fullStar.transform.localScale = Vector3.one;
        else
            fullStar.transform.localScale = Vector3.zero;
    }
}
