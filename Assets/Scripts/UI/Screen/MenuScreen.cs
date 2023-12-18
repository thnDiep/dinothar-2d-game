using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneLoader.Instance.ToLevelsScreen_NewGame();
    }
    public void OnClickContinue()
    {
        SceneLoader.Instance.ToLevelsScreen_NewGame();
    }
}

