using UnityEngine;
using System.Collections;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    //public string sceneName;
    private UnityEngine.UI.Button startButn;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //MusicManager.Instance.PlayMusicUnderground();
    }
    // private void Start()
    // {     
    //     OnButtonPress_Start();
    // }
    public void OnButtonPress_Start()
    {
        SceneManager.LoadScene("LevelsScreen", LoadSceneMode.Single);
        Debug.Log("Click");
        //MusicManager.Instance.PlayMusicLevel1();
    }
    
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

      public void OnButtonPress_Level1()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        MusicManager.Instance.PlayMusicLevel1();
        Debug.Log("level:1");
        //MusicManager.Instance.PlayMusicLevel1();
    }

    // public void LoadScene(string sceneName)
    // {
    //     SceneManager.LoadScene(sceneName);
    // }

    //Gọi hàm này từ sự kiện onClick của nút
    


    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }
}