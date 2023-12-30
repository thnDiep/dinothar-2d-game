using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private static MusicManager _instance;
    //private static SceneLoader sceneLoader;

    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MusicManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(MusicManager).Name);
                    _instance = singletonObject.AddComponent<MusicManager>();
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

    private void Start()
    {
        PlayMusicUnderground();
    }

    public static string musicLevel1 = "MusicLevel1";
    public static string musicLevel2 = "MusicLevel2";
    public static string musicLevel3 = "MusicLevel3";
    public static string musicLevel4 = "MusicLevel4";
    public static string musicLevel5 = "MusicLevel5";
    public static string musicUnderground = "MusicUnderground";
    public static string musicDoneGameSound = "MusicDoneGame";
    public static string musicBoss = "Boss";
    public static string gameoverSound = "GameOver";
    public void PlayMusicLevel1()
    {
        PlaySound(musicLevel1);
    }
    public void PlayMusicLevel2()
    {
        PlaySound(musicLevel2);
    }
    public void PlayMusicLevel3()
    {
        PlaySound(musicLevel3);
    }
    public void PlayMusicLevel4()
    {
        PlaySound(musicLevel4);
    }
    public void PlayMusicLevel5()
    {
        PlaySound(musicLevel5);
    }
    public void PlayMusicUnderground()
    {
        PlaySound(musicUnderground);
    }
    public void PlayMusicDoneGame()
    {
        PlaySound(musicDoneGameSound);
    }

    public void PlayMusicGameOver()
    {
        PlaySound(gameoverSound);
    }
    public void PlayMusicBoss()
    {
        PlaySound(musicBoss);
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
    public void PlayMusicScene(string sceneName)
    {
        if(sceneName == "Level1") PlayMusicLevel1();
        else if(sceneName == "Level2") PlayMusicLevel2();
        else if(sceneName == "Level3") PlayMusicLevel3();
        else if(sceneName == "Level4") PlayMusicLevel4();
        else if(sceneName == "Level5") PlayMusicLevel5();
    }
    public void StopMusic()
    {
        audioSource.Pause();
        audioSource.Stop();
    }
    public void MusicStatus(bool status)
    {
        if(this.audioSource.mute == !status) return; //nghịch đảo trạng thái mute của status
        this.audioSource.mute = !status;

        if(this.audioSource.mute) audioSource.Stop();
        else this.audioSource.Play();
    }
    public void MusicVolume(float volume)
    {
        this.audioSource.volume = volume;
    }
}
