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
        updateMusicUI();
        updateSoundUI();
    }

    private void OnEnable()
    {
        updateMusicUI();
        updateSoundUI();
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