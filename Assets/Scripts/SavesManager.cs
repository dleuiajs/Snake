using UnityEngine;

public class SavesManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] GameManager gameManager;

    void OnApplicationQuit()
    {
        SaveGame();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGame();
        }
    }

    void Awake()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("highscore", gameManager.highscore);
    }

    public void LoadGame()
    {
        gameManager.highscore = PlayerPrefs.GetInt("highscore", 0);
    }
}
