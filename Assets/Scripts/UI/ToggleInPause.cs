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
        onMusic = GameManager.Instance.isMusicStatus();
        onSound = GameManager.Instance.isSoundStatus();
        isCheat = GameManager.Instance.isCheat();

        musicToggleOn.gameObject.SetActive(onMusic);
        soundToggleOn.gameObject.SetActive(onSound);
        cheatToggleOn.gameObject.SetActive(isCheat);
    }
    private void OnEnable() {
        onMusic = GameManager.Instance.isMusicStatus();
        onSound = GameManager.Instance.isSoundStatus();
        isCheat = GameManager.Instance.isCheat();
        
        MusicManager.Instance.MusicStatus(onMusic);
        SoundManager.Instance.SoundStatus(onSound);    
        // if(onSound == false)
        // {
        //     SoundButtonClick.Instance.StopSoundButton();
        // } 
    }

    public void musicToggle()
    {
        onMusic = !onMusic;
        musicToggleOn.gameObject.SetActive(onMusic);
        GameManager.Instance.setMusicStatus(onMusic);
        MusicManager.Instance.StopMusic();
        MusicManager.Instance.MusicStatus(onMusic);
    }

    public void soundToggle()
    {
        onSound = !onSound;
        soundToggleOn.gameObject.SetActive(onSound);
        GameManager.Instance.setSoundStatus(onSound);
        SoundManager.Instance.StopSound();
        //SoundButtonClick.Instance.StopSoundButton();
        SoundManager.Instance.SoundStatus(onSound);        
    }

    public void cheatToggle()
    {
        isCheat = !isCheat;
        cheatToggleOn.gameObject.SetActive(isCheat);
        GameManager.Instance.setCheat(isCheat);
    }
}
