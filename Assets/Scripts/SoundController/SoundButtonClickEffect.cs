using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonClickEffect : MonoBehaviour
{
    private void Awake()
    {
        var btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySoundButtonClick();
            Debug.Log("Sound!");
        });
    }
}
