using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

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

    public void ToLevelsScreen_NewGame()
    {
        GameManager.Instance.NewGame();
        SceneManager.LoadScene("LevelsScreen", LoadSceneMode.Single);
    }

    // public void ToLevelsScreen_Continue()
    // {
    //     SceneManager.LoadScene("LevelsScreen", LoadSceneMode.Single);
    //     //MusicManager.Instance.PlayMusicLevel1();
    // }

    public void ToMenuScreen()
    {
        SceneManager.LoadScene("MenuScreen", LoadSceneMode.Single);
        //MusicManager.Instance.PlayMusicLevel1();
    }

    public void ToLevel1Screen()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        MusicManager.Instance.PlayMusicLevel1();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }
}