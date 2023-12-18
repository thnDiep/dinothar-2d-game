using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
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

