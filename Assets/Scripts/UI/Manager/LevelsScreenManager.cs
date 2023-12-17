using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsScreenManager : MonoBehaviour
{
    public void OnClickScreenLevel1()
    {
        SceneLoader.Instance.OnButtonPress_Level1();
    }
}
