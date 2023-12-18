using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInPause : MonoBehaviour
{
    public Image musicToggleOn;
    public Image soundToggleOn;
    public Image cheatToggleOn;

    private bool onMusic;
    private bool onSound;
    private bool isCheat;
    void Start()
    {
        onMusic = true; //lấy trạng thái nhạc
        onSound = true; //lấy trạng thái âm thanh
        isCheat = GameManager.Instance.isCheat();

        musicToggleOn.gameObject.SetActive(onMusic); 
        soundToggleOn.gameObject.SetActive(onSound); 
        cheatToggleOn.gameObject.SetActive(isCheat);
    }

    public void musicToggle()
    {
        onMusic = !onMusic;
        musicToggleOn.gameObject.SetActive(onMusic);
    }

    public void soundToggle()
    {
        onSound = !onSound;
        soundToggleOn.gameObject.SetActive(onSound);
    }

    public void cheatToggle()
    {
        isCheat = !isCheat;
        cheatToggleOn.gameObject.SetActive(isCheat);
        GameManager.Instance.setCheat(isCheat);
    }
}
