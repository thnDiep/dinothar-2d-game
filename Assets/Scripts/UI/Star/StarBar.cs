using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StarBar : MonoBehaviour
{
    public Star[] stars;

    public void setStars(int starNumber)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < starNumber)
                stars[i].setStar(true);
            else
                stars[i].setStar(false);
        }
    }
}
