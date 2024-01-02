using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class SliderInPause : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;

    private void Start()
    {
        StartCoroutine(CheckGameManagerInstance());
    }

    private void OnEnable()
    {
        StartCoroutine(CheckGameManagerInstance());
    }

    private IEnumerator CheckGameManagerInstance()
    {
        yield return new WaitForSeconds(0.1f); // Đợi một khoảng thời gian ngắn
        if (GameManager.Instance != null)
        {
            updateMusicUI();
            updateSoundUI();
        }
    }
    //music slider volume 
    public void updateMusicUI()
    {
        musicSlider.value = GameManager.Instance.getMusicVolume();
    }
    public void SliderMusicUpdate()
    {
        GameManager.Instance.setMusicVolume(musicSlider.value);
        MusicManager.Instance.MusicVolume(musicSlider.value);
    }
  

    //sound slider volume 
    public void updateSoundUI()
    {
        soundSlider.value = GameManager.Instance.getSoundVolume();
    }
    public void SliderSoundUpdate()
    {
        GameManager.Instance.setSoundVolume(soundSlider.value);
        SoundManager.Instance.SoundVolume(soundSlider.value);
    }
}