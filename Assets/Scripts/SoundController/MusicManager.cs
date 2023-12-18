using System.Collections;
using System.Collections.Generic;
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
        //sceneLoader = GetComponent<SceneLoader>();
        //SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private void Start()
    {
        PlayMusicUnderground();
    }

    public static string musicLevel1 = "MusicLevel1";
    public static string musicUnderground = "MusicUnderground";
    public static string gameoverSound = "GameOver";
    public void PlayMusicLevel1()
    {
        PlaySound(musicLevel1);
    }
    public void PlayMusicUnderground()
    {
        PlaySound(musicUnderground);
    }
    public void PlayMusicGameOver()
    {
        PlaySound(gameoverSound);
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

    public void StopSoundMusic()
    {
        audioSource.Pause();
        audioSource.Stop();
    }
    
}
