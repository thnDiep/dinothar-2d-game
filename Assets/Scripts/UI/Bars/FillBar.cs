using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    public FillNode[] nodes;

    public void setFullNodes(int starNumber)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (i < starNumber)
                nodes[i].setFull(true);
            else
                nodes[i].setFull(false);
        }
    }
}
