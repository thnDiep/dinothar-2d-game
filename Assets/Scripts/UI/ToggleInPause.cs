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

    private void OnEnable() {
        StartCoroutine(CheckGameManagerInstance1());
    }

    void Start()
    {
        StartCoroutine(CheckGameManagerInstance());
    }

    private IEnumerator CheckGameManagerInstance()
    {
        yield return new WaitForSeconds(0.1f); // Đợi một khoảng thời gian ngắn
        if (GameManager.Instance != null)
        {
            onMusic = GameManager.Instance.isMusicStatus();
            onSound = GameManager.Instance.isSoundStatus();
            isCheat = GameManager.Instance.isCheat();
            Debug.Log(onMusic);
            musicToggleOn.gameObject.SetActive(onMusic);
            soundToggleOn.gameObject.SetActive(onSound);
            cheatToggleOn.gameObject.SetActive(isCheat);
        }
    }

    private IEnumerator CheckGameManagerInstance1()
    {
        yield return new WaitForSeconds(0.1f); // Đợi một khoảng thời gian ngắn
        if (GameManager.Instance != null)
        {
            onMusic = GameManager.Instance.isMusicStatus();
            onSound = GameManager.Instance.isSoundStatus();
            isCheat = GameManager.Instance.isCheat();

            MusicManager.Instance.MusicStatus(onMusic);
            SoundManager.Instance.SoundStatus(onSound);
        }
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
        SoundManager.Instance.SoundStatus(onSound);        
    }

    public void cheatToggle()
    {
        isCheat = !isCheat;
        cheatToggleOn.gameObject.SetActive(isCheat);
        GameManager.Instance.setCheat(isCheat);
    }
}
