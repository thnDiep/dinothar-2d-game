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
    }

    public void ToLevelsScreen()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SoundManager.Instance.StopSound();
            MusicManager.Instance.PlayMusicUnderground();
        }
        SceneManager.LoadScene("LevelsScreen", LoadSceneMode.Single);
        Time.timeScale = 1.0f;
    }


    public void ToMenuScreen()
    {
        SceneManager.LoadScene("MenuScreen", LoadSceneMode.Single);
        Time.timeScale = 1.0f;
    }

    public void ToCutScreen()
    {
        SceneManager.LoadScene("CutScene", LoadSceneMode.Single);
        Time.timeScale = 1.0f;
    }

    // public void ToTestScreen()
    // {
    //     SceneManager.LoadScene("Level1_Diep", LoadSceneMode.Single);
    //     MusicManager.Instance.PlayMusicLevel1();
    //     Time.timeScale = 1.0f;
    // }

    // public void ToTestScreenNgan()
    // {
    //     SceneManager.LoadScene("MenuScreen_Ngan", LoadSceneMode.Single);
    //     //MusicManager.Instance.PlayMusicUnderground();
    //     Time.timeScale = 1.0f;
    // }
    // public void ToTestLevelScreenNgan()
    // {
    //     if (SceneManager.GetActiveScene().buildIndex != 0)
    //     {
    //         SoundManager.Instance.StopSound();
    //         MusicManager.Instance.PlayMusicUnderground();
    //     }
    //     SceneManager.LoadScene("LevelsScreen_Ngan", LoadSceneMode.Single);
    //     Time.timeScale = 1.0f;
    // }

    public void ToLevel1Screen()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        GameManager.Instance.StartNewLevel(1);
        MusicManager.Instance.PlayMusicLevel1();
        Time.timeScale = 1.0f;
    }

    public void ToLevel2Screen()
    {
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
        GameManager.Instance.StartNewLevel(2);
        MusicManager.Instance.PlayMusicLevel2();
        Time.timeScale = 1.0f;
    }

    public void ToLevel3Screen()
    {
        SceneManager.LoadScene("Level3", LoadSceneMode.Single);
        GameManager.Instance.StartNewLevel(3);
        MusicManager.Instance.PlayMusicLevel3();
        Time.timeScale = 1.0f;
    }

    public void ToLevel4Screen()
    {
        SceneManager.LoadScene("Level4", LoadSceneMode.Single);
        GameManager.Instance.StartNewLevel(4);
        MusicManager.Instance.PlayMusicLevel4();
        Time.timeScale = 1.0f;
    }

    public void ToLevel5Screen()
    {
        SceneManager.LoadScene("Level5", LoadSceneMode.Single);
        GameManager.Instance.StartNewLevel(5);
        MusicManager.Instance.PlayMusicLevel5();
        Time.timeScale = 1.0f;
    }

    public void NextLevel()
    {
        if (GameManager.Instance.getLevel() < 5)
        {
            GameManager.Instance.StartNewLevel(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //xử lý lấy name của scene tiếp theo 
            int nextBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
            Scene nextScene = SceneManager.GetSceneByBuildIndex(nextBuildIndex);
            string nextSceneName = nextScene.name;

            SoundManager.Instance.StopSound();
            MusicManager.Instance.PlayMusicScene(nextSceneName);
        }
        else
        {
            SceneManager.LoadScene("LevelsScreen", LoadSceneMode.Single);
            SoundManager.Instance.StopSound();
            MusicManager.Instance.PlayMusicUnderground();
        }
        Time.timeScale = 1.0f;
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        int curBuildIndex = SceneManager.GetActiveScene().buildIndex;
        Scene curScene = SceneManager.GetSceneByBuildIndex(curBuildIndex);
        string curSceneName = curScene.name;

        SoundManager.Instance.StopSound();
        MusicManager.Instance.PlayMusicScene(curSceneName);
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void SettingGame()
    {
        Time.timeScale = 0.0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }
}