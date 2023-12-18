using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionScreen : MonoBehaviour
{
    public void OnClickLevel1()
    {
        SceneLoader.Instance.ToLevel1Screen();
    }
    public void OnClickBackMenu()
    {
        SceneLoader.Instance.ToMenuScreen();
    }
    
}
