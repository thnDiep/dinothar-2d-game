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

    public void ToLevelsScreen()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            MusicManager.Instance.PlayMusicUnderground();
        SceneManager.LoadScene("LevelsScreen", LoadSceneMode.Single);
        Time.timeScale = 1.0f;
    }

    public void ToMenuScreen()
    {
        SceneManager.LoadScene("MenuScreen", LoadSceneMode.Single);
        //MusicManager.Instance.PlayMusicLevel1();
        Time.timeScale = 1.0f;
    }

    //public void ToTestScreen()
    //{
    //    SceneManager.LoadScene("Level1_Diep", LoadSceneMode.Single);
    //    MusicManager.Instance.PlayMusicLevel1();
    //    Time.timeScale = 1.0f;
    //}

    public void ToLevel1Screen()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        MusicManager.Instance.PlayMusicLevel1();
        Time.timeScale = 1.0f;
    }

    public void ToLevel2Screen()
    {
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
        MusicManager.Instance.PlayMusicLevel1();
        Time.timeScale = 1.0f;
    }

    public void ToLevel3Screen()
    {
        SceneManager.LoadScene("Level3", LoadSceneMode.Single);
        MusicManager.Instance.PlayMusicLevel1();
        Time.timeScale = 1.0f;
    }

    public void ToLevel4Screen()
    {
        SceneManager.LoadScene("Level4", LoadSceneMode.Single);
        MusicManager.Instance.PlayMusicLevel1();
        Time.timeScale = 1.0f;
    }

    public void ToLevel5Screen()
    {
        SceneManager.LoadScene("Level5", LoadSceneMode.Single);
        MusicManager.Instance.PlayMusicLevel1();
        Time.timeScale = 1.0f;
    }

    public void NextLevel()
    {
        if (GameManager.Instance.getLevel() < 5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            MusicManager.Instance.PlayMusicLevel1();
        }
        else
        {
            SceneManager.LoadScene("LevelsScreen", LoadSceneMode.Single);
        }
        Time.timeScale = 1.0f;
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