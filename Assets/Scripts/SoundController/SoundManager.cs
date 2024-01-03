using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private static SoundManager _instance;
    private bool onSoundMenu;

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
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(CheckGameManagerInstance());
    }

    private IEnumerator CheckGameManagerInstance()
    {
        yield return new WaitForSeconds(0.1f); // Đợi một khoảng thời gian ngắn
        if (GameManager.Instance != null)
        {
            onSoundMenu = GameManager.Instance.isSoundStatus();
            if (!onSoundMenu)
            {
                Debug.Log("Sound is off");
                StopSound();
                GameManager.Instance.setSoundStatus(onSoundMenu);
            }
        }
    }

    public static string jumpSound = "PlayerJump";
    public static string sitSound = "PlayerSitting";

    public static string ButtonClickSound = "ButtonClick";
    public static string collectMoneySound = "CollectMoney";
    public static string collectClueSound = "CollectClue";
    public static string bulletSound = "PlayerBullet";
    public static string combineSkilSound = "UserCombineSkill";
    public static string playerHurtSound = "PlayerHurt";
    public static string bossHurtSound = "BossHurt";
    public static string loseHeartSound = "LoseHeart";
    public static string winSound = "WinLevel";
    public static string gameoverSound = "GameOver";

    public void PlaySoundButtonClick()
    {
        PlaySound(ButtonClickSound);
    }
    public void PlaySoundJump()
    {
        PlaySound(jumpSound);
    }

    public void PlaySoundSitting()
    {
        PlaySound(sitSound);
    }

    public void PlaySoundBulletSound()
    {
        PlaySound(bulletSound);
    }
    public void PlaySoundCollectMoney()
    {
        PlaySound(collectMoneySound);
    }
    public void PlaySoundCollectClue()
    {
        PlaySound(collectClueSound);
    }

    public void PlaySoundHurt()
    {
        PlaySound(playerHurtSound);
    }

    public void PlayBossHurtSound()
    {
        PlaySound(bossHurtSound);
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
    public void SoundStatus(bool status)
    {
        if (this.audioSource.mute == !status) return; //nghịch đảo trạng thái mute của status
        this.audioSource.mute = !status;

        if (this.audioSource.mute) audioSource.Stop();
        else this.audioSource.Play();
    }
    public void SoundVolume(float volume)
    {
        this.audioSource.volume = volume;
    }
}