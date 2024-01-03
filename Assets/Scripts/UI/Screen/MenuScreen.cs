using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    private MusicManager music;
    public void OnClickStart()
    {
        GameManager.Instance.NewGame();
        
        SceneLoader.Instance.ToLevelsScreen();
    }
    public void OnClickContinue()
    {
        SceneLoader.Instance.ToLevelsScreen();
    }
}

