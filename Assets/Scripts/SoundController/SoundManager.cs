using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(SoundManager).Name);
                    _instance = singletonObject.AddComponent<SoundManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static string jumpSound = "PlayerJump";
    public static string collectMoneySound = "CollectMoney";
    public static string bulletSound = "PlayerBullet";
    public static string combineSkilSound = "UserCombineSkill";
    public static string hurtSound = "PlayerHurt";
    public static string gameoverSound = "GameOver";
    public static string loseHeartSound = "LoseHeart";
    public static string winSound = "WinLevel";
    public void PlaySoundJump()
    {
        PlaySound(jumpSound);
    }

    public void PlaySoundBulletSound()
    {
        PlaySound(bulletSound);
    }
    public void PlaySoundCollectMoney()
    {
        PlaySound(collectMoneySound);
    }

    public void PlaySoundHurt()
    {
        PlaySound(hurtSound);
    }
    public void PlaySoundUseCombineSkill()
    {
        PlaySound(combineSkilSound);
    }
    public void PlaySoundGameOver()
    {
        PlaySound(gameoverSound);
    }
    public void PlaySoundLoseHeart()
    {
        PlaySound(loseHeartSound);
    }
     public void PlaySoundWinLevel()
    {
        PlaySound(winSound);
    }
    public void PlaySound(string key)
    {
        AudioClip soundClip = Resources.Load<AudioClip>(key);
        if (soundClip != null)
        {
            // Tạo một đối tượng AudioSource để phát âm thanh
            audioSource.clip = soundClip;

            // Phát âm thanh
            audioSource.Play();
        }
    }

    public void StopSound()
    {
        audioSource.Pause();
        audioSource.Stop();
    }
}